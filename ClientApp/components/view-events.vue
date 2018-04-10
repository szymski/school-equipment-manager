<template>
<div>
    <h1 class="six wide column">Historia</h1>

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
            <tr v-for="item in items" v-bind:key="item">
                <td>0</td>
                <td>{{ item.teacher }}</td>
                <td>{{ item.date }}</td>
                <td>{{ item.type }}</td>
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
            itemId: this.$route.params.id, // TODO : <- szymek dokoncz

            items: [ ],
            searchText: "",

            modalItem: { },
            modalIdentifier: "",
        };
    },

    mounted() {
        
    },

    async created() {
        let response = await this.$http.get('/api/Items/Events?id=' + this.itemId);
        this.items = response.data;
        console.log(this.items);
    }
};
</script>

<style>

</style>
