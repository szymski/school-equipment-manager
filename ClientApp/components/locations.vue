<template>
    <div>
        <h1>Lokacje</h1>

        <table class="ui celled table">
            <thead>
                <tr>
                    <th style="width:1px;">lp.</th>
                    <th style="">Nazwa</th>
                    <th style="width:1px;"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in locations" v-bind:key="item.id">
                    <td>0</td>
                    <td>{{ item.name }}</td>
                    <td>
                        <button class="ui fluid small red button" @click="removeLocation(item.id)">Usuń</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- <div class="ui grid">
            <input class="col-7 form-control" style="display:inline;" type="text" v-model="newLocationName"/>
            <div class="col-1"></div>
            <button class="col-4 btn btn-primary" style="display:inline;" @click="addLocation">Dodaj</button>
        </div> -->
        <div class="ui action fluid input">
            <input style="" type="text" v-model="newLocationName"/>
            <button class="ui button" style="display:inline;" @click="addLocation">Dodaj</button>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            locations: [ ],
            newLocationName: "Nowa lokalizacja",
        }
    },

    methods: {
        async removeLocation(id) {
            await this.$http.post("/api/Locations/Remove", { id: id });
            var response = await this.$http.get("/api/Locations");
            this.locations = response.data;
        },
        async addLocation() {
            await this.$http.post("/api/Locations/Add", { name: this.newLocationName });
            var response = await this.$http.get("/api/Locations");
            this.locations = response.data;
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
