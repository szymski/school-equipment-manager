<template>
    <div id="app" class="pushable">
            <side-bar></side-bar>
            <div class="pusher">
                <div class="ui basic segment">
                    <router-view ></router-view>
                </div>
            </div>
    </div>
</template>

<script>
import Vue from "vue";
import HomePage from "./home-page";
import NavMenu from "./nav-menu";
import SideBar from "./side-bar";

Vue.component("home-page", HomePage);
Vue.component("nav-menu", NavMenu);
Vue.component("side-bar", SideBar);

export default {
    data() {
        return { };
    }
};
</script>

<style>
    .page-content {
        padding: 0.5em;
    }
</style>
