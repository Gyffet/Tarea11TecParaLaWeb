﻿<template>
    <v-container>
        <v-row class="text-center">


            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Registros
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
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="item in personas"
                                        :key="item.pkFormulario">
                                        <td>{{ item.nombre }}</td>
                                        <td>{{ item.correo }}</td>
                                        <td>{{ item.telefono }}</td>
                                        
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
        name: "Listados",
        data: () => ({
            personas: null,
        }),


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