<template>
<div>
    <h1 class="six wide column">Typy przedmiotów</h1>
    
    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button" @click="goToAddTemplate">Dodaj typ</button>
        </div>
        <div class="ui right floated four wide column form">
            <div class="ui icon input" style="width:100%">
                <input type="text" class="" v-model="searchText">
                <i class="search icon"></i>
            </div>
        </div>
    </div>

    <table class="ui celled table">
        <thead>
            <tr>
                <th class="collapsing">lp.</th>
                <th>Nazwa</th>
                <th>Opis dodatkowy</th>
                <th class="collapsing">Użyto</th>
                <th class="collapsing"></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(itemTemplate, index) in filterItems(items)" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td>
                    <i class="ui empty circular label color-id" :style="{ 'background': api.generateColor('template' + itemTemplate.id) }"></i>
                    {{ itemTemplate.name }}
                </td>
                <td>{{ itemTemplate.description }}</td>
                <td style="text-align:center;">{{ itemTemplate.useCount }}</td>
                <td>
                    <button class="ui fluid small red button" @click="tryRemoveItem(itemTemplate, $event)">Usuń</button>
                </td>
            </tr>
        </tbody>
    </table>

    <!-- Removal confirmation dialog -->
    <div class="ui modal" id="removeModal">
        <div class="header">
            Czy na pewno chcesz usunąć typ <i>{{ modalTemplate.name }}</i>?
        </div>
        <div class="content">
            <div class="description">
                <p>
                    Ten typ przedmiotu został użyty {{ modalTemplate.useCount }} {{ modalTemplate.useCount == 1 ? "raz" : "razy" }}.
                    Po usunięciu przedmioty używające tego typu nie będą miały żadnego typu.
                </p>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui deny red right button" @click="removeItem(modalTemplate)">
                Usuń
            </div>
        </div>
    </div>
</div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            items: [ ],
            searchText: "",

            modalTemplate: {
                id: 0,
                name: "",
                useCount: 0,
            }
        };
    },

    methods: {
        filterItems(itemsT) {
            return itemsT.filter((iT) => this.searchText.length == 0 || (iT.name + iT.description + iT.location).toLowerCase().includes(this.searchText.toLowerCase()));
        },
        
        goToAddTemplate() {
            router.push("/add-template");
        },

        async tryRemoveItem(item, event) {
            if(item.useCount == 0)
                await this.removeItem(item, event);
            else {
                this.modalTemplate = item;
                $("#removeModal").modal("show");
            }
        },

        async removeItem(item, event) {
            if(event)
                $(event.srcElement).addClass("loading");

            await this.$http.post('/api/ItemTemplates/Remove/' + item.id);
            let response = await this.$http.get('/api/ItemTemplates')
            this.items = response.data;

            if(event)
                $(event.srcElement).removeClass("loading");
        }
    },

    computed: {
        
    },

    async created() {
        this.api.loading = true;
        let response = await this.$http.get('/api/ItemTemplates')
        this.items = response.data;
        this.api.loading = false;
    }
};
</script>

<style>

</style>
