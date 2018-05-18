<template>
    <div>
        <h1>Położenia przedmiotów</h1>

        <error-display/>

        <table class="ui celled table">
            <thead>
                <tr>
                    <th style="width:1px;">lp.</th>
                    <th style="">Nazwa</th>
                    <th style="width:1px;"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in locations" v-bind:key="index">
                    <td>{{ index + 1 }}</td>
                    <td>{{ item.name }}</td>
                    <td>
                        <button class="ui fluid small red button" @click="removeLocation(item.id)">Usuń</button>
                    </td>
                </tr>
            </tbody>
        </table>

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
            this.locations = await this.api.getLocations();
        },
        async addLocation() {
            try {
                await this.$http.post("/api/Locations/Add", { name: this.newLocationName });
                this.locations = await this.api.getLocations();
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        }
    },

    async created() {
        this.locations = await this.api.getLocations();
    }
}
</script>

<style>
</style>
