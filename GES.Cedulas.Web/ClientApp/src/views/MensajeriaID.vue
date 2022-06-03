<template>
    <v-card outlined>
        <v-card-title>Cédula de Mensajería No. {{mensajeria.cedula.fcNoCed}}</v-card-title>

        <v-tabs v-model="tab"
                background-color=""
                class="elevation-2">
            <v-tabs-slider></v-tabs-slider>

            <v-tab :key="1"
                   :href="`#tab-1`">
                Información general
            </v-tab>

            <v-tab :key="2"
                   :href="`#tab-2`">
                Envío y Recolección
            </v-tab>

            <v-tab :key="3"
                   :href="`#tab-3`">
                Acuses, Mal Estado y Extravios
            </v-tab>

            <v-tab :key="4"
                   :href="`#tab-4`">
                Material y Uniforme
            </v-tab>

            <v-tab :key="5"
                   :href="`#tab-5`">
                Archivos
            </v-tab>



            <v-tab-item :key="1"
                        :value="'tab-1'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                No. Factura: {{mensajeria.cedula.fcNumFactura}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>

                            <v-card class="mx-auto">
                                <v-card-text>

                                    <div class="text--primary pb-2">
                                        <b>Mes:</b> {{mensajeria.cedula.fcMes}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Año:</b> {{mensajeria.cedula.fiAnio}}
                                    </div>
                                </v-card-text>
                            </v-card>

                            <v-card class="mt-3">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Inmueble:</b> {{mensajeria.cedula.fcInmueble}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Administración:</b> {{mensajeria.cedula.fcAdministracion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Monto de la Factura:</b> {{formatearMonto(mensajeria.cedula.fiMontoFactura)}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Calificación:</b> {{mensajeria.cedula.fiCalificacion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Llenado por:</b> {{mensajeria.cedula.fcUsuario}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Correo:</b> {{mensajeria.cedula.fcCorreoUsuario}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <v-col>
                            <h2>Penalizaciones/Deductivas | Total: {{formatearMonto(mensajeria.cedula.fcPenaRecoleccion + mensajeria.cedula.fcPenaEntrega + mensajeria.cedula.fcPenaAcuses + mensajeria.cedula.fcPenaEstado + mensajeria.cedula.fcPenaExtravio + mensajeria.cedula.fcPenaMaterial + mensajeria.cedula.fcPenaUniforme + mensajeria.cedula.fiPenaCalificacion)}}</h2>
                            <br />
                            <div class="text--primary pb-2">
                                <b>Recolección:</b> {{formatearMonto(mensajeria.cedula.fcPenaRecoleccion)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Entrega:</b> {{formatearMonto(mensajeria.cedula.fcPenaEntrega)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Acuses:</b> {{formatearMonto(mensajeria.cedula.fcPenaAcuses)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Mal Estado:</b> {{formatearMonto(mensajeria.cedula.fcPenaEstado)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Perdidos:</b> {{formatearMonto(mensajeria.cedula.fcPenaExtravio)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Material Embalaje:</b> {{formatearMonto(mensajeria.cedula.fcPenaMaterial)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Uniforme:</b> {{formatearMonto(mensajeria.cedula.fcPenaUniforme)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Calificación:</b> {{formatearMonto(mensajeria.cedula.fiPenaCalificacion)}}
                            </div>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <div v-if="mensajeria.cedula.fiEstatus==1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()">
                                <v-icon left>mdi-clipboard-check-multiple</v-icon> Aceptar Cédula
                            </v-btn>
                            <v-btn color="red darken-3" dark @click="rechazar()">
                                <v-icon left>mdi-close-box-multiple-outline</v-icon> Rechazar Cédula
                            </v-btn>
                            <v-btn color="green" dark :href='"/api/Documents/GenerarReporteMensajeria/" + mensajeria.cedula.fcNoCed' target="_blank">
                                <v-icon left>mdi-file-excel</v-icon> Generar Reporte
                            </v-btn>
                        </div>
                        <div v-if="mensajeria.cedula.fiEstatus!=1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()">
                                <v-icon left>mdi-clipboard-check-multiple</v-icon> Aceptar Cédula
                            </v-btn>
                            <v-btn color="red darken-3" dark @click="rechazar()">
                                <v-icon left>mdi-close-box-multiple-outline</v-icon> Rechazar Cédula
                            </v-btn>
                        </div>
                    </v-card-actions>
                    <v-snackbar v-model="snackbar"
                                :color="colorSnackbar"
                                :multi-line="true"
                                :timeout="3000">
                        {{ mensaje}}
                        <v-btn dark
                               text
                               @click="snackbar = false">
                            Cerrar
                        </v-btn>
                    </v-snackbar>
                </v-container>
            </v-tab-item>

            <!--tab 2 Recoleccion-->

            <v-tab-item :key="2"
                        :value="'tab-2'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(mensajeria.cedula.fcPenaRecoleccion + mensajeria.cedula.fcPenaEntrega)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias Recolección:</b> {{mensajeria.arreglos.arregloReco.length}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias Entrega:</b> {{mensajeria.arreglos.arregloEntre.length}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="mensajeria.arreglos.arregloReco.length>0">
                                <b>Recolección:</b>
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Número de Guía</th>
                                            <th> | </th>
                                            <th>Código de Rastreo</th>
                                            <th> | </th>
                                            <th>Tipo de Servicio</th>
                                            <th> | </th>
                                            <th>Fecha Recolección Programada</th>
                                            <th> | </th>
                                            <th>Fecha Recolección Efectiva</th>
                                            <th> | </th>
                                            <th>Pena/Deductiva</th>
                                            <th> | </th>
                                            <th>Eliminar</th>
                                            <th> | </th>
                                            <th>Editar</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="(item,index) in mensajeria.arreglos.arregloReco" :key="item.pkRecoleccion">
                                            <th>{{item.fcNoGuia}}</th>
                                            <th> | </th>
                                            <th>{{item.fcCodRastreo}}</th>
                                            <th> | </th>
                                            <th>{{item.fcTipoServicio}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaProgramada.substring(0, 10))}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaEfectiva.substring(0,10))}}</th>
                                            <th> | </th>
                                            <th>{{formatearMonto(item.fiPena)}}</th>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="red"
                                                    @click="eliminarReco(item)">
                                                mdi-delete-empty
                                            </v-icon>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="blue"
                                                    @click="editarRecoDiag(index)">
                                                mdi-pencil
                                            </v-icon>
                                        </tr>
                                    </tbody>

                                </table>
                            </div>


                            <div class="text--primary pb-2" v-if="mensajeria.arreglos.arregloEntre.length>0">
                                <br />
                                <b>Entrega:</b>
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Número de Guía</th>
                                            <th> | </th>
                                            <th>Código de Rastreo</th>
                                            <th> | </th>
                                            <th>Tipo de Servicio</th>
                                            <th> | </th>
                                            <th>Fecha Recolección Programada</th>
                                            <th> | </th>
                                            <th>Fecha Recolección Efectiva</th>
                                            <th> | </th>
                                            <th>Pena/Deductiva</th>
                                            <th> | </th>
                                            <th>Eliminar</th>
                                            <th> | </th>
                                            <th>Editar</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="(item,index) in mensajeria.arreglos.arregloEntre" :key="item.pkRecoleccion">
                                            <th>{{item.fcNoGuia}}</th>
                                            <th> | </th>
                                            <th>{{item.fcCodRastreo}}</th>
                                            <th> | </th>
                                            <th>{{item.fcTipoServicio}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaProgramada.substring(0, 10))}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaEfectiva.substring(0,10))}}</th>
                                            <th> | </th>
                                            <th>{{formatearMonto(item.fiPena)}}</th>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="red"
                                                    @click="eliminarReco(item)">
                                                mdi-delete-empty
                                            </v-icon>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="blue"
                                                    @click="editarRecoDiag(index)">
                                                mdi-pencil
                                            </v-icon>
                                        </tr>
                                    </tbody>

                                </table>
                            </div>
                        </v-col>
                    </v-row>

                    <v-snackbar v-model="snackbar"
                                :color="colorSnackbar"
                                :multi-line="true"
                                :timeout="3000">
                        {{ mensaje}}
                        <v-btn dark
                               text
                               @click="snackbar = false">
                            Cerrar
                        </v-btn>
                    </v-snackbar>
                </v-container>
            </v-tab-item>

            <!--tab 3 Acuses, Mal estado y Extravios-->

            <v-tab-item :key="3"
                        :value="'tab-3'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(mensajeria.cedula.fcPenaAcuses + mensajeria.cedula.fcPenaEstado + mensajeria.cedula.fcPenaExtravio)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias Acuses:</b> {{mensajeria.arreglos.arregloAcuse.length}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias Mal Estado:</b> {{mensajeria.arreglos.arregloMalEstado.length}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias Extravios:</b> {{mensajeria.arreglos.arregloExtravio.length}}
                                    </div>
                                </v-card-text>
                            </v-card>


                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="mensajeria.arreglos.arregloAcuse.length>0">
                                <b>Acuses:</b>
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Acuses Faltantes</th>
                                            <th> | </th>
                                            <th>Fecha Entrega Programada</th>
                                            <th> | </th>
                                            <th>Fecha Entrega Efectiva</th>
                                            <th> | </th>
                                            <th>Pena/Deductiva</th>
                                            <th> | </th>
                                            <th>Eliminar</th>
                                            <th> | </th>
                                            <th>Editar</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="(item,index) in mensajeria.arreglos.arregloAcuse" :key="item.pkAcuse">
                                            <th>{{item.fiCantidadAcuses}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaProgramada.substring(0, 10))}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaEfectiva.substring(0,10))}}</th>
                                            <th> | </th>
                                            <th>{{formatearMonto(item.fiPena)}}</th>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="red"
                                                    @click="eliminarReco(item)">
                                                mdi-delete-empty
                                            </v-icon>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="blue"
                                                    @click="editarRecoDiag(index)">
                                                mdi-pencil
                                            </v-icon>
                                        </tr>
                                    </tbody>

                                </table>
                            </div>


                            <div class="text--primary pb-2" v-if="mensajeria.arreglos.arregloMalEstado.length>0">
                                <br />
                                <b>Mal Estado:</b>
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>No. Guía</th>
                                            <th> | </th>
                                            <th>Código de Rastreo</th>
                                            <th> | </th>
                                            <th>Tipo de Servicio</th>
                                            <th> | </th>
                                            <th>Pena/Deductiva</th>
                                            <th> | </th>
                                            <th>Eliminar</th>
                                            <th> | </th>
                                            <th>Editar</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="(item,index) in mensajeria.arreglos.arregloMalEstado" :key="item.pkMalEstado">
                                            <th>{{item.fcNoGuia}}</th>
                                            <th> | </th>
                                            <th>{{item.fcCodRastreo}}</th>
                                            <th> | </th>
                                            <th>{{item.fcTipoServicio}}</th>
                                            <th> | </th>
                                            <th>{{formatearMonto(item.fiPena)}}</th>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="red"
                                                    @click="eliminarReco(item)">
                                                mdi-delete-empty
                                            </v-icon>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="blue"
                                                    @click="editarRecoDiag(index)">
                                                mdi-pencil
                                            </v-icon>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>


                            <div class="text--primary pb-2" v-if="mensajeria.arreglos.arregloExtravio.length>0">
                                <br />
                                <b>Extravío:</b>
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>No. Guía</th>
                                            <th> | </th>
                                            <th>Código de Rastreo</th>
                                            <th> | </th>
                                            <th>Tipo de Servicio</th>
                                            <th> | </th>
                                            <th>Pena/Deductiva</th>
                                            <th> | </th>
                                            <th>Eliminar</th>
                                            <th> | </th>
                                            <th>Editar</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="(item,index) in mensajeria.arreglos.arregloExtravio" :key="item.pkExtraviado">
                                            <th>{{item.fcNoGuia}}</th>
                                            <th> | </th>
                                            <th>{{item.fcCodRastreo}}</th>
                                            <th> | </th>
                                            <th>{{item.fcTipoServicio}}</th>
                                            <th> | </th>
                                            <th>{{formatearMonto(item.fiPena)}}</th>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="red"
                                                    @click="eliminarReco(item)">
                                                mdi-delete-empty
                                            </v-icon>
                                            <th> | </th>
                                            <v-icon class="mr-2"
                                                    color="blue"
                                                    @click="editarRecoDiag(index)">
                                                mdi-pencil
                                            </v-icon>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <br />
                        </v-col>
                    </v-row>

                    <v-snackbar v-model="snackbar"
                                :color="colorSnackbar"
                                :multi-line="true"
                                :timeout="3000">
                        {{ mensaje}}
                        <v-btn dark
                               text
                               @click="snackbar = false">
                            Cerrar
                        </v-btn>
                    </v-snackbar>
                </v-container>
            </v-tab-item>

            <!--tab 4 Material y Uniforme-->

            <v-tab-item :key="4"
                        :value="'tab-4'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(mensajeria.cedula.fcPenaMaterial + mensajeria.cedula.fcPenaUniforme)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2" v-if="mensajeria.arreglos.objetoMaterialUniforme.length==0">
                                        Sin incidencias en el mes
                                    </div>
                                    <div v-if="mensajeria.arreglos.objetoMaterialUniforme.length>0">
                                        <div class="text--primary pb-2">
                                            <b>Incidencia Material:</b> Tuvo {{mensajeria.arreglos.objetoMaterialUniforme[0].fiDiasMaterial}} días de atraso al entregar material de embalaje.
                                        </div>
                                        <div class="text--primary pb-2">
                                            <b>Incidencia Uniforme:</b> No cumplió con el uniforme uno o varios días del mes.
                                        </div>
                                    </div>
                                </v-card-text>
                            </v-card>


                        </v-col>
                    </v-row>

                    <v-snackbar v-model="snackbar"
                                :color="colorSnackbar"
                                :multi-line="true"
                                :timeout="3000">
                        {{ mensaje}}
                        <v-btn dark
                               text
                               @click="snackbar = false">
                            Cerrar
                        </v-btn>
                    </v-snackbar>
                </v-container>
            </v-tab-item>



            <!--Archivos-->

            <v-tab-item :key="5"
                        :value="'tab-5'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Archivos Enviados junto con la Cédula
                            </h3>
                        </v-col>
                    </v-row>
                    <div v-if="mensajeria.documentos != null">
                        <v-row>
                            <v-col>
                                <v-card class="mx-auto">
                                    <v-card-text>
                                        <v-btn link :to="{ path: '/api/Documents/ArchivoCedula/' + mensajeria.documentos.fcNombreOriginal}"
                                               target="_blank"
                                               color="red lighten-2"
                                               dark>

                                            <v-icon>mdi-download </v-icon>

                                            Descargar
                                        </v-btn>
                                    </v-card-text>
                                </v-card>
                            </v-col>
                        </v-row>
                    </div>
                    <div v-else>
                        <v-row>
                            <v-col>
                                <v-card class="mx-auto">
                                    <v-card-text>
                                        No se encontraron documentos para descargar
                                    </v-card-text>
                                </v-card>
                            </v-col>
                        </v-row>
                    </div>
                </v-container>
            </v-tab-item>

        </v-tabs>


        {{mensajeria}}
        <br />

    </v-card>
</template>

<script>
    import CedulasService from "../services/cedulas-service";
    import MensajeriaService from "../services/mensajeria-service";


    export default {

        name: 'MensajeriaID',
        data: () => ({
            mensajeria: [],
            message: '',
            tab: null,
            snackbar: false,
            mensaje: '',
            colorSnackbar: 'success'
        }),
        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
            rol() {
                return this.$store.state.auth.user.newDataSet.roles.rol_rd;
            }
        },
        mounted: function () {
            this.getMensajeria(this.$route.params.id);
        },
        methods: {
            dateParse(fecha) {
                var parts = fecha.split('-');
                const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
                var final = parts[2] + " " + monthNames[parseInt(parts[1]) - 1] + " " + parts[0];
                return final;
            },

            aceptar() {
                MensajeriaService.aceptarMensajeria(this.mensajeria.cedula.fcNoCed).then(
                    (g) => {
                        console.log("Guardado");
                        console.log(g);
                        this.mensaje = 'Cédula Aceptada';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                        setTimeout(() => { location.reload(); }, 2000);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                )
            },

            rechazar() {
                MensajeriaService.rechazarMensajeria(this.mensajeria.cedula.fcNoCed).then(
                    (g) => {
                        console.log("Guardado");
                        console.log(g);
                        this.mensaje = 'Cédula Rechazada';
                        this.colorSnackbar = 'red';
                        this.snackbar = true;
                        setTimeout(() => { location.reload(); }, 2000);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                )
            },

            

            getMensajeria(id) {
                this.loading = true;
                CedulasService.getCedula("Mensajeria", id).then(
                    (resp) => {
                        console.log("Resultado:")
                        console.log(resp);
                        this.mensajeria = resp;
                    },
                    error => {
                        console.log('error:' + error);
                        this.message = error.message;
                    }
                );
            },

            formatearMonto(monto) {
                console.log("monto Recibido: ", monto);
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                });
                return formatter.format(monto);
            },

            eliminar() {
                console.log('eliminar proveedor');
                this.loading = false;
                var result = confirm("¿Esta seguro de eliminar este proveedor?");
                if (result == true) {
                    console.log('A services se está enviando este ID: ', this.proveedor[0].pkProveedor)
                    //ProveedoresService.eliminarProveedor(this.proveedor[0]).then(
                    //    p => {
                    //        console.log(p);
                    //        this.loading = false;
                    //        this.mensaje = 'Proveedor eliminado';
                    //        this.colorSnackbar = 'success';
                    //        this.snackbar = true;
                    //        setTimeout(() => { this.$router.push({ name: 'ProveedoresList' }) }, 2000);
                    //    }, error => {
                    //        console.log(error);
                    //        this.loading = false;
                    //        this.mensaje = 'Error';
                    //        this.colorSnackbar = 'error';
                    //        this.snackbar = true;
                    //    }
                    //);
                }
            },
            abrirFicha() {
                console.log(this.proveedor[0].fiProveedor);
                window.open("/Ficha/Generarpdf/" + this.proveedor[0].fiProveedor, "_blank", "");
            },
            editar() {
                console.log('editar proveedor');
                this.$router.push({ name: 'ProveedorNuevo' });
            },
        }
    }
</script>
<style scoped>
</style>