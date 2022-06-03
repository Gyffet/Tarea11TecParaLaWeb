<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-1 font-weight-bold mb-3"
                    align="center">
                    Cédula Automatizada para la Supervisión y Evaluación del Servicio de Fumigación y Control de Fauna Nociva
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
                    <v-btn color="lime darken-3"
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
                    1. ¿El prestador cumplió con las FECHAS pactadas para la realización del servicio?
                </v-col>
            </v-row>
            <v-row>
                <v-radio-group v-model="datosCed.boolfechas"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>
            <div v-if="datosCed.boolfechas=='false'">
                <v-row align="center" justify="space-around">

                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogFechas"
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
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarFechas(objetoFechas)">
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
                                                                        @click="eliminarFechas(index)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarFechasDiag(index)">
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
                                            Capture todas las incidencias de retraso durante el periodo. SOLO incidencias
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
                <v-col>
                    <v-row class="title row">
                        2. ¿El prestador cumplió con las HORAS programadas para la ejecución del servicio?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        El prestador está obligado a ejecutar el servicio en las horas pactadas
                    </v-row>
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
                        3. ¿El prestador del servicio cumplió con la efectividad de la fumigación o erradicación de fauna nociva?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        No debe de aparecer fauna nociva dentro de los primeros 5 días naturales desde que se hizo la fumigación
                    </v-row>
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolFumigacion"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>



            <div v-if="datosCed.boolFumigacion=='false'">
                <v-row align="center" justify="space-around">
                    <template>
                        <div class="text-center">
                            <v-dialog v-model="dialogFumigacion"
                                      width="1000">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="lime darken-3"
                                           dark
                                           v-bind="attrs"
                                           v-on="on">
                                        Agregar Incidencia
                                    </v-btn>
                                </template>

                                <v-card>
                                    <v-card-title class="headline grey lighten-2">
                                        Registrar Incidencias de Fumigación
                                    </v-card-title>

                                    <v-card-text>
                                        Si la fumigación se hizo el 1° del mes no debe de aparecer fauna nociva por lo menos hasta el 5° del mes
                                        <v-row align="center">
                                            <v-col cols="12"
                                                   md="4">

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Fumigación</span>
                                                <vc-date-picker v-model="objetoFumigacion.fechaFumigacion"
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

                                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Reaparición de Fauna</span>

                                                <vc-date-picker v-model="objetoFumigacion.fechaReaparicion"
                                                                required
                                                                :mask="masks"
                                                                :locale="{firstDayOfWeek: 1}"
                                                                :min-date='objetoFumigacion.fechaFumigacion'
                                                                :max-date='(objetoFumigacion.fechaFumigacion != null ? mas5Dias() : ultimoDia())'
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
                                                <v-text-field v-model="objetoFumigacion.comentarios"
                                                              :counter="300"
                                                              :error-messages="validComentFumi"
                                                              @input="$v.objetoFumigacion.comentarios.$touch()"
                                                              @blur="$v.objetoFumigacion.comentarios.$touch()"
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
                                               @click="dialogFumigacion = false">
                                            Terminar
                                        </v-btn>
                                        <v-btn color="primary"
                                               text
                                               @click="agregarFumigacion(objetoFumigacion)">
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
                                <v-dialog v-model="dialogRegFumigacion"
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
                                                                <th>Fecha Fumigación</th>
                                                                <th> | </th>
                                                                <th>Fecha Reaparición</th>
                                                                <th> | </th>
                                                                <th>Comentarios</th>
                                                                <th> | </th>
                                                                <th>Eliminar</th>
                                                                <th> | </th>
                                                                <th>Editar</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <tr v-for="(item,index) in datosCed.arregloFumigacion" :key="item.comentarios">
                                                                <th>{{item.fechaFumigacionFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.fechaReaparicionFormat}}</th>
                                                                <th> | </th>
                                                                <th>{{item.comentarios}}</th>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="red"
                                                                        @click="eliminarFumigacion(index)">
                                                                    mdi-delete-empty
                                                                </v-icon>
                                                                <th> | </th>
                                                                <v-icon class="mr-2"
                                                                        color="blue"
                                                                        @click="editarFumiDiag(index)">
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
                                                   @click="dialogRegFumigacion = false">
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


                    <div v-if="dialogEditarFumigacion==true">
                        <template>
                            <div class="text-center">
                                <v-dialog v-model="dialogEditarFumigacion"
                                          width="1000">

                                    <v-card>
                                        <v-card-title class="headline grey lighten-2">
                                            Editar Incidencia
                                        </v-card-title>

                                        <v-card-text>
                                            Si la fumigación se hizo el 1° del mes no debe de aparecer fauna nociva por lo menos hasta el 5° del mes
                                            <v-row align="center">
                                                <v-col cols="12"
                                                       md="4">

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Fumigación</span>
                                                    <vc-date-picker v-model="objetoFumigacion.fechaFumigacion"
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

                                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Reaparición de Fauna</span>

                                                    <vc-date-picker v-model="objetoFumigacion.fechaReaparicion"
                                                                    required
                                                                    :mask="masks"
                                                                    :locale="{firstDayOfWeek: 1}"
                                                                    :min-date='objetoFumigacion.fechaFumigacion'
                                                                    :max-date='(objetoFumigacion.fechaFumigacion != null ? mas5Dias() : ultimoDia())'
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
                                                    <v-text-field v-model="objetoFumigacion.comentarios"
                                                                  :counter="300"
                                                                  :error-messages="validComentFumi"
                                                                  @input="$v.objetoFumigacion.comentarios.$touch()"
                                                                  @blur="$v.objetoFumigacion.comentarios.$touch()"
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
                                                   @click="dialogEditarFumigacion = false">
                                                Cancelar
                                            </v-btn>
                                            <v-btn color="primary"
                                                   text
                                                   @click="editarFumigacion(objetoFumigacion)">
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
                        4. ¿Los productos que utilizó el prestador del servicio cumplió con la regulación vigente del etiquetado y se encontraban vigentes?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        Puede consultar la regulación vigente dando <a href="http://www.dof.gob.mx/normasOficiales/4020/salud/salud.htm" target="_blank"> click aquí</a>
                    </v-row>
                </v-col>
            </v-row>

            <v-row>
                <v-radio-group v-model="datosCed.boolProductos"
                               row>
                    <v-radio label="Sí"
                             value="true"></v-radio>
                    <v-radio label="No"
                             value="false"></v-radio>
                </v-radio-group>
            </v-row>

            <v-row>
                <v-col class="title row" align="left">
                    5. ¿Cúando se estableció el cierre de mes?
                </v-col>
            </v-row>
            <v-row>
                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha Cierre de Mes</span>
            </v-row>
            <v-row>
                <vc-date-picker v-model="entregables.fechaCierre"
                                required
                                :mask="masks"
                                :locale="{firstDayOfWeek: 1}"
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
                        6-1. ¿Fue entregado el reporte de servicios?
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-radio-group v-model="entregables.boolReporteServ"
                                           column>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-col>
                        <v-col>
                            <div v-if="entregables.boolReporteServ=='true'">


                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de Entrega</span>
                                <vc-date-picker v-model="entregables.reporteServ"
                                                required
                                                :mask="masks"
                                                :locale="{firstDayOfWeek: 1}"
                                                :min-date=primerDia()>
                                    <!--
                            :max-date=ultimoDia()>-->

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
                    <v-row class="title row">
                        6-2. ¿Se entregó el listado del personal asignado para la prestación del servicio?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        Este debe de entregarse haya o no haya cambios en el personal
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-radio-group v-model="entregables.boolListaPersonal"
                                           column>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-col>
                        <v-col>
                            <div v-if="entregables.boolListaPersonal=='true'">


                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                <vc-date-picker v-model="entregables.listaPersonal"
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
                            </div>
                        </v-col>
                        <v-col></v-col>
                    </v-row>
                </v-col>
            </v-row>

            <v-row align="center">
                <v-col>

                    <v-row class="title row">
                        6-3. ¿Fue entregado el comprobante de pago y las constancias de inscripción al IMSS (SUA)?
                    </v-row>
                    <v-row class="subtitle-1 row">
                        Debe de entregarse el día 22 de cada mes
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-radio-group v-model="entregables.boolIMSS"
                                           column>
                                <v-radio label="Sí"
                                         value="true"></v-radio>
                                <v-radio label="No"
                                         value="false"></v-radio>
                            </v-radio-group>
                        </v-col>
                        <v-col>
                            <div v-if="entregables.boolIMSS=='true'">


                                <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de Entrega</span>
                                <vc-date-picker v-model="entregables.constanciaSUA"
                                                required
                                                :mask="masks"
                                                :locale="{firstDayOfWeek: 1}"
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

                <v-col>
                    <v-row class="title row">
                        6-4. ¿Se entregó el Acta Entrega-Recepción de los servicios mensual?
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
                                A. ¿Cúando entregó el Programa de Operación?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
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


                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de la Entrega</span>
                                    <vc-date-picker v-model="entregables.fechaPO"
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
                                </div>
                            </v-col>
                            <v-col></v-col>
                        </v-row>
                    </v-col>


                    <v-col>
                        <v-row>
                            <v-col class="title row" align="left">
                                B. ¿Cúando entregó la descripción y fotografías del gafete y uniforme al personal?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
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


                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de la Entrega</span>
                                    <vc-date-picker v-model="entregables.fechaUniforme"
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
                                D. ¿Cúando entregó el prestador del servicio el listado del personal asignado?
                            </v-col>
                        </v-row>
                        <v-row>
                            <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de Entrega</span>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-radio-group v-model="entregables.boolPersonal"
                                               column>
                                    <v-radio label="Sí"
                                             value="true"></v-radio>
                                    <v-radio label="No"
                                             value="false"></v-radio>
                                </v-radio-group>
                            </v-col>
                            <v-col>
                                <div v-if="entregables.boolPersonal=='true'">
                                    <span class="block text-gray-600 text-sm text-left font-bold mb-2">Fecha de entrega</span>
                                    <vc-date-picker v-model="entregables.fechaPersonal"
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
                                </div>
                            </v-col>
                            <v-col></v-col>
                        </v-row>
                    </v-col>
                </v-row>
            </div>

            <!--Acaba el formulario-->

            <br /><br /><br />
            <v-row align="center" justify="space-around">

                <v-btn color="lime darken-3"
                       dark
                       @click="guardar()">
                    Guardar sin enviar
                </v-btn>

                <div v-if="currentUser.newDataSet.roles.rol_rd==1">
                    <v-btn color="lime darken-3"
                           dark
                           @click="calcularPena()">
                        Recalcular Pena
                    </v-btn>
                </div>

                <v-btn color="lime darken-3"
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
    import { required, decimal, requiredIf } from 'vuelidate/lib/validators'
    import CedulasService from '../services/cedulas-service';
    import FumigacionService from '../services/fumigacion-service';

    export default {
        name: "Director",
        data: () => ({
            folio: null,
            primerMes: null,
            inicioContrat: new Date(2021,0,1),
            mensaje: "",
            colorSnackbar: "",
            snackbar: false,


            objetoFechas: {
                fechaProgramada: null,
                fechaRealizada: null,
                comentarios: null,
                fechaProgramadaFormat: null,
                fechaRealizadaFormat: null
            },

            objetoHoras: {
                fechaProgramada: null,
                horaProgramada: null,
                horaEfectiva: null,
                comentarios: null,
                fechaProgramadaFormat: null,
            },

            objetoFumigacion: {
                fechaFumigacion: null,
                fechaReaparicion: null,
                comentarios: null,
                fechaFumigacionFormat: null,
                fechaReaparicionFormat: null
            },

            datosCed: {
                factura: "",
                monto: null,
                boolfechas: null,
                arregloFechas: [],
                boolHoras: null,
                arregloHoras: [],
                boolFumigacion: null,
                arregloFumigacion: [],
                boolProductos: null,
            },

            entregables: {
                fechaCierre: null,
                boolReporteServ: null,
                reporteServ: null,
                boolListaPersonal: null,
                listaPersonal: null,
                boolIMSS: null,
                constanciaSUA: null,
                boolActaEntrega: null,
                fechaActaEntrega: null,

                boolPO: null,
                fechaPO: null,
                boolUniforme: null,
                fechaUniforme: null,
                boolActaInicio: null,
                fechaActaInicio: null,
                boolPersonal: null,
                fechaPersonal: null,
            },

            repetirFactura: "",
            masks: {
                input: 'YYYY-MM-DD',
            },
            
            dialogFechas: null,
            dialogRegisFechas: null,
            dialogEditarFechas: null,
            dialogHoras: null,
            dialogRegisHoras: null,
            dialogEditarHoras: null,
            dialogFumigacion: null,
            dialogRegFumigacion: null,
            dialogEditarFumigacion: null,

            mask2: {
                input: 'YYYY-MM-DD h:mm A',
            },

            opcionesServer: {
                selectPrimavera: null,
                selectInmueble: null,
                selectMes: null,
            },
            mesVacio: null,
            weekday: [1, 2, 3, 4, 5, 6, 0],
            primavera: [2021],
            qwert:null,

            fechaHoras: null,
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

            objetoHoras: {
                fechaProgramada: {
                    required
                },
                comentarios: {
                    required
                },
            },

            objetoFumigacion: {
                fechaFumigacion: {
                    required
                },
                fechaReaparicion: {
                    required
                },
                comentarios: {
                    required
                }
            },

            datosCed: {
                monto: {
                    decimal,
                    required
                },
                boolfechas: {
                    required
                },
                boolHoras: {
                    required
                },
                boolFumigacion: {
                    required
                },
                boolProductos: {
                    required
                }
            },

            entregables: {
                fechaCierre: {
                    required
                },
                boolReporteServ: {
                    required
                },
                reporteServ: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolReporteServ == "true"
                    })
                },
                boolListaPersonal: {
                    required
                },
                listaPersonal: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolListaPersonal == "true"
                    })
                },
                boolIMSS: {
                    required
                },
                constanciaSUA: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolIMSS == "true"
                    })
                },
                boolActaEntrega: {
                    required
                },
                fechaActaEntrega: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolActaEntrega == "true"
                    })
                },
                boolPO: {
                    required: requiredIf(function () {
                        return this.primerMes == true
                    })
                },
                fechaPO: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolUniforme == "boolPO" && this.primerMes == true
                    })
                },
                boolUniforme: {
                    required: requiredIf(function () {
                        return this.primerMes == true
                    })
                },
                fechaUniforme: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolUniforme == "true" && this.primerMes == true
                    })
                },
                boolActaInicio: {
                    required: requiredIf(function () {
                        return this.primerMes == true
                    })
                },
                fechaActaInicio: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolActaInicio == "true" && this.primerMes == true
                    })
                },
                boolPersonal: {
                    required: requiredIf(function () {
                        return this.primerMes == true
                    })
                },
                fechaPersonal: {
                    required: requiredIf(function (entregables) {
                        return entregables.boolPersonal == "true" && this.primerMes == true
                    })
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
                CedulasService.getCedula("Fumigacion", folio).then(
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

                        if (ced.arreglos.arregloFechas.length > 0) {
                            this.datosCed.boolfechas = "false";

                            ced.arreglos.arregloFechas.forEach(element => {
                                console.log("Insertando");
                                console.log(element);
                                this.objetoFechas.fechaProgramada = element.fdFechaProgramada;
                                this.objetoFechas.fechaRealizada = element.fdFechaRealizada;
                                this.objetoFechas.comentarios = element.fcComentarios
                                this.agregarFechas(this.objetoFechas);
                            });
                        }
                        else {
                            this.datosCed.boolfechas = "true";
                        }

                        
                        //Objeto Horas
                        if (ced.arreglos.arregloHoras.length > 0) {
                            this.datosCed.boolHoras = "false";
                            ced.arreglos.arregloHoras.forEach(element => {
                                console.log(element);
                                this.objetoHoras.fechaProgramada = element.fdFechaProgramada;
                                this.objetoHoras.horaProgramada = element.fdHoraProgramada;
                                this.objetoHoras.horaEfectiva = element.fdHoraEfectiva;
                                this.objetoHoras.comentarios = element.fcComentarios;
                                this.agregarHoras(this.objetoHoras);
                            })
                        }
                        else {
                            this.datosCed.boolHoras = "true";
                        }

                        //Objeto Fumigación
                        if (ced.arreglos.arregloFumigacion.length > 0) {
                            this.datosCed.boolFumigacion = "false";
                            ced.arreglos.arregloFumigacion.forEach(element => {
                                console.log(element);
                                this.objetoFumigacion.fechaFumigacion = element.fdFechaFumigacion;
                                this.objetoFumigacion.fechaReaparicion = element.fdFechaReaparacion;
                                this.objetoFumigacion.comentarios = element.fcComentarios;
                                this.agregarFumigacion(this.objetoFumigacion);
                            })
                        }
                        else {
                            this.datosCed.boolFumigacion = "true";
                        }

                        //Etiquetado
                        if (ced.cedula.fiEtiquetas == 0) {
                            this.datosCed.boolProductos = "false";
                        }
                        else if (ced.cedula.fiEtiquetas == 1) {
                            this.datosCed.boolProductos = "true";
                        }
                        

                        //Cierre de mes
                        if (ced.entregable.fdCierreMes != null) {
                            this.entregables.fechaCierre = ced.entregable.fdCierreMes;
                        }

                        //entregables
                        if (ced.entregable.fdReporteServ != null) {
                            this.entregables.boolReporteServ = "true";
                            this.entregables.reporteServ = ced.entregable.fdReporteServ;
                        }
                        else {
                            this.entregables.boolReporteServ = "false";
                        }

                        if (ced.entregable.fdListadePersonal != null) {
                            this.entregables.boolListaPersonal = "true";
                            this.entregables.listaPersonal = ced.entregable.fdListadePersonal;
                        }
                        else {
                            this.entregables.boolListaPersonal = "false";
                        }

                        if (ced.entregable.fdSUAIMSS != null) {
                            this.entregables.boolIMSS = "true";
                            this.entregables.constanciaSUA = ced.entregable.fdSUAIMSS;
                        }
                        else {
                            this.entregables.boolIMSS = "false";
                        }

                        if (ced.entregable.fdActaEntrega != null) {
                            this.entregables.boolActaEntrega = "true";
                            this.entregables.fechaActaEntrega = ced.entregable.fdActaEntrega;
                        }
                        else {
                            this.entregables.boolActaEntrega = "false";
                        }

                        if (ced.entregable.fdProgramaOperacion != null) {
                            this.entregables.boolPO = "true";
                            this.entregables.fechaPO = ced.entregable.fdProgramaOperacion;
                        }
                        else {
                            this.entregables.boolPO = "false";
                        }

                        if (ced.entregable.fdUniforme != null) {
                            this.entregables.boolUniforme = "true";
                            this.entregables.fechaUniforme = ced.entregable.fdUniforme;
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

                        if (ced.entregable.fdPersonal != null) {
                            this.entregables.boolPersonal = "true";
                            this.entregables.fechaPersonal = ced.entregable.fdPersonal;
                        }
                        else {
                            this.entregables.boolPersonal = "false";
                        }

                    }, error => {
                        console.log("hay error");
                        console.log(error);
                    }
                );
            }
        },

        watch: {
            qwert: function () {
                this.objetoHoras.fechaProgramada = this.qwert;
                this.objetoHoras.horaProgramada = this.qwert;
                this.objetoHoras.horaEfectiva = this.qwert;
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
                if (!this.$v.objetoFechas.comentarios.$dirty) return errors
                !this.$v.objetoFechas.comentarios.required && errors.push('El campo es requerido')
                return errors
            },

            validComentFumi() {
                const errors = [];
                if (!this.$v.objetoFumigacion.comentarios.$dirty) return errors
                !this.$v.objetoFumigacion.comentarios.required && errors.push('El campo es requerido')
                return errors
            },

            validComentHoras() {
                const errors = [];
                if (!this.$v.objetoHoras.comentarios.$dirty) return errors
                !this.$v.objetoHoras.comentarios.required && errors.push('El campo es requerido')
                return errors
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
            calcularPena() {
                console.log('calcular pena');
                FumigacionService.calcularPena(this.datosCed, this.entregables, this.primerMes).then(
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
                FumigacionService.enviar(datos).then(
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
                FumigacionService.insertArchivo(files).then(
                    c => {
                        this.mensaje = 'Enviado con éxito';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                        window.open(
                            '/api/Documents/ReporteFumigacionPorValidar/' + folio,
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

                console.log("1");
                if (this.$v.datosCed.$invalid) {
                    this.mensaje = "Falta contestar alguna pregunta o campo";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                }
                else if (!this.$v.datosCed.$invalid) {
                    if (this.datosCed.boolfechas == "false" && this.datosCed.arregloFechas.length < 1) {
                        this.mensaje = "Contastaste NO en la pregunta 1 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.$v.datosCed.monto.$invalid || this.datosCed.monto == "" || this.datosCed.monto == 0 || this.datosCed.monto < 0) {
                        console.log("monto: " + this.datosCed.monto);
                        this.mensaje = "Parece que en el monto de la factura es 0 o el campo esta vacío";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolHoras == "false" && this.datosCed.arregloHoras.length < 1) {
                        this.mensaje = "Contastaste NO en la pregunta 2 pero no capturaste ningun registro";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else if (this.datosCed.boolFumigacion == "false" && this.datosCed.arregloFumigacion.length < 1) {
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

                    else if (this.$v.entregables.$invalid) {
                        this.mensaje = "Parece que hay algo mal en los entregables (a partir de la pregunta 5)";
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }

                    else {
                        this.dialogFactura = true;
                    }
                }
            },

            agregarFechas(objeto) {
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

            eliminarFechas(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloFechas;
                var result = confirm("¿Eliminar registro?");
                if (result == true) {
                    console.log("se elimina ", arreglo[item]);
                    arreglo.splice(item, 1);
                }
                else {
                    alert("Operación cancelada");
                }
            },

            editarFechasDiag(index) {
                console.log(this.datosCed.arregloFechas[index]);
                this.objetoFechas = { ...this.datosCed.arregloFechas[index] }
                console.log(this.objetoFechas);
                this.index = index;
                this.dialogRegisFechas = false;
                this.dialogEditarFechas = true;
            },

            editarFechas(objeto) {
                this.datosCed.arregloFechas.splice(this.index, 1);
                this.agregarFechas(objeto);
            },



            agregarHoras(objeto) {
                console.log(objeto);
                if (this.$v.objetoHoras.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }

                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaProgramadaFormat = copia.fechaProgramada.toString();
                    copia.fechaProgramadaFormat = copia.fechaProgramadaFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloHoras.push(copia);

                    this.qwert = null;
                    this.objetoHoras.comentarios = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            eliminarHoras(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloHoras;
                var result = confirm("¿Eliminar registro?");
                if (result == true) {
                    console.log("se elimina ", arreglo[item]);
                    arreglo.splice(item, 1);
                }
                else {
                    alert("Operación cancelada");
                }
            },

            editarHorasDiag(index) {
                console.log(this.datosCed.arregloHoras[index]);
                this.objetoHoras = { ...this.datosCed.arregloHoras[index] }
                this.index = index;
                this.dialogRegisHoras = false;
                this.dialogEditarHoras = true;
            },

            editarHoras(objeto) {
                this.datosCed.arregloHoras.splice(this.index, 1);
                this.agregarHoras(objeto);
            },
            


            agregarFumigacion(objeto) {
                if (this.$v.objetoFumigacion.$invalid) {
                    this.mensaje = "Falta llenar o corregir algún dato";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else if (objeto.fechaFumigacion >= objeto.fechaReaparicion) {
                    this.mensaje = "La fecha de reaparicion debe ser mayor a la de fumigación";
                    this.colorSnackbar = 'error';
                    this.snackbar = true;
                    return false;
                }
                else {
                    var copia = { ...objeto };//se usa esta madre para copiar {...madre}

                    copia.fechaFumigacionFormat = copia.fechaFumigacion.toString();
                    copia.fechaFumigacionFormat = copia.fechaFumigacionFormat.substring(0, 15);

                    copia.fechaReaparicionFormat = copia.fechaReaparicion.toString();
                    copia.fechaReaparicionFormat = copia.fechaReaparicionFormat.substring(0, 15);

                    console.log(copia);

                    this.datosCed.arregloFumigacion.push(copia);
                    this.objetoFumigacion.fechaFumigacion = null;
                    this.objetoFumigacion.fechaReaparicion = null;
                    this.objetoFumigacion.comentarios = null;
                    this.mensaje = "Agregado exitosamente, checar registros";
                    this.colorSnackbar = 'success';
                    this.snackbar = true;
                    return true;
                }
            },

            eliminarFumigacion(item) {
                console.log(item);
                var arreglo = this.datosCed.arregloFumigacion;
                var result = confirm("¿Eliminar registro?");
                if (result == true) {
                    console.log("se elimina ", arreglo[item]);
                    arreglo.splice(item, 1);
                }
                else {
                    alert("Operación cancelada");
                }
            },

            editarFumiDiag(index) {
                console.log(this.datosCed.arregloFumigacion[index]);
                this.objetoFumigacion = { ...this.datosCed.arregloFumigacion[index] }
                console.log(this.objetoFumigacion);
                this.index = index;
                this.dialogRegFumigacion = false;
                this.dialogEditarFumigacion = true;
            },

            editarFumigacion(objeto) {
                this.datosCed.arregloFumigacion.splice(this.index, 1);
                this.agregarFumigacion(objeto);
            },



            formatHoras(fecha) {
                var horas = fecha.toString();
                horas = horas.substring(16, 21);
                horas = horas + (' hrs.');
                return horas;
            },

            primerDia() {
                var mes = this.getMes();
                return new Date(2021, mes, 1);
            },

            ultimoDia() {
                var mes = this.getMes();
                return new Date(2021, mes + 1, 0);
            },

            mas5Dias() {
                var dia = new Date(this.objetoFumigacion.fechaFumigacion)
                console.log("mas 5 dias: " + dia);
                return dia.setDate(dia.getDate() + 4);
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
                FumigacionService.guardar(datos).then(
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

                FumigacionService.verificarPeriodo(verificacion).then(
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