<template>
    <v-container>
        <h1>Administrador de Contrato</h1>
        <v-row>
            <v-col>
                <p class="subheading font-weight-regular">
                    <a href="/administrador/aceptadas" target="_blank">Cédulas Aceptadas</a> -
                    <a href="/administrador/rechazadas" target="_blank">Cédulas Rechazadas</a>
                </p>
                <br />


                <h3>Estas son las cédulas enviadas por los servidores hasta ahora:</h3>
                <br />
                <!--Mensajería-->
                <v-btn color="red darken-2"
                       class="ma-2"
                       dark
                       @click="dialogMensajeria = true">
                    Mensajería
                </v-btn>
                <v-dialog v-model="dialogMensajeria"
                          fullscreen
                          hide-overlay
                          transition="dialog-bottom-transition"
                          scrollable>
                    <v-card tite>
                        <v-toolbar flat
                                   dark
                                   color="red darken-2">
                            <v-btn icon
                                   dark
                                   @click="dialogMensajeria = false">
                                <v-icon>mdi-close</v-icon>
                            </v-btn>
                            <v-toolbar-title>
                                Mensajería
                            </v-toolbar-title>
                            <v-spacer></v-spacer>
                            <v-text-field v-model="search"
                                          append-icon="mdi-magnify"
                                          label="Buscar por Folio o Factura"
                                          single-line
                                          hide-details></v-text-field>
                        </v-toolbar>
                        <v-card-text>
                            <template>
                                <v-data-table :headers="headers"
                                              :items="cedulas.mensajeria"
                                              item-key="pkPenalizacion"
                                              class="elevation-1"
                                              :search="search"
                                              :sort-desc="[false, true]"
                                              multi-sort
                                              hide-default-footer
                                              show-group-by>
                                    <template v-slot:item.actions="{ item }">
                                        <v-icon small
                                                class="mr-2"
                                                @click="goToMensajeria(item.folio)">
                                            mdi-file-eye
                                        </v-icon>
                                    </template>
                                </v-data-table>
                            </template>

                        </v-card-text>

                        <div style="flex: 1 1 auto;"></div>
                    </v-card>
                </v-dialog>

                <br />

                <!--Limpieza-->
                <v-btn color="teal darken-1"
                       class="ma-2"
                       dark
                       @click="dialogLimpieza = true">
                    Limpieza
                </v-btn>
                <v-dialog v-model="dialogLimpieza"
                          fullscreen
                          hide-overlay
                          transition="dialog-bottom-transition"
                          scrollable>
                    <v-card tite>
                        <v-toolbar flat
                                   dark
                                   color="teal darken-1">
                            <v-btn icon
                                   dark
                                   @click="dialogLimpieza = false">
                                <v-icon>mdi-close</v-icon>
                            </v-btn>
                            <v-toolbar-title>
                                Limpieza
                            </v-toolbar-title>
                            <v-spacer></v-spacer>
                            <v-text-field v-model="search"
                                          append-icon="mdi-magnify"
                                          label="Buscar por Folio o Factura"
                                          single-line
                                          hide-details></v-text-field>
                        </v-toolbar>
                        <v-card-text>
                            <template>
                                <v-data-table :headers="headers"
                                              :items="cedulas.limpieza"
                                              item-key="pkPenalizacion"
                                              class="elevation-1"
                                              :search="search"
                                              :sort-desc="[false, true]"
                                              multi-sort
                                              hide-default-footer
                                              show-group-by>
                                    <template v-slot:item.actions="{ item }">
                                        <v-icon small
                                                class="mr-2"
                                                @click="goToLimpieza(item.folio)">
                                            mdi-file-eye
                                        </v-icon>
                                    </template>
                                </v-data-table>
                            </template>

                        </v-card-text>

                        <div style="flex: 1 1 auto;"></div>
                    </v-card>
                </v-dialog>


                <br />

                <!--Fumigación-->
                <v-btn color="lime darken-3"
                       class="ma-2"
                       dark
                       @click="dialogFumigacion = true">
                    Fumigación
                </v-btn>
                <v-dialog v-model="dialogFumigacion"
                          fullscreen
                          hide-overlay
                          transition="dialog-bottom-transition"
                          scrollable>
                    <v-card tite>
                        <v-toolbar flat
                                   dark
                                   color="lime darken-3">
                            <v-btn icon
                                   dark
                                   @click="dialogFumigacion = false">
                                <v-icon>mdi-close</v-icon>
                            </v-btn>
                            <v-toolbar-title>
                                Fumigación
                            </v-toolbar-title>
                            <v-spacer></v-spacer>
                            <v-text-field v-model="search"
                                          append-icon="mdi-magnify"
                                          label="Buscar por Folio o Factura"
                                          single-line
                                          hide-details></v-text-field>
                        </v-toolbar>
                        <v-card-text>
                            <template>
                                <v-data-table :headers="headers"
                                              :items="cedulas.fumigacion"
                                              item-key="pkPenalizacion"
                                              class="elevation-1"
                                              :search="search"
                                              :sort-desc="[false, true]"
                                              multi-sort
                                              hide-default-footer
                                              show-group-by>
                                    <template v-slot:item.actions="{ item }">
                                        <v-icon small
                                                class="mr-2"
                                                @click="goToFumigacion(item.folio)">
                                            mdi-file-eye
                                        </v-icon>
                                    </template>
                                </v-data-table>
                            </template>

                        </v-card-text>

                        <div style="flex: 1 1 auto;"></div>
                    </v-card>
                </v-dialog>

            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    // @ is an alias to /src
    import CedulasService from '../services/cedulas-service';

    export default {
        name: "Administrador",
        data: () => ({
            cedulas: [],
            search: "",
            dialogMensajeria: false,
            dialogLimpieza: false,
            dialogFumigacion: false,
            headers: [
                {
                    text: 'Periodo',
                    align: 'center',
                    value: 'periodo',
                    filterable: false,
                },
                { text: 'Inmueble', value: 'inmueble', align: 'center', filterable: false, },
                { text: 'No. Factura', value: 'factura', align: 'center', groupable: false, },
                { text: 'Folio', value: 'folio', align: 'center', groupable: false, },
                { text: 'Monto', value: 'monto', align: 'center', groupable: false, filterable: false, },
                { text: 'Ver Cédula', value: 'actions', align: 'center', groupable: false, sortable: false, filterable: false, },
            ],
        }),

        mounted() {
            CedulasService.getPendientes().then(
                m => {
                    this.cedulas = m;
                    console.log(this.cedulas);
                }, error => {
                    console.log("Error al cargar cédulas");
                    console.log(error);
                }
            );
        },

        methods: {
            goToMensajeria(id) {
                console.log("id mensajeria: ",id)
                this.$router.push({ name: 'mensajeriaId', params: { id: id } });
            },

            goToLimpieza(id) {
                this.$router.push({ name: 'limpiezaId', params: { id: id } });
            },

            goToFumigacion(id) {
                this.$router.push({ name: 'fumigacionId', params: { id: id } });
            },

            formatearMonto(monto) {
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                });
                return formatter.format(monto);
            },
        },

        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
        }
    };
</script>