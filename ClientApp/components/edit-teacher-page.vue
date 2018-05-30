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

                <button class="ui button" @click="resetPassword">Zresetuj hasło</button>
            </div>
        </div>

        <button class="ui primary button" @click="save">Zapisz</button>
        <button class="ui right floated red button" @click="showRemoveDialog" :disabled="api.teacherId == teacher.id">Usuń nauczyciela</button>
    </div>

    <!-- Removal confirmation dialog -->
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

    <!-- Generated password dialog -->
    <div class="ui modal" id="generatedPasswordModal">
        <div class="header">
            Wygenerowano hasło dla  nauczyciela <i>{{ teacher.name }} {{ teacher.surname }}</i>
        </div>
        <div class="content">
            <div class="description">
                <p>Zostało wygenerowane nowe hasło. Przekaż je nauczycielowi:</p>
                <h2 class="generated-password-text">{{ modalPassword }}</h2>
            </div>
        </div>
        <div class="actions">
            <div class="ui green deny right button" @click="closeGeneratedPasswordModal">
                Gotowe
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

            modalPassword: "",
        };
    },

    methods: {
        async save() {
            try {
                var data = await this.api.updateTeacher(this.teacher.id, this.name, this.surname, this.barcode,
                    this.enableAccount, this.username, this.email);

                if(data && data.generatedPassword) {
                    this.modalPassword = data.generatedPassword;
                    $("#generatedPasswordModal").modal("show");
                }
                else {
                    router.push("/teachers");
                    this.api.updateUserInfo();
                }
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }
        },

        async resetPassword() {
            this.api.clearError();

            try {
                var data = await this.api.resetPassword(this.teacher.id);
                this.modalPassword = data.generatedPassword;
                $("#generatedPasswordModal").modal("show");
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
        },

        closeGeneratedPasswordModal() {
            $("#removeModal").modal("hide");
            router.push("/teachers");
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
    .generated-password-text {
        text-align: center;
    }
</style>
