'use strict';

app.controller('loginController',
    ['$scope', 'authService', '$stateParams',
        function ($scope, authService, $stateParams) {

            $scope.formlogin = function () {
                var loginData = {
                    user: $scope.user,
                    returnUrl: $stateParams.returnUrl,
                    id: $stateParams.id
                }
                authService.login(loginData);
            }
            $scope.logout = function () {
                authService.logout($stateParams.returnUrl);
            }
        }]);