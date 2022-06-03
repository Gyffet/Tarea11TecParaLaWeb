import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class FumigacionServices {
    insertArchivo(files) {
        return axios.post(Utils.urlAPI() + 'fumigacion/subirArchivoPDF', files, { headers: authHeader() })
            .then(this.handleResponse)
            .then(response => {
                console.log('api subir archivo');
                console.log(response);
                return response.data;
            });
    }

    guardar(g) {
        console.log('service guardar cedula fumigacion');
        console.log(g);
        return axios.post(Utils.urlAPI() + 'fumigacion/guardar',
            {
                'factura': g.cedula.factura,
                'monto': g.cedula.monto,
                'boolfechas': g.cedula.boolfechas,
                'arregloFechas': g.cedula.arregloFechas,
                'boolHoras': g.cedula.boolHoras,
                'arregloHoras': g.cedula.arregloHoras,
                'boolFumigacion': g.cedula.boolFumigacion,
                'arregloFumigacion': g.cedula.arregloFumigacion,
                'boolProductos': g.cedula.boolProductos,
            },
            {
                params: {
                    entregables: g.entregables,
                    mes: g.periodo.selectMes,
                    anio: g.periodo.selectPrimavera,
                    inmueble: g.periodo.selectInmueble,
                    area: g.area,
                    folio: g.folio,
                    usuario: g.usuario
                },
                headers: authHeader()
            })
            .then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    enviar(e) {
        console.log('service');
        console.log(e);
        return axios.post(Utils.urlAPI() + 'fumigacion/enviar',
            {
                'factura': e.cedula.factura,
                'monto': e.cedula.monto,
                'boolfechas': e.cedula.boolfechas,
                'arregloFechas': e.cedula.arregloFechas,
                'boolHoras': e.cedula.boolHoras,
                'arregloHoras': e.cedula.arregloHoras,
                'boolFumigacion': e.cedula.boolFumigacion,
                'arregloFumigacion': e.cedula.arregloFumigacion,
                'boolProductos': e.cedula.boolProductos,
            },
            {
                params: {
                    jsonPenas: e.penas,
                    entregables: e.entregables,
                    mes: e.periodo.selectMes,
                    anio: e.periodo.selectPrimavera,
                    inmueble: e.periodo.selectInmueble,
                    area: e.area,
                    folio: e.folio,
                    usuario: e.usuario,
                    correo: e.correo
                },
                headers: authHeader()
            })
            .then(this.handleResponse).then(response => {
                console.log('api guardar cedula');
                console.log(response);
                return response.data;
            });
    }

    calcularPena(cp,e,pm) {
        console.log('service calcular pena');
        console.log(cp);
        return axios
            .post(Utils.urlAPI() + 'fumigacion/calcularPena',
                {
                    'factura': cp.factura,
                    'monto': cp.monto,
                    'boolfechas': cp.boolfechas,
                    'arregloFechas': cp.arregloFechas,
                    'boolHoras': cp.boolHoras,
                    'arregloHoras': cp.arregloHoras,
                    'boolFumigacion': cp.boolFumigacion,
                    'arregloFumigacion': cp.arregloFumigacion,
                    'boolProductos': cp.boolProductos,
                    'fechaPO': e.fechaPO,
                    'primerMes': pm
                },
                {
                    headers: authHeader()
                }
            )
            .then(this.handleResponse)
            .then(response => {
                console.log('api calcular monto');
                console.log(response);
                return response.data;
            });
    }

    eliminar(f) {
        return axios.get(Utils.urlAPI() + 'fumigacion/eliminar', {
                params: {
                    folio: f
                },
                headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    verificarPeriodo(m) {
        console.log(m);
        return axios
            .get(Utils.urlAPI() + 'fumigacion/verificarPeriodo', {
                params: {
                    factura: m.factura,
                    inmueble: m.inmueble,
                    mes: m.periodo.selectMes,
                    primavera: m.periodo.selectPrimavera
                },
                headers: authHeader()
            })
            .then(this.handleResponse)
            .then(response => {
                console.log('api revisar periodo');
                console.log(response);
                return response.data;
            });
    }

    recalcular(r, f) {
        console.log("Service");
        return axios.get(Utils.urlAPI() + 'limpieza/recalcular', {
            params: {
                entregables: r.entregables,
                fechaLimite: r.fechaLimite,
                fechaEntrega: r.fechaEntrega,
                tipoEntregable: r.tipoEntregable,
                capacitacion: r.capacitacion,
                noCed: f
            },
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    rechazar(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'fumigacion/rechazar', {
            params: {
                folio: f
            },
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    aceptar(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'fumigacion/aceptar', {
            params: {
                folio: f
            },


            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }


    

    


    


    logout() {
        localStorage.removeItem('user');
    }


    handleResponse(response) {
        if (response.status === 401) {
            this.logout();
            location.reload(true);

            const error = response.data && response.data.message;
            return Promise.reject(error);
        }
        return Promise.resolve(response);
    }
}

export default new FumigacionServices();