'use strict';

app.controller('changePasswordController',
    ['$scope', 'commonService', 'accountService',
       function ($scope, commonService, accountService) {
           $scope.send = function () {

               $scope.trySentInvalidForm = true;

               if ($scope.newpassword != $scope.confirmpassword) {
                   $scope.changePasswordForm.confirmpassword.$error.match = true;
                   return;
               } else {
                   $scope.changePasswordForm.confirmpassword.$error.match = false;
               }

               if ($scope.changePasswordForm.$invalid) {
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
                   commonService.$state.go('menu');
               },
               function (error) {
                   commonService.notification.error(error.data.message);
               });
           }
       }]);