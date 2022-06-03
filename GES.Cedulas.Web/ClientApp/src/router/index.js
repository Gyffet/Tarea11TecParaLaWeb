import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Altas from "../views/Altas.vue";
import Modificaciones from "../views/Modificaciones.vue"
import Listados from "../views/Listados.vue"
import Bajas from "../views/Bajas.vue"

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
    {
        path: "/listados",
        name: "Listados",
        component: Listados
    },
    {
        path: "/bajas",
        name: "Bajas",
        component: Bajas
    },
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});


export default router;