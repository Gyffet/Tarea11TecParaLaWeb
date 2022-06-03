<template>
    <v-container>
        <h1>Cédulas Rechazadas</h1>
        <v-row>
            <v-col>
                <h3>Estas son las cédulas que se rechazaron, si corrigieron los datos con otro folio, sientase libre de eliminar los registros:</h3>
                <br />

                <h4>Mensajería:</h4>

                <table class="table table-sm table-hover">
                    <thead>
                        <tr>
                            <th>Periodo</th>
                            <th> | </th>
                            <th>Inmueble</th>
                            <th> | </th>
                            <th>No. Factura</th>
                            <th> | </th>
                            <th>Folio</th>
                            <th> | </th>
                            <th>Monto</th>
                            <th> | </th>
                            <th>Ver Cédula</th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr v-for="item in cedulas.mensajeria" :key="item.pkPenalizacion">
                            <th>{{item.fcMes}}</th>
                            <th> | </th>
                            <th>{{item.fcInmueble}}</th>
                            <th> | </th>
                            <th>{{item.fcNumFactura}}</th>
                            <th> | </th>
                            <th>{{item.fcNoCed}}</th>
                            <th> | </th>
                            <th>{{formatearMonto(item.fiMontoFactura)}}</th>
                            <th> | </th>
                            <v-icon class="mr-2"
                                    color="red"
                                    @click="goToMensajeria(item.fcNoCed)">
                                mdi-file-eye
                            </v-icon>
                        </tr>
                    </tbody>
                </table>
                <br />
                <h4>Limpieza:</h4>

                <table class="table table-sm table-hover">
                    <thead>
                        <tr>
                            <th>Periodo</th>
                            <th> | </th>
                            <th>Inmueble</th>
                            <th> | </th>
                            <th>No. Factura</th>
                            <th> | </th>
                            <th>Folio</th>
                            <th> | </th>
                            <th>Monto</th>
                            <th> | </th>
                            <th>Ver Cédula</th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr v-for="item in cedulas.limpieza" :key="item.pkPenalizacion">
                            <th>{{item.fcMes}}</th>
                            <th> | </th>
                            <th>{{item.fcInmueble}}</th>
                            <th> | </th>
                            <th>{{item.fcNumFactura}}</th>
                            <th> | </th>
                            <th>{{item.fcNoCed}}</th>
                            <th> | </th>
                            <th>{{formatearMonto(item.fiMontoFactura)}}</th>
                            <th> | </th>
                            <v-icon class="mr-2"
                                    color="red"
                                    @click="goToLimpieza(item.fcNoCed)">
                                mdi-file-eye
                            </v-icon>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    // @ is an alias to /src
    import CedulasService from '../services/cedulas-service';

    export default {
        name: "Aceptadas",
        data: () => ({
            cedulas:[]
        }),

        mounted() {
            CedulasService.getRechazadas().then(
                m => {
                    this.cedulas = m;
                    console.log(this.cedulas);
                }, error => {
                    console.log("Error al cargar cédulas");
                    console.log(error);
                }
            );
        },

        methods: {
            goToMensajeria(id) {
                this.$router.push({ name: 'mensajeriaId', params: { id: id } });
            },

            goToLimpieza(id) {
                this.$router.push({ name: 'limpiezaId', params: { id: id } });
            },

            formatearMonto(monto) {
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                });
                return formatter.format(monto);
            },
        },

        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
        }
    };
</script>