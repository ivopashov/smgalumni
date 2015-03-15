'use strict';

app.controller('changePasswordController',
    ['$scope', 'commonService', 'accountService',
       function ($scope, commonService, accountService) {
           $scope.send = function () {
               if ($scope.newpassword != $scope.confirmpassword) {
                   commonService.notification.error("Двете пароли не съвпадат");
                   return;
               }

               var data = {
                   newPassword: $scope.newpassword,
                   oldPassword: $scope.currentpassword,
                   confirmPassword: $scope.confirmpassword
               }
               accountService.changePassword(data).then(
               function (success) {
                   commonService.notification.success("Паролата Ви е сменена успешно");
                   commonService.$state.go('home');
               },
               function (error) {
                   commonService.notification.error(error.data.message);
               });
           }
       }]);