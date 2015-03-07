'use strict';

app.controller('changePasswordController',
    ['$scope', 'commonService', 'accountService',
       function ($scope,commonService, accountService) {
           $scope.send = function () {
               var data = {
                   newPassword: $scope.newpassword,
                   oldPassword: $scope.currentpassword
               }
               accountService.changePassword(data).then(
               function (success) {
                   commonService.$location.path('account/manageaccount');
                   commonService.notification.success(success);
               },
               function (error) {
                   var errors = commonService.modelStateErrorsService.parseErrors(error);
                   for (var i = 0; i < errors.length; i++) {
                       commonService.notification.error(errors[i]);
                   }
               });
           }
       }]);