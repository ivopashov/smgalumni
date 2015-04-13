'use strict';

app.controller('forgotPasswordController',
    ['$scope', 'accountService','commonService',
        function ($scope, accountService, commonService) {

            $scope.send = function () {

                $scope.trySentInvalidForm = false;
                if ($scope.forgotPasswordForm.$invalid) {
                    $scope.trySentInvalidForm = true;
                    return;
                }

                accountService.forgotPassword($scope.email).then(
                    function (success) {
                        commonService.$state.go('homeauth');
                        commonService.notification.success(success.data);
                    },
                    function (error) {
                        commonService.notification.error(error.data.message);
                    });
            }
        }]);