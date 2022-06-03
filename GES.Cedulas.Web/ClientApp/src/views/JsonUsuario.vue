<template>
    <v-container>
        <v-row class="text-center">

            <v-col class="mb-4">
                <p>
                    Si usted esta en esta página es porque el administrador del sistema se lo solicitó, de lo contrario puede salir de aquí ignorando este mensaje
                    <br />
                    Para enviar por correo:
                    <br />
                    {{currentUser}}
                </p>
                <router-link to="/inicio">Regresar a Inicio</router-link>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>


    export default {
        name: "Inicio",

        data: () => ({
        }),
        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
            currentUser() {
                if (this.loggedIn)
                    return this.$store.state.auth.user;
                return false;
            },
        }
    };
</script>