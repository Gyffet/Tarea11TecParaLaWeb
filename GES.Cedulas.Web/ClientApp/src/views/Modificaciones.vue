<template>
    <v-container>
        <v-row class="text-center">


            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Modificaciones de Registros
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
                                            Modificar
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
                                                    @click="seleccionarPersona(item.pkFormulario)">
                                                mdi-file-eye
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
    </v-container>
</template>

<script>
    import FormularioService from '../services/formulario-service';
    export default {
        name: "Modificaciones",
        data: () => ({
            personas: null,
        }),

        methods: {
            seleccionarPersona(item) {
                console.log(item);
                localStorage.setItem("folio", item);
                this.$router.push('/altas');
            }
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