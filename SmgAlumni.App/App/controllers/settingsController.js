'use strict';

app.controller('settingsController',
    ['$scope', 'commonService', 'settingsService',
        function ($scope, commonService, settingsService) {

            $scope.edittedSetting = null;
            $scope.addedSetting = null;
            $scope.settings = [];

            settingsService.getAllSettings().then(function (success) {
                $scope.settings = success.data;
            }, function (err) {
                commonService.notification.error(err.data.message);
            })

            $scope.saveSetting = function () {
                settingsService.updateSetting($scope.edittedSetting).then(function (success) {
                    commonService.notification.success("Настройките бяха запазени успешно");
                    $scope.edittedSetting = null;
                    settingsService.getAllSettings().then(function (success) {
                        $scope.settings = success.data;
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }, function (err) {
                    commonService.notification.error(err.data.message);
                })
            }

            $scope.selectSetting = function (setting) {
                $scope.edittedSetting = setting;
            }

            $scope.addSetting = function () {
                settingsService.addSetting($scope.addedSetting).then(function (success) {
                    $scope.addedSetting = null;
                    commonService.notification.success("Настройката беше добавена успешно");
                    settingsService.getAllSettings().then(function (success) {
                        $scope.settings = success.data;
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }, function (err) {
                    commonService.notification.error(err.data.message);
                })
            }

            $scope.deleteSetting = function (setting) {
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                }).then(function() {
                    settingsService.deleteSetting(setting).then(function(success) {
                        commonService.notification.success("Настройката беше изтрита успешно");
                        $scope.settings.splice($scope.settings.indexOf(setting), 1);
                    }, function(err) {
                        commonService.notification.error(err.data.message);
                    });
                });
            }

        }]);