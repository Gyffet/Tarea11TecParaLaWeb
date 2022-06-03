import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class MensajeriaServices {
    insertArchivo(files) {
        return axios.post(Utils.urlAPI() + 'mensajeria/subirArchivoPDF', files, { headers: authHeader() })
            .then(this.handleResponse)
            .then(response => {
                console.log('api subir archivo');
                console.log(response);
                return response.data;
            });
    }

    verificarPeriodo(m) {
        console.log(m);
        return axios
            .get(Utils.urlAPI() + 'mensajeria/verificarPeriodo', {
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

    eliminar(f) {
        return axios.get(Utils.urlAPI() + 'mensajeria/eliminar',
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

    guardar(g) {
        console.log('service guardar cedula');
        console.log(g);
        return axios.post(Utils.urlAPI() + 'mensajeria/guardarMensajeria',
            {
                'factura': g.cedula.factura,
                'monto': g.cedula.monto,
                'boolRecoleccion': g.cedula.boolRecoleccion,
                'arregloRecoleccion': g.cedula.arregloRecoleccion,
                'boolEntrega': g.cedula.boolEntrega,
                'arregloEntrega': g.cedula.arregloEntrega,
                'boolAcuses': g.cedula.boolAcuses,
                'arregloAcuses': g.cedula.arregloAcuses,
                'boolMalEstado': g.cedula.boolMalEstado,
                'arregloMalEstado': g.cedula.arregloMalEstado,
                'boolExtraviadas': g.cedula.boolExtraviadas,
                'arregloPerdido': g.cedula.arregloPerdido,
                'boolMaterial': g.cedula.boolMaterial,
                'diasMaterial': g.cedula.diasMaterial,
                'fechaPO': g.cedula.fechaPO,
            },
            {
                params: {
                    mes: g.periodo.selectMes,
                    anio: g.periodo.selectPrimavera,
                    inmueble: g.periodo.selectInmueble,
                    area: g.area,
                    folio: g.folio
                },
                headers: authHeader()
            })
            .then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    calcularPena(cp) {
        console.log('service calcular pena');
        console.log(cp);
        return axios
            .post(Utils.urlAPI() + 'mensajeria/calcularPena',
                {
                    'factura': cp.factura,
                    'monto': cp.monto,
                    'boolRecoleccion': cp.boolRecoleccion,
                    'arregloRecoleccion': cp.arregloRecoleccion,
                    'boolEntrega': cp.boolEntrega,
                    'arregloEntrega': cp.arregloEntrega,
                    'boolAcuses': cp.boolAcuses,
                    'arregloAcuses': cp.arregloAcuses,
                    'boolMalEstado': cp.boolMalEstado,
                    'arregloMalEstado': cp.arregloMalEstado,
                    'boolExtraviadas': cp.boolExtraviadas,
                    'arregloPerdido': cp.arregloPerdido,
                    'boolMaterial': cp.boolMaterial,
                    'diasMaterial': cp.diasMaterial,
                    'fechaPO': cp.fechaPO
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
        return axios.post(Utils.urlAPI() + 'mensajeria/enviarMensajeria',
            {
                'factura': e.cedula.factura,
                'monto': e.cedula.monto,
                'boolRecoleccion': e.cedula.boolRecoleccion,
                'arregloRecoleccion': e.cedula.arregloRecoleccion,
                'boolEntrega': e.cedula.boolEntrega,
                'arregloEntrega': e.cedula.arregloEntrega,
                'boolAcuses': e.cedula.boolAcuses,
                'arregloAcuses': e.cedula.arregloAcuses,
                'boolMalEstado': e.cedula.boolMalEstado,
                'arregloMalEstado': e.cedula.arregloMalEstado,
                'boolExtraviadas': e.cedula.boolExtraviadas,
                'arregloPerdido': e.cedula.arregloPerdido,
                'boolMaterial': e.cedula.boolMaterial,
                'diasMaterial': e.cedula.diasMaterial,
                'fechaPO': e.cedula.fechaPO,
            },
            {
                params: {
                    jsonPenas: e.penas,
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

    aceptarMensajeria(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'mensajeria/aceptarMensajeria', {
            params: {
                folio: f
            },


            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    rechazarMensajeria(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'mensajeria/rechazarMensajeria', {
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

export default new MensajeriaServices();