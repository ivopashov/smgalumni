'use strict';

app.controller('manageRolesController',
    ['$scope', 'commonService', '$state', 'masterAdminService','searchService',
        function ($scope, commonService, $state, masterAdminService,searchService) {

            masterAdminService.getAllRoles().then(function (success) {
                $scope.roles = success.data;
            }, function (err) {
                commonService.notification.error(err.data.message);
            })

            $scope.findUser = function () {
                searchService.getUserByUserName($scope.searchedUserName).then(function (success) {
                    $scope.user = success.data;
                })
            }

            $scope.updateUserRoles = function () {
                masterAdminService.updateUserRoles($scope.user).then(function (success) {
                    commonService.notification.success("Ролите бяха обновени успешно");
                    $scope.user = null;
                }, function (error) {
                    commonService.notification.error(error.data.message);
                })
            }

            $scope.deleteRole = function (role) {
                $scope.user.roles.splice($scope.user.roles.indexOf(role),1);
            }

            $scope.addRole = function (role) {
                if ($scope.user.roles.indexOf(role) == -1) {
                    $scope.user.roles.push(role);
                }
            }
        }]);