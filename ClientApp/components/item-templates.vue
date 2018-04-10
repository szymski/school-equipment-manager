<template>
<div>
    <h1 class="six wide column">Typy przedmiotów</h1>
    
    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button" @click="goToAddTemplate">Dodaj typ</button>
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
                <th>Nazwa</th>
                <th>Opis dodatkowy</th>
                <th style="width:1px;"></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(itemTemplate, index) in filterItems(items)" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>{{ itemTemplate.name }}</td>
                <td>{{ itemTemplate.description }}</td>
                <td>
                    <button class="ui fluid small red button" @click="removeItem(itemTemplate.id)">Usuń</button>
                </td>
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
            items: [ ],
            searchText: "",
        };
    },

    methods: {
        filterItems(itemsT) {
            return itemsT.filter((iT) => this.searchText.length == 0 || (iT.name + iT.description + iT.location).toLowerCase().includes(this.searchText.toLowerCase()));
        },
        goToAddTemplate() {
            router.push("/add-template");
        },
        async removeItem(id) {
            await this.$http.post('/api/ItemTemplates/Remove', "id=" + id);
            let response = await this.$http.get('/api/ItemTemplates')
            this.items = response.data;
        }
    },

    computed: {
        
    },

    async created() {
        let response = await this.$http.get('/api/ItemTemplates')
        this.items = response.data;
    }
};
</script>

<style>

</style>
