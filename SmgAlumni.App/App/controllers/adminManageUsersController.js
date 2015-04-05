'use strict';

app.controller('adminManageUsersController',
    ['$scope', 'searchService', 'commonService', 'ngDialog', 'accountService',
        function ($scope, searchService, commonService, ngDialog, accountService) {


            $scope.students = [];
            $scope.search = {};

            $scope.getUsers = function () {
                if ($scope.studentSearchForm.$invalid) {
                    commonService.notification.error("Формата е невалидна");
                    return;
                };

                searchService.getUserByUserNameContains($scope.search.userName).then(function (success) {
                    $scope.students = success.data;
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            }

            $scope.deleteStudent = function (student) {
                ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                    scope: $scope
                }).then(function (success) {
                    accountService.deleteUser(student.userName).then(function () {
                        commonService.notification.success("Потребителят беше изтрит успешно");
                        $scope.students.splice($scope.students.indexOf(student), 1);
                    }, function (error) {
                        commonService.notification.error(error.data.message);
                    })
                })
            }

            $scope.resetStudentPass = function (student) {
                ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/resetUserPass.html',
                    scope: $scope
                }).then(function (success) {

                    if (success.password != success.newpassword) {
                        commonService.notification.error("Паролите не съвпадат");
                        return;
                    }

                    var model = {
                        id: student.id,
                        password: success.password,
                        newpassword: success.newpassword,
                    }
                    accountService.resetUserPass(model).then(function () {
                        commonService.notification.success("Потребителската парола беше сменена успешно");
                    }, function (error) {
                        commonService.notification.error(error.data.message);
                    })
                })
            }

        }]);