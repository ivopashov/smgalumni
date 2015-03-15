'use strict';

app.controller('forgotPasswordController',
    ['$scope', 'accountService','commonService',
        function ($scope, accountService, commonService) {

            $scope.send = function () {

                if (!$scope.forgotPasswordForm.$valid)
                    return;

                accountService.forgotPassword($scope.email).then(
                    function (success) {
                        commonService.$state.go('home');
                        commonService.notification.success(success.data);
                    },
                    function (error) {
                        commonService.notification.error(error.data.message);
                    });
            }
        }]);