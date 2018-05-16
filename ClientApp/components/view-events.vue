<template>
<div>
    <h1 class="six wide column">Historia przedmiotu - {{ item.name }} ({{ item.location != "" ? item.location : "Brak lokalizacji" }})</h1>

    <div class="ui grid">
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
                <th>Teacher</th>
                <th>Data</th>
                <th>Type</th>
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
            
            searchText: "",
        };
    },

    mounted() {
        
    },

    async created() {
        await this.api.fetchTeachers();
        this.item = await this.api.getItem(this.itemId);

        let response = await this.$http.get('/api/Items/Events?id=' + this.itemId);
        response.data.forEach(event => {
            console.log(this.api.teachers[event.teacher]);
            event.teacherName = this.api.teachers[event.teacher].name + " " + this.api.teachers[event.teacher].surname;
            console.log(event.teacherName);
        });
        this.events = response.data;
    }
};
</script>

<style>

</style>
