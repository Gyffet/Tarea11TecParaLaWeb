class Utils {
    dgSigla(dg1) {
        var dg = '';

        switch (dg1) {
            case 90239://DIRECCIÓN GENERAL DE SERVICIOS GENERALES.
                dg = 'DGSG';
                break;
            case 90014://Secretaría Ejecutiva de Administración
                dg = 'SEA';
                break;
            case 90230://Coordinación de Administración Regional  
                dg = 'CAR';
                break;
            case 90017://Dirección General de Inmuebles y Mantenimiento 
                dg = 'DGIM';
                break;
            case 90240://Dirección General de Recursos Materiales
                dg = 'DGRM';
                break;
            case 90015://Dirección General de Recursos Humanos
                dg = 'DGRH';
                break;
            case 90228://Dirección General de Tecnologías de la Información
                dg = 'DGTI';
                break;
            case 90202://Dirección General de Servicios al Personal
                dg = 'DGSP';
                break;
            case 9471://Dirección de Innovación, Planeación y Desarrollo Institucional
                dg = 'DGIPDI';
                break;
            case 90328://Direccion General de Programación, Presupuesto y Tesorería
                dg = 'DGPPT';
                break;
            case 90231://Dirección General de Protección Civil y Salud en el Trabajo
                dg = 'DGPCST';
                break;
            default:
                dg = 'NA';
                break;
        }

        return dg;
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

    urlAPI() {
        //return 'http://localhost:49817/api/';
        return process.env.NODE_ENV === 'production' ? 'http://10.100.66.25:8088/api/' : 'http://localhost:50248/api/';
    }
}

export default new Utils();