<template>
    <div>
        <h1>Moje konto</h1>

        <error-display/>

        <div class="ui form">
            <div class="two fields">
                <div class="field">
                    <label>Imię</label>
                    <input disabled type="text" v-model="name">
                </div>
                <div class="field">
                    <label>Nazwisko</label>
                    <input disabled type="text" v-model="surname">
                </div>
            </div>
            <div class="field">
                <label>Nazwa użytkownika</label>
                <input disabled type="text" v-model="username">
            </div>
            <div class="field">
                <label>Adres E-Mail</label>
                <input type="text" v-model="email">
            </div>

            <div class="two fields">
                <div class="field" :class="{ 'error': !passwordsValid() }">
                    <label>Nowe hasło</label>
                    <input type="password" v-model="password1">
                </div>
                <div class="field" :class="{ 'error': !passwordsValid() }">
                    <label>Potwierdź hasło</label>
                    <input type="password" v-model="password2">
                </div>
            </div>

            <button id="saveButton" class="ui primary button" @click="save" :disabled="!passwordsValid()">Zapisz</button>
        </div>

        <!-- Done modal -->
        <div class="ui modal" id="doneModal">
            <div class="header">
                Zaktualizowano informacje o koncie
            </div>
            <div class="content">
                <div class="description">
                    <p>Informacje o twoim koncie zostały zauktualizowane.</p>
                </div>
            </div>
            <div class="actions">
                <div class="ui green deny right button">
                    Zamknij
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
            loading: false,

            name: "",
            surname: "",
            email: "",
            username: "",

            password1: "",
            password2: "",
        }
    },

    async created() {
        this.api.loading = true;

        var data = await this.api.getTeacher(this.api.teacherId);
        this.name = data.name;
        this.surname = data.surname;
        this.username = data.username;
        this.email = data.email;
    
        this.api.loading = false;
    },

    methods: {
        passwordsValid() {
            if(this.password1.length > 0 || this.password2.length > 0)
                return this.password1 == this.password2;

            return true;
        },

        async save() {
            if(this.loading)
                return;

            this.api.clearError();
            this.loading = true;
            $("#saveButton").addClass("loading");

            try {
                await this.api.updateTeacherSelf(this.email, this.password1);
                $("#doneModal").modal("show");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
            }

            this.loading = false;
            $("#saveButton").removeClass("loading");
        }
    }
}
</script>

<style>
    .dashboard-entry-wrapper {
        text-align: center;
    }

    .dashboard-entry-wrapper .number {
        margin-bottom: 0;
        font-size: 80px;
    }

    .dashboard-entry-wrapper .description {
        font-size: 24px;
        margin-bottom: 0.5em;
    }
</style>
