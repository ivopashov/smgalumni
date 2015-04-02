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

                if (!$scope.requestVerified) return;

                $scope.resetPassword.token = $stateParams.guid;
                $scope.resetPassword.email = $stateParams.email;

                accountService.resetPassword($scope.resetPassword).then(
                function (success) {
                    commonService.notification.success("Паролата Ви беше променена успешно");
                    commonService.$state.go('homeauth');
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });
            }
        }]);