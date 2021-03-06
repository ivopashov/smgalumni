﻿'use strict';

app.controller('manageAccountController',
    ['authHelper', '$scope', 'accountService', 'commonService', '$upload', '$state','$loading',
        function (authHelper, $scope, accountService, commonService, $upload, $state, $loading) {

            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            accountService.getUserAccount().then(function (success) {
                $scope.user = success.data;
            }, function (errorMsg) {
                commonService.notification.error(errorMsg.data.message);
            });

            $scope.update = function () {
                accountService.updateUser($scope.user).then(function (success) {
                    commonService.notification.success("Потребителят е обновен успешно");
                    $state.go('homeauth');
                })
            };


            //$scope.image = {};

            //$scope.upload = function (file) {

            //    $scope.image = file[0];

            //    if (!file || file.length==0) return;
            //    if (file[0].type.indexOf('image') < 0)commonService.notification.error('Файлът не е снимка');
            //    //$upload.upload({
            //    //    url: 'upload/url',
            //    //    fields: { 'username': $scope.username },
            //    //    file: file
            //    //}).success(function (data, status, headers, config) {
            //    //    commonService.notification.success('Файлът ' + config.file.name + 'е качен успешно.');
            //    //});
            //}
        }]);