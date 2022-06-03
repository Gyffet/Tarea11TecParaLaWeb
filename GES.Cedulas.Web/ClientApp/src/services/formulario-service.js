import axios from 'axios';
import authHeader from '../services/auth-header';
import Utils from '../services/utils';

class FormularioServices {
    enviar(e) {
        console.log('service formulario');
        console.log(e);
        return axios.post(Utils.urlAPI() + 'formulario/enviar',
            {
                params: {
                    jsonPersona: e.persona,
                    jsonIntitucion: e.institucion,
                    jsonCapacitacion: e.capacitacion,
                },
                headers: authHeader()
            })
            .then(this.handleResponse).then(response => {
                console.log('api guardar formulario');
                console.log(response);
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