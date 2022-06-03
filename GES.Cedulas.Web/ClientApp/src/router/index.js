import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Administrador from "../views/Administrador.vue";
import Director from "../views/Director.vue";
import Login from "../views/LoginView.vue";
import Inicio from "../views/InicioView.vue";
import Mensajeria from "../views/Mensajeria.vue";
import MensajeriaID from "../views/MensajeriaID.vue";
import Limpieza from "../views/Limpieza.vue";
import LimpiezaID from "../views/LimpiezaID.vue";
import Fumigacion from "../views/Fumigacion.vue";
import FumigacionID from "../views/FumigacionID.vue";
import AguaBeber from "../views/AguaBeber.vue";
import NoAutorizado from "../views/Noautorizado.vue";
import Aceptadas from "../views/CedulasAceptadas.vue";
import Rechazadas from "../views/CedulasRechazadas.vue";
import JsonUsuario from "../views/JsonUsuario.vue";
import PreguntasFrecuentes from "../views/PreguntasFrecuentes.vue";
import Master from "../views/CedulasMaestro.vue"


Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home
    },
    {
        path: "/director/fumigacion",
        name: "Fumigacion",
        component: Fumigacion
    },
    {
        path: "/master",
        name: "master",
        component: Master
    },
    {
        path: "/director/limpieza",
        name: "Limpieza",
        component: Limpieza
    },
    {
        path: "/director/mensajeria",
        name: "Mensajeria",
        component: Mensajeria
    },
    {
        path: "/director/aguabeber",
        name: "AguaBeber",
        component: AguaBeber
    },
    {
        path: "/administrador/aceptadas",
        name: "Aceptadas",
        component: Aceptadas
    },
    {
        path: "/administrador/rechazadas",
        name: "Rechazadas",
        component: Rechazadas
    },
    {
        path: "/inicio",
        name: "Inicio",
        component: Inicio
    },
    {
        path: "/login",
        name: "login",
        component: Login
    },
    {
        path: "/administrador",
        name: "Administrador",
        component: Administrador
    },
    {
        path: "/administrador/mensajeria/:id",
        name: "mensajeriaId",
        component: MensajeriaID
    },
    {
        path: "/administrador/limpieza/:id",
        name: "limpiezaId",
        component: LimpiezaID
    },
    {
        path: "/administrador/fumigacion/:id",
        name: "fumigacionId",
        component: FumigacionID
    },
    {
        path: "/Director",
        name: "Director",
        component: Director
    },
    {
        path: "/Noautorizado",
        name: "NoAutorizado",
        component: NoAutorizado
    },
    {
        path: "/FAQ",
        name: "FAQ",
        component: PreguntasFrecuentes
    },
    {
        path: "/userjson",
        name: "userJson",
        component: JsonUsuario
    },
    {
        path: "/about",
        name: "About",
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () =>
            import(/* webpackChunkName: "about" */ "../views/About.vue")
    }
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});

router.beforeEach((to, from, next) => {
    const publicPages = ['/login', '/'];
    const authRequired = !publicPages.includes(to.path);
    const loggedIn = localStorage.getItem('user');
    if (loggedIn) {
        const usuario = JSON.parse(loggedIn);
        const rol = usuario.newDataSet.roles;
        console.log(rol.rol_rd);
        if (to.path == '/administrador' && rol.rol_rd == 3) {
            return next('/Noautorizado');
        }

        if (to.path == '/master' && rol.rol_rd != 1) {
            return next('/Noautorizado');
        }

        
    }

    //console.log(authRequired);
    //console.log(to);


    
    if (authRequired && !loggedIn) {
        return next('/login');
    }
    else if (to.path == '/' && loggedIn) {
        return next('/inicio');
    }
    else if (to.path == '/login' && loggedIn) {
        return next('/user');
    }
    

    next();
});

export default router;