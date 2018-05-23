<template>
<div>
    <div class="ui segment">
        <button class="ui button" @click="processCode('KAR-WOJ-592479')">Nauczyciel</button>
        <button class="ui button" @click="processCode('POBR')">Pobranie</button>
        <button class="ui button" @click="processCode('ZWROT')">Zwrot</button>
        <button class="ui button" @click="processCode('974050523.83391')">Przedmiot</button>
    </div>

    <error-display/>

    <div class="ui form">
        <input type="text" id="codeInput" v-model="codeInput">
    </div>

    <div v-if="!teacher">
        <h2 class="scan-teacher-code-text">Zeskanuj kod nauczyciela</h2>
    </div>

    <div v-if="teacher">
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
                        Przedmiot {{ item.name }} już został pobrany przez nauczyciela {{ borrowedTeacher.name }} {{ borrowedTeacher.surname }}
                    </h1>
                </div>
                <div v-else-if="!alreadyBorrowed && currentAction == 'return'">
                    <div class="failed-icon-wrapper">
                        <i class="times circle outline icon failed-icon"/>
                    </div>
                    <h1 class="scan-item-code-text">
                        Przedmiot {{ item.name }} nie został wcześniej pobrany
                    </h1>
                </div>
                <div v-else>
                    <div class="success-icon-wrapper">
                        <i class="check circle outline icon success-icon"/>
                    </div>
                    <h1 class="scan-item-code-text">
                        {{ ({ "borrow": "Pobrano", "return": "Zwrócono" })[currentAction] }}
                        przedmiot {{ item.name }}
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
            teacher: null,
            currentAction: null,
            item: null,
            alreadyBorrowed: false,
            borrowedTeacher: null,

            codeInput: "",
        };
    },

    methods: {
        async reload() {
            
        },

        async addEvent() {
            
        },

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
    },

    mounted() {
        $("#codeInput").keyup(ev => {
            if(ev.keyCode === 13) {
                this.processCode(this.codeInput);
                this.codeInput = "";
                $("#codeInput").select();
            }
        });
    },

    async created() {
        this.api.loading = true;

        await this.api.fetchTeachers();

        this.api.loading = false;
    },
};
</script>

<style>
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
