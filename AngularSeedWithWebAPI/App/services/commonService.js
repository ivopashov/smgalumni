app.service('commonService', [
    '$http', '$q', '$location', '$rootScope', 'notificationService', 'modelStateErrorsService', 'ngDialog', '$timeout', '$parse', '$compile', '$location',
    function ($http, $q, $location, $rootScope, notificationService, modelStateErrorsService, ngDialog, $timeout, $parse, $compile, $location) {

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
            modelStateErrorsService: modelStateErrorsService
        });
    }
]);