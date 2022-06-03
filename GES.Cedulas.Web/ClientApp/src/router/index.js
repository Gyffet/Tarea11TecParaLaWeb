import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import Altas from "../views/Altas.vue";



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
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});

/*router.beforeEach((to, from, next) => {
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
});*/

export default router;