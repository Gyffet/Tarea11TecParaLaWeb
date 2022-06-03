<template>
    <v-app>
        <v-app-bar absolute
                   color="grey darken-4"
                   dark>

            <v-toolbar-title>Formulario Tarea 11</v-toolbar-title>
            <v-spacer></v-spacer>
        </v-app-bar>


        <br /><br /><br />
        <v-main>
            <router-view></router-view>
        </v-main>
        <v-card height="60">
            <v-footer absolute class="font-weight-medium"
                      color="grey darken-4">
                <v-col class="text-center white--text"
                       cols="12">
                    <strong>Oscar Iván Guerrero Esquea</strong>
                </v-col>
            </v-footer>
        </v-card>
    </v-app>
</template>



<script>
    import Utils from './services/utils';

    export default {
        name: "App",

        components: {

        },

        data: () => ({
            itemSalir: { text: 'Salir', icon: 'mdi-logout' },
            drawer: false,
            group: null,
        }),


        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : this.$store.state.auth.user;
            },
            currentUser() {
                if (this.loggedIn)
                    return this.$store.state.auth.user;
                return false;
            },

            isIdle() {
                if (this.loggedIn && this.$store.state.idleVue.isIdle)
                    this.salir();
                return this.$store.state.idleVue.isIdle;
            },

            dgS() {
                return Utils.dgSigla(this.cveArea);
            }
        },
        methods: {
            salir() {
                console.log('salir');
                localStorage.removeItem('user');
                location.reload(true);
            }
        }
    };
</script>
