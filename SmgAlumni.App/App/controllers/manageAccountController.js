'use strict';

app.controller('manageAccountController',
    ['authHelper', '$scope', 'accountService', 'commonService', '$upload', '$state',
        function (authHelper, $scope, accountService, commonService, $upload, $state) {

            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            accountService.getUserAccount().then(function (success) {
                $scope.user = success.data;
            }, function (errorMsg) {
                commonService.notification.error(errorMsg.data.message);
            });

            $scope.update = function () {

                $scope.trySentInvalidForm = true;
                if ($scope.editProfile.$invalid) {
                    return;
                }

                accountService.updateUser($scope.user).then(function (success) {
                    commonService.notification.success("Потребителят е обновен успешно");
                    $state.go('menu');
                });
            };

            $scope.image = {};

            $scope.upload = function (file) {
                $scope.fileUploaded = false;
                $scope.image = file[0];

                if (!file || file.length == 0) return;
                if (file[0].type.indexOf('image') < 0) commonService.notification.error('Файлът не е снимка');
                $upload.upload({
                    url: 'api/file/upload',
                    fields: { 'username': $scope.username },
                    file: file
                }).success(function (data, status, headers, config) {
                    commonService.notification.success('Файлът е качен успешно.');
                    $scope.fileUploaded = true;
                });
            }
        }]);