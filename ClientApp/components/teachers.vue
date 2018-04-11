<template>
<div>
    <h1 class="six wide column">Nauczyciele</h1>

    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button">Dodaj nauczyciela</button>
        </div>
        <div class="ui right floated four wide column form">
            <div class="ui icon input" style="width:100%">
                <input type="text" class="" v-model="searchText">
                <i class="search icon"></i>
            </div>
        </div>
    </div>

    <table class="ui celled table">
        <thead>
            <tr>
                <th style="width:1px;">lp.</th>
                <th>Imie</th>
                <th>Nazwisko</th>
                <th>Barcode</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(teacher, index) in filterTeachers(teachers)" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ teacher.name }}</td>
                <td>{{ teacher.surname }}</td>
                <td>{{ teacher.barcode }}</td>
            </tr>
        </tbody>
    </table>
</div>
</template>

<script>
import router from "../router.js";

export default {
    data() {
        return {
            teachers: [ ],
            searchText: "",
        };
    },

    methods: {
        filterTeachers(teachers) {
            return teachers.filter((t) => this.searchText.length == 0 || (t.name + t.surname).toLowerCase().includes(this.searchText.toLowerCase()));
        },
    },

    async created() {
        let response = await this.$http.get('/api/Teachers')
        this.teachers = response.data;
    }
};
</script>

<style>

</style>
