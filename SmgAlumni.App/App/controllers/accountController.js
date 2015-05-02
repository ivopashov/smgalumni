'use strict';
app.controller('accountController',
    ['$scope',
        function ($scope) {

            $scope.showEditAccount = true;
            $scope.showChangePass = false;

            $scope.editAccount = function () {
                $scope.showEditAccount = true;
                $scope.showChangePass = false;
            };
            $scope.changePass = function () {
                $scope.showEditAccount = false;
                $scope.showChangePass = true;
            };
        }]);