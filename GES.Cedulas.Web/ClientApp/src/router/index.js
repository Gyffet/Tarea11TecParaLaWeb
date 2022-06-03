import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Altas from "../views/Altas.vue";
import Modificaciones from "../views/Modificaciones.vue"



Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home
    },
    {
        path: "/altas",
        name: "Altas",
        component: Altas
    },
    {
        path: "/modificaciones",
        name: "Modificaciones",
        component: Modificaciones
    },
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});


export default router;