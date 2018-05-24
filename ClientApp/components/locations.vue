<template>
    <div>
        <h1>Położenia przedmiotów</h1>

        <error-display/>

        <table class="ui celled table">
            <thead>
                <tr>
                    <th class="collapsing">lp.</th>
                    <th style="">Nazwa</th>
                    <th class="collapsing">Użyto</th>                    
                    <th class="collapsing"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in locations" v-bind:key="index">
                    <td>{{ index + 1 }}</td>
                    <td>{{ item.name }}</td>
                    <td style="text-align:center;">{{ item.useCount }}</td>
                    <td>
                        <button class="ui fluid small red button" @click="removeLocation(item.id, $event)">Usuń</button>
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
        async removeLocation(id, event) {
            $(event.srcElement).addClass("loading");
            await this.$http.post("/api/Locations/Remove", { id: id });
            this.locations = await this.api.getLocations();
            $(event.srcElement).removeClass("loading");
        },
        async addLocation() {
            this.api.clearError();

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
        this.api.loading = true;
        this.locations = await this.api.getLocations();
        this.api.loading = false;
    }
}
</script>

<style>
</style>
