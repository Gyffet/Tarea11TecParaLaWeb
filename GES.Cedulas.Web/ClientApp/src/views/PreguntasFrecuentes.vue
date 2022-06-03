<template>
    <v-container>
        <v-row class="text-center">

            
            <v-col class="mb-4">
                <br />
                <h1 class="display-2 font-weight-bold mb-3">
                    Preguntas Frecuentes y Solución de Problemas.
                </h1>
                <br /><br /><br />
                <p class="subheading font-weight-bold">
                    1.- Cuando intenta llenar una cédula, en la sección de "Inmueble a evaluar" aparece un mensaje "No data aviable" (sin datos disponibles) o bien no aparece ningún inmueble.
                    <br />
                </p>
                <p class="subheading font-weight-regular, text-justify">
                    Cierre el navegador e inicie sesión de nuevo con el botón situado en la parte derecha de la cinta negra superior (rectángulo vertical con flecha), su sesión caducará después de unos minutos de inactividad pero podría no verse reflejado en la página, si esto no funciona hay un conflicto en los datos obtenidos y tendrá que ponerse en contacto con el administrador del sistema.
                    <br />
                </p>
                <p class="subheading font-weight-bold">
                    2.- Cuando intenta llenar una cédula, en la sección de "Inmueble a evaluar" los inmuebles disponibles no son los que usted desea seleccionar.
                    <br />
                </p>
                <p class="subheading font-weight-regular, text-justify">
                    Al usted iniciar sesión el sistema recoge información básica de su expediente, NUNCA INFORMACIÓN SENSIBLE. Esta información incluye su inmueble de adscripción, y sólo podrá llenar formularios de la administración a la que usted está adscrito. Es posible cambiar la administración que se refleja en ÉSTE sistema a efectos de llevar a cabo una correcta evaluación; para ello tiene que enviar un correo al administrador del sistema con un documento soporte en donde se evidencíe que usted esta adscrito a un inmueble de la administración A pero labora en un inmueble de la administración B.
                    <br />
                </p>
                <p class="subheading font-weight-bold">
                    3.- Cuando verifica el periodo para llenar una cédula aparece un mensaje que dice que ya existe un registro en ese mes e inmueble.
                    <br />
                </p>
                <p class="subheading font-weight-regular, text-justify">
                    Sólo se admite una cédula por mes e inmueble con EL MISMO número de factura; si por ejemplo usted ya ha agregado una cédula de la Administración Ajusco 170 en el inmueble Tolima 29 con EL NÚMERO DE FACTURA "aa-bb-cc" no podrá agregar otra factura "aa-bb-cc" EN ESE MISMO INMUEBLE. Si este no es el caso y aparece el error puede tratarse de un problema de comunicación con el servidor, revise su conexión a la red y/o intente en unos minutos.
                    <br />
                </p>
                <p class="subheading font-weight-bold">
                    4.- ¿Ha guardado y ENVIADO una cédula a la DAS y no lo ve en la lista "Cédulas Guardadas"?
                    <br />
                </p>
                <p class="subheading font-weight-regular, text-justify">
                    Cuando se guarda y envía una cédula  a la DAS, el responsable del servicio requiere validar la información para posteriormente aceptar o rechazar su cédula. Mientras esto ocurre, usted no podrá ver la cédula. Si el responsable del servicio lo considera necesario, podría solicitar una corrección de dicha cédula, o bien, repetir la misma; de lo contrario verá su cédula en el apartado "Cédulas Aceptadas".
                    <br />
                </p>
                
                <p class="subheading font-weight-bold">
                    <br /><br />
                    Si su duda o problema no aparece aquí, por favor contacte al responsable del servicio o del sistema, según sea el caso. Para saber quienes son los administradores revise nuestro directorio.
                    <br />
                </p>
            </v-col>

            
        </v-row>
    </v-container>
</template>


<script>
    export default {
        name: "FAQ",
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