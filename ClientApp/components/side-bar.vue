<template>
<div class="col-2 navbar1 m-0 p-0">
    <p class="nav-title1">School equipment manager</p>
    <button class="btn nav-button1" v-bind:class="{ 'nav-button-active': router.currentRoute.path == route.path }" v-for="route in routes" v-bind:key="route" @click="navigate(route.path)">
        {{ route.display }}
    </button>
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
</style>
