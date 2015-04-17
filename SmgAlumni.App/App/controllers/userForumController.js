'use strict';

app.controller('userForumController',
    ['$scope', 'commonService', 'forumThreadService',
        function ($scope, commonService, forumThreadService) {

            $scope.params = {};
            $scope.items = [];
            $scope.currentSelectedPage = { number: -1 };

            $scope.init = function () {
                forumThreadService.count().then(function (success) {
                    $scope.totalCount = success.data;
                    $scope.params = forumThreadService.itemsPerPage();
                })
            }
            $scope.init();

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };

            $scope.retrieveItems = function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
                forumThreadService.skipAndTake($scope.kind, skip, $scope.params.itemsPerPage).then(function (success) {
                    $scope.items = success.data;
                }, function (error) {
                    commonService.notification.error('Темите не можаха да бъдат заредени');
                })
            }

            $scope.createNew = function () {
                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editCauseNews.html',
                    scope: $scope
                }).then(function (success) {
                    forumThreadService.addNew($scope.selectedItem).then(function (success) {
                        commonService.notification.success("Успешно добавихте темата");
                        $scope.selectedItem = {};
                        $scope.init();
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            }
        }]);