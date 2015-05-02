'use strict';

//test

app.controller('resetPasswordController',
    ['$scope', 'accountService', '$stateParams', 'commonService',
        function ($scope, accountService, $stateParams, commonService) {
            accountService.checkGuid($stateParams.guid, $stateParams.email).then(
                function (success) {
                    $scope.requestVerified = success.data;
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });

            $scope.changePassword = function () {

                $scope.trySentInvalidForm = true;

                if (!$scope.requestVerified) return;

                if ($scope.resetPassword.password != $scope.resetPassword.confirmpassword) {
                    $scope.resetPasswordForm.confirmpassword.$error.match = true;
                    return;
                } else {
                    $scope.resetPasswordForm.confirmpassword.$error.match = false;
                }

                if ($scope.resetPasswordForm.$invalid) {
                    return;
                }

                $scope.resetPassword.token = $stateParams.guid;
                $scope.resetPassword.email = $stateParams.email;

                accountService.resetPassword($scope.resetPassword).then(
                function (success) {
                    commonService.notification.success("Паролата Ви беше променена успешно");
                    commonService.$state.go('menu');
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });
            }
        }]);