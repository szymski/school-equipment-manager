<template>
<div>
    <h1 class="six wide column">Przedmioty</h1>

    <error-display/>

    <div class="ui grid">
        <div class="four wide column">
            <button class="ui button" @click="goToAddItem">Dodaj przedmiot</button>
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
                <th>Identyfikator</th>
                <th>Nazwa</th>
                <th>Opis</th>
                <th>Uwagi</th>
                <th style="width:auto;">Położenie</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in filterItems(items)" v-bind:key="index">
                <td>{{ index + 1 }}</td>
                <td style="text-align:center;">
                    <div v-if="item.shortId" class="item-short-id">
                        {{ item.shortId }}
                        <button class="ui mini basic icon circular button edit-id-btn" @click="showEnterIdDialog(item)">
                            <i class="pencil icon"></i>
                        </button>
                    </div>
                    <a v-if="!item.shortId || item.shortId == ''" @click="showEnterIdDialog(item)">Dodaj</a>
                </td>
                <td>
                    <div :data-tooltip="item.returned ? null : 'Ten przedmiot nie został jeszcze zwrócony.'">
                        <i v-if="!item.returned" class="exclamation circle icon not-returned-icon"/>
                        <a @click="goToEventList(item.id)">
                            {{ item.name }}
                        </a>
                    </div>
                </td>
                <td>{{ item.description }}</td>
                <td>
                    <div class="editable-property">
                        {{ item.notes }}
                        <button class="ui mini basic icon circular button edit-id-btn" @click="showNotesModal(item)">
                            <i class="pencil icon"></i>
                        </button>
                    </div>
                </td>
                <td>
                    <div class="editable-property">
                        {{ item.location || "Brak" }}
                        <button class="ui mini basic icon circular button edit-id-btn" @click="showNotesModal(item)">
                            <i class="pencil icon"></i>
                        </button>
                    </div>
                </td>
                <td>
                    <button class="ui fluid tiny red button" @click="removeItem(item.id, $event)">Usuń</button>
                </td>
            </tr>
        </tbody>
    </table>

    <div class="ui modal" id="enterIdModal">
        <div class="header">
            Wprowadź identyfikator dla <i>{{ modalItem.name }}</i> (<i>{{ modalItem.location }}</i>)
        </div>
        <div class="content">
            <div class="description">
                <div class="ui header" v-if="modalFirstTime">Ten przedmiot nie posiada jeszcze identyfikatora.</div>
                <p>Zaznacz poniższe pole tekstowe i użyj skanera kodów kreskowych lub wprowadź kod ręcznie.</p>
                <div class="ui form">
                    <div class="field">
                        <input type="text" v-model="modalIdentifier" id="modalIdentifierInput">
                    </div>
                </div>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui positive right labeled icon button" @click="setIdentifier(modalItem, modalIdentifier)" id="modalIdentifierButton">
                Ustaw kod
                <i class="checkmark icon"></i>
            </div>
        </div>
    </div>

    <div class="ui modal" id="notesModal">
        <div class="header">
            Uwagi dla <i>{{ modalItem.name }}</i> (<i>{{ modalItem.location }}</i>)
        </div>
        <div class="content">
            <div class="description">
                <div class="ui form">
                    <div class="field">
                        <textarea v-model="modalNotes" id="notesModalInput"/>
                    </div>
                </div>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui positive right labeled icon button" @click="setNotes(modalItem, modalNotes)" id="notesModalButton">
                Aktualizuj
                <i class="checkmark icon"></i>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import router from "../router.js";

export default {
    data() {
        return {
            items: [ ],
            searchText: "",

            modalItem: { },
            modalIdentifier: "",
            modalFirstTime: false,
        };
    },

    methods: {
        filterItems(items) {
            return items.filter((i) => this.searchText.length == 0 || (i.name + i.description + i.location + i.shortId + i.notes).toLowerCase().includes(this.searchText.toLowerCase()));
        },

        goToAddItem() {
            router.push("/add-item");
        },

        goToEventList(id) {
            router.push("/view-events/" + id);
        },
        
        async removeItem(id, event) {
            $(event.srcElement).addClass("loading");
            await this.$http.post('/api/Items/Remove', "id=" + id);
            let response = await this.$http.get('/api/Items')
            this.items = response.data;
            $(event.srcElement).removeClass("loading");            
        },

        showEnterIdDialog(item) {
            this.modalItem = item;
            this.modalIdentifier = item.shortId || "";
            this.modalFirstTime = !this.modalIdentifier || this.modalIdentifier == "";
            $("#enterIdModal").modal("show");
        },

        showNotesModal(item) {
            this.modalItem = item;
            this.modalNotes = item.notes || "";
            $("#notesModal").modal("show");
        },

        async setIdentifier(item, identifier) {
            try {
                await this.api.updateItemIdentifier(item.id, identifier);
                item.shortId = identifier;
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        },
    },

    computed: {

    },

    mounted() {
        $("#modalIdentifierInput").keyup(ev => {
            console.log(ev.keyCode);
            if(ev.keyCode === 13)
                $("#modalIdentifierButton").click();
        });
    },

    async created() {
        this.api.loading = true;
        let response = await this.$http.get('/api/Items')
        this.items = response.data;
        this.api.loading = false;
    }
};
</script>

<style>
    /* TODO: Move this to a global style */
    a {
        cursor: pointer;
    }

    .item-short-id {
        margin-left: 32px;
    }

    .item-short-id .edit-id-btn {
        opacity: 0;
    }

    .item-short-id:hover .edit-id-btn {
        opacity: 1;
    }

    .editable-property {
    }

    .editable-property .edit-id-btn {
        opacity: 0;
    }

    .editable-property:hover .edit-id-btn {
        opacity: 1;
    }

    .not-returned-icon {
        color: #bb4444;
    }
</style>
