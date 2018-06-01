<template>
<div>
    <h1 class="six wide column">Historia przedmiotu - {{ item.name }} ({{ item.location != "" ? item.location : "Brak lokalizacji" }})</h1>

    <error-display/>

    <div class="ui grid">
        <div class="ui eight wide column">
            <span v-if="!returned" class="not-returned-text"><i class="exclamation circle icon"/>Przedmiot nie został jeszcze zwrócony</span>
        </div>
        <div class="ui right floated four wide column form">
            <div class="ui icon input" style="width:100%">
                <input type="text" class="">
                <i class="search icon"></i>
            </div>
        </div>
    </div>

    <table class="ui celled table">
        <thead>
            <tr>
                <th class="collapsing">lp.</th>
                <th>Nauczyciel</th>
                <th>Data</th>
                <th>Typ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in [].concat(events).reverse()" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ item.teacherName }}</td>
                <td>{{ item.date }}</td>
                <td>{{ ({ borrow: "Pobrano", return: "Zwrócono" })[item.type] }}</td>
            </tr>
        </tbody>
    </table>

    <div v-if="api.useDevVersion" class="ui segment">
        <h3>Dodawanie zdarzeń</h3>
        <div class="ui form">
            <div class="field">
                <label>Nauczyciel</label>
                <select v-model="teacherId">
                    <option v-for="(teacher, index) in api.teachers" :key="index" :value="teacher.id" default>
                        {{ teacher.name }} {{ teacher.surname }}
                    </option>
                </select>
            </div>
            <div class="field">
                <label>Typ zdarzenia</label>
                <select v-model="type">
                    <option value="borrow">Pobrano</option>
                    <option value="return">Zwrócono</option>
                </select>
            </div>
            <button class="ui primary button" :class="{ 'loading': adding }" @click="addEvent">Dodaj zdarzenie</button>
        </div>
    </div>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            itemId: this.$route.params.id,
            item: {
                name: "No name",
            },

            events: {
                
            },
            
            returned: true,
            
            searchText: "",

            teacherId: 0,
            type: "borrow",
            adding: false,
        };
    },

    methods: {
        async reload() {
            var data = await this.api.getEvents(this.itemId);
            data.forEach(event => {
                if(event.teacher == -1)
                    event.teacherName = "Usunięto";
                else {
                    var teacher = this.api.teachers[event.teacher];
                    event.teacherName = teacher.name + " " + teacher.surname;
                }
            });
            this.events = data;

            this.returned = !this.api.isItemBorrowed(this.events);
        },

        async addEvent() {
            if(this.adding)
                return;

            this.adding = true;

            try {
                await this.api.addItemEvent(this.itemId, this.teacherId, this.type);
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }

            await this.reload();
            this.adding = false;
        },
    },

    mounted() {
        
    },

    async created() {
        this.api.loading = true;

        await this.api.fetchTeachers();
        this.item = await this.api.getItem(this.itemId);
        this.teacherId = Object.keys(this.api.teachers)[0];

        await this.reload();

        this.api.loading = false;
    },
};
</script>

<style>
    .not-returned-text {
        display: inline;
        color: #bb4444;
        font-size: 18px;
    }
</style>
