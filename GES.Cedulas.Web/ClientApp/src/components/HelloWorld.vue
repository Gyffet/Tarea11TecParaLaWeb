<template>
    <v-container>
        <v-row class="text-center">
            <v-col class="mb-4">
                <h1 class="display-2 font-weight-bold mb-3">
                    Bienvenido al Formulario de la Tarea 11 de Tecnologias para la Web
                </h1>
            </v-col>

            <v-col class="mb-5" cols="12">

                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/altas' }"
                       x-large
                       rounded>
                    Altas
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/modificaciones' }"
                       x-large
                       rounded>
                    Modificaciones
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/listados' }"
                       x-large
                       rounded>
                    Listados
                </v-btn>
                <br />
                <v-btn class="mb-4"
                       elevation="2"
                       color="blue darken-1"
                       raised
                       dark
                       link :to="{ path: '/bajas' }"
                       x-large
                       rounded>
                    Bajas
                </v-btn>

            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        name: "Inicio",
        data: () => ({
            
        }),
        
    };
</script>