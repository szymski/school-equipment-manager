<template>
    <div id="app" class="pushable" style="min-height:100vh !important;">
        <!-- Cookies nag -->
        <div class="ui inline cookie nag">
            <span class="title">
                Używamy ciasteczek, aby zapewnić prawidłowe funkcjonowanie aplikacji.
            </span>
            <i class="close icon"></i>
        </div>

        <!-- Connection lost message -->
        <div v-if="api.connectionLost">
            <div class="ui one column stackable center aligned page grid">
                <div class="ui eight wide column">
                    <div class="ui raised red segment login-box">
                        <h3>Utracono połączenie z serwerem</h3>
                        <p>Prosimy o spróbowanie ponownie za chwilę lub skontaktowanie się z administratorem.</p>
                        <button class="ui gray button" @click="refresh">Odśwież</button>
                    </div>
                </div>
            </div>
        </div>
        <div v-else>
            <div v-if="api.loggedIn">
                <div class="pusher">
                    <side-bar/>
                    <div class="ui basic segment">
                        <router-view></router-view>
                        <div class="ui inverted top aligned dimmer" :class="{ 'active': api.loading }">
                            <div class="content">
                                <div class="ui inline loader my-loader"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else>
                <template v-if="!resettingPassword">
                    <div class="ui one column stackable center aligned page grid">
                        <div class="ui eight wide column">
                            <h1 class="app-title">System ewidencji inwentarzu</h1>
                            <div class="ui raised segment login-box">
                                <error-display/>
                                <h3>Wymagane zalogowanie</h3>
                                <div class="ui form">
                                    <div class="ui field" id="loginField">
                                        <input type="text" placeholder="Nazwa użytkownika" v-model="username">
                                    </div>
                                    <div class="ui field" id="loginField">
                                        <input type="password" placeholder="Hasło" v-model="password">
                                    </div>

                                    <button id="loginButton" class="ui fluid primary button" @click="login">Zaloguj</button>

                                    <a class="reset-link" @click="resettingPassword = true">Resetuj hasło</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>
                <template v-else>
                    <div class="ui one column stackable center aligned page grid">
                        <div class="ui eight wide column">
                            <h1 class="app-title">System ewidencji inwentarzu</h1>
                            <div class="ui raised segment login-box">
                                <error-display/>
                                <h3>Resetowanie hasła</h3>
                                <p>
                                    Aby zresetować hasło, podaj poniżej swój adres E-Mail.<br>
                                    Jeśli istnieje konto z takim adresem, otrzymasz wiadomość, która pozwoli ustawić nowe hasło.
                                </p>
                                <div class="ui form">
                                    <div class="ui field" id="loginField">
                                        <input type="text" placeholder="Adres E-Mail" v-model="email">
                                    </div>
                                    
                                    <button id="resetButton" class="ui fluid primary button" @click="requestPasswordReset" :disabled="email == ''">Resetuj hasło</button>

                                    <a class="reset-link" @click="resettingPassword = false">Wróć do logowania</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>

                <!-- Done modal -->
                <div class="ui modal" id="doneModal">
                    <div class="header">
                        Sprawdź swoją skrzynkę pocztową
                    </div>
                    <div class="content">
                        <div class="description">
                            <p>Otrzymasz link, który pozwoli na zresetowanie hasła.</p>
                        </div>
                    </div>
                    <div class="actions">
                        <div class="ui green deny right button">
                            Zamknij
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Vue from "vue";
import SideBar from "./side-bar";

Vue.component("side-bar", SideBar);

export default {
    data() {
        return {
            loggingIn: false,
            resettingPassword: false,

            username: "",
            password: "",

            email: "",
        };
    },

    mounted() {
        if(!localStorage.acceptCookies) {
            $(".cookie.nag").nag("show");
            localStorage.acceptCookies = true;
        }
    },

    updated() {
        $("[id=loginField]").keyup(ev => {
            if(ev.keyCode === 13)
                $("#loginButton").click();
        });  
    },

    async created() {
        this.api.loading = true;

        await this.api.updateUserInfo();

        this.api.loading = false;
    },

    methods: {
        async login() {
            if(this.loggingIn)
                return;

            this.loggingIn = true;

            this.api.clearError();

            $("#loginButton").addClass("loading");

            try {
                await this.api.login(this.username, this.password);
                await this.api.updateUserInfo();
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }
            
            $("#loginButton").removeClass("loading");

            this.loggingIn = false;
        },

        async requestPasswordReset() {
            if(this.loggingIn)
                return;

            this.loggingIn = true;

            this.api.clearError();

            $("#resetButton").addClass("loading");

            try {
                await this.api.requestPasswordReset(this.email);
                this.resettingPassword = false;
                $("#doneModal").modal("show");
            }
            catch(e) {
                this.api.displayError("Wystąpił błąd", this.api.parseError(e.response));
            }

            $("#resetButton").removeClass("loading");

            this.loggingIn = false;
        },

        refresh() {
            location.reload();
        }
    }
};
</script>

<style>
body, h1, h2, h3, h4, h5, .ui.button, .ui.menu, .header {
    font-family: Arial, Helvetica, sans-serif !important;
}

.ui.accordion .title:not(.ui) {
    font-family: Arial, Helvetica, sans-serif !important;
}

.page-content {
    padding: 0.5em;
}

.login-box {
    margin-top: 1em !important;
}

.my-loader {
    margin-top: 2em !important;
}

.reset-link {
    display: block;
    margin-top: 1em !important;
}

.app-title {
    padding-top: 2em;
}
</style>
