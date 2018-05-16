<template>
    <div>
        <h1>Dodaj nauczyciela</h1>

        <div v-if="error" class="ui negative message">
            <div class="header">
                Wystąpił problem
            </div>
            <p>
                {{ error }}
            </p>
        </div>

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

            error: null,
        }
    },

    methods: {
        async submit() {
            try {
                var response = await this.api.addTeacher(this.name, this.surname, this.barcode);
            }
            catch(e) {
                console.log(e);
                this.error = e.response.data;
            }
        }
    },

    async created() {

    }
}
</script>

<style>
</style>
