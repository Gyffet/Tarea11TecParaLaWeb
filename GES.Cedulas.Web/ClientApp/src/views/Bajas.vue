<template>
    <v-container>
        <v-row class="text-center">


            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Bajas de Registros
                </h1>
            </v-col>

            <v-col class="mb-5" cols="12">
                <div>
                    <v-theme-provider light>
                        <template>
                            <v-simple-table>
                                <thead>
                                    <tr>
                                        <th class="text-center">
                                            Nombre
                                        </th>
                                        <th class="text-center">
                                            Correo
                                        </th>
                                        <th class="text-center">
                                            Teléfono
                                        </th>
                                        <th class="text-center">
                                            Elmininar
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="item in personas"
                                        :key="item.pkFormulario">
                                        <td>{{ item.nombre }}</td>
                                        <td>{{ item.correo }}</td>
                                        <td>{{ item.telefono }}</td>
                                        <td>
                                            <v-icon small
                                                    class="mr-2"
                                                    @click="eliminarPersona(item.pkFormulario)">
                                                mdi-delete-empty
                                            </v-icon>
                                        </td>
                                    </tr>
                                </tbody>
                            </v-simple-table>
                        </template>
                    </v-theme-provider>
                </div>
            </v-col>
        </v-row>
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
        name: "Bajas",
        data: () => ({
            personas: null,
            mensaje:null,
        }),

        methods: {
            

            eliminarPersona(id) {
                
                console.log(id);
                FormularioService.eliminar(id).then(
                    g => {
                        console.log("Enviado");
                        console.log(g);
                        this.mensaje = 'Eliminado';
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
        },

        mounted() {
            FormularioService.getPersonas().then(
                v => {
                    this.personas = v;
                    console.log(this.personas);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );
        }
    };
</script>