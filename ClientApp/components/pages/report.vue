<template>
<div>
    <h1 class="six wide column">Raporty</h1>
    <p>Wybierz rodzaj raportu z poniższej listy.</p>
    
    <button class="ui primary button">Wyświetl niezwrócone przedmioty</button>
    <button class="ui primary button">Wyświetl przedmioty z danej sali</button>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            items: [
                {
                    id: 0,
                    location: "Jakaś tam sala",
                    name: "Myszek",
                    quantity: 10,
                },
            ],
        };
    },

    methods: {

    },

    computed: {

    },

    mounted() {
        
    },

    async created() {
        // let response = await this.$http.get('/api/Items')
        // this.items = response.data;
    }
};
</script>

<style>

</style>
