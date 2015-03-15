app.factory('authService', ['commonService', '$rootScope', '$state','accountService',
    function (commonService, $rootScope, $state, accountService) {

        var login = function (loginData) {

            var deferred = commonService.$q.defer();

            commonService.$http.post('api/formsauthentication', loginData.user)
                .then(function (authenticationResponse) {
                    sessionStorage.authenticationData = JSON.stringify(authenticationResponse.data);
                    $rootScope.$broadcast('login');
                    commonService.notification.success('Успешно влизане');
                    $state.go('homeauth');
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