<template>
    <v-data-table :headers="headers"
                  :items="cedulas"
                  :search="search"
                  multi-sort
                  class="elevation-1">
        <template v-slot:top>
            <v-toolbar flat>
                <v-toolbar-title>Todas las Cédulas</v-toolbar-title>
                <v-divider class="mx-4"
                           inset
                           vertical></v-divider>
                <v-spacer></v-spacer>
                <v-dialog v-model="dialogDelete" max-width="500px">
                    <v-card>
                        <v-card-title class="headline">¿Está seguro de borrar este registro?</v-card-title>
                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="red darken-2" text @click="closeDelete">Cancelar</v-btn>
                            <v-btn color="green darken-2" text @click="deleteItemConfirm">Aceptar</v-btn>
                            <v-spacer></v-spacer>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <v-text-field v-model="search"
                              append-icon="mdi-magnify"
                              label="Buscar"
                              single-line
                              hide-details></v-text-field>
            </v-toolbar>
        </template>
        <template v-slot:item.actions="{ item }">
            <v-icon small
                    class="mr-2"
                    @click="seleccionarCedula(item)">
                mdi-file-eye
            </v-icon>
            -
            <v-icon small
                    @click="deleteItem(item)">
                mdi-delete
            </v-icon>
            -
            <v-icon small
                    @click="borrador(item)">
                mdi-file-pdf
            </v-icon>
            -
            <v-icon small
                    @click="imprimirReporte(item)">
                mdi-file-pdf
            </v-icon>
        </template>
    </v-data-table>
</template>


<script>
    import CedulasService from '../services/cedulas-service';
    import FumigacionService from '../services/fumigacion-service';
    import MensajeriaService from '../services/mensajeria-service';
    import LimpiezaService from '../services/limpieza-service';
    // @ is an alias to /src
    export default {
        name: "Director",
        data: () => ({
            cedulas: [],
            search: "",
            dialog: false,
            dialogDelete:false,
            
            headers: [
                {
                    text: 'Servicio',
                    sortable: true,
                    value: 'fcServicio',
                    align: 'center'
                },
                { text: 'Mes', value: 'fcMes', align: 'center' },
                { text: 'Inmueble', value: 'fcInmueble', align: 'center' },
                { text: 'Folio', value: 'fcNoCed', align: 'center' },
                { text: 'Factura', value: 'fcNumFactura', align: 'center' },
                { text: 'Fecha Creación', value: 'fdFechaGuardado', align: 'center' },
                { text: '¿Enviado?', value: 'fdFechaEnviado', align: 'center' },
                { text: 'Seleccionar/Borrar/Preliminar/Final', value: 'actions', sortable: false, align: 'center' },
            ],

            editedIndex: -1,
            editedItem: null
        }),
        mounted() {
            CedulasService.getTodas().then(
                v => {
                    console.log("Cedulas");
                    console.log(v);
                    v.fumigacion.forEach(element =>
                        this.cedulas.push(element));
                    v.limpieza.forEach(element =>
                        this.cedulas.push(element));
                    v.mensajeria.forEach(element =>
                        this.cedulas.push(element));
                    console.log(this.cedulas);
                }, error => {
                    console.log("hay error");
                    console.log(error);
                }
            );
        },

        watch: {
            dialog(val) {
                val || this.close()
            },
            dialogDelete(val) {
                val || this.closeDelete()
            },
        },
        
        methods: {
            seleccionarCedula(item) {
                localStorage.setItem("folio", item.fcNoCed);
                switch (item.fiIDServicio) {
                    case 1:
                        this.$router.push('/director/mensajeria');
                        break;
                    case 2:
                        this.$router.push('/director/limpieza');
                        break;
                    case 3:
                        this.$router.push('/director/fumigacion');
                        break;
                    default:
                    // code block
                }
            },

            imprimirReporte(item) {
                switch (item.fiIDServicio) {
                    case 1:
                        window.open(
                            '/api/Documents/GenerarReporteMensajeria/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 2:
                        window.open(
                            '/api/Documents/GenerarReporteLimpieza/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 3:
                        window.open(
                            '/api/Documents/GenerarReporteFumigacion/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    default:
                    // code block
                }
            },

            borrador(item) {
                switch (item.fiIDServicio) {
                    case 1:
                        break;
                    case 2:
                        window.open(
                            '/api/Documents/ReporteLimpiezaPorValidar/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    case 3:
                        window.open(
                            '/api/Documents/ReporteFumigacionPorValidar/' + item.fcNoCed,
                            '_blank' // <- This is what makes it open in a new window.
                        );
                        break;
                    default:
                    // code block
                }
            },

            deleteItem(item) {
                this.editedIndex = this.cedulas.indexOf(item)
                this.editedItem = Object.assign({}, item)
                console.log(this.editedIndex)
                console.log(this.editedItem)
                this.dialogDelete = true
            },

            deleteItemConfirm() {
                this.cedulas.splice(this.editedIndex, 1)

                switch (this.editedItem.fiIDServicio) {
                    case 1:
                        MensajeriaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 2:
                        LimpiezaService.eliminar(this.editedItem.fcNoCed);
                        break;
                    case 3:
                        FumigacionService.eliminar(this.editedItem.fcNoCed);
                        break;
                    default:
                    // code block
                }

                this.closeDelete()
            },

            closeDelete() {
                this.dialogDelete = false
                this.$nextTick(() => {
                    this.editedIndex = -1
                })
            },
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
        }

    };
</script>