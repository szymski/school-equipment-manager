<template>
<div>
    <div v-if="api.useDevVersion" class="ui segment">
        <button class="ui button" @click="processCode('KAR-WOJ-592479')">Nauczyciel</button>
        <button class="ui button" @click="processCode('POBR')">Pobranie</button>
        <button class="ui button" @click="processCode('ZWROT')">Zwrot</button>
        <button class="ui button" @click="processCode('165063918.54584')">Przedmiot</button>
    </div>

    <error-display/>

    <div class="ui form">
        <input type="text" id="codeInput" v-model="codeInput">
    </div>

    <div v-if="!teacher">
        <h2 class="scan-teacher-code-text">Zeskanuj kod nauczyciela</h2>
    </div>

    <div v-if="teacher">
        <div class="logout-text">Wylogowanie za {{ displayTime }}</div>
        <h2 class="scan-teacher-code-text">{{ teacher.name }} {{ teacher.surname }}</h2>

        <div v-if="!currentAction">
            <h3 class="scan-action-code-text">Teraz zeskanuj kod pobrania lub zwrotu.</h3>
        </div>
        <div v-else>
            <h3 class="scan-action-code-text">
                Wybrane działanie:
                {{ ({ "borrow": "Pobieranie", "return": "Zwrot" })[currentAction] }}
            </h3>

            <div v-if="!item">
                <h2 class="scan-item-code-text">Teraz zeskanuj kod przedmiotu</h2>
            </div>
            <div v-else>
                <div v-if="alreadyBorrowed && currentAction == 'borrow'">
                    <div class="failed-icon-wrapper">
                        <i class="times circle outline icon failed-icon"/>
                    </div>
                    <h1 class="scan-item-code-text">
                        Przedmiot {{ item.name }} <span v-if="item.location && item.location != ''">({{ item.location }})</span> już został pobrany przez nauczyciela {{ borrowedTeacher.name }} {{ borrowedTeacher.surname }}
                    </h1>
                </div>
                <div v-else-if="!alreadyBorrowed && currentAction == 'return'">
                    <div class="failed-icon-wrapper">
                        <i class="times circle outline icon failed-icon"/>
                    </div>
                    <h1 class="scan-item-code-text">
                        Przedmiot {{ item.name }} <span v-if="item.location && item.location != ''">({{ item.location }})</span> nie został wcześniej pobrany
                    </h1>
                </div>
                <div v-else>
                    <div class="success-icon-wrapper">
                        <i class="check circle outline icon success-icon"/>
                    </div>
                    <h1 class="scan-item-code-text">
                        {{ ({ "borrow": "Pobrano", "return": "Zwrócono" })[currentAction] }}
                        przedmiot {{ item.name }} <span v-if="item.location && item.location != ''">({{ item.location }})</span>
                    </h1>
                </div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import router from "../router.js";
import { parse } from 'querystring';

export default {
    data() {
        return {
            codeInput: "",

            teacher: null,
            currentAction: null,
            item: null,
            alreadyBorrowed: false,
            borrowedTeacher: null,

            logoutDate: null,
            displayTime: "00:00",
        };
    },

    methods: {
        async processCode(code) {
            this.api.loading = true;

            this.api.clearError();

            try {
                console.log(code);
                var parsed = await this.api.parseCode(code);

                if(parsed.type == "teacher") {
                    this.teacher = this.api.teachers[parsed.id];
                    this.currentAction = null;
                    this.item = null;
                    this.refreshLogoutDate();

                    this.api.loading = false;
                    return;
                }

                if(!this.teacher) {
                    this.api.displayError("Wystąpił błąd", "Najpierw zeskanuj kod nauczyciela.");

                    this.api.loading = false;
                    return;
                }

                if(parsed.type == "borrow" || parsed.type == "return") {
                    this.currentAction = parsed.type;
                    this.item = null;

                    this.api.loading = false;
                    return;
                }

                if(this.currentAction == null) {
                    this.api.displayError("Wystąpił błąd", "Najpierw zeskanuj kod pobrania lub zwrotu.");

                    this.api.loading = false;
                    return;
                }

                if(parsed.type == "item") {
                    this.refreshLogoutDate();

                    this.alreadyBorrowed = parsed.alreadyBorrowed;
                    this.item = await this.api.getItem(parsed.id);
                    
                    if(this.currentAction == "borrow") {
                        if(this.alreadyBorrowed) {
                            this.borrowedTeacher = this.api.teachers[parsed.whoBorrowed];
                        }
                        else {
                            await this.api.addItemEvent(this.item.id, this.teacher.id, this.currentAction);
                        }
                    }
                    else if(this.currentAction == "return") {
                        if(!this.alreadyBorrowed) {
                            
                        }
                        else {
                            await this.api.addItemEvent(this.item.id, this.teacher.id, this.currentAction);
                        }
                    }
                }
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
                this.item = null;
            }

            this.api.loading = false;
        },

        refreshLogoutDate() {
            var minutesToAdd = 2;
            this.logoutDate = new Date(new Date().getTime() + minutesToAdd * 60000);
        }
    },

    mounted() {
        $("#codeInput").keyup(ev => {
            if(ev.keyCode === 13) {
                this.processCode(this.codeInput);
                this.codeInput = "";
                $("#codeInput").select();
            }
        });

        $("#codeInput").select();
    },

    async created() {
        this.api.loading = true;

        await this.api.fetchTeachers();

        this.api.loading = false;

        setInterval(() => {
            if(!this.teacher || !this.logoutDate)
                return;

            var timespan = new Date(this.logoutDate.getTime() - new Date().getTime());

            if(this.logoutDate.getTime() < new Date().getTime()) {
                this.teacher = null;
                return;
            }

            var hours = timespan.getMinutes();
            var minutes = timespan.getSeconds();

            if (hours < 10)
                hours = "0" + hours;
            if (minutes < 10)
                minutes = "0" + minutes;

            this.displayTime = "" + hours + ":" + minutes;
        }, 1000);
    },
};
</script>

<style>
    .logout-text {
        display: block;
        margin-top: 1em;
        margin-bottom: -2em;
        text-align: center;
        color: rgb(179, 179, 179);
    }

    .scan-teacher-code-text {
        text-align: center;
    }
    
    .scan-action-code-text {
        text-align: center;
    }
    
    .scan-item-code-text {
        text-align: center;
    }

    .success-icon-wrapper {
        text-align: center;
    }

    .success-icon {
        margin-top: 100px !important;
        margin-bottom: -80px !important;
        font-size: 180px !important;
        color: #09681d;
    }

    .failed-icon-wrapper {
        text-align: center;
    }

    .failed-icon {
        margin-top: 100px !important;
        margin-bottom: -80px !important;
        font-size: 180px !important;
        color: #720e0e;
    }
</style>
