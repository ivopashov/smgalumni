'use strict';

app.controller('registerController',
    ['$scope', 'accountService', 'commonService',
        function ($scope, accountService, commonService) {

            $scope.user = {};
            $scope.validation = {};
            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            $scope.user.division = { division: $scope.divisions[0]};

            $scope.register = function () {

                $scope.validation.trySentInvalidForm = true;

                if ($scope.user.confirmpassword != $scope.user.password) {
                    $scope.registerForm.confirmpassword.$error.match = true;
                    return;
                } else {
                    $scope.registerForm.confirmpassword.$error.match = false;
                }
                
                if ($scope.registerForm.$invalid) {
                    return;
                }

                $scope.validation.trySentInvalidForm = false;
                accountService.register($scope.user).then(
                function (success) {
                    commonService.$state.go('menu.successfullregistration');
                    commonService.notification.success("Successfully registered");
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });
            }
        }]);