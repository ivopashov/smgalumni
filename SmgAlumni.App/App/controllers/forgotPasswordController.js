'use strict';

app.controller('forgotPasswordController',
    ['$scope', 'accountService','commonService',
        function ($scope, accountService, commonService) {

            $scope.send = function () {

                if (!$scope.forgotPasswordForm.$valid)
                    return;

                accountService.forgotPassword($scope.email).then(
                    function (success) {
                        commonService.$location.path('account/login');
                        commonService.notification.success(success);
                    },
                    function (error) {
                        var errors = commonService.modelStateErrorsService.parseErrors(error);
                        for (var i = 0; i < errors.length; i++) {
                            commonService.notification.error(errors[i]);
                        }
                    });
            }
        }]);