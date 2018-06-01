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
                        <button class="ui fluid small red button" @click="tryRemoveLocation(item, $event)">Usuń</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <div class="ui action fluid input">
            <input style="" type="text" v-model="newLocationName"/>
            <button class="ui button" style="display:inline;" @click="addLocation">Dodaj</button>
        </div>

        <!-- Removal confirmation dialog -->
        <div class="ui modal" id="removeModal">
            <div class="header">
                Czy na pewno chcesz usunąć położenie <i>{{ modalLocation.name }}</i>?
            </div>
            <div class="content">
                <div class="description">
                    <p>
                        To położenie zostało użyte {{ modalLocation.useCount }} {{ modalLocation.useCount == 1 ? "raz" : "razy" }}.
                        Po usunięciu przedmioty używające tego położenia nie będą miały żadnego położenia.
                    </p>
                </div>
            </div>
            <div class="actions">
                <div class="ui deny button">
                    Anuluj
                </div>
                <div class="ui deny red right button" @click="removeLocation(modalLocation)">
                    Usuń
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            locations: [ ],
            newLocationName: "Nowa lokalizacja",

            modalLocation: {
                id: 0,
                useCount: 0,
                name: "",
            },
        }
    },

    methods: {
        async tryRemoveLocation(location, event) {
            if(location.useCount == 0)
                await this.removeLocation(location, event);
            else {
                this.modalLocation = location;
                $("#removeModal").modal("show");
            }
        },

        async removeLocation(location, event) {
            if(event)
                $(event.srcElement).addClass("loading");

            await this.$http.post("/api/Locations/Remove", { id: location.id });
            this.locations = await this.api.getLocations();

            if(event)
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
