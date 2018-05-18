﻿import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { api } from './api'
import ErrorDisplay from 'components/error-display'

require('semantic-ui-css/semantic.css')
require('../node_modules/semantic-ui-css/semantic.js')

Vue.prototype.$http = axios;

sync(store, router)

const app = new Vue({
    store,
    router,
    ...App
})

Vue.mixin({
    data: function() {
        return {
            get api() {
                return api;
            }
        }
    }
})

Vue.component("error-display", ErrorDisplay);

export {
    app,
    router,
    store
}