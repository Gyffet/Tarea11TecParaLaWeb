import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class LimpiezaServices {
    insertArchivo(files) {
        return axios.post(Utils.urlAPI() + 'limpieza/subirArchivoPDF', files, { headers: authHeader() })
            .then(this.handleResponse)
            .then(response => {
                console.log('api subir archivo');
                console.log(response);
                return response.data;
            });
    }

    recalcular(r,f) {
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

    eliminar(f) {
        return axios.get
            (Utils.urlAPI() + 'limpieza/eliminar',
            {
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

    rechazarLimpieza(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'limpieza/rechazarCedula', {
            params: {
                folio: f
            },
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    aceptarLimpieza(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'limpieza/aceptarCedula', {
            params: {
                folio: f
            },


            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    calcularPena(cp,e,pm) {
        console.log('service calcular pena');
        console.log(pm);
        return axios
            .post(Utils.urlAPI() + 'limpieza/calcularPena',
                {
                    'factura': cp.factura,
                    'monto': cp.monto,
                    'boolActividades': cp.boolActividades,
                    'arregloActividades': cp.arregloActividades,
                    'boolUniforme': cp.boolUniforme,
                    'boolEquipo': cp.boolEquipo,
                    'arregloEquipo': cp.arregloEquipo,
                    'boolAsistencias': cp.boolAsistencias,
                    'inasistencias': cp.inasistencias,
                    'fechaPO': e.fechaPO,
                    'fechaVisita': e.fechaVisita,
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

    enviar(e) {
        console.log('service');
        console.log(e);
        return axios.post(Utils.urlAPI() + 'limpieza/enviarLimpieza',
            {
                'factura': e.cedula.factura,
                'monto': e.cedula.monto,
                'boolActividades': e.cedula.boolActividades,
                'arregloActividades': e.cedula.arregloActividades,
                'boolUniforme': e.cedula.boolUniforme,
                'boolEquipo': e.cedula.boolEquipo,
                'arregloEquipo': e.cedula.arregloEquipo,
                'boolAsistencias': e.cedula.boolAsistencias,
                'inasistencias': e.cedula.inasistencias,
                'boolIMSS': e.cedula.boolIMSS,
                'constanciaSUA': e.cedula.constanciaSUA,
                'fechaVisita': e.cedula.fechaVisita,
                'fechaPO': e.cedula.fechaPO
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

    guardar(g) {
        console.log('service guardar cedula limpieza');
        console.log(g);
        return axios.post(Utils.urlAPI() + 'limpieza/guardarLimpieza',
            {
                'factura': g.cedula.factura,
                'monto': g.cedula.monto,
                'boolActividades': g.cedula.boolActividades,
                'arregloActividades': g.cedula.arregloActividades,
                'boolUniforme': g.cedula.boolUniforme,
                'boolEquipo': g.cedula.boolEquipo,
                'arregloEquipo': g.cedula.arregloEquipo,
                'boolAsistencias': g.cedula.boolAsistencias,
                'inasistencias': g.cedula.inasistencias,
                'boolIMSS': g.cedula.boolIMSS,
                'constanciaSUA': g.cedula.constanciaSUA,
                'fechaVisita': g.cedula.fechaVisita,
                'fechaPO': g.cedula.fechaPO
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


    verificarPeriodo(m) {
        console.log(m);
        return axios
            .get(Utils.urlAPI() + 'limpieza/verificarPeriodo', {
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

export default new LimpiezaServices();