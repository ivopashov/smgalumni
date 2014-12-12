'use strict';

//test

app.controller('resetPasswordController',
    ['$scope', 'accountService', '$stateParams', 'commonService',
        function ($scope, accountService, $stateParams, commonService) {
            accountService.checkGuid($stateParams).then(
                function (success) {
                },
                function (error) {
                    var errors = commonService.modelStateErrorsService.parseErrors(error);
                    for (var i = 0; i < errors.length; i++) {
                        commonService.notification.error(errors[i]);
                    }
                });

            $scope.changePassword = function () {
                $scope.resetPassword.token = $stateParams.guid;
                $scope.resetPassword.email = $stateParams.email;
                console.log($scope.resetPassword);
                accountService.changePassword($scope.resetPassword).then(
                function (success) {
                    commonService.$location.path('account/logon');
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