<template>
    <div>
        <h1>Dodaj Template</h1>

        <div class="form-group">
            <label>Template Name</label>
            <input class="form-control" type="text" v-model="name">
        </div>

        <div class="form-group">
            <label>Template Description</label>
            <input class="form-control" type="text" v-model="description">
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
        }
    },

    methods: {
        async submit() {
            await this.$http.post("/api/ItemTemplateController/Add", { name: this.name, description: this.description});
            router.push("/ItemTemplateController");
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
