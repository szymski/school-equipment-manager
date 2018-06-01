<template>
    <div>
        <h1>Dodaj typ przedmiotu</h1>

        <error-display/>

        <div class="ui form">
            <div class="field">
                <label>Nazwa typu</label>
                <input type="text" v-model="name">
            </div>

            <div class="field">
                <label>Opis typu</label>
                <input type="text" v-model="description">
            </div>

            <button class="ui primary button" @click="submit">Dodaj</button>
        </div>
    </div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            name: "",
            description: "",
        }
    },

    methods: {
        async submit() {
            this.api.clearError();

            try {
                await this.$http.post("/api/ItemTemplates/Add", { name: this.name, description: this.description});
                router.push("/item-templates");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        }
    },

    async created() {
        
    }
}
</script>

<style>
</style>
