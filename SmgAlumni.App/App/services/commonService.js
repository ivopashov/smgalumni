app.service('commonService', [
    '$http', '$q', '$location', '$rootScope', 'notificationService', 'ngDialog', '$timeout', '$parse', '$compile', '$state',
    function ($http, $q, $location, $rootScope, notificationService, ngDialog, $timeout, $parse, $compile,  $state) {

        return ({
            $q: $q,
            $http: $http,
            ngDialog: ngDialog,
            $rootScope: $rootScope,
            notification: notificationService,
            $timeout: $timeout,
            $parse: $parse,
            $compile: $compile,
            $location: $location,
            $state:$state
        });
    }
]);