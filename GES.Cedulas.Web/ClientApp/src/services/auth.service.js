import axios from 'axios';
import Utils from '../services/utils';



class AuthService {
    login(user) {
        console.log('llegaste a service y el usuario es ', user);
        return axios
            .post(Utils.urlAPI() + 'Autorizacion/Auth', {
                user: user.user,
                password: user.password
            })
            .then(this.handleResponse)
            .then(response => {
                //console.log('AuthService login');
                console.log('el servidor regreso');
                console.log(response);
                if (response.data.accessToken) {
                    localStorage.setItem('user', JSON.stringify(response.data));
                }
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

export default new AuthService();