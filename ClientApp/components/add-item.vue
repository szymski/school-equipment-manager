<template>
    <div>
        <h1>Dodaj przedmiot</h1>

        <error-display/>

        <div class="ui form">
            <div class="field">
                <label>Typ przedmiotu</label>            
                <select v-model="template" class="ui dropdown">
                    <option selected value="0">Wybiorę później</option>
                    <option v-for="item in templates" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
                </select>
            </div>

            <div class="field">
                <label>Lokalizacja</label>            
                <select v-model="location" class="ui dropdown">
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
                <input type="number" value="1" v-model="number" min="1">
            </div>

            <button class="ui primary button" @click="submit" :class="{ 'disabled': !canSubmit() }">Dodaj</button>
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
        canSubmit() {
            return this.number > 0;
        },

        async submit() {
            try {
                await this.$http.post("/api/Items/Add", { notes: this.description, location: this.location, template: this.template, number: this.number });
                router.push("/items");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
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
