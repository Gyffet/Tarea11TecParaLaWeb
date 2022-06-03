class Utils {
    

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