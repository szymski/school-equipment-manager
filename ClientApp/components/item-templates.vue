<template>
<div>
    <div class="row">
        <h1 class="col-md-6">Templates</h1>
        <div class="col-md-3 col-center-items">
            <input type="text" class="form-control mr-0" v-model="searchText">
        </div>
        <div class="col-md-3 col-center-items">
            <button class="btn btn-primary btn-block add-item-btn" @click="goToAddItem">Dodaj Template</button>
        </div>
    </div>

    <table class="table">
        <thead>
            <tr>
                <th style="width:1px;">lp.</th>
                <th>Nazwa</th>
                <th>Opis dodatkowy</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="itemTemplate in filterItems(items)" v-bind:key="itemTemplate.id">
                <td>{{ itemTemplate.id }}</td>
                <td>{{ itemTemplate.name }}</td>
                <td>{{ itemTemplate.description }}</td>
                <td>
                    <button class="btn btn-danger btn-sm" @click="removeItem(itemTemplate.id)">Usuń</button>
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
        goToAddItem() {
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
