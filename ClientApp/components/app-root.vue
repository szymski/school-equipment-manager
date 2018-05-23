<template>
    <div id="app" class="pushable">
        <div v-if="loggedIn">
            <side-bar/>
            <div class="pusher">
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
            <div class="ui one column stackable center aligned page grid">
                <div class="ui eight wide column">
                    <div class="ui raised segment login-box">
                        <h3>Wymagane zalogowanie</h3>
                        <div class="ui form">
                            <div class="ui field">
                                <input type="text" placeholder="Login">
                            </div>
                            <div class="ui field">
                                <input type="password" placeholder="Hasło">
                            </div>
                            <button class="ui fluid primary button">Zaloguj</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import Vue from "vue";
import HomePage from "./home-page";
import SideBar from "./side-bar";

Vue.component("home-page", HomePage);
Vue.component("side-bar", SideBar);

export default {
  data() {
    return {
        loggedIn: true,
    };
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
    margin-top: 4em !important;
}

.my-loader {
    margin-top: 2em !important;
}
</style>
