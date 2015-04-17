'use strict';

app.controller('userSearchController',
    ['$scope', 'searchService', 'commonService', 'ngDialog',
        function ($scope, searchService, commonService, ngDialog) {

            
            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            $scope.students = [];

            $scope.getUsers = function () {

                $scope.trySentInvalidForm = true;

                if ($scope.studentSearchForm.$invalid) {
                    return;
                };

                var vm = {
                    division: $scope.division,
                    yearOfGraduation: $scope.yearOfGraduation
                }

                searchService.getUsersByYearAndDivision(vm).then(function (success) {
                    $scope.students = success.data;
                });
            }

            $scope.selectUser = function (id) {
                searchService.getUserById(id).then(function (success) {
                    $scope.selectedUser = success.data;
                    ngDialog.openConfirm({
                        templateUrl: '/App/templates/dialog/userDetails.html',
                        scope: $scope
                    })
                })
            }
        }]);