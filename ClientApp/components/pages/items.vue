<template>
<div>
    <h1 class="six wide column">Wyposażenie</h1>

    <error-display/>

    <div class="ui grid">
        <div class="ten wide column">
            <button v-if="api.isMod" class="ui button" @click="goToAddItem">Dodaj przedmiot</button>

            <template v-if="!selecting">
                <button v-if="api.isMod" class="ui button" @click="startSelecting" data-tooltip="Pozwala szybko uzupełnić identyfikatory wielu przedmiotów na raz.">Uzupełnij identyfikatory</button>
            </template>
            <template v-else>
                <button v-if="api.isMod" class="ui primary button" @click="goToCompletionPage">Uzupełnij identyfikatory</button>
                <button v-if="api.isMod" class="ui button" @click="stopSelecting">Anuluj</button>
            </template>
        </div>
        <div class="ui right floated four wide column form">
            <div class="ui icon input" style="width:100%">
                <input type="text" placeholder="Szukaj" v-model="searchText">
                <i class="search icon"></i>
            </div>
        </div>
    </div>

    <!-- Needed: type, location, with/without barcode, returned/unreturned -->

    <div class="ui divider"></div>

    <div class="ui form">
        <div class="four fields">
            <div class="field">
                <select class="ui dropdown" v-model="filterIdentifier">
                    <option value="all" selected>Z identyfikatorem lub bez</option>
                    <option value="with">
                        <i class="check icon"></i>
                        Z identyfikatorem
                    </option>
                    <option value="without">
                        <i class="times icon"></i>
                        Bez identyfikatora
                    </option>
                </select>
            </div>
            <div class="field">
                <select class="ui dropdown" v-model="filterTemplate">
                    <option value="all" selected>Dowolny typ</option>
                    <option value="none">
                        <i class="times icon"></i>
                        Brak typu
                    </option>
                    <option v-for="(item, index) in templates" :key="index" :value="item.id">
                        <i class="ui empty circular label color-id" :style="{ 'background': api.generateColor('template' + item.id) }"></i>
                        {{ item.name }}
                    </option>
                </select>
            </div>
            <div class="field">
                <select class="ui dropdown" v-model="filterLocation">
                    <option value="all" selected>Dowolne położenie</option>
                    <option value="none">
                        <i class="times icon"></i>
                        Brak położenia
                    </option>
                    <option v-for="(item, index) in locations" :key="index" :value="item.id">
                        <i class="ui empty circular label color-id" :style="{ 'background': api.generateColor('location' + item.id) }"></i>
                        {{ item.name }}
                    </option>
                </select>
            </div>
            <div class="field">
                <select class="ui dropdown" v-model="filterState">
                    <option value="all" selected>Przedmioty zwrócone oraz niezwrócone</option>
                    <option value="returned">
                        <i class="check icon"></i>
                        Przedmioty zwrócone
                    </option>
                    <option value="borrowed">
                        <i class="times icon"></i>
                        Przedmioty niezwrócone
                    </option>
                    <option value="borrowed-self">
                        <i class="times icon"></i>
                        Przedmioty niezwrócone przez ciebie
                    </option>
                </select>
            </div>
        </div>
    </div>

    <table class="ui celled table">
        <thead>
            <tr>
                <th class="collapsing" v-if="selecting"></th>
                <th class="collapsing">lp.</th>
                <th>Identyfikator</th>
                <th>Typ</th>
                <th>Opis</th>
                <th>Uwagi</th>
                <th class="collapsing">Położenie</th>
                <th v-if="api.isMod" class="collapsing"></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in filterItems(items)" v-bind:key="index">
                <td v-if="selecting">
                    <div class="ui fitted checkbox">
                        <input type="checkbox" v-model="item.selected">
                        <label></label>
                    </div>
                </td>
                <td>
                    {{ index + 1 }}
                </td>
                <td style="text-align:center;">
                    <div v-if="item.shortId" class="item-short-id single line">
                        {{ item.shortId }}
                        <button v-if="api.isMod" class="ui mini basic icon circular button edit-id-btn" @click="showEnterIdDialog(item)">
                            <i class="pencil icon"></i>
                        </button>
                    </div>
                    <a v-if="!item.shortId || item.shortId == ''" @click="showEnterIdDialog(item)"><b>Dodaj</b></a>
                </td>
                <td>
                    <div :data-tooltip="item.returned ? null : 'Ten przedmiot nie został jeszcze zwrócony.'">
                        <div class="editable-property single line" v-if="item.name && item.name != ''">
                            <i v-if="!item.returned" class="exclamation circle icon not-returned-icon"/>
                            <i class="ui empty circular label color-id" :style="{ 'background': api.generateColor('template' + item.template) }"></i>
                            <a @click="goToEventList(item.id)">
                                {{ item.name }}
                            </a>
                            <button v-if="api.isMod" class="ui mini basic icon circular button edit-id-btn" @click="showTemplateModal(item)">
                                <i class="pencil icon"></i>
                            </button>
                        </div>
                        <div class="centered" v-else>
                            <b>
                                <i v-if="!item.returned" class="exclamation circle icon not-returned-icon"/>
                                <a @click="showTemplateModal(item)">
                                    Ustaw typ
                                </a>
                            </b>
                        </div>
                    </div>
                </td>
                <td>{{ item.description }}</td>
                <td>
                    <div class="editable-property single line">
                        <div v-if="!areNotesTooBigToDisplay(item.notes)">
                            {{ item.notes }}
                            <button v-if="api.isMod" class="ui mini basic icon circular button edit-id-btn" @click="showNotesModal(item)">
                                <i class="pencil icon"></i>
                            </button>
                        </div>
                        <div class="centered" v-else>
                            <a @click="showNotesModal(item)">Kliknij aby wyświetlić</a>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="editable-property single line">
                        <i v-if="item.location != 0" class="ui empty circular label color-id" :style="{ 'background': api.generateColor('location' + item.location) }"></i>
                        {{ item.location == 0 ? "Brak" : item.locationName }}
                        <button v-if="api.isMod" class="ui mini basic icon circular button edit-id-btn" @click="showLocationModal(item)">
                            <i class="pencil icon"></i>
                        </button>
                    </div>
                </td>
                <td v-if="api.isMod">
                    <button class="ui fluid tiny red button" @click="removeItem(item.id, $event)">Usuń</button>
                </td>
            </tr>
        </tbody>
    </table>

    <!-- Identifier modal -->
    <div class="ui modal" id="enterIdModal">
        <div class="header">
            Wprowadź identyfikator dla <i>{{ modalItem.name }}</i> <template v-if="modalItem.locationName && modalItem.locationName != ''">(<i>{{ modalItem.locationName }}</i>)</template>
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

    <!-- Notes modal -->
    <div class="ui modal" id="notesModal">
        <div class="header">
            Uwagi dla <i>{{ modalItem.name }}</i> <template v-if="modalItem.locationName && modalItem.locationName != ''">(<i>{{ modalItem.locationName }}</i>)</template>
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

    <!-- Template modal -->
    <div class="ui modal" id="templateModal">
        <div class="header">
            Zmień typ przedmiotu
        </div>
        <div class="content">
            <div class="description">
                <div class="ui form">
                    <div class="field">
                        <select v-model="modalTemplate" class="ui dropdown" id="templateSelect">
                            <option value="0">Brak</option>
                            <option v-for="item in templates" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui positive right labeled icon button" @click="setItemTemplate(modalItem, modalTemplate)" id="templateModalButton">
                Aktualizuj
                <i class="checkmark icon"></i>
            </div>
        </div>
    </div>

    <!-- Location modal -->
    <div class="ui modal" id="locationModal">
        <div class="header">
            Zmień położenie przedmiotu
        </div>
        <div class="content">
            <div class="description">
                <div class="ui form">
                    <div class="field">
                        <select v-model="modalLocation" class="ui dropdown" id="locationSelect">
                            <option value="0">Brak</option>
                            <option v-for="item in locations" v-bind:key="item.id" v-bind:value="item.id">{{ item.name }}</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui positive right labeled icon button" @click="setItemLocation(modalItem, modalLocation)" id="locationModalButton">
                Aktualizuj
                <i class="checkmark icon"></i>
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
            templates: [ ],
            locations: { },
            searchText: "",

            modalItem: { },
            modalIdentifier: "",
            modalFirstTime: false,

            filterIdentifier: "all",
            filterTemplate: "all",
            filterLocation: "all",
            filterState: this.$route.params.filterState || "all",

            modalNotes: "",

            modalTemplate: 0,

            modalLocation: 0,

            selecting: false,
        };
    },

    methods: {
        filterItems(items) {
            return items.filter((i) => {
                var show = true;

                if(this.filterIdentifier == "with") {
                    if(!i.shortId || i.shortId == "")
                        return false;
                }
                else if(this.filterIdentifier == "without") {
                    if(i.shortId && i.shortId != "")
                        return false;
                }

                if(this.filterTemplate == "none") {
                    if(i.template != 0)
                        return false;
                }
                else if(this.filterTemplate != "all") {
                    if(i.template != this.filterTemplate)
                        return false;
                }

                if(this.filterLocation == "none") {
                    if(i.template != 0)
                        return false;
                }
                else if(this.filterLocation != "all") {
                    if(i.location != this.filterLocation)
                        return false;
                }

                if(this.filterState == "borrowed") {
                    if(i.returned)
                        return false;
                }
                else if(this.filterState == "borrowed-self") {
                    if(i.returned || i.borrowedTeacher != this.api.teacherId)
                        return false;
                }
                else if(this.filterState == "returned") {
                    if(!i.returned)
                        return false;
                }

                show = this.searchText.length == 0 || (i.name + i.description + i.locationName + i.shortId + i.notes).toLowerCase().includes(this.searchText.toLowerCase());

                return show;
            });
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
            this.modalIdentifier = item.shortId || ""; // This doesn't always update the modal. wtf
            $("#modalIdentifierInput").val(this.modalIdentifier);
            this.modalFirstTime = !this.modalIdentifier || this.modalIdentifier == "";
            $("#enterIdModal").modal("show");
        },

        showNotesModal(item) {
            this.modalItem = item;
            this.modalNotes = item.notes || ""; // This doesn't always update the modal. wtf
            $("#notesModalInput").val(this.modalNotes);
            $("#notesModal").modal("show");
        },

        showTemplateModal(item) {
            this.modalItem = item;
            this.modalTemplate = item.template; 
            $("#templateSelect").val(item.template).change();
            $("#templateModal").modal("show");
        },

        showLocationModal(item) {
            this.modalItem = item;
            this.modalLocation = item.location;
            $("#locationSelect").val(item.location).change();
            $("#locationModal").modal("show");
        },

        async setIdentifier(item, identifier) {
            try {
                await this.api.updateItemIdentifier(item.id, identifier);
                item.shortId = identifier;
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
        },

        async setNotes(item, newNotes) {
            try {
                await this.api.updateItemNotes(item.id, newNotes);
                item.notes = newNotes;
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
        },

        async setItemTemplate(item, templateId) {
            try {
                await this.api.updateItemTemplate(item.id, templateId);

                item.template = templateId;

                if(templateId == 0) {
                    item.name = "";
                    item.description = "";
                }
                else {
                    var template = this.templates.find(t => t.id == templateId);
                    item.name = template.name;
                    item.description = template.description;
                }
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
        },

        async setItemLocation(item, locationId) {
            try {
                await this.api.updateItemLocation(item.id, locationId);

                item.location = locationId;

                if(locationId != 0)
                    item.locationName = this.locations[locationId].name;
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
        },

        areNotesTooBigToDisplay(notes) {
            return notes.length > 50;
        },

        startSelecting() {
            for(var key in this.items)
                this.items[key].selected = false;

            this.selecting = true;
        },

        stopSelecting() {
            this.selecting = false;
        },

        goToCompletionPage() {
            this.api.clearError();

            var selectedItems = [];

            for(var key in this.items)
                if(this.items[key].selected)
                    selectedItems.push(this.items[key].id);

            if(selectedItems.length == 0) {
                this.api.displayError("Wystąpił błąd", "Musisz zaznaczyć przynajmniej jeden przedmiot.");
                return;
            }

            router.push({
                name: "identifier-completion",
                params: {
                    itemIds: selectedItems,
                }
            });
        }
    },

    computed: {

    },

    mounted() {
        $(".ui.dropdown").dropdown();

        $("#modalIdentifierInput").keyup(ev => {
            if(ev.keyCode === 13)
                $("#modalIdentifierButton").click();
        });
    },

    async created() {
        this.api.loading = true;
        
        try {
            let response = await this.$http.get('/api/Items')
            this.items = response.data;
        }
        catch(e) {
            this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
        }

        this.templates = (await this.$http.get("/api/ItemTemplates")).data;

        this.locations = await this.api.getLocationsAsTable();

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

    .centered {
        text-align: center;
    }
</style>
