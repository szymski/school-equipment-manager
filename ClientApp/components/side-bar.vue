<template>
    <!-- TODO: Side-bar isn't taking the entire page - when there's a scrollbar visible, side-bar stays at the top. -->

    <div class="ui visible sidebar inverted vertical menu left">
        <div class="item user-header">
            Witaj, {{ api.username }}!
        </div>

        <a class="item" v-if="item.display" v-for="(item, index) in routes" :key="index" :class="{'active':router.currentRoute.path==item.path}" @click="navigate(item.path)">
            <span><i v-if="item.icon" :class="{'icon': true, [item.icon]: true}"/>{{ item.display }}</span>
        </a>

        <div class="item">
            <div class="ui inverted green progress">
                <div class="bar">
                    <div class="progress"></div>
                </div>
                <div class="label">Gotowość projektu</div>
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
            percent: 30
        });

        $(".sidebar").sidebar();
    },

    created() {
         
    },

    methods: {
        navigate(route) {
            router.push(route);
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
</style>
