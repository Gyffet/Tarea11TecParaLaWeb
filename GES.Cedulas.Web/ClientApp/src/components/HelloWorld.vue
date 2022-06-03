<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Bienvenido al Formulario de la Tarea 11 de Tecnologias para la Web
                </h1>
            </v-col>

            <v-col class="mb-5" cols="12">

                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/altas' }"
                       x-large
                       rounded>
                    Altas
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/modificaciones' }"
                       x-large
                       rounded>
                    Modificaciones
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/altas' }"
                       x-large
                       rounded>
                    Listados
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/altas' }"
                       x-large
                       rounded>
                    Bajas
                </v-btn>

            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        name: "Inicio",
        data: () => ({
            administradores: [
                {
                    nombre: 'Cuautémoc del Río Reyes',
                    servicio: 'Agua para Beber',
                    extencion: '#318-2643',
                    correo: 'cuauhtemoc.delrio.reyes@correo.cjf.gob.mx'
                },
                {
                    nombre: 'Carlos Agustín Espinosa Barrera',
                    servicio: 'Limpieza',
                    extencion: '#318-2643',
                    correo: 'carlos.espinosa.barrera@correo.cjf.gob.mx'
                },
                {
                    nombre: 'Marco Antonio Paredes Vázquez',
                    servicio: 'Mensajería',
                    extencion: '#318-2643',
                    correo: 'marco.paredes.vazquez@correo.cjf.gob.mx'
                },
                {
                    nombre: 'Gerardo Benjamin Reyna Cruz',
                    servicio: 'Fumigación',
                    extencion: '#318-2507',
                    correo: 'gerardo.reyna.cruz@correo.cjf.gob.mx'
                },
                {
                    nombre: 'María Belén Serrano Olvera',
                    servicio: 'Comedor',
                    extencion: '#318-2515',
                    correo: 'maria.serrano.olvera@correo.cjf.gob.mx'
                },
                {
                    nombre: 'Oscar Iván Guerrero Esqueda',
                    servicio: 'Administrador del sistema',
                    extencion: '#318-2507',
                    correo: 'oscar.guerrero.esqueda@correo.cjf.gob.mx'
                },
            ]
            //whatsNext: [
            //    {
            //        text: "Explore components",
            //        href: "https://vuetifyjs.com/components/api-explorer"
            //    },
            //    {
            //        text: "Select a layout",
            //        href: "https://vuetifyjs.com/layout/pre-defined"
            //    },
            //    {
            //        text: "Frequently Asked Questions",
            //        href: "https://vuetifyjs.com/getting-started/frequently-asked-questions"
            //    }
            //]
        }),
        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
            currentUser() {
                if (this.loggedIn)
                    return this.$store.state.auth.user;
                return false;
            },
        }
    };
</script>