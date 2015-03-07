app.factory('authInterceptor', ['$window', 'authHelper', '$q', function ($window, authHelper, $q) {

    'use strict';

    var checkNeedAuthToken = function (config) {

        if (config.url.toLowerCase().indexOf("admin") > -1) {
            return true;
        }

        return false;

    };

    var addAuthToken = function (config) {
        if (!checkNeedAuthToken(config)) return;

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
        if (rejection.status === 403) {
            authHelper.clearAuth();
            $window.location.reload();
        }
        return $q.reject(rejection);
    };

    return {
        request: request,
        responseError: responseError
    };
}]);