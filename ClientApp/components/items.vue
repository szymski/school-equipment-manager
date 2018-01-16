<template>
<div>
    <h1>Jakieś itemy</h1>

    <table class="table">
        <thead>
            <tr>
                <th style="width:1px;">lp.</th>
                <th style="width:1px;">Identyfikator</th>
                <th>Nazwa</th>
                <th>Opis dodatkowy</th>
                <th style="width:1px;">Położenie</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in items" v-bind:key="item">
                <td>0</td>
                <td>{{ item.id }}</td>
                <td>{{ item.name }}</td>
                <td>{{ item.description }}</td>
                <td>{{ item.location }}</td>
                <td>
                    <button class="btn btn-danger btn-sm">Usuń</button>
                </td>
            </tr>
        </tbody>
    </table>
</div>
</template>

<script>
export default {
    data() {
        return {
            items: [ ],
        };
    },

    async created() {
        let response = await this.$http.get('/api/Items')
        this.items = response.data;
    }
};
</script>

<style>

</style>
