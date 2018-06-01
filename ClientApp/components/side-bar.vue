<template>
    <div class="ui visible sidebar inverted vertical menu left">
        <div class="item user-header">
            Witaj, {{ api.username }}!
            <a class="logout-button" @click="logout">
                <i class="sign out icon" title="Wyloguj"/>
            </a>
        </div>

        <a class="item" v-if="item.display" v-for="(item, index) in routes" :key="index" :class="{'active':router.currentRoute.path==item.path}" @click="navigate(item.path)">
            <div>
                <i v-if="item.icon" :class="{'icon': true, [item.icon]: true}"/>
                <span>{{ item.display }}</span>
                <template v-if="item.path == '/messages'">
                    <div v-if="api.messageCount && api.messageCount" class="ui circular red label message-count">
                        {{ api.messageCount }}
                    </div>
                </template>
            </div>
        </a>

        <div v-if="api.useDevVersion" class="item">
            <div class="ui inverted green progress">
                <div class="bar">
                    <div class="progress"></div>
                </div>
                <div class="label"><p>😁 Gotowość projektu 😁</p></div>
            </div>
        </div>

        <div class="item">
            <div class="ui checkbox">
                <input type="checkbox" v-model="api.useDevVersion">
                <label style="color:white;">Użyj wersji deweloperskiej</label>
            </div>
        </div>

        <div class="footer">
            Copyright © 2018<br>
            Szymon Jankowski
        </div>
    </div>
</template>

<script>
import router from "../router.js";
import { routes } from "../routes";

export default {
    data() {
        return {
            routes,
            router,
        };
    },

    mounted() {
        $(".progress").progress({
            percent: 80
        });

        $(".sidebar").sidebar();
    },

    created() {
         
    },

    methods: {
        navigate(route) {
            router.push(route);
        },

        async logout() {
            await this.api.logout();
        }
    }
};
</script>

<style>
.fill {
    height: 100vh;
    width: 100%;
}

.navbar1 {
    background-color: #1E1E1E;
}

.nav-button1 {
    color: #E2E2E2;
    background: #1E1E1E;
    width: 100%;
    border-left: 5px solid #1E1E1E;
    font-size: 25px;
    vertical-align: text-top;
    margin-top: 5px;
}

.nav-button-active {
    border-left: 5px solid #E2E2E2;
    background-color: #364656;
}

.nav-button1:hover {
    border-left: 5px solid #E2E2E2;
    background-color: #364656;
}

.nav-title1 {
    font-size: 38px;
    text-align: center;
    color: #E2E2E2;
    border-bottom: 2px solid #E2E2E2;
    padding: 15px;
}

.footer {
    text-align: center;
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    margin: 0.5em;
    color: white;
}

.user-header {
    text-align: center;
}

.message-count {
    /* display: absolute !important; */
    position: absolute !important;
    top: 50%;
    bottom: 50%;
    margin: auto !important;
    /* position: absolute;
    margin-left: auto !important;
    right: 1em;
    margin: 0 !important; */

    right: 1em !important;
}

.logout-button {
    position: absolute !important;
    margin: auto !important;

    right: 0.8em !important;
    margin-top: -2px !important;
}
</style>
