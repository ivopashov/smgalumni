app.controller('headerController',
    ['$scope', 'authHelper', 'authService', '$http', 'accountService', '$state', '$rootScope', 'commonService',
    function ($scope, authHelper, authService, $http, accountService, $state, $rootScope, commonService) {

        (function () {
            $scope.username = accountService.getUser();
        }())
        
        $rootScope.$on('login', function () {
            $scope.username = accountService.getUser();
        });

        $scope.logout = function () {
            authService.logout();
            commonService.$state.go('homeauth');
        }
        $scope.isLoggedIn = function () {
            return accountService.isUserLoggedIn();
        }
    }
    ]);