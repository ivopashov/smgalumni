app.factory('authService', ['commonService', '$rootScope', '$state',
    function (commonService, $rootScope, $state) {

        var login = function (loginData) {

            var deferred = commonService.$q.defer();

            commonService.$http.post('api/formsauthentication', loginData.user)
                .then(function (authenticationResponse) {
                    sessionStorage.authenticationData = JSON.stringify(authenticationResponse.data);
                    $rootScope.$broadcast('login');
                    commonService.notification.success('Успешно влизане');

                    if (loginData.returnUrl !== undefined) {
                        $state.go(loginData.returnUrl, { id: loginData.id });
                    } else {
                        $state.go('home');
                    }
                }, function (error) {
                    commonService.notification.error(error.data.message);
                });
        };

        var logout = function () {
            sessionStorage.authenticationData = '';
            $rootScope.$broadcast('logout');
        };
        return ({
            login: login,
            logout: logout
        });
    }])