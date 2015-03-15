app.factory('authInterceptor', ['$window', 'authHelper', '$q', function ($window, authHelper, $q) {

    'use strict';

    var addAuthToken = function (config) {

        config.headers = config.headers || {};
        var authData = authHelper.getAuth();
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }
    };

    var request = function (config) {
        addAuthToken(config);

        return config;
    };

    var responseError = function (rejection) {
        if (rejection.status === 403 || rejection.status === 401) {
            $window.location = $window.location.origin;
        }
        return $q.reject(rejection);
    };

    return {
        request: request,
        responseError: responseError
    };
}]);