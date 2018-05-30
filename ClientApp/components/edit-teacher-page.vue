<template>
<div>
    <h1 class="six wide column">Edycja nauczyciela <span v-if="teacher"> - {{ teacher.name }} {{ teacher.surname }}</span></h1>

    <error-display/>

    <div class="ui form">
        <div class="field">
            <label>Imię</label>
            <input type="text" v-model="name">
        </div>

        <div class="field">
            <label>Nazwisko</label>
            <input type="text" v-model="surname">
        </div>

        <div class="field">
                <label>Kod kreskowy</label>
                <div class="ui action input">
                    <input type="text" v-model="barcode">
                    <button class="ui blue button" :disabled="!canGenerateBarcode(name, surname)" @click="generateBarcode">Wygeneruj</button>
                </div>
            </div>

        <div class="ui segments">
            <h5 class="ui top attached segment">
                <div class="ui checkbox">
                    <input type="checkbox" v-model="enableAccount">
                    <label>Zezwól na logowanie</label>
                </div>
            </h5>
            <div v-if="enableAccount" class="ui attached segment">
                <div class="field">
                    <label>Nazwa użytkownika</label>
                    <input type="text" v-model="username">
                </div>

                <div class="field">
                    <label>Adres E-Mail</label>
                    <input type="text" v-model="email">
                </div>

                <button class="ui button" @click="save">Zresetuj hasło</button>
            </div>
        </div>

        <button class="ui primary button" @click="save">Zapisz</button>
        <button class="ui right floated red button" @click="showRemoveDialog">Usuń nauczyciela</button>
    </div>

    <div class="ui modal" id="removeModal">
        <div class="header">
            Czy na pewno chcesz usunąć nauczyciela <i>{{ teacher.name }} {{ teacher.surname }}</i>?
        </div>
        <div class="content">
            <div class="description">
                <p>Tej zmiany nie będzie dało się cofnąć!</p>
            </div>
        </div>
        <div class="actions">
            <div class="ui deny button">
                Anuluj
            </div>
            <div class="ui deny red right button" @click="remove">
                Usuń
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
            teacher: null,

            name: "",
            surname: "",
            barcode: "",

            enableAccount: false,
            username: "",
            email: "",
        };
    },

    methods: {
        async save() {
            try {
                await this.api.updateTeacher(this.teacher.id, this.name, this.surname, this.barcode,
                    this.enableAccount, this.username, this.email);
                router.push("/teachers");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        },

        showRemoveDialog() {
            $("#removeModal").modal("show");
        },

        async remove() {
            try {
                await this.api.removeTeacher(this.teacher.id);
                router.push("/teachers");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        },

        canGenerateBarcode(name, surname) {
            return name.length > 0 && surname.length > 0;
        },

        generateBarcode() {
            this.barcode = this.api.generateBarcodeForTeacher(this.name, this.surname);
        }
    },

    async created() {
        this.api.loading = true;

        try {
            this.teacher = await this.api.getTeacher(this.$route.params.id);
            
            this.name = this.teacher.name;
            this.surname = this.teacher.surname;
            this.barcode = this.teacher.barcode;

            this.enableAccount = this.teacher.enableAccount;
            this.username = this.teacher.username || "";
            this.email = this.teacher.email || "";
        }
        catch(e) {
            this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
        }

        this.api.loading = false;
    },
};
</script>

<style>

</style>
