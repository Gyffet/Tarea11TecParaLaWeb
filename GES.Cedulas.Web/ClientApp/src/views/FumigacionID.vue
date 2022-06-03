<template>
    <v-card outlined>
        <v-card-title>Cédula de Fumigación No. {{fumigacion.cedula.fcNoCed}}</v-card-title>

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
                Fechas y Horas
            </v-tab>

            <v-tab :key="3"
                   :href="`#tab-3`">
                Efectividad
            </v-tab>

            <v-tab :key="4"
                   :href="`#tab-4`">
                Etiquetado
            </v-tab>


            <v-tab v-if="fumigacion.cedula.fcMes=='Enero'"
                   :key="5"
                   :href="`#tab-5`">
                Primer Mes
            </v-tab>

            <v-tab :key="6"
                   :href="`#tab-6`">
                Archivos
            </v-tab>

            <!--tab 1 Informacion-->

            <v-tab-item :key="1"
                        :value="'tab-1'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                No. Factura: {{fumigacion.cedula.fcNumFactura}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>

                            <v-card class="mx-auto">
                                <v-card-text>

                                    <div class="text--primary pb-2">
                                        <b>Mes:</b> {{fumigacion.cedula.fcMes}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Año:</b> {{fumigacion.cedula.fiAnio}}
                                    </div>
                                </v-card-text>
                            </v-card>

                            <v-card class="mt-3">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Inmueble:</b> {{fumigacion.cedula.fcInmueble}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Administración:</b> {{fumigacion.cedula.fcAdministracion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Monto de la Factura:</b> {{formatearMonto(fumigacion.cedula.fiMontoFactura)}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Cierre del Mes:</b> {{dateParse(fumigacion.entregable.fdCierreMes.substring(0, 10))}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Calificación:</b> {{fumigacion.cedula.fiCalificacion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Llenado por:</b> {{fumigacion.cedula.fcUsuario}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Correo:</b> {{fumigacion.cedula.fcCorreoUsuario}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <v-col>
                            <h2>Penalizaciones/Deductivas | Total: {{formatearMonto(fumigacion.cedula.fiPenaFechas + fumigacion.cedula.fiPenaHorario + fumigacion.cedula.fiPenaFumigacion + fumigacion.cedula.fiPenaEtiquetas + fumigacion.cedula.fiPenaCalificacion + fumigacion.cedula.fiPenaPO)}}</h2>
                            <br />
                            <div class="text--primary pb-2">
                                <b>Fechas Actividades:</b> {{formatearMonto(fumigacion.cedula.fiPenaFechas)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Horas Actividades:</b> {{formatearMonto(fumigacion.cedula.fiPenaHorario)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Efectividad:</b> {{formatearMonto(fumigacion.cedula.fiPenaFumigacion)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Etiquetas:</b> {{formatearMonto(fumigacion.cedula.fiPenaEtiquetas)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Calificación:</b> {{formatearMonto(fumigacion.cedula.fiPenaCalificacion)}}
                            </div>
                            <div class="text--primary pb-2" v-if="fumigacion.cedula.fcMes=='Enero'">
                                <b>Programa de Operación:</b> {{formatearMonto(fumigacion.cedula.fiPenaPO)}}
                            </div>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <div v-if="fumigacion.cedula.fiEstatus==1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()">
                                <v-icon left>mdi-clipboard-check-multiple</v-icon> Aceptar Cédula
                            </v-btn>
                            <v-btn color="red darken-3" dark @click="rechazar()">
                                <v-icon left>mdi-close-box-multiple-outline</v-icon> Rechazar Cédula
                            </v-btn>
                            <v-btn color="green" dark :href='"/api/Documents/GenerarReporteLimpieza/" + fumigacion.cedula.fcNoCed' target="_blank">
                                <v-icon left>mdi-file-excel</v-icon> Generar Reporte
                            </v-btn>
                        </div>
                        <div v-if="fumigacion.cedula.fiEstatus!=1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()" :disabled="fumigacion.cedula.fiRecalculado != 1 ? true : false">
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

            <!--tab 2 Fechas y horas-->

            <v-tab-item :key="2"
                        :value="'tab-2'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(fumigacion.cedula.fiPenaFechas + fumigacion.cedula.fiPenaHorario)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias relacionadas a las FECHAS:</b> {{fumigacion.arreglos.arregloFechas.length}}
                                        <br />
                                        <b>Incidencias relacionadas a las HORAS:</b> {{fumigacion.arreglos.arregloHoras.length}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="fumigacion.arreglos.arregloFechas.length>0">

                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Fecha Programada</th>
                                            <th> | </th>
                                            <th>Fecha Realizada</th>
                                            <th> | </th>
                                            <th>Comentarios</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="item in fumigacion.arreglos.arregloFechas" :key="item.pkFecha">
                                            <th></th>
                                            <th> | </th>
                                            <th></th>
                                            <th> | </th>
                                            <th>{{item.fcComentarios}}</th>
                                        </tr>
                                    </tbody>

                                </table>
                            </div>

                            <br />
                            <div class="text--primary pb-2" v-if="fumigacion.arreglos.arregloHoras.length>0">

                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Fecha Programada</th>
                                            <th> | </th>
                                            <th>Hora Programada</th>
                                            <th> | </th>
                                            <th>Hora Realizada</th>
                                            <th> | </th>
                                            <th>Comentarios</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="item in fumigacion.arreglos.arregloHoras" :key="item.pkHoras">
                                            <th></th>
                                            <th> | </th>
                                            <th>{{timeParse(item.fdHoraProgramada)}}</th>
                                            <th> | </th>
                                            <th>{{timeParse(item.fdHoraEfectiva)}}</th>
                                            <th> | </th>
                                            <th>{{item.fcComentarios}}</th>
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

            <!--tab 3 Efectividad-->

            <v-tab-item :key="3"
                        :value="'tab-3'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(fumigacion.cedula.fiPenaFumigacion)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2" v-if="fumigacion.arreglos.arregloFumigacion.length > 0">
                                        Incidencias en el mes: {{fumigacion.arreglos.arregloFumigacion.length}}
                                    </div>
                                    <div v-else class="text--primary pb-2">

                                        <b>Sin incidencias en el mes.</b>

                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="fumigacion.arreglos.arregloFumigacion.length>0">
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Fecha Fumigación</th>
                                            <th> | </th>
                                            <th>Fecha Reaparición</th>
                                            <th> | </th>
                                            <th>Comentarios</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="item in fumigacion.arreglos.arregloFumigacion" :key="item.pkFumigacion">
                                            <th></th>
                                            <th> | </th>
                                            <th></th>
                                            <th> | </th>
                                            <th>{{item.fcComentarios}}</th>
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

            <!--tab 4 Etiquetado-->

            <v-tab-item :key="4"
                        :value="'tab-4'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(fumigacion.cedula.fiPenaEtiquetas)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2" v-if="fumigacion.cedula.fiPenaFumigacion > 0">
                                        El prestador de servicios no cumplio con la regulación vigente en su etiquetado o bien los productos que utilizó estaban caducados, por lo que su penalización es de {{fumigacion.cedula.fiPenaEtiquetas}}
                                    </div>
                                    <div v-else class="text--primary pb-2">
                                        <b>el prestador de servicios cumplió con la regulación vigente de etiquetado de sus productos y no estaban caducados</b>
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>

                </v-container>
            </v-tab-item>
            <!--Primer mes-->

            <v-tab-item :key="5"
                        :value="'tab-5'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(fumigacion.cedula.fiPenaPO)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <div v-if="fumigacion.entregable.fdProgramaOperacion != null">
                                            El prestador entregó su Programa de operacion el {{dateParse(fumigacion.entregable.fdProgramaOperacion.substring(0, 10))}}
                                        </div>
                                        <div v-else>
                                            El prestador NO entregó su Programa de operacion
                                        </div>
                                        <br /><br />
                                        <div v-if="fumigacion.entregable.fdUniforme != null">
                                            Entregó los uniformes y gafetes de su personal el {{dateParse(fumigacion.entregable.fdUniforme.substring(0, 10))}}
                                        </div>
                                        <div v-else>
                                            NO entregó los uniformes y gafetes de su personal
                                        </div>
                                        <br /><br />
                                        <div v-if="fumigacion.entregable.fdProgramaOperacion != null">
                                            Entregó el acta de inicio de servicios el {{dateParse(fumigacion.entregable.fdActaInicio.substring(0, 10))}}
                                        </div>
                                        <div v-else>
                                            NO entregó el acta de inicio de servicios
                                        </div>
                                        <br /><br />
                                        <div v-if="fumigacion.entregable.fdProgramaOperacion != null">
                                            Y el listado del personal asignado lo entregó el {{dateParse(fumigacion.entregable.fdPersonal.substring(0, 10))}}
                                        </div>
                                        <div v-else>
                                            Y NO entregó el listado del personal asignado
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

            <v-tab-item :key="6"
                        :value="'tab-6'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Archivos Enviados junto con la Cédula
                            </h3>
                        </v-col>
                    </v-row>
                    <div v-if="fumigacion.documentos != null">
                        <v-row>
                            <v-col>
                                <v-card class="mx-auto">
                                    <v-card-text>
                                        <v-btn link :to="{ path: '/api/Documents/ArchivoCedula/' + fumigacion.documentos.fcNombreOriginal}"
                                               target="_blank"
                                               color="teal darken-1"
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

        <br />

    </v-card>
</template>

<script>
    import CedulasService from "../services/cedulas-service";
    import FumigacionService from "../services/fumigacion-service";


    export default {

        name: 'FumigacionID',
        data: () => ({
            fumigacion: [],
            message: '',
            tab: null,
            snackbar: false,
            mensaje: '',
            colorSnackbar: 'success',
            masks: {
                input: 'YYYY-MM-DD',
            },
            pAdicionales: {
                entregables: null,
                fechaLimite: null,
                fechaEntrega: null,
                tipoEntregable: null,
                capacitacion: null,
            }
        }),
        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
            rol() {
                return this.$store.state.auth.user.newDataSet.roles.rol_rd;
            },
        },

        mounted: function () {
            this.getFumigacion(this.$route.params.id);
        },

        methods: {
            getFumigacion(id) {
                this.loading = true;
                CedulasService.getCedula("Fumigacion", id).then(
                    (resp) => {
                        console.log("Resultado:")
                        console.log(resp);
                        this.fumigacion = resp;
                    },
                    error => {
                        console.log('error:' + error);
                        this.message = error.message;
                    }
                );
            },

            timeParse(fecha) {
                let now = new Date(fecha);
                return (now.getHours().toString() + now.getMinutes().toString());
            },

            dateParse(fecha) {
                var parts = fecha.split('-');
                const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
                var final = parts[2] + " " + monthNames[parseInt(parts[1]) - 1] + " " + parts[0];
                return final;
            },

            formatearMonto(monto) {
                console.log("monto Recibido: ", monto);
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                });
                return formatter.format(monto);
            },

            recalcular() {
                FumigacionService.recalcular(this.pAdicionales, this.limpieza.cedula.fcNoCed).then(
                    (g) => {
                        console.log("Desde el serv");
                        console.log(g);
                        setTimeout(() => { location.reload(); }, 1000);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                )
            },

            aceptar() {
                FumigacionService.aceptar(this.fumigacion.cedula.fcNoCed).then(
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
                FumigacionService.rechazar(this.fumigacion.cedula.fcNoCed).then(
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