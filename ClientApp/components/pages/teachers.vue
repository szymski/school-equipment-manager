<template>
<div>
    <h1 class="six wide column">Nauczyciele</h1>

    <error-display/>

    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button" @click="goToAddTeacher()">Dodaj nauczyciela</button>
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
                <th class="collapsing">lp.</th>
                <th>Imię i nazwisko</th>
                <th class="collapsing">Niezwrócone&nbsp;przedmioty</th>
                <th>Kod kreskowy</th>
                <th class="collapsing"></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(teacher, index) in filterTeachers(teachers)" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>
                    <i class="ui empty circular label color-id" :style="{ 'background': api.generateColor('teacher' + teacher.id) }"></i>
                    {{ teacher.name }} {{ teacher.surname }}
                </td>
                <td style="text-align:center;">{{ teacher.borrowedItems }}</td>
                <td>{{ teacher.barcode }}</td>
                <td class="single line">
                    <button class="ui tiny green button" @click="goToEditTeacher(teacher.id)">Edytuj</button>
                    <button class="ui tiny primary button" @click="goToBarcodes(teacher.id)">Wyświetl kod</button>
                </td>
            </tr>
        </tbody>
    </table>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            teachers: [ ],
            searchText: "",
        };
    },

    methods: {
        goToAddTeacher() {
            router.push("/add-teacher");
        },

        goToBarcodes(teacherId) {
            router.push("/teacher-barcodes/" + teacherId);
        },

        goToEditTeacher(teacherId) {
            router.push("/edit-teacher/" + teacherId);
        },

        filterTeachers(teachers) {
            return teachers.filter((t) => this.searchText.length == 0 || (t.name + t.surname + t.barcode || "").toLowerCase().includes(this.searchText.toLowerCase()));
        },
    },

    async created() {
        this.api.loading = true;
        let response = await this.$http.get('/api/Teachers')
        this.teachers = response.data;
        this.api.loading = false;
    }
};
</script>

<style>

</style>
