import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import { api } from './api'
import ErrorDisplay from 'components/error-display'
import ScanPage from 'components/pages/scan-page'

require('semantic-ui-css/semantic.css')
require('../node_modules/semantic-ui-css/semantic.js')
require('../node_modules/chart.js/dist/Chart.min.js')
require('./assets/favicon.ico')

Vue.prototype.$http = axios;

sync(store, router)

Vue.mixin({
    data: function() {
        return {
            get api() {
                return api;
            }
        }
    }
})

api.authToken = localStorage.authToken;

axios.interceptors.request.use((config) => {
    var authToken = api.authToken;
    if (authToken) {
        config.headers.Authorization = `Bearer ${authToken}`;
    }
    return config;
}, (err) => {
    return Promise.reject(err);
});

const app = new Vue({
    store,
    router,
    ...App
})

Vue.component("error-display", ErrorDisplay);
Vue.component("scan-page", ScanPage);

export {
    app,
    router,
    store
}