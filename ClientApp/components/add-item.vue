<template>
    <div>
        <h1>Dodaj przedmiot</h1>

        <error-display/>

        <div class="ui form">
            <div class="field">
                <label>Typ przedmiotu</label>            
                <select v-model="template" class="ui dropdown" id="inlineFormCustomSelectPref">
                    <option selected value="0">Wybiorę później</option>
                    <option v-for="item in templates" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
                </select>
            </div>

            <div class="field">
                <label>Lokalizacja</label>            
                <select v-model="location" class="ui dropdown" id="inlineFormCustomSelectPref">
                    <option selected value="0">Wybiorę później</option>
                    <option v-for="item in locations" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
                </select>
            </div>

            <div class="field">
                <label>Uwagi</label>
                <input class="form-control" type="text" v-model="description">
            </div>

            <div class="two wide field">
                <label>Liczba</label>
                <input type="number" value="1" v-model="number">
            </div>

            <button class="ui primary button" @click="submit">Dodaj</button>
        </div>
    </div>
</template>

<script>
import router from "../router.js";

export default {
    data() {
        return {
            description: "",
            location: 0,
            locations: [],
            template: 0,
            templates: [],
            number: 1,
        }
    },

    methods: {
        async submit() {
            await this.$http.post("/api/Items/Add", { notes: this.description, location: this.location, template: this.template, number: this.number });
            router.push("/items");
        }
    },

    mounted() {
        $('.ui.dropdown').dropdown();
    },

    async created() {
        var response = await this.$http.get("/api/Locations");
        this.locations = response.data;

        var response = await this.$http.get("/api/ItemTemplates");
        this.templates = response.data;
    }
}
</script>

<style>
</style>
