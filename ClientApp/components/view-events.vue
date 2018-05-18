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
                <th style="width:1px;">lp.</th>
                <th>Nauczyciel</th>
                <th>Data</th>
                <th>Typ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in events" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ item.teacherName }}</td>
                <td>{{ item.date }}</td>
                <td>{{ item.type }}</td>
            </tr>
        </tbody>
    </table>

    <div class="ui segment">
        <h3>Dodawanie zdarzeń</h3>
        <div class="ui form">
            <div class="field">
                <label>Nauczyciel</label>
                <select>
                    <option v-for="(item, index) in api.teachers" :key="index">
                        {{ item.name }} {{ item.surname }}
                    </option>
                </select>
            </div>
            <div class="field">
                <label>Typ zdarzenia</label>
                <select>
                    <option value="borrowed">Pobrano</option>
                    <option value="returned">Zwrócono</option>
                </select>
            </div>
            <button class="ui primary button" @click="addEvent">Dodaj zdarzenie</button>
        </div>
    </div>
</div>
</template>

<script>
import router from "../router.js";

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
        };
    },

    methods: {
        async addEvent() {

        },
    },

    mounted() {
        
    },

    async created() {
        await this.api.fetchTeachers();
        this.item = await this.api.getItem(this.itemId);

        var data = await this.api.getEvents(this.itemId);
        data.forEach(event => {
            event.teacherName = this.api.teachers[event.teacher].name + " " + this.api.teachers[event.teacher].surname;
        });
        this.events = data;

        this.returned = !this.api.isItemBorrowed(this.events);
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
