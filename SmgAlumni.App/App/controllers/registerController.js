'use strict';

app.controller('registerController',
    ['$scope', 'accountService', 'commonService',
        function ($scope, accountService, commonService) {

            $scope.user = {};
            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            $scope.user.division = { division: $scope.divisions[0]};

            $scope.register = function () {
                accountService.register($scope.user).then(
                function (success) {
                    commonService.$state.go('successfullregistration');
                    commonService.notification.success("Successfully registered");
                },
                function (error) {
                    commonService.notification.error(error.data.message);
                });
            }
        }]);