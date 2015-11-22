'use strict';

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


            $scope.getDetailsFromIN = function () {
                IN.User.authorize(function () {
                    $loading.start('linkedin');
                    IN.API.Raw("/people/~:(firstName,lastName,emailAddress,headline,industry,positions,summary,location)?format=json")
                        .result($scope.onLinkedInRetrieveSuccess)
                        .error($scope.onLinkedInRetrieveError);
                });
            };

            $scope.onLinkedInRetrieveSuccess = function (params) {
                $scope.user.email = params.emailAddress;
                $scope.user.firstName = params.firstName;
                $scope.user.lastName = params.lastName;
                $scope.user.dwellingCountry = params.location.name;
                $scope.user.profession = params.headline;
                $scope.user.company = params.positions.values[0].company.name;
                $loading.finish('linkedin');
            };

            $scope.onLinkedInRetrieveError = function () {
                $loading.finish('linkedin');
            }

            //Client ID:	77z1ecze2nw7oa
            //Client Secret:	8GYxHFeft6HYlz04

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