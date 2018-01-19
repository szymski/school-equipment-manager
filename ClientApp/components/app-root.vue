<template>
    <div id="app">
        <div class="container-fluid">
            <div class="row fill">
                <side-bar></side-bar>
                <div class="col-10">
                    <div class="container mt-2">
                        <router-view></router-view>
                    </div>
                </div>
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

</style>
