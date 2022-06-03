<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="font-weight-bold mb-3"
                    align="justify">
                    {{currentUser.newDataSet.datosGenerales.nombre_emp}} usted puede evaluar los servicios de la administración donde está adscrito: {{currentUser.newDataSet.areas.nom_area}}<br />Seleccione el deseado:
                </h1>
            </v-col>
        </v-row>
        <br />
        <v-row>
            <v-col class="mb-4" align="center">
                <v-btn class="mb-4"
                       elevation="2"
                       color="teal darken-1"
                       raised
                       dark
                       link :to="{ path: '/director/limpieza' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-duck
                    </v-icon>Limpieza
                </v-btn>
                <br /><br /><br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/director/aguabeber' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-cup-water
                    </v-icon>Agua para Beber
                </v-btn>
                <br /><br /><br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="red darken-2"
                       raised
                       dark
                       link :to="{ path: '/director/mensajeria' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-email-outline
                    </v-icon>Mensajería
                </v-btn>
            </v-col>
            <v-col class="mb-4" align="center">
                <v-btn class="mb-4"
                       elevation="2"
                       color="lime darken-3"
                       raised
                       dark
                       link :to="{ path: '/director/fumigacion' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-bottle-tonic-skull
                    </v-icon>Fumigación
                </v-btn>
                <br /><br /><br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="indigo"
                       raised
                       dark
                       disabled
                       link :to="{ path: '/celular' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-cellphone-iphone
                    </v-icon>Telefonía Celular
                </v-btn>
                <br /><br /><br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="light-green"
                       raised
                       disabled
                       dark
                       link :to="{ path: '/comedor' }"
                       x-large
                       rounded>
                    <v-icon dark
                            left>
                        mdi-silverware-variant
                    </v-icon>Comedor
                </v-btn>

            </v-col>
        </v-row>


        <template>
            <v-row align="center" justify="space-around">

                <v-dialog v-model="dialogAceptadas">
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn color="green darken-2"
                               dark
                               v-bind="attrs"
                               v-on="on">
                            Aceptadas
                        </v-btn>
                    </template>
                    <template>
                        <v-data-table :headers="headersAceptadas"
                                      :items="cedulasAceptadas"
                                      sort-by="fcInmueble"
                                      class="elevation-1">
                            <template v-slot:top>
                                <v-toolbar flat>
                                    <v-toolbar-title>Cedulas Aceptadas</v-toolbar-title>
                                    <v-divider class="mx-4"
                                               inset
                                               vertical></v-divider>
                                    <v-spacer></v-spacer>
                                </v-toolbar>
                            </template>
                            <template v-slot:item.actions="{ item }">
                                <v-icon small
                                        @click="imprimirReporte(item)">
                                    mdi-file-pdf
                                </v-icon>
                            </template>
                        </v-data-table>
                    </template>
                </v-dialog>


                <v-dialog v-model="dialog">
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn color="yellow darken-2"
                               dark
                               v-bind="attrs"
                               v-on="on">
                            Guardadas
                        </v-btn>
                    </template>
                    <template>
                        <v-data-table :headers="headers"
                                      :items="cedulas"
                                      class="elevation-1">
                            <template v-slot:top>
                                <v-toolbar flat>
                                    <v-toolbar-title>Cedulas Guardadas</v-toolbar-title>
                                    <v-divider class="mx-4"
                                               inset
                                               vertical></v-divider>
                                    <v-spacer></v-spacer>
                                    <v-dialog v-model="dialogDelete" max-width="500px">
                                        <v-card>
                                            <v-card-title class="headline">¿Está seguro de borrar este registro?</v-card-title>
                                            <v-card-actions>
                                                <v-spacer></v-spacer>
                                                <v-btn color="red darken-2" text @click="closeDelete">Cancelar</v-btn>
                                                <v-btn color="green darken-2" text @click="deleteItemConfirm">Aceptar</v-btn>
                                                <v-spacer></v-spacer>
                                            </v-card-actions>
                                        </v-card>
                                    </v-dialog>
                                </v-toolbar>
                            </template>
                            <template v-slot:item.actions="{ item }">
                                <v-icon small
                                        class="mr-2"
                                        @click="seleccionarCedula(item)">
                                    mdi-file-eye
                                </v-icon>
                                -
                                <v-icon small
                                        @click="deleteItem(item)">
                                    mdi-delete
                                </v-icon>
                                -
                                <v-icon small
                                        @click="borrador(item)">
                                    mdi-file-pdf
                                </v-icon>
                            </template>
                        </v-data-table>
                    </template>
                </v-dialog>

                <v-dialog v-model="dialogRechazadas">
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn color="red darken-2"
                               dark
                               v-bind="attrs"
                               v-on="on">
                            Rechazadas
                        </v-btn>
                    </template>
                    <template>
                        <v-data-table :headers="headersRechazadas"
                                      :items="cedulasRechazadas"
                                      sort-by="fcInmueble"
                                      class="elevation-1">
                            <template v-slot:top>
                                <v-toolbar flat>
                                    <v-toolbar-title>Cedulas Rechazadas</v-toolbar-title>
                                    <v-divider class="mx-4"
                                               inset
                                               vertical></v-divider>
                                    <v-spacer></v-spacer>
                                </v-toolbar>
                            </template>
                            <template v-slot:item.actions="{ item }">
                                <v-icon small
                                        class="mr-2"
                                        @click="seleccionarCedula(item)">
                                    mdi-file-eye
                                </v-icon>
                            </template>
                        </v-data-table>
                    </template>
                </v-dialog>
            </v-row>
        </template>
        <!--{{cedulasRechazadas}}-->
    </v-container>
</template>
<script>
    import CedulasService from '../services/cedulas-service';
    import MensajeriaService from '../services/mensajeria-service';
    import LimpiezaService from '../services/limpieza-service';
    import FumigacionService from '../services/fumigacion-service';
    // @ is an alias to /src
    export default {
        name: "Director",
        data: () => ({
            cedulas: [],
            cedulasRechazadas: [],
            cedulasAceptadas: [],

            dialog: false,
            dialogDelete: false,
            dialogAceptadas: false,
            dialogRechazadas: false,

            headersRechazadas: [
                {
                    text: 'Servicio',
                    sortable: true,
                    value: 'fcServicio',
                    align: 'center'
                },
                { text: 'Mes', value: 'fcMes', align: 'center' },
                { text: 'Inmueble', value: 'fcInmueble', align: 'center' },
                { text: 'Folio', value: 'fcNoCed', align: 'center' },
                { text: 'Factura', value: 'fcNumFactura', align: 'center' },
                { text: 'Editar', value: 'actions', sortable: false, align: 'center' },
                
            ],

            headersAceptadas: [
                {
                    text: 'Servicio',
                    sortable: true,
                    value: 'fcServicio',
                    align: 'center'
                },
                { text: 'Mes', value: 'fcMes', align: 'center' },
                { text: 'Inmueble', value: 'fcInmueble', align: 'center' },
                { text: 'Folio', value: 'fcNoCed', align: 'center' },
                { text: 'Factura', value: 'fcNumFactura', align: 'center' },
                { text: 'Reporte', value: 'actions', sortable: false, align: 'center' },
            ],

            headers: [
                {
                    text: 'Servicio',
                    sortable: true,
                    value: 'fcServicio',
                    align: 'center'
                },
                { text: 'Mes', value: 'fcMes', align: 'center' },
                { text: 'Inmueble', value: 'fcInmueble', align: 'center' },
                { text: 'Folio', value: 'fcNoCed', align: 'center' },
                { text: 'Factura', value: 'fcNumFactura', align: 'center' },
                { text: 'Fecha Creación', value: 'fdFechaGuardado', align: 'center' },
                { text: 'Seleccionar/Borrar/Preliminar', value: 'actions', sortable: false, align: 'center' },
            ],

            editedIndex: -1,
            editedItem: null
        }),
        mounted() {
            CedulasService.getCedulasPendientes(this.currentUser.newDataSet.areas.cveArea).then(
                v => {
                    console.log("Cedulas Guardadas");
                    v.Fumigacion.forEach(element =>
                        this.cedulas.push(element));
                    v.Limpieza.forEach(element =>
                        this.cedulas.push(element));
                    v.Mensajeria.forEach(element =>
                        this.cedulas.push(element));
                    console.log(this.cedulas);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );

            CedulasService.getPendRech(this.currentUser.newDataSet.areas.cveArea).then(
                v => {
                    console.log("Cedulas Rechazadas");
                    console.log(v);
                    v.Fumigacion.forEach(element =>
                        this.cedulasRechazadas.push(element));
                    v.Limpieza.forEach(element =>
                        this.cedulasRechazadas.push(element));
                    v.Mensajeria.forEach(element =>
                        this.cedulasRechazadas.push(element));
                    console.log(this.cedulasRechazadas);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );

            CedulasService.getAceptadasAdmin(this.currentUser.newDataSet.areas.cveArea).then(
                v => {
                    console.log("Cedulas Aceptadas");
                    console.log(v);
                    v.Fumigacion.forEach(element =>
                        this.cedulasAceptadas.push(element));
                    v.Limpieza.forEach(element =>
                        this.cedulasAceptadas.push(element));
                    v.Mensajeria.forEach(element =>
                        this.cedulasAceptadas.push(element));
                    console.log(this.cedulasAceptadas);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );
        },

        watch: {
            dialog(val) {
                val || this.close()
            },
            dialogDelete(val) {
                val || this.closeDelete()
            },
        },

        methods: {

            deleteItem(item) {
                this.editedIndex = this.cedulas.indexOf(item)
                this.editedItem = Object.assign({}, item)
                console.log(this.editedIndex)
                console.log(this.editedItem)
                this.dialogDelete = true
            },

            deleteItemConfirm() {
                this.cedulas.splice(this.editedIndex, 1)

                switch (this.editedItem.fiIDServicio) {
                    case 1:
                        MensajeriaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 2:
                        LimpiezaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 3:
                        FumigacionService.eliminar(this.editedItem.fcNoCed);
                        break;
                    default:
                    // code block
                }

                this.closeDelete()
            },

            closeDelete() {
                this.dialogDelete = false
                this.$nextTick(() => {
                    this.editedIndex = -1
                })
            },

            borrador(item) {
                switch (item.fiIDServicio) {
                    case 1:
                        break;
                    case 2:
                        window.open(
                            '/api/Documents/ReporteLimpiezaPorValidar/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 3:
                        window.open(
                            '/api/Documents/ReporteFumigacionPorValidar/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    default:
                    // code block
                }
            },

            
            imprimirReporte(item) {
                switch (item.fiIDServicio) {
                    case 1:
                        window.open(
                            '/api/Documents/GenerarReporteMensajeria/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 2:
                        window.open(
                            '/api/Documents/GenerarReporteLimpieza/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 3:
                        window.open(
                            '/api/Documents/GenerarReporteFumigacion/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    default:
                    // code block
                }
            },

            seleccionarCedula(item) {
                localStorage.setItem("folio", item.fcNoCed);
                switch (item.fiIDServicio) {
                    case 1:
                        this.$router.push('/director/mensajeria');
                        break;
                    case 2:
                        this.$router.push('/director/limpieza');
                        break;
                    case 3:
                        this.$router.push('/director/fumigacion');
                        break;
                    default:
                    // code block
                }

            }

            /*seleccionarCedula(item, servicio) {
                this.editedItem = Object.assign({}, item)
                switch (this.editedItem.fiIDServicio) {
                    case 1:
                        MensajeriaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 2:
                        LimpiezaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 3:
                        FumigacionService.eliminar(this.editedItem.fcNoCed);
                        break;
                    default:
                    // code block
                }

                console.log(item);
                localStorage.setItem("folio", item.fcNoCed);
                console.log(localStorage);
                this.$router.push('/director/' + servicio);
            }*/
        },

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

    };</script>
