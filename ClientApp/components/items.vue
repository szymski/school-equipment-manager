<template>
<div>
    <h1 class="six wide column">Przedmioty</h1>

    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button" @click="goToAddItem">Dodaj przedmiot</button>
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
                <th style="width:1px;">Identyfikator</th>
                <th>Nazwa</th>
                <th>Opis</th>
                <th>Uwagi</th>
                <th style="width:1px;">Położenie</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in filterItems(items)" v-bind:key="item.id">
                <td>0</td>
                <td>{{ item.id }}</td>
                <td>{{ item.name }}</td>
                <td>{{ item.description }}</td>
                <td>{{ item.notes }}</td>
                <td>{{ item.location }}</td>
                <td>
                    <button class="ui fluid tiny red button" @click="removeItem(item.id)">Usuń</button>
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
        filterItems(items) {
            return items.filter((i) => this.searchText.length == 0 || (i.name + i.description + i.location).toLowerCase().includes(this.searchText.toLowerCase()));
        },
        goToAddItem() {
            router.push("/add-item");
        },
        async removeItem(id) {
            await this.$http.post('/api/Items/Remove', "id=" + id);
            let response = await this.$http.get('/api/Items')
            this.items = response.data;
        }
    },

    computed: {
        
    },

    async created() {
        let response = await this.$http.get('/api/Items')
        this.items = response.data;
    }
};
</script>

<style>

</style>
