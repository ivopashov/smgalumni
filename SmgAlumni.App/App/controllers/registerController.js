'use strict';

app.controller('registerController',
    ['$scope', 'accountService', 'commonService',
        function ($scope, accountService, commonService) {

            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            //$scope.divisions = ['A','B'];
            $scope.user = { division: $scope.divisions[0]};

            $scope.register = function () {
                accountService.register($scope.user).then(
                function (success) {
                    commonService.$location.path('account/login');
                    commonService.notification.success("Successfully registered");
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });
            }
        }]);