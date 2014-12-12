'use strict';

app.controller('manageAccountController',
    ['authHelper', '$scope', 'accountService', 'managedata', 'commonService',
        function (authHelper, $scope, accountService, managedata, commonService) {
            $scope.account = {
                email: managedata.email,
                userName: managedata.userName
            };
            $scope.save = function () {
                accountService.saveAccountData($scope.account).then(
                    function (success) {
                        commonService.notification.success("Account data saved successfully!");
                        authHelper.getUserName(success.displayName);
                    },
                    function (error) {
                        var errors = commonService.modelStateErrorsService.parseErrors(error);
                        for (var i = 0; i < errors.length; i++) {
                            commonService.notification.error(errors[i]);
                        }
                    });
            }
        }]);