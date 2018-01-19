<template>
    <div>
        <h1>Dodaj przedmiot</h1>

        <div class="form-group">
            <label>Nazwa przedmiotu</label>
            <input class="form-control" type="text" v-model="name">
        </div>

        <div class="form-group">
            <label>Opis przedmiotu</label>
            <input class="form-control" type="text" v-model="description">
        </div>

        <div class="form-group">
            <label>Lokalizacja</label>            
            <select v-model="location" class="custom-select my-1 mr-sm-2" id="inlineFormCustomSelectPref">
                <option selected value="0">Wybiorę później</option>
                <option v-for="item in locations" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
            </select>
        </div>

        <button class="btn btn-primary float-right" @click="submit">Dodaj</button>
    </div>
</template>

<script>
import router from "../router.js";

export default {
    data() {
        return {
            name: "",
            description: "",
            location: 0,
            locations: [],
        }
    },

    methods: {
        async submit() {
            await this.$http.post("/api/Items/Add", { name: this.name, description: this.description, location: this.location });
            router.push("/items");
        }
    },

    async created() {
        var response = await this.$http.get("/api/Locations");
        this.locations = response.data;
    }
}
</script>

<style>
</style>
