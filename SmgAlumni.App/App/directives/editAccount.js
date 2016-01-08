app.directive('editAccount', [
    'authHelper', 'accountService', 'commonService', 'Upload', '$state', '$loading', function (authHelper, accountService, commonService, $upload, $state, $loading) {

        var editAccountCtrl = function (authHelper, $scope, accountService, commonService, $upload, $state, $loading) {

            $scope.divisions = ['А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М'];
            accountService.getUserAccount().then(function (success) {
                $scope.user = success.data;
                var img = angular.element('<img src="api/file/avatar?username=' + $scope.user.userName + '" />');
                var holder = $('.manageAccountAvatarHolder');
                holder.append(img);
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
                if (!file || file.length == 0) {
                    return;
                }

                $scope.image = file[0];

                if (!file || file.length == 0) {
                    return;
                }
                if (file[0].type.indexOf('image') < 0) {
                    commonService.notification.error('Файлът не е снимка');
                }
                $upload.upload({
                    url: 'api/file/avatar',
                    fields: { 'username': $scope.username },
                    data: { file: file[0] }
                }).success(function (data, status, headers, config) {
                    commonService.notification.success('Файлът е качен успешно.');
                    
                    $('.manageAccountAvatarHolder img').remove();

                    var img = angular.element('<img src="api/file/avatar?username=' + $scope.user.userName + '" />');
                    var holder = $('.manageAccountAvatarHolder');
                    holder.append(img);
                });
            }

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
        }

        return {
            restrict: 'AE',
            templateUrl: '/App/templates/directives/manageAccount.html',
            scope: {},
            controller: ['authHelper', '$scope', 'accountService', 'commonService', 'Upload', '$state', '$loading', editAccountCtrl]
        }
    }
]);
