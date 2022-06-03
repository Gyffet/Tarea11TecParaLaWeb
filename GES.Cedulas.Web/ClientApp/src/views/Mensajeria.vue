<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-1 font-weight-bold mb-3"
                    align="center">
                    cédula Automatizada para la Supervisión y Evaluación del Servicio de Mensajería Acelerada
                </h1>
            </v-col>
        </v-row>
        <v-row>
            <v-col align="left">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Año de evaluación:
                        <v-select id="selectPrimavera"
                                  v-model="opcionesServer.selectPrimavera"
                                  :items="primavera"
                                  label="Año"
                                  dense
                                  outlined></v-select>
                    </v-col>

                    <v-col class="mb-4" align="left">
                        Inmueble a evaluar:
                        <v-select id="selectInmueble"
                                  v-model="opcionesServer.selectInmueble"
                                  :items="inmuebles"
                                  item-text="nombre"
                                  label="Inmueble"
                                  dense
                                  :hint=direccion
                                  @change="setDireccion($event)"
                                  outlined></v-select>
                    </v-col>




                    <v-col class="mb-4" align="left">

                        Mes de evaluación:
                        <div v-if="opcionesServer.selectPrimavera==null || opcionesServer.selectInmueble==null">
                            <v-select label="Mes"
                                      dense
                                      disabled
                                      outlined></v-select>
                        </div>
                        <div v-if="opcionesServer.selectPrimavera!=null && opcionesServer.selectInmueble!=null">
                            <v-select id="selectMes"
                                      v-model="opcionesServer.selectMes"
                                      :items="mes"
                                      label="Mes"
                                      dense
                                      @change="mesVacio=false"
                                      class="form-control"
                                      outlined></v-select>
                        </div>
                    </v-col>
                </div>
            </v-col>
            <v-snackbar v-model="snackbar"
                        :color="colorSnackbar"
                        :multi-line="true"
                        :timeout="5000">
                {{ mensaje }}
                <v-btn dark
                       text
                       @click="snackbar = false">
                    Cerrar
                </v-btn>
            </v-snackbar>
        </v-row>
        <v-row>
            <v-col align="left">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Número de Factura:
                        <v-text-field v-model="datosCed.factura"
                                      required
                                      dense></v-text-field>
                        <!--<v-text-field @change="setFactura($event)"
                    required
                    dense></v-text-field>-->
                    </v-col>

                    <v-col class="mb-4" align="left">
                        Confirmar No. de Factura:
                        <v-text-field required
                                      v-model="repetirFactura"
                                      dense></v-text-field>
                        <!--<v-text-field required
                    @change="setRepFactura($event)"
                    dense></v-text-field>-->
                    </v-col>


                    <v-col class="mb-4" align="left">
                        Monto Factura sin I.V.A.
                        <v-text-field v-model="datosCed.monto"
                                      label="Sin comas ni símbolo de pesos"
                                      :error-messages="validMonto"
                                      @input="$v.datosCed.monto.$touch()"
                                      @blur="$v.datosCed.monto.$touch()"
                                      required
                                      :disabled="mesVacio!=true ? true : false"
                                      dense></v-text-field>
                    </v-col>
                </div>
            </v-col>
        </v-row>
        <v-row>
            <v-col align="left">
                <div v-if="mesVacio != true">
                    <v-btn color="red lighten-2"
                           dark
                           @click="preVerificarPeriodo()">
                        Verificar Periodo
                    </v-btn>

                </div>
            </v-col>
        </v-row>

        <div v-if="opcionesServer.selectMes!=null && mesVacio == true">
            <v-row>
                <v-col class="title row" align="left">
                    1. ¿El prestador del servicio se presentó a la RECOLECCIÓN de guías en tiempo y forma? En caso de existir algún retraso registre la incidencia
                </v-col>
            </v-row>
            <v-row>
                <v-radio-group v-model="datosCed.boolRecoleccion"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>
            <div v-if="datosCed.boolRecoleccion=='false'">
                <v-row align="center" justify="space-around">

                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogReco"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="red lighten-2"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias
                                    </v-card-title>

                                    <v-card-text>
                                        El cálculo de días de retraso se hace automaticamente
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada Recolección</span>
                                                <vc-date-picker v-model="objetoRecoleccion.fechaProgramada"
                                                                required
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


                                            <v-col cols="12"
                                                   md="4">
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Efectiva Recolección</span>
                                                <vc-date-picker v-model="objetoRecoleccion.fechaEfectiva"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :mask="masks"
                                                                :min-date="objetoRecoleccion.fechaProgramada"
                                                                :disabled-dates='{ weekdays: [1, 7] }'
                                                                is-dark>

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



                                        </v-row>
                                        <br /><br /><br /><br />
                                        <v-row>
                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoRecoleccion.noGuiaRec"
                                                              :counter="22"
                                                              :error-messages="validGuiaReco"
                                                              @input="$v.objetoRecoleccion.noGuiaRec.$touch()"
                                                              @blur="$v.objetoRecoleccion.noGuiaRec.$touch()"
                                                              label="Número de Guía"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoRecoleccion.codRastreoRec"
                                                              :counter="10"
                                                              :error-messages="validCodReco"
                                                              @input="$v.objetoRecoleccion.codRastreoRec.$touch()"
                                                              @blur="$v.objetoRecoleccion.codRastreoRec.$touch()"
                                                              label="Código Rastreo"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">

                                                <v-select id="selectTipoRec"
                                                          v-model="objetoRecoleccion.tipoServRec"
                                                          :items="TipoServicio"
                                                          :error-messages="validServReco"
                                                          @input="$v.objetoRecoleccion.tipoServRec.$touch()"
                                                          @blur="$v.objetoRecoleccion.tipoServRec.$touch()"
                                                          label="Tipo de Servicio"
                                                          dense
                                                          outlined></v-select>

                                            </v-col>
                                        </v-row>

                                        <!--<br /><br /><br /><br />
                                {{objetoRecoleccion}}-->
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogReco = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarReco(objetoRecoleccion)">
                                            Agregar
                                        </v-btn>
                                    </v-card-actions>
                                </v-card>



                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>





                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogRecoExcel"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="green"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        <v-icon dark
                                                left>
                                            mdi-file-excel
                                        </v-icon>
                                        Agregar por Excel
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Subir Incidencias Recolección por Excel
                                    </v-card-title>

                                    <v-card-text>
                                        Por favor llenar de acuerdo a la platilla
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <input class="form-control" type="file" id="input" accept=".xls,.xlsx" @change="subirExcelReco($event)">

                                            </v-col>
                                        </v-row>
                                        <br />
                                        <v-row align="center" justify="space-around">

                                            <v-btn color="green"
                                                   dark
                                                   :href='"/api/Documents/DescargarArchivo/MensajeriaReco-Entr.xlsx"'
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-excel
                                                </v-icon>
                                                Plantilla Recolección
                                            </v-btn>


                                            <v-btn color="red"
                                                   dark
                                                   link :to="{ path: '/api/Documents/DescargarArchivo/Manual_Mensajeria_Recoleccion_Entrega.pdf' }"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-pdf
                                                </v-icon>
                                                Manual Plantilla
                                            </v-btn>

                                        </v-row>

                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogRecoExcel = false">
                                            Terminar
                                        </v-btn>

                                    </v-card-actions>
                                </v-card>



                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>

                    <div>
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogRegReco"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="red lighten-2"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Revisar Registros
                                        </v-btn>
                                    </template>

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Registros Incidencias
                                        </v-card-title>

                                        <v-card-text>
                                            <v-row align="center">
                                                <v-col align="center">
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
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloRecoleccion" :key="item.noGuiaRec">
                                                                <th>{{item.noGuiaRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.codRastreoRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.tipoServRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fechaPrograFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fehcaEfecFormat}}</th>
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
                                                </v-col>
                                            </v-row>

                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogRegReco = false">
                                                Cerrar
                                            </v-btn>

                                        </v-card-actions>
                                    </v-card>
                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>

                    <div v-if="dialogEditarReco==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarReco"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            El cálculo de días de retraso se hace automaticamente
                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada Recolección</span>
                                                    <vc-date-picker v-model="objetoRecoleccion.fechaProgramada"
                                                                    required
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


                                                <v-col cols="12"
                                                       md="4">
                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Efectiva Recolección</span>
                                                    <vc-date-picker v-model="objetoRecoleccion.fechaEfectiva"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :mask="masks"
                                                                    :min-date="objetoRecoleccion.fechaProgramada"
                                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                                    is-dark>

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



                                            </v-row>
                                            <br /><br /><br />
                                            <v-row>
                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoRecoleccion.noGuiaRec"
                                                                  :counter="22"
                                                                  :error-messages="validGuiaReco"
                                                                  @input="$v.objetoRecoleccion.noGuiaRec.$touch()"
                                                                  @blur="$v.objetoRecoleccion.noGuiaRec.$touch()"
                                                                  label="Número de Guía"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoRecoleccion.codRastreoRec"
                                                                  :counter="10"
                                                                  :error-messages="validCodReco"
                                                                  @input="$v.objetoRecoleccion.codRastreoRec.$touch()"
                                                                  @blur="$v.objetoRecoleccion.codRastreoRec.$touch()"
                                                                  label="Código Rastreo"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">

                                                    <v-select id="selectTipoRec"
                                                              v-model="objetoRecoleccion.tipoServRec"
                                                              :items="TipoServicio"
                                                              :error-messages="validServReco"
                                                              @input="$v.objetoRecoleccion.tipoServRec.$touch()"
                                                              @blur="$v.objetoRecoleccion.tipoServRec.$touch()"
                                                              label="Tipo de Servicio"
                                                              dense
                                                              outlined></v-select>

                                                </v-col>
                                            </v-row>

                                            <!--<br /><br /><br /><br />
                                    {{objetoRecoleccion}}-->
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarReco = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarReco(objetoRecoleccion)">
                                                Editar
                                            </v-btn>
                                        </v-card-actions>
                                    </v-card>



                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>
                </v-row>
            </div>

            <v-row>
                <v-col class="title row" align="left">
                    2. ¿El prestador del servicio se presentó a la ENTREGA de guías en tiempo y forma? En caso de existir algún retraso registre la incidencia
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolEntrega"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>
            <div v-if="datosCed.boolEntrega=='false'">
                <v-row align="center" justify="space-around">


                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogEntr"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="red lighten-2"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias
                                    </v-card-title>

                                    <v-card-text>
                                        El cálculo de días de retraso se hace automaticamente
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada Recolección</span>
                                                <vc-date-picker v-model="objetoEntrega.fechaProgramada"
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


                                            <v-col cols="12"
                                                   md="4">
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Efectiva Recolección</span>
                                                <vc-date-picker v-model="objetoEntrega.fechaEfectiva"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :mask="masks"
                                                                :min-date="objetoEntrega.fechaProgramada"
                                                                :disabled-dates='{ weekdays: [1, 7] }'
                                                                is-dark>

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



                                        </v-row>
                                        <br /><br /><br />
                                        <v-row>
                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoEntrega.noGuiaRec"
                                                              :counter="22"
                                                              label="Número de Guía"
                                                              :error-messages="validGuiaEntr"
                                                              @input="$v.objetoEntrega.noGuiaRec.$touch()"
                                                              @blur="$v.objetoEntrega.noGuiaRec.$touch()"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoEntrega.codRastreoRec"
                                                              :counter="10"
                                                              label="Código Rastreo"
                                                              :error-messages="validCodEntr"
                                                              @input="$v.objetoEntrega.codRastreoRec.$touch()"
                                                              @blur="$v.objetoEntrega.codRastreoRec.$touch()"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">

                                                <v-select id="selectTipoRec"
                                                          v-model="objetoEntrega.tipoServRec"
                                                          :items="TipoServicio"
                                                          label="Tipo de Servicio"
                                                          :error-messages="validServEntr"
                                                          @input="$v.objetoEntrega.tipoServRec.$touch()"
                                                          @blur="$v.objetoEntrega.tipoServRec.$touch()"
                                                          dense
                                                          outlined></v-select>

                                            </v-col>
                                        </v-row>

                                        <!--<br /><br /><br /><br />
                                {{objetoEntrega}}-->
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogEntr = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarEntr(objetoEntrega)">
                                            Agregar
                                        </v-btn>
                                    </v-card-actions>
                                </v-card>
                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>


                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogEntrExcel"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="green"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        <v-icon dark
                                                left>
                                            mdi-file-excel
                                        </v-icon>
                                        Agregar por Excel
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Subir Incidencias Entrega por Excel
                                    </v-card-title>

                                    <v-card-text>
                                        Por favor llenar de acuerdo a la platilla
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <input class="form-control" type="file" id="input" accept=".xls,.xlsx" @change="subirExcelEntr($event)">

                                            </v-col>_
                                        </v-row>

                                        <br />
                                        <v-row align="center" justify="space-around">

                                            <v-btn color="green"
                                                   dark
                                                   link :to="{ path: '/api/Documents/DescargarArchivo/MensajeriaReco-Entr.xlsx'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-excel
                                                </v-icon>
                                                Plantilla Entrega
                                            </v-btn>

                                            <v-btn color="red"
                                                   dark
                                                   link :to="{ path: '/api/Documents/DescargarArchivo/Manual_Mensajeria_Recoleccion_Entrega.pdf'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-pdf
                                                </v-icon>
                                                Manual Plantilla
                                            </v-btn>

                                        </v-row>

                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogEntrExcel = false">
                                            Terminar
                                        </v-btn>

                                    </v-card-actions>
                                </v-card>

                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>


                    <div>
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogRegEntr"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="red lighten-2"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Revisar Registros
                                        </v-btn>
                                    </template>

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Registros Incidencias
                                        </v-card-title>

                                        <v-card-text>
                                            <v-row align="center">
                                                <v-col align="center">
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
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloEntrega" :key="item.noGuiaRec">
                                                                <th>{{item.noGuiaRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.codRastreoRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.tipoServRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fechaPrograFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fehcaEfecFormat}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarEntr(item)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarEntrDiag(index)">
                                                                    mdi-pencil
                                                                </v-icon>
                                                            </tr>
                                                        </tbody>

                                                    </table>
                                                </v-col>

                                            </v-row>

                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogRegEntr = false">
                                                Cerrar
                                            </v-btn>

                                        </v-card-actions>
                                    </v-card>
                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>


                    <div v-if="dialogEditarEntre==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarEntre"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            El cálculo de días de retraso se hace automaticamente
                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada Recolección</span>
                                                    <vc-date-picker v-model="objetoEntrega.fechaProgramada"
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


                                                <v-col cols="12"
                                                       md="4">
                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Efectiva Recolección</span>
                                                    <vc-date-picker v-model="objetoEntrega.fechaEfectiva"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :mask="masks"
                                                                    :min-date="objetoEntrega.fechaProgramada"
                                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                                    is-dark>

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



                                            </v-row>
                                            <br /><br /><br />
                                            <v-row>
                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoEntrega.noGuiaRec"
                                                                  :counter="22"
                                                                  label="Número de Guía"
                                                                  :error-messages="validGuiaEntr"
                                                                  @input="$v.objetoEntrega.noGuiaRec.$touch()"
                                                                  @blur="$v.objetoEntrega.noGuiaRec.$touch()"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoEntrega.codRastreoRec"
                                                                  :counter="10"
                                                                  label="Código Rastreo"
                                                                  :error-messages="validCodEntr"
                                                                  @input="$v.objetoEntrega.codRastreoRec.$touch()"
                                                                  @blur="$v.objetoEntrega.codRastreoRec.$touch()"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">

                                                    <v-select id="selectTipoRec"
                                                              v-model="objetoEntrega.tipoServRec"
                                                              :items="TipoServicio"
                                                              label="Tipo de Servicio"
                                                              :error-messages="validServEntr"
                                                              @input="$v.objetoEntrega.tipoServRec.$touch()"
                                                              @blur="$v.objetoEntrega.tipoServRec.$touch()"
                                                              dense
                                                              outlined></v-select>

                                                </v-col>
                                            </v-row>

                                            <!--<br /><br /><br /><br />
                                    {{objetoEntrega}}-->
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarEntre = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarEntrega(objetoEntrega)">
                                                Editar
                                            </v-btn>
                                        </v-card-actions>
                                    </v-card>



                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>

                </v-row>
            </div>


            <v-row>
                <v-col>
                    <v-row class="title row">
                        3. ¿El prestador del servicio entregó el viernes los acuses de recibo de la semana inmediata anterior?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        Si una semana hábil abarca del 1 al 5, los acuses de dicha semana se han de entregar el viernes 12
                    </v-row>
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolAcuses"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>

            <div v-if="datosCed.boolAcuses=='false'">
                <v-row align="center" justify="space-around">
                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogAcuses"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="red lighten-2"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias
                                    </v-card-title>

                                    <v-card-text>
                                        Solo registre incidencias, si una o varias semanas el proveedor entrego sus acuses no es necesario registrar esos datos
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field id="fieldCantidadAcuses"
                                                              v-model="objetoAcuses.cantidadAcuses"
                                                              label="Acuses faltantes en UNA semana:"
                                                              dense
                                                              required
                                                              :error-messages="validSemana"
                                                              @input="$v.objetoAcuses.cantidadAcuses.$touch()"
                                                              @blur="$v.objetoAcuses.cantidadAcuses.$touch()"
                                                              hint="Si hubo más incidencias en otra semana, capture otro registro"
                                                              outlined></v-text-field>


                                            </v-col>
                                        </v-row>
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">


                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Espera Acuses</span>
                                                <vc-date-picker v-model="objetoAcuses.fechaProgramada"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :disabled-dates='{ weekdays: [1,2,3,4,5, 7] }'>

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


                                            <v-col cols="12"
                                                   md="4">
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Entrega Acuses</span>
                                                <vc-date-picker v-model="objetoAcuses.fechaEfectiva"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :mask="masks"
                                                                :min-date="objetoAcuses.fechaProgramada"
                                                                :disabled-dates='{ weekdays: [1, 7] }'
                                                                is-dark>

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


                                        </v-row>
                                        <br /><br /><br /><br /><br />


                                        <!--<br /><br /><br /><br />
    {{objetoAcuses}}-->
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogAcuses = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarAcuses(objetoAcuses)">
                                            Agregar
                                        </v-btn>
                                    </v-card-actions>
                                </v-card>
                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>

                    <div>
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogRegAcuses"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="red lighten-2"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Revisar Registros
                                        </v-btn>
                                    </template>

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Registros Incidencias
                                        </v-card-title>

                                        <v-card-text>
                                            <v-row align="center">
                                                <v-col align="center">
                                                    <table class="table table-sm table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Acuses Faltantes</th>
                                                                <th> | </th>
                                                                <th>Entrega Esperada</th>
                                                                <th> | </th>
                                                                <th>Entrega Efectiva</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="item in datosCed.arregloAcuses" :key="item.cantidadAcuses">
                                                                <th>{{item.cantidadAcuses}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fechaPrograFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fehcaEfecFormat}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarAcuse(item)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                            </tr>
                                                        </tbody>

                                                    </table>
                                                </v-col>
                                            </v-row>

                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogRegAcuses = false">
                                                Cerrar
                                            </v-btn>

                                        </v-card-actions>
                                    </v-card>
                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>

                </v-row>
            </div>


            <v-row>
                <v-col class="title row" align="left">
                    4. ¿El prestador del servicio entregó guías en mal estado? (Paquete maltratado, averiado, manchado, enmendado, etc.)
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolMalEstado"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>


            <div v-if="datosCed.boolMalEstado=='true'">
                <v-row align="center" justify="space-around">
                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogMalEstado"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="red lighten-2"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias
                                    </v-card-title>

                                    <v-card-text>
                                        En esta sección omita registrar extravíos

                                        <v-row>
                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoMalEstado.noGuiaRec"
                                                              :counter="22"
                                                              :error-messages="validGuiaMalE"
                                                              @input="$v.objetoMalEstado.noGuiaRec.$touch()"
                                                              @blur="$v.objetoMalEstado.noGuiaRec.$touch()"
                                                              label="Número de Guía"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoMalEstado.codRastreoRec"
                                                              :counter="10"
                                                              :error-messages="validCodMalE"
                                                              @input="$v.objetoMalEstado.codRastreoRec.$touch()"
                                                              @blur="$v.objetoMalEstado.codRastreoRec.$touch()"
                                                              label="Código Rastreo"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">

                                                <v-select id="selectTipoRec"
                                                          v-model="objetoMalEstado.tipoServRec"
                                                          :items="TipoServicio"
                                                          label="Tipo de Servicio"
                                                          dense
                                                          :error-messages="validServMalE"
                                                          @input="$v.objetoMalEstado.tipoServRec.$touch()"
                                                          @blur="$v.objetoMalEstado.tipoServRec.$touch()"
                                                          outlined></v-select>

                                            </v-col>
                                        </v-row>
                                        <br />
                                        <v-row align="center" justify="space-around">

                                            <v-btn color="green"
                                                   dark
                                                   link :to="{path: '/api/Documents/DescargarArchivo/MensajeriaMalEstado_Perdido.xlsx'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-excel
                                                </v-icon>
                                                Plantilla Mal Estado
                                            </v-btn>

                                            <v-btn color="red"
                                                   dark
                                                   link :to="{path: '/api/Documents/DescargarArchivo/Manual_Mensajeria_Mal_Estado_Extravio.pdf'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-pdf
                                                </v-icon>
                                                Manual Plantilla
                                            </v-btn>

                                            <input class="form-control" type="file" id="input" accept=".xls,.xlsx" @change="subirExcelMalE($event)">


                                        </v-row>



                                        <!--{{objetoMalEstado}}-->
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogMalEstado = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarMalEstado(objetoMalEstado)">
                                            Agregar
                                        </v-btn>
                                    </v-card-actions>
                                </v-card>
                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>

                    <div>
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogRegMalEstado"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="red lighten-2"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Revisar Registros
                                        </v-btn>
                                    </template>

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Registros Incidencias
                                        </v-card-title>

                                        <v-card-text>
                                            <v-row align="center">
                                                <v-col align="center">
                                                    <table class="table table-sm table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>No. Guía</th>
                                                                <th> | </th>
                                                                <th>Código de Rastreo</th>
                                                                <th> | </th>
                                                                <th>Tipo de Servicio</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloMalEstado" :key="item.noGuiaRec">
                                                                <th>{{item.noGuiaRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.codRastreoRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.tipoServRec}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarMalEstado(item)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarMalEstadoDiag(index)">
                                                                    mdi-pencil
                                                                </v-icon>
                                                            </tr>
                                                        </tbody>

                                                    </table>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogRegMalEstado = false">
                                                Cerrar
                                            </v-btn>

                                        </v-card-actions>
                                    </v-card>
                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>

                    <div v-if="dialogEditarMalEstado==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarMalEstado"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            En esta sección omita registrar extravíos

                                            <v-row>
                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoMalEstado.noGuiaRec"
                                                                  :counter="22"
                                                                  :error-messages="validGuiaMalE"
                                                                  @input="$v.objetoMalEstado.noGuiaRec.$touch()"
                                                                  @blur="$v.objetoMalEstado.noGuiaRec.$touch()"
                                                                  label="Número de Guía"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">
                                                    <v-text-field v-model="objetoMalEstado.codRastreoRec"
                                                                  :counter="10"
                                                                  :error-messages="validCodMalE"
                                                                  @input="$v.objetoMalEstado.codRastreoRec.$touch()"
                                                                  @blur="$v.objetoMalEstado.codRastreoRec.$touch()"
                                                                  label="Código Rastreo"
                                                                  required></v-text-field>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="4">

                                                    <v-select id="selectTipoRec"
                                                              v-model="objetoMalEstado.tipoServRec"
                                                              :items="TipoServicio"
                                                              label="Tipo de Servicio"
                                                              dense
                                                              :error-messages="validServMalE"
                                                              @input="$v.objetoMalEstado.tipoServRec.$touch()"
                                                              @blur="$v.objetoMalEstado.tipoServRec.$touch()"
                                                              outlined></v-select>

                                                </v-col>
                                            </v-row>

                                            <!--{{objetoMalEstado}}-->
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarMalEstado = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarMalEstado(objetoMalEstado)">
                                                Editar
                                            </v-btn>
                                        </v-card-actions>
                                    </v-card>



                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>
                </v-row>
            </div>

            <v-row>
                <v-col class="title row" align="left">
                    5. ¿Resultaron guías extraviadas este mes?
                </v-col>
            </v-row>
            <v-row>
                <v-radio-group v-model="datosCed.boolExtraviadas"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>

            <div v-if="datosCed.boolExtraviadas=='true'">
                <v-row align="center" justify="space-around">


                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogPerdido"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="red lighten-2"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias
                                    </v-card-title>

                                    <v-card-text>

                                        <v-row>
                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoPerdido.noGuiaRec"
                                                              :counter="22"
                                                              :error-messages="validGuiaPerdido"
                                                              @input="$v.objetoPerdido.noGuiaRec.$touch()"
                                                              @blur="$v.objetoPerdido.noGuiaRec.$touch()"
                                                              label="Número de Guía"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">
                                                <v-text-field v-model="objetoPerdido.codRastreoRec"
                                                              :counter="10"
                                                              :error-messages="validCodPerdido"
                                                              @input="$v.objetoPerdido.codRastreoRec.$touch()"
                                                              @blur="$v.objetoPerdido.codRastreoRec.$touch()"
                                                              label="Código Rastreo"
                                                              required></v-text-field>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="4">

                                                <v-select id="selectTipoRec"
                                                          v-model="objetoPerdido.tipoServRec"
                                                          :items="TipoServicio"
                                                          :error-messages="validServPerdido"
                                                          @input="$v.objetoPerdido.tipoServRec.$touch()"
                                                          @blur="$v.objetoPerdido.tipoServRec.$touch()"
                                                          label="Tipo de Servicio"
                                                          dense
                                                          outlined></v-select>

                                            </v-col>
                                        </v-row>
                                        <br />
                                        <v-row align="center" justify="space-around">

                                            <v-btn color="green"
                                                   dark
                                                   link :to="{ path: '/api/Documents/DescargarArchivo/MensajeriaMalEstado_Perdido.xlsx'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-excel
                                                </v-icon>
                                                Plantilla Perdido
                                            </v-btn>

                                            <v-btn color="red"
                                                   dark
                                                   link :to="{path: '/api/Documents/DescargarArchivo/Manual_Mensajeria_Mal_Estado_Extravio.pdf'}"
                                                   target="_blank">
                                                <v-icon dark
                                                        left>
                                                    mdi-file-pdf
                                                </v-icon>
                                                Manual Plantilla
                                            </v-btn>

                                            <input class="form-control" type="file" id="input" accept=".xls,.xlsx" @change="subirExcelPerdido($event)">


                                        </v-row>



                                        <!--{{objetoPerdido}}-->
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogPerdido = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarPerdido(objetoPerdido)">
                                            Agregar
                                        </v-btn>
                                    </v-card-actions>
                                </v-card>
                                <v-snackbar v-model="snackbar"
                                            :color="colorSnackbar"
                                            :multi-line="true"
                                            :timeout="4000">
                                    {{ mensaje }}
                                    <v-btn dark
                                           text
                                           @click="snackbar = false">
                                        Cerrar
                                    </v-btn>
                                </v-snackbar>
                            </v-dialog>
                        </div>
                    </template>

                    <div>
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogRegPerdido"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="red lighten-2"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Revisar Registros
                                        </v-btn>
                                    </template>

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Registros Incidencias
                                        </v-card-title>

                                        <v-card-text>
                                            <v-row align="center">
                                                <v-col align="center">
                                                    <table class="table table-sm table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>No. Guía</th>
                                                                <th> | </th>
                                                                <th>Código de Rastreo</th>
                                                                <th> | </th>
                                                                <th>Tipo de Servicio</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="item in datosCed.arregloPerdido" :key="item.noGuiaRec">
                                                                <th>{{item.noGuiaRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.codRastreoRec}}</th>
                                                                <th> | </th>
                                                                <th>{{item.tipoServRec}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarPerdido(item)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </v-col>
                                            </v-row>

                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogRegPerdido = false">
                                                Cerrar
                                            </v-btn>

                                        </v-card-actions>
                                    </v-card>
                                    <v-snackbar v-model="snackbar"
                                                :color="colorSnackbar"
                                                :multi-line="true"
                                                :timeout="4000">
                                        {{ mensaje }}
                                        <v-btn dark
                                               text
                                               @click="snackbar = false">
                                            Cerrar
                                        </v-btn>
                                    </v-snackbar>
                                </v-dialog>
                            </div>
                        </template>
                    </div>
                </v-row>
            </div>

            <v-row>
                <v-col class="title row" align="left">
                    6. ¿el prestador del servicio entregó material suficiente para el adecuado embalaje de las guías?
                </v-col>
            </v-row>
            <v-row>
                <v-col class="title row" align="left">
                    <v-radio-group v-model="datosCed.boolMaterial"
                                   row>
                        <v-radio label="Sí"
                                 value="true"></v-radio>
                        <v-radio label="No"
                                 value="false"></v-radio>
                    </v-radio-group>
                </v-col>
            </v-row>


            <div v-if="datosCed.boolMaterial=='false'">
                <v-col md="3">

                    Capture días naturales de atraso:
                    <br />
                    <v-text-field v-model="diasMat"
                                  label="Máximo 30 días"
                                  :error-messages="validDiasAtraso"
                                  @input="$v.diasMat.$touch()"
                                  @blur="$v.diasMat.$touch()"
                                  @change="setDiasMat($event)"
                                  required
                                  dense></v-text-field>
                </v-col>
            </div>




            

            <div v-if="primerMes==true">
                <v-row>
                    <v-col class="title row" align="left">
                        A. ¿Cúando entregó el prestador de servicio el Programa de Operación?
                    </v-col>
                </v-row>
                <v-row>
                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha entrega del Programa de Operación &nbsp;</span>
                </v-row>
                <v-row>
                    <vc-date-picker v-model="datosCed.fechaPO"
                                    required
                                    :mask="masks"
                                    :locale="{firstDayOfWeek: 1}"
                                    :disabled-dates='{ weekdays: [1, 7] }'
                                    :min-date="inicioContrat">


                        <template v-slot="{ inputValue, inputEvents }">
                            <v-text-field readonly
                                          outlined
                                          dense
                                          label="Ingrese Fecha"
                                          :value="inputValue"
                                          v-on="inputEvents" />
                        </template>

                    </vc-date-picker>
                </v-row>
            </div>

            <v-row align="center" justify="space-around">
                <v-btn color="red lighten-2"
                       dark
                       @click="guardar()">
                    Guardar sin enviar
                </v-btn>

                

                <v-btn color="red lighten-2"
                       dark
                       @click="verificarFormulario()">
                    Guardar y enviar a DAS
                </v-btn>
                <v-dialog v-model="dialogFactura"
                          hide-overlay
                          persistent
                          width="500">
                    <v-card>
                        <v-card-title class="headline grey lighten-2">
                            Subir soporte documental
                        </v-card-title>

                        <v-card-text>
                            <v-row>
                                <v-col>
                                    Tales como la factura, actas, reportes de servicio, etc. en un archivo comprimido (.rar o .zip) de hasta 128 MB
                                </v-col>
                            </v-row>
                            <v-row align="center">
                                <v-col align="center">
                                    <input class="form-control" type="file" id="input" accept=".rar,.zip" @change="fileChange($event.target.files)">
                                </v-col>
                            </v-row>

                        </v-card-text>

                        <v-divider></v-divider>

                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="primary"
                                   text
                                   @click="dialogFactura = false">
                                Cancelar
                            </v-btn>
                            <v-btn color="primary"
                                   text
                                   :disabled="loading || (files == null ? true : false)"
                                   :loading="loading"
                                   @click="calcularPena()">
                                Enviar todo
                            </v-btn>

                        </v-card-actions>
                    </v-card>
                </v-dialog>
            </v-row>
        </div>
        <!--{{penas}}-->
    </v-container>
</template>

<script>
    import { required, requiredIf, decimal, minLength, maxLength, between, numeric } from 'vuelidate/lib/validators'
    import CedulasService from '../services/cedulas-service';
    import MensajeriaService from '../services/mensajeria-service';
    import XLSX from 'xlsx';

    document.getElementById
    export default {
        name: "Director",
        data: () => ({
            archivo: [],
            primerMes: null,
            inicioContrat: new Date(2021, 0, 25),
            folio: null,
            mensaje: "",
            colorSnackbar: "",
            snackbar: false,
            dialogRecoExcel: false,
            dialogEntrExcel: false,
            objetoAcuses: {
                cantidadAcuses:null,
                //noSemana: null,
                fechaProgramada: null,
                fechaEfectiva: null,
                fechaPrograFormat: null,
                fehcaEfecFormat: null,
            },
            objetoRecoleccion: {
                fechaProgramada: null,
                fechaEfectiva: null,
                noGuiaRec: null,
                codRastreoRec: null,
                tipoServRec: null,
                fechaPrograFormat: null,
                fehcaEfecFormat: null,
            },
            objetoPerdido: {
                noGuiaRec: null,
                codRastreoRec: null,
                tipoServRec: null,
            },
            objetoMalEstado: {
                noGuiaRec: null,
                codRastreoRec: null,
                tipoServRec: null,
            },
            objetoEntrega: {
                fechaProgramada: null,
                fechaEfectiva: null,
                noGuiaRec: null,
                codRastreoRec: null,
                tipoServRec: null,
                fechaPrograFormat: null,
                fehcaEfecFormat: null,
            },
            datosCed: {
                factura: "",
                monto: null,
                boolRecoleccion: null,
                arregloRecoleccion: [],
                boolEntrega: null,
                arregloEntrega: [],
                boolAcuses: null,
                arregloAcuses: [],
                boolMalEstado: null,
                arregloMalEstado: [],
                boolExtraviadas: null,
                arregloPerdido: [],
                boolMaterial: null,
                diasMaterial: null,
                fechaPO: null,
            },
            repetirFactura: "",
            diasMat: null,
            masks: {
                input: 'YYYY-MM-DD',
            },
            dialogReco: false,
            dialogEntr: false,
            dialogAcuses: false,
            dialogMalEstado: false,
            dialogPerdido: false,
            dialogRegReco: false,
            dialogRegEntr: false,
            dialogRegAcuses: false,
            dialogRegMalEstado: false,
            dialogRegPerdido: false,
            dialogEditarReco: false,
            dialogEditarEntre: false,
            dialogEditarMalEstado: false,
            dialogFactura: false,
            opcionesServer: {
                selectPrimavera: null,
                selectInmueble: null,
                selectMes: null,
            },
            mesVacio: null,
            penas: null,
            totalFormat: null,
            weekday: [1, 2, 3, 4, 5, 6, 0],
            primavera: [2021],
            NoSem: [1, 2, 3, 4, 5],
            TipoServicio: ['Nacional', 'Nacional Sobrepeso', 'Internacional', 'Internacional Sobrepeso'],
            inmuebles: [],
            inmueble: null,
            mes: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            bool: ['Sí', 'No'],
            archivoReco: null,
            direccion: null,
            index: null,
            files: null,
            loading: false,
        }),

        validations: {

            datosCed: {
                monto: {
                    decimal
                },
                boolRecoleccion: {
                    required
                },
                boolEntrega: {
                    required
                },
                boolAcuses: {
                    required
                },
                boolMalEstado: {
                    required
                },
                boolExtraviadas: {
                    required
                },
                boolMaterial: {
                    required
                },
            },

            diasMat: {
                required,
                numeric,
                between: between(1, 30)
            },

            objetoRecoleccion: {
                noGuiaRec: {
                    minLength: minLength(22),
                    maxLength: maxLength(22),
                },
                codRastreoRec: {
                    required: requiredIf(function (objetoRecoleccion) {
                        return (objetoRecoleccion.noGuiaRec == null || objetoRecoleccion.noGuiaRec == "")
                    }),
                    numeric,
                    minLength: minLength(10),
                    maxLength: maxLength(10),
                },
                tipoServRec: {
                    required
                },
                fechaProgramada: {
                    required
                },
                fechaEfectiva: {
                    required
                }
            },

            objetoEntrega: {
                noGuiaRec: {
                    minLength: minLength(22),
                    maxLength: maxLength(22),
                },
                codRastreoRec: {
                    required: requiredIf(function (objetoEntrega) {
                        return (objetoEntrega.noGuiaRec == null || objetoEntrega.noGuiaRec == "")
                    }),
                    numeric,
                    minLength: minLength(10),
                    maxLength: maxLength(10),
                },
                tipoServRec: {
                    required
                },
                fechaProgramada: {
                    required
                },
                fechaEfectiva: {
                    required
                }
            },

            objetoAcuses: {
                cantidadAcuses: {
                    required,
                    numeric
                },
                fechaProgramada: {
                    required
                },
                fechaEfectiva: {
                    required
                }
            },

            objetoMalEstado: {
                noGuiaRec: {
                    minLength: minLength(22),
                    maxLength: maxLength(22),
                },
                codRastreoRec: {
                    required: requiredIf(function (objetoMalEstado) {
                        return (objetoMalEstado.noGuiaRec == null || objetoMalEstado.noGuiaRec == "")
                    }),
                    numeric,
                    minLength: minLength(10),
                    maxLength: maxLength(10),
                },
                tipoServRec: {
                    required
                },
            },

            objetoPerdido: {
                noGuiaRec: {
                    minLength: minLength(22),
                    maxLength: maxLength(22),
                },
                codRastreoRec: {
                    required: requiredIf(function (objetoPerdido) {
                        return (objetoPerdido.noGuiaRec == null || objetoPerdido.noGuiaRec == "")
                    }),
                    numeric,
                    minLength: minLength(10),
                    maxLength: maxLength(10),
                },
                tipoServRec: {
                    required
                },
            }
        },

        mounted() {
            CedulasService.getInmuebles(this.currentUser.newDataSet.areas.cveArea).then(
                v => {
                    this.inmuebles = v;
                    console.log(this.inmuebles);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );


            const folio = localStorage.getItem('folio');
            console.log("folio");
            console.log(folio);
            if (folio) {
                localStorage.removeItem('folio');
                this.folio = folio;
                CedulasService.getCedula("Mensajeria", folio).then(
                    ced => {
                        console.log("Cédula Recuperada");
                        console.log(ced);
                        this.opcionesServer.selectPrimavera = ced.cedula.fiAnio;
                        this.opcionesServer.selectInmueble = ced.cedula.fcInmueble;
                        this.opcionesServer.selectMes = ced.cedula.fcMes;
                        this.mes = [this.opcionesServer.selectMes];
                        this.primavera = [this.opcionesServer.selectPrimavera];
                        this.inmuebles = [this.opcionesServer.selectInmueble];

                        if (this.mes == "Enero") {
                            this.primerMes = true;
                        }

                        if (ced.cedula.fcNumFactura == "Por Asignar") {
                            this.datosCed.factura = "";
                        }
                        else {
                            this.datosCed.factura = ced.cedula.fcNumFactura;
                            this.repetirFactura = this.datosCed.factura;
                        }
                        this.datosCed.monto = ced.cedula.fiMontoFactura;
                        this.mesVacio = true;

                        if (ced.arreglos.arregloReco.length > 0) {
                            this.datosCed.boolRecoleccion = "false";

                            ced.arreglos.arregloReco.forEach(element => {
                                console.log("Insertando");
                                console.log(element);
                                this.objetoRecoleccion.fechaProgramada = element.fdFechaProgramada;
                                this.objetoRecoleccion.fechaEfectiva = element.fdFechaEfectiva;
                                this.objetoRecoleccion.noGuiaRec = element.fcNoGuia;
                                this.objetoRecoleccion.codRastreoRec = element.fcCodRastreo;
                                this.objetoRecoleccion.tipoServRec = element.fcTipoServicio
                                this.agregarReco(this.objetoRecoleccion);
                            });
                        }
                        else {
                            this.datosCed.boolRecoleccion = "true";
                        }

                        if (ced.arreglos.arregloEntre.length > 0) {
                            this.datosCed.boolEntrega = "false";
                            ced.arreglos.arregloEntre.forEach(element => {
                                console.log(element);
                                this.objetoEntrega.fechaProgramada = element.fdFechaProgramada;
                                this.objetoEntrega.fechaEfectiva = element.fdFechaEfectiva;
                                this.objetoEntrega.noGuiaRec = element.fcNoGuia;
                                this.objetoEntrega.codRastreoRec = element.fcCodRastreo;
                                this.objetoEntrega.tipoServRec = element.fcTipoServicio;
                                this.agregarEntr(this.objetoEntrega);
                            })
                        }
                        else {
                            this.datosCed.boolEntrega = "true";
                        }

                        if (ced.arreglos.arregloAcuse.length > 0) {
                            this.datosCed.boolAcuses = "false";
                            ced.arreglos.arregloAcuse.forEach(element => {
                                console.log(element);
                                this.objetoAcuses.fechaProgramada = element.fdFechaProgramada;
                                this.objetoAcuses.fechaEfectiva = element.fdFechaEfectiva;
                                this.objetoAcuses.cantidadAcuses = element.fiCantidadAcuses;//marcador acuses
                                this.agregarAcuses(this.objetoAcuses);

                            })
                        }
                        else {
                            this.datosCed.boolAcuses = "true";
                        }

                        if (ced.arreglos.arregloMalEstado.length > 0) {
                            this.datosCed.boolMalEstado = "true";
                            ced.arreglos.arregloMalEstado.forEach(element => {
                                console.log(element);
                                this.objetoMalEstado.codRastreoRec = element.fcCodRastreo;
                                this.objetoMalEstado.noGuiaRec = element.fcNoGuia;
                                this.objetoMalEstado.tipoServRec = element.fcTipoServicio;
                                this.agregarMalEstado(this.objetoMalEstado);

                            })
                        }
                        else {
                            this.datosCed.boolMalEstado = "false";
                        }

                        if (ced.arreglos.arregloExtravio.length > 0) {
                            this.datosCed.boolExtraviadas = "true";
                            ced.arreglos.arregloExtravio.forEach(element => {
                                console.log(element);
                                this.objetoPerdido.codRastreoRec = element.fcCodRastreo;
                                this.objetoPerdido.noGuiaRec = element.fcNoGuia;
                                this.objetoPerdido.tipoServRec = element.fcTipoServicio;
                                this.agregarPerdido(this.objetoPerdido);

                            })
                        }
                        else {
                            this.datosCed.boolExtraviadas = "false";
                        }

                        if (ced.arreglos.objetoMaterialUniforme.length > 0) {
                            if (ced.arreglos.objetoMaterialUniforme[0].fiMaterial == 1) {
                                this.datosCed.boolMaterial = "false";
                                this.diasMat = ced.arreglos.objetoMaterialUniforme[0].fiDiasMaterial;
                                this.datosCed.diasMaterial = this.diasMat;
                            }
                            else {
                                this.datosCed.boolMaterial = "true";
                            }

                            
                        }
                        else {
                            this.datosCed.boolMaterial = "true";
                            
                        }
                        console.log(this.datosCed);
                    }, error => {
                        console.log("hay error");
                        console.log(error);
                    }
                );
            }



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



            validDiasAtraso() {
                const errors = [];
                if (!this.$v.diasMat.$dirty) return errors
                !this.$v.diasMat.required && errors.push('El campo es requerido');
                !this.$v.diasMat.between && errors.push('Entre 1 y 30 días');
                !this.$v.diasMat.numeric && errors.push('Solo números');
                return errors
            },

            validMonto() {
                const errors = [];
                if (!this.$v.datosCed.monto.$dirty) return errors;
                //!this.$v.datosCed.monto.required && errors.push('El campo es requerido');
                !this.$v.datosCed.monto.decimal && errors.push('Solo números sin comas ni $');
                return errors;
            },

            validGuiaReco() {
                const errors = [];
                if (!this.$v.objetoRecoleccion.noGuiaRec.$dirty) return errors
                !this.$v.objetoRecoleccion.noGuiaRec.required && errors.push('El campo es requerido')
                !this.$v.objetoRecoleccion.noGuiaRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoRecoleccion.noGuiaRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validCodReco() {
                const errors = [];
                if (!this.$v.objetoRecoleccion.codRastreoRec.$dirty) return errors
                !this.$v.objetoRecoleccion.codRastreoRec.required && errors.push('El campo es requerido')
                !this.$v.objetoRecoleccion.codRastreoRec.numeric && errors.push('Solo se aceptan números')
                !this.$v.objetoRecoleccion.codRastreoRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoRecoleccion.codRastreoRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validServReco() {
                const errors = [];
                if (!this.$v.objetoRecoleccion.tipoServRec.$dirty) return errors
                !this.$v.objetoRecoleccion.tipoServRec.required && errors.push('El campo es requerido')
                return errors
            },

            validGuiaEntr() {
                const errors = [];
                if (!this.$v.objetoEntrega.noGuiaRec.$dirty) return errors
                !this.$v.objetoEntrega.noGuiaRec.required && errors.push('El campo es requerido')
                !this.$v.objetoEntrega.noGuiaRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoEntrega.noGuiaRec.minLength && errors.push('El campo es demasioado corto')
                return errors
            },

            validCodEntr() {
                const errors = [];
                if (!this.$v.objetoEntrega.codRastreoRec.$dirty) return errors
                !this.$v.objetoEntrega.codRastreoRec.required && errors.push('El campo es requerido')
                !this.$v.objetoEntrega.codRastreoRec.numeric && errors.push('Solo se aceptan números')
                !this.$v.objetoEntrega.codRastreoRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoEntrega.codRastreoRec.minLength && errors.push('El campo es demasioado corto')
                return errors
            },

            validServEntr() {
                const errors = [];
                if (!this.$v.objetoEntrega.tipoServRec.$dirty) return errors
                !this.$v.objetoEntrega.tipoServRec.required && errors.push('El campo es requerido')
                return errors
            },

            validSemana() {
                const errors = [];
                if (!this.$v.objetoAcuses.cantidadAcuses.$dirty) return errors
                !this.$v.objetoAcuses.cantidadAcuses.numeric && errors.push('Solo números')
                !this.$v.objetoAcuses.cantidadAcuses.required && errors.push('El campo es requerido')
                return errors
            },

            validGuiaMalE() {
                const errors = [];
                if (!this.$v.objetoMalEstado.noGuiaRec.$dirty) return errors
                !this.$v.objetoMalEstado.noGuiaRec.required && errors.push('El campo es requerido')
                !this.$v.objetoMalEstado.noGuiaRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoMalEstado.noGuiaRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validCodMalE() {
                const errors = [];
                if (!this.$v.objetoMalEstado.codRastreoRec.$dirty) return errors
                !this.$v.objetoMalEstado.codRastreoRec.required && errors.push('El campo es requerido')
                !this.$v.objetoMalEstado.codRastreoRec.numeric && errors.push('Solo se aceptan números')
                !this.$v.objetoMalEstado.codRastreoRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoMalEstado.codRastreoRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validServMalE() {
                const errors = [];
                if (!this.$v.objetoEntrega.tipoServRec.$dirty) return errors
                !this.$v.objetoMalEstado.tipoServRec.required && errors.push('El campo es requerido')
                return errors
            },

            validGuiaPerdido() {
                const errors = [];
                if (!this.$v.objetoPerdido.noGuiaRec.$dirty) return errors
                !this.$v.objetoPerdido.noGuiaRec.required && errors.push('El campo es requerido')
                !this.$v.objetoPerdido.noGuiaRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoPerdido.noGuiaRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validCodPerdido() {
                const errors = [];
                if (!this.$v.objetoPerdido.codRastreoRec.$dirty) return errors
                !this.$v.objetoPerdido.codRastreoRec.required && errors.push('El campo es requerido')
                !this.$v.objetoPerdido.codRastreoRec.numeric && errors.push('Solo se aceptan números')
                !this.$v.objetoPerdido.codRastreoRec.maxLength && errors.push('El campo es demasiado largo')
                !this.$v.objetoPerdido.codRastreoRec.minLength && errors.push('El campo es demasiado corto')
                return errors
            },

            validServPerdido() {
                const errors = [];
                if (!this.$v.objetoPerdido.tipoServRec.$dirty) return errors
                !this.$v.objetoPerdido.tipoServRec.required && errors.push('El campo es requerido')
                return errors
            }
        },

        methods: {
            calcularPena() {
                console.log('calcular pena');
                MensajeriaService.calcularPena(this.datosCed).then(
                    cp => {
                        console.log("Objeto Penas regresado del servidor:")
                        this.penas = cp;
                        this.enviar(this.penas);
                    }, error => {
                        console.log("hay error");
                        console.log(error);
                        this.mesVacio = false;
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    });
                console.log(this.totalFormat);
            },

            enviar(penas) {
                console.log('Enviar a DGSG');
                console.log(penas);
                var datos = {
                    cedula: this.datosCed,
                    penas: JSON.stringify(this.penas),
                    periodo: this.opcionesServer,
                    area: this.currentUser.newDataSet.areas.cveArea,
                    folio: this.folio,
                    usuario: this.currentUser.newDataSet.datosGenerales.nombre_NPM,
                    correo: this.currentUser.newDataSet.datosGenerales.correo_electronico
                };
                console.log(datos);
                MensajeriaService.enviar(datos).then(
                    g => {
                        console.log("Enviado");
                        console.log(g);
                        this.loading = false;
                        this.mensaje = 'Enviado con éxito';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                        this.insertArchivo(g);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                );
            },

            insertArchivo(folio) {
                console.log('Guardar Archivo');
                this.files.set('id', folio);
                const files = this.files;
                this.loading = true;
                MensajeriaService.insertArchivo(files).then(
                    c => {
                        console.log("Response subir archivo: ", c);
                        this.loading = false;
                        setTimeout(() => { this.$router.push({ name: 'Director' }) }, 3000);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.loading = false;
                        this.mensaje = 'Ha habido un error al subir el archivo, reduzca su tamaño e intentelo de nuevo';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                )
            },

            fileChange(fileList) {
                console.log(fileList);
                console.log(this.files);
                this.files = new FormData();
                this.files.append("file", fileList[0], fileList[0].name);
                console.log(this.files);
            },

            descargar(doc) {
                CedulasService.descargarDoc(doc)
            },

            setDiasMat(event) {
                this.datosCed.diasMaterial = event
                console.log(this.datosCed.diasMaterial)
            },

            editarRecoDiag(index) {
                console.log(this.datosCed.arregloRecoleccion[index]);
                this.objetoRecoleccion = { ...this.datosCed.arregloRecoleccion[index] }
                console.log(this.objetoRecoleccion);
                this.index = index;
                this.dialogRegReco = false;
                this.dialogEditarReco = true;
            },

            editarReco(objeto) {
                this.datosCed.arregloRecoleccion.splice(this.index, 1);
                this.agregarReco(objeto);
            },

            editarMalEstadoDiag(index) {
                console.log(this.datosCed.arregloMalEstado[index]);
                this.objetoMalEstado = { ...this.datosCed.arregloMalEstado[index] }
                console.log(this.objetoMalEstado);
                this.index = index;
                this.dialogRegMalEstado = false;
                this.dialogEditarMalEstado = true;
            },

            editarMalEstado(objeto) {
                this.datosCed.arregloMalEstado.splice(this.index, 1);
                this.agregarMalEstado(objeto);
            },

            editarEntrDiag(index) {
                console.log(this.datosCed.arregloEntrega[index]);
                this.objetoEntrega = { ...this.datosCed.arregloEntrega[index] }
                console.log(this.objetoEntrega);
                this.index = index;
                this.dialogRegEntr = false;
                this.dialogEditarEntre = true;
            },

            editarEntrega(objeto) {
                this.datosCed.arregloEntrega.splice(this.index, 1);
                this.agregarEntr(objeto);
            },

            setFactura(event) {
                this.datosCed.factura = event;
                console.log('set Factura');
                console.log(this.datosCed.factura);
            },

            setRepFactura(event) {
                this.repetirFactura = event;
            },

            guardar() {
                console.log('guardar sin enviar');
                var datos = {
                    cedula: this.datosCed,
                    periodo: this.opcionesServer,
                    area: this.currentUser.newDataSet.areas.cveArea,
                    folio: this.folio
                };
                console.log(datos);
                MensajeriaService.guardar(datos).then(
                    g => {
                        console.log("Guardado");
                        console.log(g);
                        this.mensaje = 'Guardado con éxito';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                        setTimeout(() => { this.$router.push({ name: 'Director' }) }, 2000);
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                );
            },

            setDireccion(nombre) {
                this.inmueble = nombre;
                this.inmuebles.forEach(element => {
                    console.log('elemento');
                    console.log(element);
                    if (element.nombre == nombre) {
                        this.direccion = element.direccion;
                        console.log(this.direccion);
                    }
                });
            },

            ExcelDateToJSDate(serial) {
                var utc_days = Math.floor(serial - 25569);
                var utc_value = utc_days * 86400;
                var date_info = new Date(utc_value * 1000);

                var fractional_day = serial - Math.floor(serial) + 0.0000001;

                var total_seconds = Math.floor(86400 * fractional_day);

                var seconds = total_seconds % 60;

                total_seconds -= seconds;

                var hours = Math.floor(total_seconds / (60 * 60));
                var minutes = Math.floor(total_seconds / 60) % 60;

                return new Date(date_info.getFullYear(), date_info.getMonth(), date_info.getDate(), hours, minutes, seconds);
            },

            subirExcelReco(event) {
                console.log(event.target.files);
                this.archivoReco = event.target.files[0];
                console.log(this.archivoReco);
                let fileReader = new FileReader();
                fileReader.onload = (event) => {
                    console.log('aqui');
                    console.log(event);
                    let data = event.target.result;
                    let workbook = XLSX.read(data, { type: "binary" });
                    console.log('worbook');
                    console.log(workbook);
                    workbook.SheetNames.forEach(sheet => {
                        let rowObject = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
                        console.log('rowObject');
                        console.log(rowObject);

                        for (var i = 0; i < rowObject.length; i++) {

                            console.log("elemento");
                            rowObject[i].fechaProgramada = this.ExcelDateToJSDate(rowObject[i].fechaProgramada);
                            rowObject[i].fechaEfectiva = this.ExcelDateToJSDate(rowObject[i].fechaEfectiva);
                            rowObject[i].codRastreoRec = rowObject[i].codRastreoRec.toString();
                            console.log(rowObject[i]);
                            //aqui acaba de leer y convertir datos


                            //Aqui agregar los elementos validando
                            this.objetoRecoleccion = { ...rowObject[i] };
                            console.log(this.objetoRecoleccion);
                            var validacion = this.agregarReco(rowObject[i]);
                            console.log(validacion);
                            if (validacion == false) {
                                event = null;
                                this.arregloRecoleccion = null;
                                var indice = (i + 1).toString();
                                this.mensaje = "Algo hay mal en el archivo, registro " + indice + ", revisa el manual";
                                this.colorSnackbar = 'error';
                                this.snackbar = true;
                                break;
                            }
                        }

                        //rowObject.forEach(element => {
                        //    console.log("elemento");
                        //    console.log(element);
                        //    element.fechaProgramada = this.ExcelDateToJSDate(element.fechaProgramada);
                        //    element.fechaEfectiva = this.ExcelDateToJSDate(element.fechaEfectiva);
                        //    //aqui acaba de leer y convertir datos


                        //    //Aqui agregar los elementos validando


                        //    var validacion = this.agregarReco(element);
                        //    if (!validacion) {
                        //        this.mensaje = "Algo hay mal en el archivo, revisa el manual";
                        //        this.colorSnackbar = 'error';
                        //        this.snackbar = true;
                        //    }

                        //});
                    });
                }
                console.log('filereader:');
                console.log(fileReader);
                fileReader.readAsBinaryString(this.archivoReco);
                console.log('sin errores');
                console.log(fileReader);
            },

            subirExcelEntr(event) {
                console.log(event.target.files);
                this.archivoReco = event.target.files[0];
                console.log(this.archivoReco);
                let fileReader = new FileReader();
                fileReader.onload = (event) => {
                    console.log('aqui');
                    console.log(event);
                    let data = event.target.result;
                    let workbook = XLSX.read(data, { type: "binary" });
                    console.log('worbook');
                    console.log(workbook);
                    workbook.SheetNames.forEach(sheet => {
                        let rowObject = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
                        console.log('rowObject');
                        console.log(rowObject);

                        for (var i = 0; i < rowObject.length; i++) {

                            console.log("elemento");
                            rowObject[i].fechaProgramada = this.ExcelDateToJSDate(rowObject[i].fechaProgramada);
                            rowObject[i].fechaEfectiva = this.ExcelDateToJSDate(rowObject[i].fechaEfectiva);
                            rowObject[i].codRastreoRec = rowObject[i].codRastreoRec.toString();
                            console.log(rowObject[i]);
                            //aqui acaba de leer y convertir datos


                            //Aqui agregar los elementos validando
                            this.objetoEntrega = { ...rowObject[i] };
                            console.log(this.objetoEntrega);
                            var validacion = this.agregarEntr(rowObject[i]);
                            console.log(validacion);
                            if (validacion == false) {
                                event = null;
                                this.arregloEntrega = null;
                                var indice = (i + 1).toString();
                                this.mensaje = "Algo hay mal en el archivo, registro " + indice + ", revisa el manual";
                                this.colorSnackbar = 'error';
                                this.snackbar = true;
                                break;
                            }
                        }
                    });
                }
                console.log('filereader:');
                console.log(fileReader);
                fileReader.readAsBinaryString(this.archivoReco);
                console.log('sin errores');
                console.log(fileReader);
            },

            subirExcelMalE(event) {
                console.log(event.target.files);
                this.archivoReco = event.target.files[0];
                console.log(this.archivoReco);
                let fileReader = new FileReader();
                fileReader.onload = (event) => {
                    console.log('aqui');
                    console.log(event);
                    let data = event.target.result;
                    let workbook = XLSX.read(data, { type: "binary" });
                    console.log('worbook');
                    console.log(workbook);
                    workbook.SheetNames.forEach(sheet => {
                        let rowObject = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
                        console.log('rowObject');
                        console.log(rowObject);



                        for (var i = 0; i < rowObject.length; i++) {

                            console.log("elemento");
                            rowObject[i].codRastreoRec = rowObject[i].codRastreoRec.toString();
                            console.log(rowObject[i]);
                            //aqui acaba de leer y convertir datos


                            //Aqui agregar los elementos validando
                            this.objetoMalEstado = { ...rowObject[i] };
                            console.log(this.objetoMalEstado);
                            var validacion = this.agregarMalEstado(rowObject[i]);
                            console.log(validacion);
                            if (validacion == false) {
                                event = null;
                                this.arregloMalEstado = null;
                                var indice = (i + 1).toString();
                                this.mensaje = "Algo hay mal en el archivo, registro " + indice + ", revisa el manual";
                                this.colorSnackbar = 'error';
                                this.snackbar = true;
                                break;
                            }
                        }
                    });
                }
                console.log('filereader:');
                console.log(fileReader);
                fileReader.readAsBinaryString(this.archivoReco);
                console.log('sin errores');
                console.log(fileReader);
            },

            subirExcelPerdido(event) {
                console.log(event.target.files);
                this.archivoReco = event.target.files[0];
                console.log(this.archivoReco);
                let fileReader = new FileReader();
                fileReader.onload = (event) => {
                    console.log('aqui');
                    console.log(event);
                    let data = event.target.result;
                    let workbook = XLSX.read(data, { type: "binary" });
                    console.log('worbook');
                    console.log(workbook);
                    workbook.SheetNames.forEach(sheet => {
                        let rowObject = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheet]);
                        console.log('rowObject');
                        console.log(rowObject);




                        for (var i = 0; i < rowObject.length; i++) {

                            console.log("elemento");
                            rowObject[i].codRastreoRec = rowObject[i].codRastreoRec.toString();
                            console.log(rowObject[i]);
                            //aqui acaba de leer y convertir datos


                            //Aqui agregar los elementos validando
                            this.objetoPerdido = { ...rowObject[i] };
                            console.log(this.objetoPerdido);
                            var validacion = this.agregarPerdido(rowObject[i]);
                            console.log(validacion);
                            if (validacion == false) {
                                event = null;
                                this.arregloPerdido = null;
                                var indice = (i + 1).toString();
                                this.mensaje = "Algo hay mal en el archivo, registro " + indice + ", revisa el manual";
                                this.colorSnackbar = 'error';
                                this.snackbar = true;
                                break;
                            }
                        }
                    });
                }
                console.log('filereader:');
                console.log(fileReader);
                fileReader.readAsBinaryString(this.archivoReco);
                console.log('sin errores');
                console.log(fileReader);
            },

            verificarFormulario() {
                if (this.$v.datosCed.$invalid) {
                    this.mensaje = "Falta contestar alguna pregunta o campo";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }

                else if (!this.$v.datosCed.$invalid) {
                    if (this.datosCed.boolRecoleccion == "false" && this.datosCed.arregloRecoleccion.length < 1) {
                        this.mensaje = "Contastaste NO en la pregunta 1 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.primerMes == true && this.datosCed.fechaPO == null) {
                        this.mensaje = "Falta contestar alguna pregunta o campo";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.$v.datosCed.monto.$invalid || this.datosCed.monto == null || this.datosCed.monto == "" || this.datosCed.monto == "0" || this.datosCed.monto == 0) {
                        this.mensaje = "Parece que en el monto de la factura es 0 o el campo esta vacío";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolEntrega == "false" && this.datosCed.arregloEntrega.length < 1) {
                        console.log(!this.$v.datosCed.$invalid);
                        console.log(this.datosCed.arregloEntrega.length < 1);
                        this.mensaje = "Contastaste NO en la pregunta 2 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolAcuses == "false" && this.datosCed.arregloAcuses.length < 1) {
                        this.mensaje = "Contastaste NO en la pregunta 3 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolMalEstado == "true" && this.datosCed.arregloMalEstado.length < 1) {
                        this.mensaje = "Contastaste SÍ en la pregunta 4 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolExtraviadas == "true" && this.datosCed.arregloPerdido.length < 1) {
                        this.mensaje = "Contastaste SÍ en la pregunta 5 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolMaterial == 'false' && this.$v.diasMat.$invalid) {
                        this.mensaje = "Contestaste NO en la pregunta 6 pero no capturaste días de atraso";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                    else if (this.datosCed.factura != this.repetirFactura) {
                        console.log(this.datosCed.factura);
                        console.log(this.repetirFactura);
                        this.mensaje = "Los números de factura deben ser iguales";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                    else if (this.datosCed.factura == "") {
                        console.log(this.datosCed.factura);
                        console.log(this.repetirFactura);
                        this.mensaje = "No puedes enviar sin # de factura, solo guardar";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                    else {
                        this.dialogFactura = true;
                    }
                }
            },

            formatearMonto(monto) {
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                });
                return formatter.format(monto);
            },

            preVerificarPeriodo() {
                console.log('Verificar Disponibilidad Cédula');

                var verificacion = {
                    factura: this.datosCed.factura,
                    inmueble: this.inmueble,
                    periodo: this.opcionesServer
                };
                console.log(verificacion);

                if (verificacion.factura != this.repetirFactura) {
                    console.log(verificacion.factura);
                    console.log(this.repetirFactura);
                    this.mensaje = "Los número de factura no coinciden";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }
                else if (verificacion.inmueble == null || verificacion.periodo.selectPrimavera == null || verificacion.periodo.selectMes == null) {
                    this.mensaje = "Debe llenar los datos de la primera fila para verificar";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }

                else if (verificacion.factura == null || verificacion.factura == "" || verificacion.factura == " ") {
                    var result = confirm("Llenará una cédula sin número de factura. ¿Desea Continuar?");
                    if (result == true) {
                        console.log("es cierto");
                        this.verificarPeriodo(verificacion);
                    }
                    else {
                        alert("Por favor llene los campos de número de factura");
                        console.log("es falso")
                    }
                    console.log("termino");
                }

                else {
                    this.verificarPeriodo(verificacion);
                }
            },

            verificarPeriodo(verificacion) {
                this.datosCed.monto = null;
                this.datosCed.boolRecoleccion = null;
                this.datosCed.arregloRecoleccion = [];
                this.datosCed.boolEntrega = null;
                this.datosCed.arregloEntrega = [];
                this.datosCed.boolAcuses = null;
                this.datosCed.arregloAcuses = [];
                this.datosCed.boolMalEstado = null;
                this.datosCed.arregloMalEstado = [];
                this.datosCed.boolExtraviadas = null;
                this.datosCed.arregloPerdido = [];
                this.datosCed.boolMaterial = null;
                this.datosCed.diasMaterial = null;

                MensajeriaService.verificarPeriodo(verificacion).then(
                    v => {
                        console.log(v);
                        this.mesVacio = true;
                        if (verificacion.periodo.selectMes == 'Enero') {
                            this.primerMes = true;
                        }
                        else {
                            this.primerMes = false;
                        }
                    }, error => {
                        console.log("hay error");
                        console.log(error);
                        this.mesVacio = false;
                        this.mensaje = 'Ya existe un registro de este periodo en este inmueble';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                );
            },

            eliminarReco(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloRecoleccion;
                console.log("var arreglo:");
                console.log(arreglo);
                var result = confirm("¿Eliminar registro?");
                console.log("var result:");
                console.log(result);
                if (result == true) {
                    console.log("es cierto");
                    for (var i = 0; i < arreglo.length; i++) {
                        console.log("en el loop");
                        console.log(arreglo[i]);
                        if (arreglo[i].noGuiaRec == item.noGuiaRec) {
                            console.log("se elimina");
                            console.log(arreglo[i]);
                            console.log(item);
                            arreglo.splice(i, 1);
                        }
                    }
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            eliminarEntr(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloEntrega;
                console.log("var arreglo:");
                console.log(arreglo);
                var result = confirm("¿Eliminar registro?");
                console.log("var result:");
                console.log(result);
                if (result == true) {
                    console.log("es cierto");
                    for (var i = 0; i < arreglo.length; i++) {
                        console.log("en el loop");
                        console.log(arreglo[i]);
                        if (arreglo[i].noGuiaRec == item.noGuiaRec) {
                            console.log("se elimina");
                            console.log(arreglo[i]);
                            console.log(item);
                            arreglo.splice(i, 1);
                        }
                    }
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            eliminarAcuse(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloAcuses;
                console.log("var arreglo:");
                console.log(arreglo);
                var result = confirm("¿Eliminar registro?");
                console.log("var result:");
                console.log(result);
                if (result == true) {
                    console.log("es cierto");
                    for (var i = 0; i < arreglo.length; i++) {
                        console.log("en el loop");
                        console.log(arreglo[i]);
                        if (arreglo[i].cantidadAcuses == item.cantidadAcuses) {
                            console.log("se elimina");
                            console.log(arreglo[i]);
                            console.log(item);
                            arreglo.splice(i, 1);
                        }
                    }
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            eliminarMalEstado(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloMalEstado;
                console.log("var arreglo:");
                console.log(arreglo);
                var result = confirm("¿Eliminar registro?");
                console.log("var result:");
                console.log(result);
                if (result == true) {
                    console.log("es cierto");
                    for (var i = 0; i < arreglo.length; i++) {
                        console.log("en el loop");
                        console.log(arreglo[i]);
                        if (arreglo[i].noGuiaRec == item.noGuiaRec) {
                            console.log("se elimina");
                            console.log(arreglo[i]);
                            console.log(item);
                            arreglo.splice(i, 1);
                        }
                    }
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            eliminarPerdido(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloPerdido;
                console.log("var arreglo:");
                console.log(arreglo);
                var result = confirm("¿Eliminar registro?");
                console.log("var result:");
                console.log(result);
                if (result == true) {
                    console.log("es cierto");
                    for (var i = 0; i < arreglo.length; i++) {
                        console.log("en el loop");
                        console.log(arreglo[i]);
                        if (arreglo[i].noGuiaRec == item.noGuiaRec) {
                            console.log("se elimina");
                            console.log(arreglo[i]);
                            console.log(item);
                            arreglo.splice(i, 1);
                        }
                    }
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            agregarReco(objeto) {
                if (this.$v.objetoRecoleccion.$invalid) {
                    console.log(this.objetoRecoleccion);
                    console.log("fallo primer if");

                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else if (objeto.fechaProgramada >= objeto.fechaEfectiva) {

                    console.log("fallo segundo if");

                    this.mensaje = "La fecha efectiva debe ser mayor a la programada";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaPrograFormat = copia.fechaProgramada.toString();
                    copia.fechaPrograFormat = copia.fechaPrograFormatc

                    copia.fehcaEfecFormat = copia.fechaEfectiva.toString();
                    copia.fehcaEfecFormat = copia.fehcaEfecFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloRecoleccion.push(copia);

                    this.objetoRecoleccion.fechaProgramada = null;
                    this.objetoRecoleccion.fechaEfectiva = null;
                    this.objetoRecoleccion.noGuiaRec = null;
                    this.objetoRecoleccion.codRastreoRec = null;
                    this.objetoRecoleccion.tipoServRec = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            agregarEntr(objeto) {
                if (this.$v.objetoEntrega.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else if (objeto.fechaProgramada >= objeto.fechaEfectiva) {
                    this.mensaje = "La fecha efectiva debe ser mayor a la programada";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    console.log("copia");
                    console.log(copia);

                    copia.fechaPrograFormat = copia.fechaProgramada.toString();
                    copia.fechaPrograFormat = copia.fechaPrograFormat.substring(0, 15);

                    copia.fehcaEfecFormat = copia.fechaEfectiva.toString();
                    copia.fehcaEfecFormat = copia.fehcaEfecFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloEntrega.push(copia);
                    console.log("arreglo modificado");
                    console.log(this.datosCed.arregloEntrega);

                    this.objetoEntrega.fechaProgramada = null;
                    this.objetoEntrega.fechaEfectiva = null;
                    this.objetoEntrega.noGuiaRec = null;
                    this.objetoEntrega.codRastreoRec = null;
                    this.objetoEntrega.tipoServRec = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            agregarAcuses(objeto) {
                if (this.$v.objetoAcuses.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else if (objeto.fechaProgramada >= objeto.fechaEfectiva) {
                    this.mensaje = "La fecha efectiva debe ser mayor a la programada";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaPrograFormat = copia.fechaProgramada.toString();
                    copia.fechaPrograFormat = copia.fechaPrograFormat.substring(0, 15);

                    copia.fehcaEfecFormat = copia.fechaEfectiva.toString();
                    copia.fehcaEfecFormat = copia.fehcaEfecFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloAcuses.push(copia);

                    this.objetoAcuses.cantidadAcuses = null;
                    this.objetoAcuses.fechaProgramada = null;
                    this.objetoAcuses.fechaEfectiva = null;
                    this.mensaje = "Agregado exitosamente, checar registros";

                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            agregarMalEstado(objeto) {

                if (this.$v.objetoMalEstado.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    console.log(copia);

                    this.datosCed.arregloMalEstado.push(copia);

                    this.objetoMalEstado.noGuiaRec = null;
                    this.objetoMalEstado.codRastreoRec = null;
                    this.objetoMalEstado.tipoServRec = null;
                    this.mensaje = "Agregado exitosamente, checar registros";

                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            agregarPerdido(objeto) {

                if (this.$v.objetoPerdido.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    console.log(copia);

                    this.datosCed.arregloPerdido.push(copia);

                    this.objetoPerdido.noGuiaRec = null;
                    this.objetoPerdido.codRastreoRec = null;
                    this.objetoPerdido.tipoServRec = null;
                    this.mensaje = "Agregado exitosamente, checar registros";

                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },
        }
    }
</script>