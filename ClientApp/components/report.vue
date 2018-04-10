<template>
<div>
    <h1 class="six wide column">Raporty</h1>
    <p>Tutaj możesz wygenerować raport zawierający wszystkie przedmioty.</p>
    
    <table class="ui celled table">
        <thead>
            <tr>
                <th>Lokalizacja</th>
                <th>Nazwa przedmiotu</th>
                <th>Liczba</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in items" v-bind:key="item.id">
                <td>{{ item.location }}</td>
                <td>{{ item.description }}</td>
                <td>{{ item.notes }}</td>
                <td>{{ item.location }}</td>
                <td>
                    <button class="ui fluid tiny red button" @click="removeItem(item.id)">Usuń</button>
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
            items: [
                {
                    id: 0,
                    location: "Jakaś tam sala",
                    items: [
                        {
                            name: "Myszek",
                            quantity: 10,
                        }
                    ]
                }
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
