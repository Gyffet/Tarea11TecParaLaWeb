<template>
    <v-app>
        <v-app-bar absolute
                   color="grey darken-4"
                   dark>
            <v-app-bar-nav-icon @click="drawer = true" v-if="loggedIn"></v-app-bar-nav-icon>

            <v-toolbar-title>Sistema para el Registro de Cédulas Automatizadas de Supervisión y Evaluación de Servicios Generales (CASESG)</v-toolbar-title>
            <v-spacer></v-spacer>

            <v-menu left bottom v-if="loggedIn">
                <template v-slot:activator="{ on }">
                    <v-btn icon v-on="on">
                        <v-icon v-on:click="salir()">mdi-logout</v-icon>
                    </v-btn>
                </template>

                
            </v-menu>

        </v-app-bar>

        <v-navigation-drawer v-model="drawer"
                             absolute
                             temporary>
            <v-list nav
                    dense>
                <v-list-item-group v-model="group"
                                   active-class="deep-purple--text text--accent-4">
                    <v-list-item link :to="{ path: '/inicio' }">
                        <v-list-item-icon>
                            <v-icon>mdi-home</v-icon>
                        </v-list-item-icon>
                        <v-list-item-title>Inicio</v-list-item-title>
                    </v-list-item>
                    <v-divider></v-divider>

                    <v-list-item link :to="{ path: '/director' }">
                        <v-list-item-icon>
                            <v-icon>mdi-account</v-icon>
                        </v-list-item-icon>
                        <v-list-item-title>Administrador Inmueble</v-list-item-title>
                    </v-list-item>

                    <v-list-item link :to="{ path: '/administrador' }">
                        <v-list-item-icon>
                            <v-icon>mdi-account</v-icon>
                        </v-list-item-icon>
                        <v-list-item-title>DAS / CAE</v-list-item-title>
                    </v-list-item>

                    <v-list-item link :to="{ path: '/api/Documents/DescargarArchivo/Guia_CASESG_Mar2021.pdf' }"
                                 target="_blank">
                        <v-list-item-icon>
                            <v-icon>mdi-help-circle-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-title>Manual para llenado</v-list-item-title>
                    </v-list-item>

                    <v-switch v-model="$vuetify.theme.dark"
                              inset
                              label="Modo Obscuro"
                              persistent-hint></v-switch>

                </v-list-item-group>
            </v-list>
        </v-navigation-drawer>
        <br /><br /><br />
        <v-main>
            <router-view></router-view>
        </v-main>
        <v-card height="60">
            <v-footer absolute class="font-weight-medium"
                      color="grey darken-4">
                <v-col class="text-center white--text"
                       cols="12">
                    {{ new Date().getFullYear()
                            }} — <strong>DGSG</strong>
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
