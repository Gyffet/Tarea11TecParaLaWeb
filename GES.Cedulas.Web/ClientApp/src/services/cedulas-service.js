import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class CedulasServices {
    getTodas() {
        return axios.get(Utils.urlAPI() + 'cedulas/getTodas', {
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    getPendientes() {
        return axios.get(Utils.urlAPI() + 'cedulas/getPendientes', {
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    getPendRech(a) {
        return axios.get(Utils.urlAPI() + 'cedulas/getPendRech', {
            params: {
                area: a
            },
            headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                console.log('api get cedulas pendientes');
                console.log(response);
                return response.data;
            });
    }

    getAceptadasAdmin(a) {
        return axios.get(Utils.urlAPI() + 'cedulas/getAceptadasAdmin', {
            params: {
                area:a
            },
            headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                console.log('api get cedulas aceptadas');
                console.log(response);
                return response.data;
            });
    }

    getAceptadas() {
        return axios.get(Utils.urlAPI() + 'cedulas/getAceptadas', {
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    getRechazadas() {
        return axios.get(Utils.urlAPI() + 'cedulas/getRechazadas', {
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    

    

    

    getCedulasPendientes(a) {
        console.log(a);
        console.log('servicio');
        return axios.get(Utils.urlAPI() + 'cedulas/getCedulasPend', {
            params: {
                area: a
            },
            headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                console.log('api get cedulas pendientes');
                console.log(response);
                return response.data;
            });
    }

    getCedula(servicio, folio) {
        return axios.get(Utils.urlAPI() + 'cedulas/getCedula', {
            params: {
                servicio: servicio,
                folio: folio
            },
            headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                console.log('api get Cedula');
                console.log(response);
                return response.data;
            });
    }

    getInmuebles(a) {
        console.log(a);
        console.log('servicio');
        return axios.get(Utils.urlAPI() + 'cedulas/getInmuebles', {
            params: {
                area: a
            },
            headers: authHeader()
        })
            .then(this.handleResponse)
            .then(response => {
                console.log('api get inmuebles');
                console.log(response);
                return response.data;
            });
    }

    

    

    

    //calcularPena(cp) {
    //    console.log('service');
    //    console.log(cp);
    //    return axios
    //        .get(Utils.urlAPI() + 'cedulas/calcularPena',
    //            //{
    //            //    'factura': cp.factura,
    //            //    'monto': cp.monto,
    //            //    'boolRecoleccion': cp.boolRecoleccion,
    //            //    'arregloRecoleccion': cp.arregloRecoleccion,
    //            //    'boolEntrega': cp.boolEntrega,
    //            //    'arregloEntrega': cp.arregloEntrega,
    //            //    'boolAcuses': cp.boolAcuses,
    //            //    'arregloAcuses': cp.arregloAcuses,
    //            //    'boolMalEstado': cp.boolMalEstado,
    //            //    'arregloMalEstado': cp.arregloMalEstado,
    //            //    'boolExtraviadas': cp.boolExtraviadas,
    //            //    'arregloPerdido': cp.arregloPerdido,
    //            //    'boolMaterial': cp.boolMaterial,
    //            //    'diasMaterial': cp.diasMaterial,
    //            //    'boolUniforme': cp.boolUniforme,
                    
    //            //},
    //            {
    //                params: {
    //                    cedula: cp
    //                },
                
    //                headers: authHeader()
    //            }
    //        )
    //        .then(this.handleResponse)
    //        .then(response => {
    //            console.log('api calcular monto');
    //            console.log(response);
    //            return response.data;
    //        });
    //}


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


export default new CedulasServices();