<template>
    <v-container>
        <v-row class="text-center">

            
            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Bienvenido al Sistema de Supervisión y Evaluación de Servicios Generales
                </h1>

                <p class="subheading font-weight-regular">
                    Para dudas relacionadas con los servicios, por favor póngase en contacto con el supervisor del servicio. Consulte el directorio.
                    <br />
                    <br />
                    Para dudas o problemas técnicos relacionados con el sistema puede leer el apartado del sitio <a href="/FAQ">solución de problemas y preguntas frecuentes</a> o ponerse en contacto con el administrador del sistema a través del correo oscar.guerrero.esqueda@correo.cjf.gob.mx ext. #318-2507
                </p>
            </v-col>

            <v-col class="mb-5" cols="12">
                <h2 class="headline font-weight-bold mb-3">
                    Directorio
                </h2>
                <div v-if="$vuetify.theme.dark">
                    <v-theme-provider light>
                        <template>
                            <v-simple-table>
                                <thead>
                                    <tr>
                                        <th class="text-center">
                                            Nombre
                                        </th>
                                        <th class="text-center">
                                            Servicio
                                        </th>
                                        <th class="text-center">
                                            Teléfono
                                        </th>
                                        <th class="text-center">
                                            Correo
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="item in administradores"
                                        :key="item.nombre">
                                        <td>{{ item.nombre }}</td>
                                        <td>{{ item.servicio }}</td>
                                        <td>{{ item.extencion }}</td>
                                        <td>{{ item.correo }}</td>
                                    </tr>
                                </tbody>
                            </v-simple-table>
                        </template>
                    </v-theme-provider>
                </div>

                <div v-if="!$vuetify.theme.dark">
                    <v-theme-provider dark>
                        <template>
                            <v-simple-table>
                                <thead>
                                    <tr>
                                        <th class="text-center">
                                            Nombre
                                        </th>
                                        <th class="text-center">
                                            Servicio
                                        </th>
                                        <th class="text-center">
                                            Teléfono
                                        </th>
                                        <th class="text-center">
                                            Correo
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="item in administradores"
                                        :key="item.nombre">
                                        <td>{{ item.nombre }}</td>
                                        <td>{{ item.servicio }}</td>
                                        <td>{{ item.extencion }}</td>
                                        <td>{{ item.correo }}</td>
                                    </tr>
                                </tbody>
                            </v-simple-table>
                        </template>
                    </v-theme-provider>
                </div>

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