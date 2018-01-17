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
        submit() {
            this.$http.post("/api/Items/Add", { name: this.name, description: this.description });
            router.push("/items");
        }
    }
}
</script>

<style>
</style>
