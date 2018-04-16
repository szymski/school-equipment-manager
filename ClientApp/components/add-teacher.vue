<template>
    <div>
        <h1>Dodaj nauczyciela</h1>

        <div class="ui form">
            <div class="field">
                <label>Imie</label>
                <input type="text" v-model="name">
            </div>
            <div class="field">
                <label>Nazwisko</label>
                <input type="text" v-model="surname">
            </div>
            <div class="field">
                <label>BarCode</label>
                <input type="text" v-model="barcode">
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
            name: "",
            surname: "",
            barcode: "",
        }
    },

    methods: {
        async submit() {
            await this.$http.post("/api/Teachers/Add", { name: this.name, surname: this.surname, barcode: this.barcode });
            //router.push("/teachers");
        }
    },

    async created() {

    }
}
</script>

<style>
</style>
