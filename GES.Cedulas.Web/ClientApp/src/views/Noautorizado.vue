<template>
    <v-container>
        <v-row class="text-center">

            <v-col class="mb-4">
                <br /><br /><br /><br /><br /><br /><br /><br /><br />
                <h1 class="display-2 font-weight-bold mb-3">
                    Usted no tiene los permisos para entrar a esta página
                </h1>
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