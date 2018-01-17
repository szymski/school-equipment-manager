<template>
<div>
    <div class="row">
        <h1 class="col-md-6">Przedmioty</h1>
        <div class="col-md-3 col-center-items">
            <input type="text" class="form-control mr-0" v-model="searchText">
        </div>
        <div class="col-md-3 col-center-items">
            <button class="btn btn-primary btn-block add-item-btn" @click="goToAddItem">Dodaj przedmiot</button>
        </div>
    </div>

    <table class="table">
        <thead>
            <tr>
                <th style="width:1px;">lp.</th>
                <th style="width:1px;">Identyfikator</th>
                <th>Nazwa</th>
                <th>Opis dodatkowy</th>
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
                <td>{{ item.location }}</td>
                <td>
                    <button class="btn btn-danger btn-sm" @click="removeItem(item.id)">Usuń</button>
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
            this.$http.post('/api/Items/Remove', "id=" + id);
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
