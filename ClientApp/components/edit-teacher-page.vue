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
        };
    },

    methods: {
        async save() {
            try {
                await this.api.updateTeacher(this.teacher.id, this.name, this.surname, this.barcode);
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
