<template>
    <v-card outlined>
        <v-card-title>Cédula de Limpieza No. {{limpieza.cedula.fcNoCed}}</v-card-title>

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
                Actividades del PO
            </v-tab>

            <v-tab :key="3"
                   :href="`#tab-3`">
                Uniforme y Equipo
            </v-tab>

            <v-tab :key="4"
                   :href="`#tab-4`">
                Preguntas Adicionales
            </v-tab>


            <v-tab v-if="limpieza.cedula.fcMes=='Enero'"
                   :key="5"
                   :href="`#tab-5`">
                Primer Mes
            </v-tab>

            <v-tab :key="6"
                   :href="`#tab-6`">
                Entregables
            </v-tab>

            <v-tab :key="7"
                   :href="`#tab-7`">
                Archivos
            </v-tab>

            <!--tab 1 Informacion-->

            <v-tab-item :key="1"
                        :value="'tab-1'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                No. Factura: {{limpieza.cedula.fcNumFactura}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>

                            <v-card class="mx-auto">
                                <v-card-text>

                                    <div class="text--primary pb-2">
                                        <b>Mes:</b> {{limpieza.cedula.fcMes}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Año:</b> {{limpieza.cedula.fiAnio}}
                                    </div>
                                </v-card-text>
                            </v-card>

                            <v-card class="mt-3">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Inmueble:</b> {{limpieza.cedula.fcInmueble}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Administración:</b> {{limpieza.cedula.fcAdministracion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Monto de la Factura:</b> {{formatearMonto(limpieza.cedula.fiMontoFactura)}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Calificación:</b> {{limpieza.cedula.fiCalificacion}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Llenado por:</b> {{limpieza.cedula.fcUsuario}}
                                    </div>
                                    <div class="text--primary pb-2">
                                        <b>Correo:</b> {{limpieza.cedula.fcCorreoUsuario}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                        <v-col>
                            <h2>Penalizaciones/Deductivas | Total: {{formatearMonto(limpieza.cedula.fcPenaActividades + limpieza.cedula.fcPenaUniforme + limpieza.cedula.fcPenaMaterial + limpieza.cedula.fiPenaCalificacion + limpieza.cedula.fiPenaVisita + limpieza.cedula.fiPenaPO + limpieza.cedula.fiPenaEntregables + limpieza.cedula.fiPenaCapacitacion)}}</h2>
                            <br />
                            <div class="text--primary pb-2">
                                <b>Actividades:</b> {{formatearMonto(limpieza.cedula.fcPenaActividades)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Uniforme:</b> {{formatearMonto(limpieza.cedula.fcPenaUniforme)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Material:</b> {{formatearMonto(limpieza.cedula.fcPenaMaterial)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Calificación:</b> {{formatearMonto(limpieza.cedula.fiPenaCalificacion)}}
                            </div>
                            <div class="text--primary pb-2" v-if="limpieza.cedula.fcMes=='Enero'">
                                <b>Programa de Operación:</b> {{formatearMonto(limpieza.cedula.fiPenaPO)}}
                            </div>
                            <div class="text--primary pb-2">
                                <b>Visita Instalaciones:</b> {{formatearMonto(limpieza.cedula.fiPenaVisita)}}
                            </div>
                            <div class="text--primary pb-2" v-if="limpieza.cedula.fiPenaEntregables!=0">
                                <b>Entregables:</b> {{formatearMonto(limpieza.cedula.fiPenaEntregables)}}
                            </div>
                            <div class="text--primary pb-2" v-if="limpieza.cedula.fiPenaCapacitacion!=0">
                                <b>Capacitación:</b> {{formatearMonto(limpieza.cedula.fiPenaCapacitacion)}}
                            </div>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <div v-if="limpieza.cedula.fiEstatus==1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()">
                                <v-icon left>mdi-clipboard-check-multiple</v-icon> Aceptar Cédula
                            </v-btn>
                            <v-btn color="red darken-3" dark @click="rechazar()">
                                <v-icon left>mdi-close-box-multiple-outline</v-icon> Rechazar Cédula
                            </v-btn>
                            <v-btn color="green" dark :href='"/api/Documents/GenerarReporteLimpieza/" + limpieza.cedula.fcNoCed' target="_blank">
                                <v-icon left>mdi-file-excel</v-icon> Generar Reporte
                            </v-btn>
                        </div>
                        <div v-if="limpieza.cedula.fiEstatus!=1">
                            <v-btn color="light-green darken-1" dark @click="aceptar()" :disabled="limpieza.cedula.fiRecalculado != 1 ? true : false">
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

            <!--tab 2 Actividades-->

            <v-tab-item :key="2"
                        :value="'tab-2'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(limpieza.cedula.fcPenaActividades)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <b>Incidencias en el Programa de Operación:</b> {{limpieza.arreglos.arregloActi.length}}
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="limpieza.arreglos.arregloActi.length>0">

                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Tipo Incidencia</th>
                                            <th> | </th>
                                            <th>Área Incidencia</th>
                                            <th> | </th>
                                            <th>Fecha</th>
                                            <th> | </th>
                                            <th>Comentarios</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="item in limpieza.arreglos.arregloActi" :key="item.fcTipo">
                                            <th>{{item.fcTipo}}</th>
                                            <th> | </th>
                                            <th>{{item.fcArea}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaInci.substring(0, 10))}}</th>
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

            <!--tab 3 Uniforme y equipo-->

            <v-tab-item :key="3"
                        :value="'tab-3'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(limpieza.cedula.fcPenaUniforme + limpieza.cedula.fcPenaMaterial)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2" v-if="limpieza.cedula.fcPenaMaterial==0 && limpieza.cedula.fcPenaUniforme==null">
                                        Sin incidencias en el mes
                                    </div>
                                    <div v-else>
                                        <div class="text--primary pb-2" v-if="limpieza.cedula.fcPenaUniforme == null">
                                            <b>Sin incidencias de uniforme en el periodo.</b>
                                        </div>
                                        <div class="text--primary pb-2" v-else>
                                            <b>El personal no porto el uniforme y/o gafete en algún momento del periodo.</b>
                                        </div>
                                        <div class="text--primary pb-2">
                                            <b>Incidencias de Equipo:</b> {{limpieza.arreglos.arregloEquipo.length}}
                                        </div>
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Incidencias:</h2>
                            <br />
                            <div class="text--primary pb-2" v-if="limpieza.arreglos.arregloEquipo.length>0">
                                <table class="table table-sm table-hover">
                                    <thead>
                                        <tr>
                                            <th>Equipo Faltante</th>
                                            <th> | </th>
                                            <th>Fecha Incidencia</th>
                                            <th> | </th>
                                            <th>Comentarios</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr v-for="item in limpieza.arreglos.arregloEquipo" :key="item.fcMaquina">
                                            <th>{{item.fcMaquina}}</th>
                                            <th> | </th>
                                            <th>{{dateParse(item.fdFechaInci.substring(0, 10))}}</th>
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

            <!--tab 4 Preguntas Adicionales-->

            <v-tab-item :key="4"
                        :value="'tab-4'">
                <v-container>
                    <div v-if="limpieza.cedula.fiRecalculado != 1">

                        <v-row>
                            <v-col>
                                <h5>
                                    Si durante el periodo no se requirieron estos puntos, conteste "Sí"
                                </h5>
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-col>
                                <h3>
                                    1. ¿El proveedor entrego en tiempo los entregables bajo demanda del periodo?
                                </h3>
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-radio-group v-model="pAdicionales.entregables"
                                           @change="aNull()"
                                           row>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-row>
                        <div v-if="pAdicionales.entregables=='false'">
                            <v-row>
                                <v-col>
                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Límite de Entrega</span>
                                    <vc-date-picker v-model="pAdicionales.fechaLimite"
                                                    :mask="masks"
                                                    :locale="{firstDayOfWeek: 1}"
                                                    :disabled-dates='{ weekdays: [1, 7] }'>

                                        <template v-slot="{ inputValue, inputEvents }">
                                            <v-text-field readonly
                                                          outlined
                                                          dense
                                                          label="Ingrese Fecha"
                                                          :value="inputValue"
                                                          v-on="inputEvents" />
                                        </template>

                                    </vc-date-picker>
                                </v-col>

                                <v-col>
                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de Entrega</span>
                                    <vc-date-picker v-model="pAdicionales.fechaEntrega"
                                                    :mask="masks"
                                                    :locale="{firstDayOfWeek: 1}"
                                                    :disabled-dates='{ weekdays: [1, 7] }'>

                                        <template v-slot="{ inputValue, inputEvents }">
                                            <v-text-field readonly
                                                          outlined
                                                          dense
                                                          label="Ingrese Fecha"
                                                          :value="inputValue"
                                                          v-on="inputEvents" />
                                        </template>

                                    </vc-date-picker>
                                </v-col>

                                <v-col>
                                    <v-radio-group v-model="pAdicionales.tipoEntregable"
                                                   row>
                                        <v-radio label="Normales"
                                                 value="1"></v-radio>
                                        <v-radio label="Urgentes"
                                                 value="2"></v-radio>
                                    </v-radio-group>
                                </v-col>
                                <v-col></v-col>
                            </v-row>
                        </div>


                        <v-row>
                            <v-col>
                                <h3>
                                    2. ¿El proveedor demostró que su personal estaba debidamente capacitado?
                                </h3>
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-radio-group v-model="pAdicionales.capacitacion"
                                           row>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-row>

                        <template>
                            <v-row justify="space-around">
                                <v-col cols="auto">
                                    <v-dialog transition="dialog-bottom-transition"
                                              max-width="600">
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-btn color="teal darken-1"
                                                   dark
                                                   v-bind="attrs"
                                                   :disabled="pAdicionales.entregables != null && pAdicionales.capacitacion != null ? false : true"
                                                   v-on="on">Recalcular</v-btn>
                                        </template>
                                        <template v-slot:default="dialog">
                                            <v-card>
                                                <v-toolbar color="primary"
                                                           dark>Recalcular Penalizaciones/Deductivas</v-toolbar>
                                                <v-card-text>
                                                    <div class="text-h5 pa-12">¿La información es correcta? No podrá ser modificada dentro del sistema</div>
                                                </v-card-text>
                                                <v-card-actions class="justify-end">
                                                    <v-btn text
                                                           @click="dialog.value = false">Cancelar</v-btn>
                                                    <v-btn text
                                                           @click="recalcular()">Recalcular</v-btn>
                                                </v-card-actions>
                                            </v-card>
                                        </template>
                                    </v-dialog>
                                </v-col>
                            </v-row>
                        </template>

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
                    </div>

                    <div v-else>
                        <v-row>
                            <v-col>
                                <h3>
                                    Penalización/Deductiva: {{formatearMonto(limpieza.cedula.fiPenaEntregables + limpieza.cedula.fiPenaCapacitacion)}}
                                </h3>
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-col>
                                <v-card class="mx-auto">
                                    <v-card-text>
                                        <div class="text--primary pb-2" v-if="limpieza.cedula.fiPenaEntregables!=0">
                                            <b>El proveedor no entregó sus entregables bajo demanda por lo que su penalización es de {{formatearMonto(limpieza.cedula.fiPenaEntregables)}}.</b>
                                        </div>
                                        <div class="text--primary pb-2" v-else>
                                            <b>Entrego los entregables bajo demanda en tiempo y forma.</b>
                                        </div>
                                        <div class="text--primary pb-2" v-if="limpieza.cedula.fiPenaCapacitacion!=0">
                                            <b>El proveedor no pudo comprobar que su personal esta debidamente capacitado por lo que su penalización es de {{formatearMonto(limpieza.cedula.fiPenaCapacitacion)}}.</b>
                                        </div>
                                        <div class="text--primary pb-2" v-else>
                                            <b>El proveedor comprobó la capacitación de su personal.</b>
                                        </div>
                                    </v-card-text>
                                </v-card>
                            </v-col>
                        </v-row>
                    </div>
                </v-container>
            </v-tab-item>

            <!--Primer mes-->

            <v-tab-item :key="5"
                        :value="'tab-5'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Penalización/Deductiva: {{formatearMonto(limpieza.cedula.fiPenaPO + limpieza.cedula.fiPenaVisita)}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <div v-if="limpieza.entregable.fdProgramaOperacion!=null">
                                            <b>El prestador entregó su Programa de operacion en la fecha {{dateParse(limpieza.entregable.fdProgramaOperacion.substring(0, 10))}} por lo que su penalización es de {{formatearMonto(limpieza.cedula.fiPenaPO)}}</b>
                                        </div>
                                        <br /><br />
                                        <div v-if="limpieza.entregable.fdVisitaInstalaciones!=null">
                                            <b>Visitó las instalaciones el {{dateParse(limpieza.entregable.fdVisitaInstalaciones.substring(0, 10))}} por lo que su penalización es de {{formatearMonto(limpieza.cedula.fiPenaVisita)}}</b>
                                        </div>
                                        <br /><br />
                                        <div v-if="limpieza.entregable.fdGafeteUniforme!=null">
                                            <b>Entregó los uniformes y gafetes de su personal el {{dateParse(limpieza.entregable.fdGafeteUniforme.substring(0, 10))}}; no está sujeto a penalización</b>
                                        </div>
                                        <br /><br />
                                        <div v-if="limpieza.entregable.fdActaInicio!=null">
                                            <b>Y se formalizó el acta de Inicio del Servicio el {{dateParse(limpieza.entregable.fdActaInicio.substring(0, 10))}}; tampoco esta sujeto a penalización</b>
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

            <!--Entregables-->

            <v-tab-item :key="6"
                        :value="'tab-6'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Fecha de Cierre de Mes Establecida: {{dateParse(limpieza.entregable.fdCierreMes.substring(0, 10))}}
                            </h3>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col>
                            <v-card class="mx-auto">
                                <v-card-text>
                                    <div class="text--primary pb-2">
                                        <div v-if="limpieza.entregable.fdSUAIMSS != null">
                                            <b>El comprobante de pago y las constancias de inscripción al IMSS (SUA) se entregó el {{dateParse(limpieza.entregable.fdSUAIMSS.substring(0, 10))}}; debe de entregarse el 22 de cada mes</b>
                                        </div>
                                        <div v-else>
                                            <b>El comprobante de pago y las constancias de inscripción al IMSS (SUA) NO fueron entregadas</b>
                                        </div>
                                        <br /><br />
                                        <div v-if="limpieza.entregable.fdActaServicios != null">
                                            <b>El Acta Entrega-Recepción de los servicios mensual se entregó el {{dateParse(limpieza.entregable.fdActaServicios.substring(0, 10))}}</b>
                                        </div>
                                        <div v-else>
                                            <b>El Acta Entrega-Recepción de los servicios mensual NO se entregó</b>
                                        </div>
                                    </div>
                                </v-card-text>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-container>
            </v-tab-item>

            <!--Archivos-->

            <v-tab-item :key="7"
                        :value="'tab-7'">
                <v-container>
                    <v-row>
                        <v-col>
                            <h3>
                                Archivos Enviados junto con la Cédula
                            </h3>
                        </v-col>
                    </v-row>
                    <div v-if="limpieza.documentos != null">
                        <v-row>
                            <v-col>
                                <v-card class="mx-auto">
                                    <v-card-text>
                                        <v-btn link :to="{ path: '/api/Documents/ArchivoCedula/' + limpieza.documentos.fcNombreOriginal}"
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
        
        {{limpieza}}
    </v-card>
</template>

<script>
    import CedulasService from "../services/cedulas-service";
    import LimpiezaService from "../services/limpieza-service";


    export default {

        name: 'LimpiezaID',
        data: () => ({
            limpieza: [],
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
            this.getLimpieza(this.$route.params.id);
            
        },

        methods: {
            aNull() {
                this.pAdicionales.fechaLimite = null;
                this.pAdicionales.fechaEntrega = null;
                this.pAdicionales.tipoEntregable = null;
            },

            recalcular() {
                LimpiezaService.recalcular(this.pAdicionales, this.limpieza.cedula.fcNoCed).then(
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

            dateParse(fecha) {
                var parts = fecha.split('-');
                const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
                var final = parts[2] + " " + monthNames[parseInt(parts[1]) - 1] + " " + parts[0];
                return final;
            },

            aceptar() {
                LimpiezaService.aceptarLimpieza(this.limpieza.cedula.fcNoCed).then(
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
                LimpiezaService.rechazarLimpieza(this.limpieza.cedula.fcNoCed).then(
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



            getLimpieza(id) {
                this.loading = true;
                CedulasService.getCedula("Limpieza", id).then(
                    (resp) => {
                        console.log("Resultado:")
                        console.log(resp);
                        this.limpieza = resp;
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