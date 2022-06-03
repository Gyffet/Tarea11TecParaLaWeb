<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-1 font-weight-bold mb-3"
                    align="center">
                    Cédula Automatizada para la Supervisión y Evaluación del Servicio de Agua para Beber
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
                        Monto Factura sin I.V.A. ni Retenciones
                        <v-text-field v-model="datosCed.monto"
                                      label="Sin comas ni símbolo de pesos"
                                      :error-messages="validMonto"
                                      @input="$v.datosCed.monto.$touch()"
                                      @blur="$v.datosCed.monto.$touch()"
                                      required
                                      prefix="$"
                                      :disabled="mesVacio!=true ? true : false"
                                      dense></v-text-field>
                    </v-col>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div v-if="mesVacio != true">
                    <v-btn color="teal darken-1"
                           dark
                           @click="preVerificarPeriodo()">
                        Verificar Periodo
                    </v-btn>

                </div>
                <div v-if="mesVacio == true">
                    <v-btn color="teal darken-1"
                           dark
                           link :to="{ path: '/api/Documents/DescargarArchivo/Resumen_PO_Limpieza.pdf' }"
                           target="_blank">
                        <v-icon dark
                                left>
                            mdi-file-pdf
                        </v-icon>
                        Programa de Operación
                    </v-btn>

                </div>
            </v-col>
        </v-row>

        <div v-if="opcionesServer.selectMes!=null && mesVacio == true">

            <v-row>
                <v-col class="title row" align="left">
                    1. ¿El prestador de servicios cumplió con las FECHAS pactadas para la realización del servicio?
                </v-col>
            </v-row>
            <v-row>
                <v-radio-group v-model="datosCed.boolFechas"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>
            <div v-if="datosCed.boolFechas=='false'">
                <v-row align="center" justify="space-around">

                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogFechas"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="teal darken-1"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Registrar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencia
                                    </v-card-title>


                                    <v-card-text>
                                        Capture todas las incidencias de retraso durante el periodo.
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada</span>
                                                <vc-date-picker v-model="objetoFechas.fechaProgramada"
                                                                required
                                                                :mask="masks"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :min-date=primerDia()
                                                                :max-date=ultimoDia()>

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
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Realizada</span>
                                                <vc-date-picker v-model="objetoFechas.fechaRealizada"
                                                                required
                                                                :mask="masks"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :min-date=primerDia()
                                                                :max-date=ultimoDia()
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
                                                   md="12">
                                                <v-text-field v-model="objetoFechas.comentarios"
                                                              :counter="300"
                                                              :error-messages="validComentFechas"
                                                              @input="$v.objetoFechas.comentarios.$touch()"
                                                              @blur="$v.objetoFechas.comentarios.$touch()"
                                                              label="Comentarios"
                                                              required></v-text-field>
                                            </v-col>
                                        </v-row>
                                        <br />
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogFechas = false">
                                            Cerrar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarIncFechas(objetoFechas)">
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
                                <v-dialog v-model="dialogRegisFechas"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="teal darken-1"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Consultar Incidencias
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
                                                                <th>Fecha Programada</th>
                                                                <th> | </th>
                                                                <th>Fecha Realizada</th>
                                                                <th> | </th>
                                                                <th>Comentarios</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloFechas" :key="item.tipoIncidencia">
                                                                <th>{{item.fechaProgramadaFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fechaRealizadaFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.comentarios}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarFecha(index)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="preEditarFecha(index)">
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
                                                   @click="dialogRegisFechas = false">
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

                    <div v-if="dialogEditarFechas==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarFechas"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            Capture todas las incidencias de retraso durante el periodo.
                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Programada</span>
                                                    <vc-date-picker v-model="objetoFechas.fechaProgramada"
                                                                    required
                                                                    :mask="masks"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :min-date=primerDia()
                                                                    :max-date=ultimoDia()>

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
                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Realizada</span>
                                                    <vc-date-picker v-model="objetoFechas.fechaRealizada"
                                                                    required
                                                                    :mask="masks"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :min-date=primerDia()
                                                                    :max-date=ultimoDia()
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
                                                       md="12">
                                                    <v-text-field v-model="objetoFechas.comentarios"
                                                                  :counter="300"
                                                                  :error-messages="validComentFechas"
                                                                  @input="$v.objetoFechas.comentarios.$touch()"
                                                                  @blur="$v.objetoFechas.comentarios.$touch()"
                                                                  label="Comentarios"
                                                                  required></v-text-field>
                                                </v-col>
                                            </v-row>
                                            <br />
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarFechas = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarFechas(objetoFechas)">
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
                    2. ¿El prestador de servicios cumplió con los HORARIOS pactados para la realización del servicio?
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolHoras"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>
            <div v-if="datosCed.boolHoras=='false'">
                <v-row align="center" justify="space-around">

                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogHoras"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="lime darken-3"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Registrar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencia
                                    </v-card-title>

                                    <v-card-text>
                                        Capture todas las incidencias de retraso durante el periodo. SOLO incidencias
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">



                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha y Hora de Incidencia</span>

                                                <vc-date-picker v-model="qwert"
                                                                required
                                                                :mask="masks"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :min-date=primerDia()
                                                                :max-date=ultimoDia()
                                                                v-on:change="setFechas($event)">

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
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Hora Programada</span>
                                                <vc-date-picker v-model="objetoHoras.horaProgramada"
                                                                mode="time"
                                                                :masks="mask2">

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
                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Hora Realizada</span>
                                                <vc-date-picker v-model="objetoHoras.horaEfectiva"
                                                                mode="time"
                                                                :masks="mask2">

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

                                        <br />
                                        <v-row>

                                            <v-col cols="12"
                                                   md="12">
                                                <v-text-field v-model="objetoHoras.comentarios"
                                                              :counter="300"
                                                              :error-messages="validComentFechas"
                                                              @input="$v.objetoHoras.comentarios.$touch()"
                                                              @blur="$v.objetoHoras.comentarios.$touch()"
                                                              label="Comentarios"
                                                              required></v-text-field>
                                            </v-col>
                                        </v-row>
                                        <br />
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogHoras = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarHoras(objetoHoras)">
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
                                <v-dialog v-model="dialogRegisHoras"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="lime darken-3"
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
                                                                <th>Fecha Programada</th>
                                                                <th> | </th>
                                                                <th>Hora Programada</th>
                                                                <th> | </th>
                                                                <th>Hora Realizada</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloHoras" :key="item.tipoIncidencia">
                                                                <th>{{item.fechaProgramadaFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{formatHoras(item.horaProgramada)}}</th>
                                                                <th> | </th>
                                                                <th>{{formatHoras(item.horaEfectiva)}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarHoras(index)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarHorasDiag(index)">
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
                                                   @click="dialogRegisHoras = false">
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

                    <div v-if="dialogEditarHoras==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarHoras"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            Capture todas las incidencias de retraso durante el periodo.
                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">



                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha y Hora de Incidencia</span>

                                                    <vc-date-picker v-model="qwert"
                                                                    required
                                                                    :mask="masks"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :min-date=primerDia()
                                                                    :max-date=ultimoDia()
                                                                    v-on:change="setFechas($event)">

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
                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Hora Programada</span>
                                                    <vc-date-picker v-model="objetoHoras.horaProgramada"
                                                                    mode="time"
                                                                    :masks="mask2">

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
                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Hora Realizada</span>
                                                    <vc-date-picker v-model="objetoHoras.horaEfectiva"
                                                                    mode="time"
                                                                    :masks="mask2">

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

                                            <br />
                                            <v-row>

                                                <v-col cols="12"
                                                       md="12">
                                                    <v-text-field v-model="objetoHoras.comentarios"
                                                                  :counter="300"
                                                                  :error-messages="validComentFechas"
                                                                  @input="$v.objetoHoras.comentarios.$touch()"
                                                                  @blur="$v.objetoHoras.comentarios.$touch()"
                                                                  label="Comentarios"
                                                                  required></v-text-field>
                                                </v-col>
                                            </v-row>
                                            <br />
                                            {{objetoHoras}}
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarHoras = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarHoras(objetoFechas)">
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
                        3. ¿El prestador de servicios entregó los reportes del servicios? (tickets, remisiones, resúmenes, etc.)?
                    </v-row>
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolReportes"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>



            <div v-if="datosCed.boolReportes=='false'">
                <v-row align="center" justify="space-around">
                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogReportes"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="teal darken-1"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias de Equipo
                                    </v-card-title>

                                    <v-card-text>

                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de la incidencia</span>
                                                <vc-date-picker v-model="objetoEquipo.fechaIncidencia"
                                                                required
                                                                :mask="masks"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :disabled-dates='{ weekdays: [1, 7] }'
                                                                :min-date=primerDia()
                                                                :max-date=ultimoDia()>

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
                                                <v-select id="selectMaquina"
                                                          v-model="objetoEquipo.maquinaria"
                                                          :items="maquinas"
                                                          :error-messages="validMaquinas"
                                                          @input="$v.objetoEquipo.maquinaria.$touch()"
                                                          @blur="$v.objetoEquipo.maquinaria.$touch()"
                                                          label="Maquinaría faltante"
                                                          dense
                                                          outlined></v-select>
                                            </v-col>

                                            <v-col cols="12"
                                                   md="12">
                                                <v-text-field v-model="objetoEquipo.comentarios"
                                                              :counter="300"
                                                              :error-messages="validComentEquipo"
                                                              @input="$v.objetoEquipo.comentarios.$touch()"
                                                              @blur="$v.objetoEquipo.comentarios.$touch()"
                                                              label="Comentarios"
                                                              required></v-text-field>
                                            </v-col>
                                        </v-row>

                                        <br />
                                    </v-card-text>

                                    <v-divider></v-divider>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary"
                                               text
                                               @click="dialogEquipo = false">
                                            Cerrar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarEquipo(objetoEquipo)">
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
                                <v-dialog v-model="dialogRegEquipo"
                                          width="1000">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-btn color="teal darken-1"
                                               dark
                                               v-bind="attrs"
                                               v-on="on">
                                            Consultar Incidencias
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
                                                                <th>Fecha Incidencia</th>
                                                                <th> | </th>
                                                                <th>Maquinara Faltante</th>
                                                                <th> | </th>
                                                                <th>Comentarios</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloEquipo" :key="item.maquinaria">
                                                                <th>{{item.fechaIncidenciaFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.maquinaria}}</th>
                                                                <th> | </th>
                                                                <th>{{item.comentarios}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarEquipo(item)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarEquipo(index)">
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
                                                   @click="dialogRegEquipo = false">
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


                    <div v-if="dialogEditarEquipo==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarEquipo"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>

                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de la incidencia</span>
                                                    <vc-date-picker v-model="objetoEquipo.fechaIncidencia"
                                                                    required
                                                                    :mask="masks"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                                    :min-date=primerDia()
                                                                    :max-date=ultimoDia()>

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
                                                    <v-select id="selectMaquina"
                                                              v-model="objetoEquipo.maquinaria"
                                                              :items="maquinas"
                                                              :error-messages="validMaquinas"
                                                              @input="$v.objetoEquipo.maquinaria.$touch()"
                                                              @blur="$v.objetoEquipo.maquinaria.$touch()"
                                                              label="Maquinaría faltante"
                                                              dense
                                                              outlined></v-select>
                                                </v-col>

                                                <v-col cols="12"
                                                       md="12">
                                                    <v-text-field v-model="objetoEquipo.comentarios"
                                                                  :counter="300"
                                                                  :error-messages="validComentEquipo"
                                                                  @input="$v.objetoEquipo.comentarios.$touch()"
                                                                  @blur="$v.objetoEquipo.comentarios.$touch()"
                                                                  label="Comentarios"
                                                                  required></v-text-field>
                                                </v-col>
                                            </v-row>

                                            <br />
                                        </v-card-text>

                                        <v-divider></v-divider>

                                        <v-card-actions>
                                            <v-spacer></v-spacer>
                                            <v-btn color="primary"
                                                   text
                                                   @click="dialogEditarEquipo = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarEqui(objetoEquipo)">
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
                        4. ¿El prestador de servicios cumplió con la regulación vigente y con la fecha de caducidad respecto de los garrafones que entregó en los inmuebles? (Revisar la fecha de caducidad de los garrafones)
                    </v-row>
                </v-col>
            </v-row>
            <v-row>
                <v-radio-group v-model="datosCed.boolAsistencias"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>

            <div v-if="datosCed.boolAsistencias=='true'">
                <v-col md="3">
                    Capture las inasistencias totales:
                    <br />
                    <v-text-field v-model="datosCed.inasistencias"
                                  :error-messages="validInasistencias"
                                  @input="$v.datosCed.inasistencias.$touch()"
                                  @blur="$v.datosCed.inasistencias.$touch()"
                                  required
                                  dense></v-text-field>

                </v-col>
            </div>



            <v-row>
                <v-col class="title row" align="left">
                    5. ¿Los garrafones cumplen con las características requeridas por el Consejo?
                </v-col>
            </v-row>
            <v-row>
                <vc-date-picker v-model="entregables.fechaCierre"
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




            <v-row align="center">
                
                <v-col>
                    <v-row class="title row">
                        6-1. ¿Se entregó el Acta Entrega-Recepción de los servicios mensual?
                    </v-row>
                    <v-row>
                        <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Entrega del Acta</span>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-radio-group v-model="entregables.boolActaEntrega"
                                           column>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-col>
                        <v-col>
                            <div v-if="entregables.boolActaEntrega=='true'">


                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                <vc-date-picker v-model="entregables.fechaActaEntrega"
                                                required
                                                :mask="masks"
                                                :locale="{firstDayOfWeek: 1}"
                                                :disabled-dates='{ weekdays: [1, 7] }'
                                                :min-date=primerDia()>

                                    <template v-slot="{ inputValue, inputEvents }">
                                        <v-text-field readonly
                                                      outlined
                                                      dense
                                                      label="Ingrese Fecha"
                                                      :value="inputValue"
                                                      v-on="inputEvents" />
                                    </template>

                                </vc-date-picker>
                            </div>
                        </v-col>
                        <v-col></v-col>
                    </v-row>
                </v-col>
            </v-row>

            <div v-if="primerMes==true">
                <v-row align="center">
                    <v-col>
                        <v-row>
                            <v-col class="title row" align="left">
                                A. ¿Cúando entregó el prestador del servicio el Programa de Operación?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha entrega del Programa de Operación &nbsp;</span>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-radio-group v-model="entregables.boolPO"
                                               column>
                                    <v-radio label="Sí"
                                             value="true"></v-radio>
                                    <v-radio label="No"
                                             value="false"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col>
                                <div v-if="entregables.boolPO=='true'">


                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                    <vc-date-picker v-model="entregables.fechaPO"
                                                    required
                                                    :mask="masks"
                                                    :locale="{firstDayOfWeek: 1}"
                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                    :min-date=primerDia()
                                                    :max-date=ultimoDia()>

                                        <template v-slot="{ inputValue, inputEvents }">
                                            <v-text-field readonly
                                                          outlined
                                                          dense
                                                          label="Ingrese Fecha"
                                                          :value="inputValue"
                                                          v-on="inputEvents" />
                                        </template>

                                    </vc-date-picker>
                                </div>
                            </v-col>
                            <v-col></v-col>
                        </v-row>

                    </v-col>

                    <v-col>
                        <v-row>
                            <v-col class="title row" align="left">
                                B. ¿Cúando entregó el prestador del servicio los uniformes y gafetes a su personal?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de Entrega de Uniformes</span>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-radio-group v-model="entregables.boolUniforme"
                                               column>
                                    <v-radio label="Sí"
                                             value="true"></v-radio>
                                    <v-radio label="No"
                                             value="false"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col>
                                <div v-if="entregables.boolUniforme=='true'">
                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                    <vc-date-picker v-model="entregables.fechaUniforme"
                                                    required
                                                    :mask="masks"
                                                    :locale="{firstDayOfWeek: 1}"
                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                    :min-date=primerDia()
                                                    :max-date=ultimoDia()>

                                        <template v-slot="{ inputValue, inputEvents }">
                                            <v-text-field readonly
                                                          outlined
                                                          dense
                                                          label="Ingrese Fecha"
                                                          :value="inputValue"
                                                          v-on="inputEvents" />
                                        </template>

                                    </vc-date-picker>
                                </div>
                            </v-col>
                            <v-col></v-col>
                        </v-row>
                    </v-col>
                </v-row>

                <v-row align="center">
                    

                    <v-col>
                        <v-row>
                            <v-col class="title row" align="left">
                                C. ¿Cúando entregó el prestador del servicio el Acta de Inicio del servicio?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Entrega Acta de Inicio</span>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-radio-group v-model="entregables.boolActaInicio"
                                               column>
                                    <v-radio label="Sí"
                                             value="true"></v-radio>
                                    <v-radio label="No"
                                             value="false"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col>
                                <div v-if="entregables.boolActaInicio=='true'">
                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                    <vc-date-picker v-model="entregables.fechaActaInicio"
                                                    required
                                                    :mask="masks"
                                                    :locale="{firstDayOfWeek: 1}"
                                                    :disabled-dates='{ weekdays: [1, 7] }'
                                                    :min-date=primerDia()
                                                    :max-date=ultimoDia()>

                                        <template v-slot="{ inputValue, inputEvents }">
                                            <v-text-field readonly
                                                          outlined
                                                          dense
                                                          label="Ingrese Fecha"
                                                          :value="inputValue"
                                                          v-on="inputEvents" />
                                        </template>

                                    </vc-date-picker>
                                </div>
                            </v-col>
                            <v-col></v-col>
                        </v-row>
                    </v-col>
                </v-row>
            </div>
            <!--Acaba el formulario-->

            <br /><br /><br /><br />
            <v-row align="center" justify="space-around">

                <v-btn color="teal darken-1"
                       dark
                       @click="guardar()">
                    Guardar sin enviar
                </v-btn>

                <div v-if="currentUser.newDataSet.roles.rol_rd==1">
                    <v-btn color="teal darken-1"
                           dark
                           @click="calcularPena()">
                        Recalcular Pena
                    </v-btn>
                </div>
                <v-btn color="teal darken-1"
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
        <!--{{currentUser.newDataSet.roles.rol_rd}}-->
        <!--{{penas}}-->
    </v-container>
</template>

<script>
    import { required, decimal, numeric, minValue, requiredIf } from 'vuelidate/lib/validators'
    import CedulasService from '../services/cedulas-service';
    import LimpiezaService from '../services/limpieza-service';

    export default {
        name: "Director",
        data: () => ({
            folio: null,
            primerMes: null,
            inicioContrat: new Date(2021, 0, 1),
            mensaje: "",
            colorSnackbar: "",
            snackbar: false,

            objetoFechas: {
                fechaProgramada: null,
                fechaRealizada: null,
                comentarios: null,
            },

            datosCed: {
                factura: "",
                monto: null,
                boolFechas: null,
                arregloFechas: [],



                //borrar desde aqui
                boolUniforme: null,
                boolEquipo: null,
                arregloEquipo: [],
                boolAsistencias: null,
                inasistencias: null,
            },
            entregables: {
                fechaCierre:null,
                boolIMSS: null,
                constanciaSUA: null,
                boolActaEntrega: null,
                fechaActaEntrega: null,
                boolVisita:null,
                fechaVisita: null,
                boolPO: null,
                fechaPO: null,
                boolUniforme: null,
                fechaUniforme: null,
                boolActaInicio: null,
                fechaActaInicio: null,
            },
            repetirFactura: "",
            masks: {
                input: 'YYYY-MM-DD',
            },

            dialogFechas: null,
            dialogRegisFechas: null,
            dialogEditarFechas: null,


            //borrar desde aqui

            //dialogProgramaOP: false,
            ///dialogRegisPO: false,
            //dialogRegEquipo: false,
            //dialogEditarEquipo: false,
            //dialogEquipo: false,

            opcionesServer: {
                selectPrimavera: null,
                selectInmueble: null,
                selectMes: null,
            },
            mesVacio: null,
            weekday: [1, 2, 3, 4, 5, 6, 0],
            primavera: [2021],
            incidenciaTipo: ['Sanitarios', 'Vestibulos y Pasillos', 'Elevadores', 'Oficinas', 'Exteriores', 'Oficinas Comunes', 'Desinfección', 'Vehículos'],
            inciSanitario: ['Pisos', 'Muros', 'Mamparas de Acero', 'Espejos', 'WC', 'Puertas'],
            inciPasillos: ['Pisos', 'Muros', 'Plafón', 'Puertas y Barandales Metálicos', 'Puertas Madera', 'Elementos de Cristal'],
            inciElevadores: ['Pisos', 'Muros', 'Acero', 'Plafón'],
            inciOficinas: ['Pisos', 'Alfombras', 'Cubiertas y Mesas', 'Teléfonos', 'Computadoras', 'Archiveros', 'Paneles Forrados', 'Sillas Forradas', 'Muros', 'Esculturas y Lámparas', 'Persianas', 'Lambrines de madera'],
            inciExtreriores: ['Pisos', 'Vidrios', 'Azoteas', 'Estacionamiento', 'Muros Estacionamiento', 'Lámparas-Tuberías-Gabinetes-Escalerillas', 'Rejillas Difusoras'],
            inciComunes: ['Pisos'],
            inciDesinfeccion: ['Desinfección'],
            inciVehiculos: ['Auto', 'Motocicleta', 'Desinfección Interna'],
            maquinas: ['Aspiradora Industrial', 'Apiradora para Agua', 'Pulidora Industrial', 'Pulidora de Mano', 'Manguera Flexible (agua)', 'Lavadora de agua a Presión', 'Extensión Eléctrica (40m)', 'Pulidor de Escalera', 'Carro de Limpieza', 'Señalamientos de Precaución', 'Escalera Aluminio de Tijera Reforzada (7 Peldaños)', 'Escalera Aluminio de Tijera Reforzada (9 Peldaños)', 'Escalera Aluminio de Tijera Reforzada (15 Peldaños)', 'Escalera Extensible Reforzada (8m a 10m)', 'Pala para Cernir Ceniceros', 'Codos para Mangueras', 'Nebulizador o Portátil o similar', 'Radio de Comunicación', 'Arnés de seguridad'],
            arrayVacio: [],
            inmuebles: [],
            inmueble: null,
            mes: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            direccion: null,
            index: null,
            dialogFactura: null,
            files: null,
            loading: false,
        }),

        validations: {
            datosCed: {
                monto: {
                    decimal
                },
                boolActividades: {
                    required
                },
                boolUniforme: {
                    required
                },
                boolEquipo: {
                    required
                },
                boolAsistencias: {
                    required
                },
                inasistencias: {
                    numeric,
                    minValue: minValue(1)
                }
            },

            entregables: {
                fechaCierre: {
                    required
                },
                boolIMSS: {
                    required
                },
                constanciaSUA: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolIMSS=="true"
                    })
                },
                //boolActaEntrega: {
                //    required
                //},
                //fechaActaEntrega: {
                //    required: requiredIf(this.entregables.boolActaEntrega=="true")
                //},
                //boolVisita: {
                //    required: requiredIf(this.primerMes==true)
                //},
                //fechaVisita: {
                //    required: requiredIf(this.entregables.boolVisita=="true")
                //},
                //boolPO: {
                //    required: requiredIf(this.primerMes == true)
                //},
                //fechaPO: {
                //    required: requiredIf(this.entregables.boolPO == "true")
                //},
                //boolUniforme: {
                //    required: requiredIf(this.primerMes == true)
                //},
                //fechaUniforme: {
                //    required: requiredIf(this.entregables.boolUniforme == "true")
                //},
                //boolActaInicio: {
                //    required: requiredIf(this.primerMes == true)
                //},
                //fechaActaInicio: {
                //    required: requiredIf(this.entregables.boolActaInicio == "true")
                //}
            },

            objetoFechas: {
                fechaProgramada: {
                    required
                },
                fechaRealizada: {
                    required
                },
                comentarios: {
                    required
                },
            },

            objetoManual: {
                fechaIncidencia: {
                    required
                },
                tipoIncidencia: {
                    required
                },
                areaIncidencia: {
                    required
                },
                comentarios: {
                    required
                }
            },

            objetoEquipo: {
                fechaIncidencia: {
                    required
                },
                maquinaria: {
                    required
                },
                comentarios: {
                    required
                }
            },
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
                CedulasService.getCedula("Limpieza", folio).then(
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
                        else {
                            this.primerMes = false;
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

                        if (ced.cedula.fiInasistencias != null) {
                            this.datosCed.boolAsistencias = "true";
                            this.datosCed.inasistencias = ced.cedula.fiInasistencias;
                        }
                        else {
                            this.datosCed.boolAsistencias = "false";
                        }

                        if (ced.cedula.fdSUA != null) {
                            this.datosCed.boolIMSS = "true";
                            this.datosCed.constanciaSUA = ced.cedula.fdSUA;
                        }
                        else {
                            this.datosCed.boolIMSS = "false";
                        }

                        this.datosCed.fechaPO = ced.cedula.fdFechaPO;
                        this.datosCed.fechaVisita = ced.cedula.fdFechaActividades;

                        if (ced.arreglos.arregloActi.length > 0) {
                            this.datosCed.boolActividades = "false";

                            ced.arreglos.arregloActi.forEach(element => {
                                console.log("Insertando");
                                console.log(element);
                                this.objetoManual.fechaIncidencia = element.fdFechaInci;
                                this.objetoManual.tipoIncidencia = element.fcTipo;
                                this.objetoManual.areaIncidencia = element.fcArea;
                                this.objetoManual.comentarios = element.fcComentarios
                                this.agregarIncPO(this.objetoManual);
                            });
                        }
                        else {
                            this.datosCed.boolActividades = "true";
                        }

                        if (ced.cedula.fcPenaUniforme == null) {
                            this.datosCed.boolUniforme = "true";
                        }
                        else {
                            this.datosCed.boolUniforme = "false";
                        }

                        if (ced.arreglos.arregloEquipo.length > 0) {
                            this.datosCed.boolEquipo = "false";
                            ced.arreglos.arregloEquipo.forEach(element => {
                                console.log(element);
                                this.objetoEquipo.fechaIncidencia = element.fdFechaInci;
                                this.objetoEquipo.maquinaria = element.fcMaquina;
                                this.objetoEquipo.comentarios = element.fcComentarios;
                                this.agregarEquipo(this.objetoEquipo);
                            })
                        }
                        else {
                            this.datosCed.boolEquipo = "true";
                        }

                        this.entregables.fechaCierre = ced.entregable.fdCierreMes;

                        if (ced.entregable.fdSUAIMSS != null) {
                            this.entregables.boolIMSS = "true";
                            this.entregables.constanciaSUA = ced.entregable.fdSUAIMSS;
                        }
                        else {
                            this.entregables.boolIMSS = "false";
                        }

                        if (ced.entregable.fdActaServicios != null) {
                            this.entregables.boolActaEntrega = "true";
                            this.entregables.fechaActaEntrega = ced.entregable.fdActaServicios;
                        }
                        else {
                            this.entregables.boolActaEntrega = "false";
                        }

                        if (ced.entregable.fdVisitaInstalaciones != null) {
                            this.entregables.boolVisita = "true";
                            this.entregables.fechaVisita = ced.entregable.fdVisitaInstalaciones;
                        }
                        else {
                            this.entregables.boolVisita = "false";
                        }

                        if (ced.entregable.fdProgramaOperacion != null) {
                            this.entregables.boolPO = "true";
                            this.entregables.fechaPO = ced.entregable.fdProgramaOperacion;
                        }
                        else {
                            this.entregables.boolPO = "false";
                        }

                        if (ced.entregable.fdGafeteUniforme != null) {
                            this.entregables.boolUniforme = "true";
                            this.entregables.fechaUniforme = ced.entregable.fdGafeteUniforme;
                        }
                        else {
                            this.entregables.boolUniforme = "false";
                        }

                        if (ced.entregable.fdActaInicio != null) {
                            this.entregables.boolActaInicio = "true";
                            this.entregables.fechaActaInicio = ced.entregable.fdActaInicio;
                        }
                        else {
                            this.entregables.boolActaInicio = "false";
                        }

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

            validComentFechas() {
                const errors = [];
                if (!this.$v.objetoFechas.comentarios.$dirty) return errors;
                !this.$v.objetoFechas.comentarios.required && errors.push('El campo es requerido');
                return errors;
            },

            //borrar en adelante

            validFechaInci() {
                const errors = [];
                if (!this.$v.objetoManual.fechaIncidencia.$dirty) return errors
                !this.$v.objetoManual.fechaIncidencia.required && errors.push('El campo es requerido')
                return errors
            },

            validTipoInci() {
                const errors = [];
                if (!this.$v.objetoManual.tipoIncidencia.$dirty) return errors
                !this.$v.objetoManual.tipoIncidencia.required && errors.push('El campo es requerido')
                return errors
            },

            validAreaInci() {
                const errors = [];
                if (!this.$v.objetoManual.areaIncidencia.$dirty) return errors
                !this.$v.objetoManual.areaIncidencia.required && errors.push('El campo es requerido')
                return errors
            },

            validComentInci() {
                const errors = [];
                if (!this.$v.objetoManual.comentarios.$dirty) return errors
                !this.$v.objetoManual.comentarios.required && errors.push('El campo es requerido')
                return errors
            },

            validMaquinas() {
                const errors = [];
                if (!this.$v.objetoEquipo.maquinaria.$dirty) return errors
                !this.$v.objetoEquipo.maquinaria.required && errors.push('El campo es requerido')
                return errors
            },

            validInasistencias() {
                const errors = [];
                if (!this.$v.datosCed.inasistencias.$dirty) return errors;
                !this.$v.datosCed.inasistencias.numeric && errors.push('Solo se aceptan números');
                !this.$v.datosCed.inasistencias.minValue && errors.push('Números mayores a 0');
                return errors;
            },

            validComentEquipo() {
                const errors = [];
                if (!this.$v.objetoEquipo.comentarios.$dirty) return errors
                !this.$v.objetoEquipo.comentarios.required && errors.push('El campo es requerido')
                return errors
            },

            validMonto() {
                const errors = [];
                if (!this.$v.datosCed.monto.$dirty) return errors;
                //!this.$v.datosCed.monto.required && errors.push('El campo es requerido');
                !this.$v.datosCed.monto.decimal && errors.push('Solo números sin comas ni $');
                return errors;
            },
        },//Listo

        methods: {
            agregarIncFechas(objeto) {
                if (this.$v.objetoFechas.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaProgramadaFormat = copia.fechaProgramada.toString();
                    copia.fechaProgramadaFormat = copia.fechaProgramadaFormat.substring(0, 15);

                    copia.fechaRealizadaFormat = copia.fechaRealizada.toString();
                    copia.fechaRealizadaFormat = copia.fechaRealizadaFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloFechas.push(copia);

                    this.objetoFechas.fechaProgramada = null;
                    this.objetoFechas.fechaRealizada = null;
                    this.objetoFechas.comentarios = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            eliminarFecha(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloFechas;
                var result = confirm("¿Eliminar registro?");
                if (result == true) {
                    console.log("se elimina ", arreglo[item]);
                    arreglo.splice(item, 1);
                }
                else {
                    alert("Operación cancelada");
                    console.log("es falso")
                }
                console.log("termino");
            },

            preEditarFecha(index) {
                console.log(this.datosCed.arregloFechas[index]);
                this.objetoFechas = { ...this.datosCed.arregloFechas[index] }
                console.log(this.objetoFechas);
                this.index = index;
                this.dialogRegisFechas = false;
                this.dialogEditarFechas = true;
            },

            editarFechas(objeto) {
                this.datosCed.arregloFechas.splice(this.index, 1);
                this.agregarIncFechas(objeto);
            },

            //aqui van las viejas, no editadas, tal vez se desechen al acabar

            calcularPena() {
                console.log('calcular pena');
                console.log(this.primerMes);
                LimpiezaService.calcularPena(this.datosCed, this.entregables, this.primerMes).then(
                    cp => {
                        console.log("Objeto Penas regresado del servidor:")
                        this.penas = cp;
                        console.log(cp);
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
                    entregables: JSON.stringify(this.entregables),
                    periodo: this.opcionesServer,
                    area: this.currentUser.newDataSet.areas.cveArea,
                    folio: this.folio,
                    usuario: this.currentUser.newDataSet.datosGenerales.nombre_NPM,
                    correo: this.currentUser.newDataSet.datosGenerales.correo_electronico
                };
                console.log(datos);
                LimpiezaService.enviar(datos).then(
                    g => {
                        console.log("Enviado");
                        console.log(g);

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
                LimpiezaService.insertArchivo(files).then(
                    c => {
                        this.mensaje = 'Enviado con éxito';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                        window.open(
                            '/api/Documents/ReporteLimpiezaPorValidar/' + folio,
                            '_blank' // <- This is what makes it open in a new window.
                        );
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

            verificarFormulario() {
                if (this.$v.datosCed.$invalid) {
                    console.log("1");
                    this.mensaje = "Falta contestar alguna pregunta o campo";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }
                else if (!this.$v.datosCed.$invalid) {
                    if (this.$v.entregables.$invalid) {
                        this.mensaje = "Faltan contestar preguntas de entregables";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolActividades == "false" && this.datosCed.arregloActividades.length < 1) {
                        this.mensaje = "Contastaste NO en la pregunta 1 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.primerMes == true && this.entregables.fechaVisita == null && this.entregables.boolVisita == "true") {
                        console.log("2");
                        this.mensaje = "Falta contestar alguna pregunta o campo";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.primerMes == true && this.entregables.fechaPO == null && this.entregables.boolPO == "true") {
                        console.log("3");
                        this.mensaje = "Falta contestar alguna pregunta o campo";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.$v.datosCed.monto.$invalid || this.datosCed.monto == null || this.datosCed.monto == "" || this.datosCed.monto == "0" || this.datosCed.monto == 0 || this.datosCed.monto < 0) {
                        console.log("4");
                        console.log("monto: " + this.datosCed.monto);
                        this.mensaje = "Parece que en el monto de la factura es 0 o el campo esta vacío";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolEquipo == "false" && this.datosCed.arregloEquipo.length < 1) {
                        console.log("5");
                        this.mensaje = "Contastaste NO en la pregunta 3 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.factura != this.repetirFactura) {
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

            primerDia() {
                var mes = this.getMes();
                return new Date(2021, mes, 1);
            },

            ultimoDia() {
                var mes = this.getMes();
                return new Date(2021, mes + 1, 0);
            },

            getMes() {
                var mes = null;
                switch (this.opcionesServer.selectMes) {
                    case "Enero":
                        mes = 0;
                        break;
                    case "Febrero":
                        mes = 1;
                        break;
                    case "Marzo":
                        mes = 2;
                        break;
                    case "Abril":
                        mes = 3;
                        break;
                    case "Mayo":
                        mes = 4;
                        break;
                    case "Junio":
                        mes = 5;
                        break;
                    case "Julio":
                        mes = 6;
                        break;
                    case "Agosto":
                        mes = 7;
                        break;
                    case "Septiembre":
                        mes = 8;
                        break;
                    case "Octubre":
                        mes = 9;
                        break;
                    case "Noviembre":
                        mes = 10;
                        break;
                    case "Diciembre":
                        mes = 11;
                        break;
                    default:
                        mes = null;
                }
                return mes;
            },

            guardar() {
                console.log('guardar sin enviar');
                var datos = {
                    cedula: this.datosCed,
                    entregables: JSON.stringify(this.entregables),
                    periodo: this.opcionesServer,
                    area: this.currentUser.newDataSet.areas.cveArea,
                    folio: this.folio,
                    usuario: this.currentUser.newDataSet.datosGenerales.nombre_NPM
                };
                console.log(datos);
                LimpiezaService.guardar(datos).then(
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

            arregloFechas(objeto) {
                this.datosCed.arregloEquipo.splice(this.index, 1);
                this.agregarEquipo(objeto);
            },

            

            eliminarEquipo(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloEquipo;
                var result = confirm("¿Eliminar registro?");
                if (result == true) {
                    console.log("se elimina ", arreglo[item]);
                    arreglo.splice(item, 1);
                }
                else {
                    alert("Operación cancelada");
                }
            },

            

            preEditarFechafdsfsdfs(objeto) {
                this.datosCed.arregloFechas.splice(this.index, 1);
                this.agregarIncPO(objeto);
            },

            agregarIncFechastertet(index) {
                console.log(this.datosCed.arregloFechas[index]);
                this.objetoFechas = { ...this.datosCed.arregloFechas[index] }
                console.log(this.objetoFechas);
                this.index = index;
                this.dialogProgramaOP = false;
                this.dialogEditarFechas = true;
            },

            

            agregarIncPO(objeto) {
                if (this.$v.objetoManual.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else {


                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaIncidenciaFormat = copia.fechaIncidencia.toString();
                    copia.fechaIncidenciaFormat = copia.fechaIncidenciaFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloActividades.push(copia);

                    this.objetoManual.fechaIncidencia = null;
                    this.objetoManual.tipoIncidencia = null;
                    this.objetoManual.areaIncidencia = null;
                    this.objetoManual.comentarios = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            tipoInci(incidencia) {
                console.log(incidencia);
                switch (incidencia) {
                    case 'Sanitarios':
                        return this.inciSanitario;
                    case 'Vestibulos y Pasillos':
                        return this.inciPasillos;
                    case 'Elevadores':
                        return this.inciElevadores;
                    case 'Oficinas':
                        return this.inciOficinas;
                    case 'Exteriores':
                        return this.inciExtreriores;
                    case 'Oficinas Comunes':
                        return this.inciComunes;
                    case 'Desinfección':
                        return this.inciDesinfeccion;
                    case 'Vehículos':
                        return this.inciVehiculos;

                    default:
                        return this.arrayVacio;
                }
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
                    this.mensaje = "El Número de la Factura y Confirmar No, de la Factura no coinciden";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }
                else if (verificacion.inmueble == null || verificacion.periodo.selectPrimavera == null || verificacion.periodo.selectMes == null) {
                    this.mensaje = "No se han registrado los datos de identificación de la cédula: Año de evaluación, Inmueble a evaluar y/o Mes de evaluación";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }

                else if (verificacion.factura == null || verificacion.factura == "" || verificacion.factura == " ") {
                    var result = confirm("No se ha registrado el Número de la Factura, Confirmar No. de la Factura . ¿Desea dejar pendiente su registro?");
                    if (result == true) {
                        this.verificarPeriodo(verificacion);
                    }
                    else {
                        alert("Por favor llene los campos de número de factura");
                    }
                }

                else {
                    this.verificarPeriodo(verificacion);
                }
            },

            verificarPeriodo(verificacion) {
                this.datosCed.monto = null;
                this.datosCed.boolActividades = null;
                this.datosCed.arregloRecoleccion = [];
                this.datosCed.boolEntrega = null;
                this.datosCed.arregloEntrega = [];
                this.datosCed.boolEquipo = null;
                this.datosCed.arregloAcuses = [];
                this.datosCed.boolMalEstado = null;
                this.datosCed.arregloMalEstado = [];
                this.datosCed.boolExtraviadas = null;
                this.datosCed.arregloPerdido = [];
                this.datosCed.boolMaterial = null;
                this.datosCed.diasMaterial = null;
                this.datosCed.boolUniforme = null;

                LimpiezaService.verificarPeriodo(verificacion).then(
                    v => {
                        console.log("Resultado");
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
        }
    }
</script>