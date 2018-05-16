<template>
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="a">
        <a class="navbar-brand" href="#">SchoolEqMan</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
                <li v-for="route in routes" v-bind:key="route" class="nav-item">
                    <!-- <a class="nav-link" href="#">{{ route.display }}</a> -->
                    <router-link v-bind:to="route.path" class="nav-link">{{ route.display }}</router-link>
                </li>
            </ul>
        </div>
    </div>
</nav>
</template>

<script>
import { routes } from "../routes";

export default {
  data() {
    return {
      routes,
      collapsed: true
    };
  },
  methods: {
    toggleCollapsed: function(event) {
      this.collapsed = !this.collapsed;
    }
  }
};
</script>

<style>
    .router-link-active {
        color: red;
    }
</style>
