<template>
    <v-container class="fill-height" fluid>
        <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="4">
                <br /><br /><br /><br /><br /><br /><br />
                <v-card class="elevation-12">
                    <!--<v-toolbar color="indigo" dark flat>
            <v-toolbar-title>Login form</v-toolbar-title>
            <v-spacer />
        </v-toolbar>-->

                    <v-card-text>
                        <v-img :src="require('../assets/logotipoCJF_vertical_color.png')"
                               class=""
                               contain
                               height="50"></v-img>
                        <v-form>
                            <v-text-field label="Número de expediente (usuario de nómina)"
                                          name="login"
                                          prepend-icon="mdi-account-circle"
                                          v-model="usuario.user"
                                          required
                                          type="text" />

                            <v-text-field id="password"
                                          label="Contraseña"
                                          name="password"
                                          v-model="usuario.password"
                                          prepend-icon="mdi-lock"
                                          required
                                          type="password" />
                        </v-form>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer />
                        <v-btn color="primary"
                               :loading="loading"
                               :disabled="loading"
                               @click="login()">
                            <v-icon left>mdi-login</v-icon>Login
                        </v-btn>
                    </v-card-actions>
                </v-card>

            </v-col>
        </v-row>

        <v-row align="center" justify="center" v-if="message!=''">
            <v-col>
                <v-alert type="error">
                    {{message}}
                </v-alert>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        name: "Login",
        data() {
            return {
                usuario: { user: '', password: '' },
                message: '',
                loading: false
            }
        },
        computed: {
            loggedIn() {
                return this.$store.state.auth.user;
            }
        },

        mounted() {
            let self = this;

            window.addEventListener('keypress', function (e) {
                if (e.key === 'Enter') {
                    self.login(); //por alguna razon desconocida no funciona con this.login();
                }
            });
        },

        methods: {
            login() {
                //console.log('login vue method');
                this.loading = true;
                this.message = '';
                this.$store.dispatch('auth/login', this.usuario).then(
                    () => {
                        //console.log('ok');
                        this.loading = false;
                        this.$router.push('/inicio');
                    },
                    error => {
                        //console.log('login vue method error' + error);
                        //console.log(error);
                        this.loading = false;
                        this.message = error;
                    }
                );
            }
        },
    }
</script>

<style scoped>
</style>