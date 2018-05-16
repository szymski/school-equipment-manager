<template>
<div>
    <h1 class="six wide column">Przedmioty</h1>

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
                <th style="width:1px;">lp.</th>
                <th>Identyfikator</th>
                <th>Nazwa</th>
                <th>Opis</th>
                <th>Uwagi</th>
                <th style="width:1px;">Położenie</th>
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
                    <a v-if="!item.shortId || item.shortId == ''" href="#" @click="showEnterIdDialog(item)">Dodaj</a>
                </td>
                <td><a @click="goToEventList(item.id)" href="#">{{ item.name }}</a></td>
                <td>{{ item.description }}</td>
                <td>{{ item.notes }}</td>
                <td>{{ item.location }}</td>
                <td>
                    <button class="ui fluid tiny red button" @click="removeItem(item.id)">Usuń</button>
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
            <div class="ui black deny button">
                Anuluj
            </div>
            <div class="ui positive right labeled icon button" @click="setIdentifier(modalItem, modalIdentifier)" id="modalIdentifierButton">
                Ustaw kod
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
            return items.filter((i) => this.searchText.length == 0 || (i.name + i.description + i.location + i.shortId).toLowerCase().includes(this.searchText.toLowerCase()));
        },
        goToAddItem() {
            router.push("/add-item");
        },
        goToEventList(id) {
            router.push("/view-events/" + id);
        },
        async removeItem(id) {
            await this.$http.post('/api/Items/Remove', "id=" + id);
            let response = await this.$http.get('/api/Items')
            this.items = response.data;
        },
        showEnterIdDialog(item) {
            this.modalItem = item;
            this.modalIdentifier = item.shortId ? item.shortId : "";
            this.modalFirstTime = !this.modalIdentifier || this.modalIdentifier == "";
            $("#enterIdModal").modal("show");
        },
        setIdentifier(item, identifier) {
            item.shortId = identifier;
            this.api.updateItemIdentifier(item.id, identifier);
        }
    },

    computed: {

    },

    mounted() {
        $("#modalIdentifierInput").keyup(ev => {
            if(ev.keyCode === 13)
                $("#modalIdentifierButton").click();
        });
    },

    async created() {
        let response = await this.$http.get('/api/Items')
        this.items = response.data;
    }
};
</script>

<style>
    .item-short-id {
        margin-left: 32px;
    }

    .item-short-id .edit-id-btn {
        opacity: 0;
    }

    .item-short-id:hover .edit-id-btn {
        opacity: 1;
    }
</style>
