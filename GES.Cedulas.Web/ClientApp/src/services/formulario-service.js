import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class FormularioServices {
    enviar(e) {
        console.log('service formulario');
        console.log(e);
        return axios.post(Utils.urlAPI() + 'formulario/enviar',
            {
                'nombre': e.persona.nombre,
                'apellido': e.persona.apellido,
                'direccion': e.persona.direccion,
                'telefono': e.persona.telefono,
                'correo': e.persona.correo,
                'documento': e.persona.documento,
                'fechaNacimiento': e.persona.fechaNacimiento,
            },
            {
                params: {
                    jsonIntitucion: e.institucion,
                    jsonCapacitacion: e.capacitacion,
                    jsonPago: e.pago
                },
                headers: authHeader()
            })
            .then(this.handleResponse).then(response => {
                console.log('api guardar formulario');
                console.log(response);
                return response.data;
            });
    }

    modificar(e) {
        console.log('service formulario');
        console.log(e);
        return axios.post(Utils.urlAPI() + 'formulario/modificar',
            {
                'nombre': e.persona.nombre,
                'apellido': e.persona.apellido,
                'direccion': e.persona.direccion,
                'telefono': e.persona.telefono,
                'correo': e.persona.correo,
                'documento': e.persona.documento,
                'fechaNacimiento': e.persona.fechaNacimiento,
            },
            {
                params: {
                    jsonIntitucion: e.institucion,
                    jsonCapacitacion: e.capacitacion,
                    jsonPago: e.pago,
                    id: e.id
                },
                headers: authHeader()
            })
            .then(this.handleResponse).then(response => {
                console.log('api guardar formulario');
                console.log(response);
                return response.data;
            });
    }

    getPersonas() {
        return axios.get(Utils.urlAPI() + 'formulario/getTodas', {
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    getPersona(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'formulario/getPersona', {
            params: {
                folio: f
            },
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
    }

    eliminar(f) {
        console.log("Service folio:" + f);
        return axios.get(Utils.urlAPI() + 'formulario/eliminar', {
            params: {
                folio: f
            },
            headers: authHeader()
        }).then(this.handleResponse)
            .then(response => {
                return response.data;
            });
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


export default new FormularioServices();