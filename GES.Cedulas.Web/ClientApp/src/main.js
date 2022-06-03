import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import VCalendar from 'v-calendar';
import Vuelidate from 'vuelidate';
import IdleVue from "idle-vue";

// Axios
import axios from "axios";
import VueAxios from "vue-axios";

axios.defaults.baseURL = process.env.VUE_APP_API_URL;

Vue.config.productionTip = false;

Vue.use(Vuelidate);

Vue.use(VCalendar, {
    componentPrefix: 'vc',
});

Vue.use(IdleVue, {
    store,
    idleTime: 3600000, // 1 hora
    startAtIdle: false
});

new Vue({
    router,
    store,
    axios,
    VueAxios,
    vuetify,
    render: h => h(App)
}).$mount("#app");