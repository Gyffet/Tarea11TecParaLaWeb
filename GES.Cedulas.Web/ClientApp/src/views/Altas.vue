<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-1 font-weight-bold mb-3"
                    align="center">
                    Formulario de inscripción
                </h1>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="center">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Datos Personales
                    </v-col>

                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Nombre       -
                    <v-text-field v-model="datosPersonales.nombre"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Nombre"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Apellido       -
                    <v-text-field v-model="datosPersonales.apellido"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Apellido"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Dirección       -
                    <v-text-field v-model="datosPersonales.direccion"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Dirección"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Teléfono       -
                    <v-text-field v-model="datosPersonales.telefono"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Teléfono"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Correo Electrónico       -
                    <v-text-field v-model="datosPersonales.correo"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese correo"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Documento       -
                    <v-text-field v-model="datosPersonales.documento"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese texto"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Fecha de Nacimiento       -
                    <vc-date-picker v-model="datosPersonales.fechaNacimiento"
                                    required
                                    :mask="masks"
                                    :locale="{firstDayOfWeek: 1}">

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
        </v-row>






        <v-row>
            <v-col align="center">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Capacitación
                    </v-col>

                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Curso       -
                    <v-text-field v-model="Capacitacion.nombre"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Nombre"></v-text-field>
                </div>
            </v-col>
            <v-col align="left">
                <div class="title row">
                    Días
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Elige un horario       -
                </div>
            </v-col>
            <v-radio-group v-model="Capacitacion.horario"
                           row>
                <v-radio label="Lunes"
                         value="Lunes"></v-radio>
                <v-radio label="Martes"
                         value="Martes"></v-radio>
                <v-radio label="Miercoles"
                         value="Miercoles"></v-radio>
                <v-radio label="Jueves"
                         value="Jueves"></v-radio>.
                <v-radio label="Viernes"
                         value="Viernes"></v-radio>
            </v-radio-group>
        </v-row>






        <v-row>
            <v-col align="center">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Plan de Pago
                    </v-col>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    <v-radio-group v-model="planPago.pago"
                                   row>
                        <v-radio label="Contado"
                                 value="Contado"></v-radio>
                        <v-radio label="Crédito"
                                 value="Credito"></v-radio>
                        <v-radio label="Moneda"
                                 value="Moneda"></v-radio>
                        <v-radio label="Tarjeta"
                                 value="Tarjeta"></v-radio>
                    </v-radio-group>
                </div>
            </v-col>
        </v-row>






        <v-row>
            <v-col align="center">
                <div class="title row">
                    <v-col class="mb-4" align="left">
                        Datos de la Institución
                    </v-col>

                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    No. de contrato       -
                    <v-text-field v-model="Institucion.numero"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese No. Contrato"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Personas que atendió       -
                    <v-text-field v-model="Institucion.personas"
                                  required
                                  outlined
                                  dense
                                  label="Ingrese Apellido"></v-text-field>
                </div>
            </v-col>
        </v-row>

        <v-row>
            <v-col align="left">
                <div class="title row">
                    Fecha de inscripción       -
                    <vc-date-picker v-model="Institucion.fecha"
                                    required
                                    :mask="masks"
                                    :locale="{firstDayOfWeek: 1}">

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
        </v-row>

        <v-btn color="lime darken-3"
               dark
               @click="Alta()">
            Guardar
        </v-btn>

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

    </v-container>
</template>

<script>
    import FormularioService from '../services/formulario-service';
    export default {
        name: "Director",
        data: () => ({
            mensaje: "",
            colorSnackbar: "",
            snackbar: false,
            datosPersonales: {
                nombre: null,
                apellido: null,
                direccion: null,
                telefono: null,
                correo: null,
                documento: null,
                fechaNacimiento: null
            },
            Institucion: {
                numero: null,
                personas: null,
                fecha: null
            },
            planPago: {
                pago: null
            },
            Capacitacion: {
                nombre: null,
                horario: null,
            },

            masks: {
                input: 'YYYY-MM-DD',
            },
            loading: false,
        }),

        methods: {
            
            Alta() {
                console.log('Guardar');
                var datos = {
                    persona: this.datosPersonales,
                    institucion: JSON.stringify(this.Institucion),
                    capacitacion: JSON.stringify(this.Capacitacion),
                    pago: JSON.stringify(this.planPago),
                };
                console.log(datos);
                FormularioService.enviar(datos).then(
                    g => {
                        console.log("Enviado");
                        console.log(g);
                        this.mensaje = 'Guardado';
                        this.colorSnackbar = 'green';
                        this.snackbar = true;
                    }, error => {
                        console.log("Ha habido un error");
                        console.log(error);
                        this.mensaje = 'Ha habido un error';
                        this.colorSnackbar = 'error';
                        this.snackbar = true;
                    }
                );
            },
        }
    }
</script>