'use strict';

app.controller('verifyUsersController',
    ['$scope', 'commonService','adminService','$state',
        function ($scope, commonService, adminService,$state) {

            $scope.verifiedIds = [];

            adminService.getUnverifiedUsers().then(function (success) {
                $scope.users = success.data;
            }, function (error) {
                commonService.notification.error(error.data.message);
            })

            $scope.confirmUser = function (user) {
                if (!user.verified) {
                    user.verified = true;
                }

                $scope.verifiedIds.push(user.id);
            }

            $scope.unConfirmUser = function (user) {
                if (user.verified) {
                    user.verified = false;
                }

                $scope.verifiedIds.splice($scope.verifiedIds.indexOf(user.id),1);
            }

            $scope.sendVerifiedUserIds = function () {
                var vm = { IdsToVerify: $scope.verifiedIds };
                adminService.postVerifiedUsers(vm).then(function (success) {
                    commonService.notification.success("Потребителите бяха одобрени успешно");
                    commonService.$state.go('menu');
                }, function (error) {
                    commonService.notification.error(error.data.message);
                });
            }

        }]);