'use strict';

app.controller('registerController',
    ['$scope', 'accountService', 'commonService',
        function ($scope, accountService, commonService) {

            $scope.register = function () {
                accountService.register($scope.user).then(
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
            $scope.validate = function () {
                return true;
                //if (($scope.user != undefined) && ($scope.user.password != undefined) && ($scope.user.email != undefined)
                //    && ($scope.user.displayname != undefined) && ($scope.user.password == $scope.user.confirmpassword) && 
                //    ($scope.user.email == $scope.user.confirmemail)) {
                //    return true;
                //}
                //return false;
            }
        }]);