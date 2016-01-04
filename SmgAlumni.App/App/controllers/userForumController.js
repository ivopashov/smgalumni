'use strict';

app.controller('userForumController',
    ['$scope', 'commonService', 'forumThreadService', '$state',
        function ($scope, commonService, forumThreadService, $state) {

            $scope.params = {};
            $scope.items = [];
            $scope.currentSelectedPage = { number: -1 };
            $scope.kind = 'forumThread';

            $scope.init = function () {
                forumThreadService.count().then(function (success) {
                    $scope.totalCount = success.data;
                    $scope.params = forumThreadService.itemsPerPage();
                });
            }
            $scope.init();

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };

            $scope.retrieveItems = function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
                forumThreadService.skipandtake(skip, $scope.params.itemsPerPage).then(function (success) {
                    $scope.items = success.data;
                }, function (error) {
                    commonService.notification.error('Темите не можаха да бъдат заредени');
                });
            }

            $scope.getCurrentPageItems = function () {
                $scope.retrieveItems($scope.currentSelectedPage.number);
            }

            $scope.createNew = function () {
                if (!sessionStorage.authenticationData) {
                    $state.go('menu.login', { returnUrl: 'menu.forum' });
                    event.preventDefault();
                    return;
                }
                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editCauseNews.html',
                    scope: $scope
                }).then(function (success) {
                    forumThreadService.addNew($scope.selectedItem).then(function (success) {
                        commonService.notification.success("Успешно добавихте темата");
                        $scope.selectedItem = {};
                        $scope.getCurrentPageItems();
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    });
                });
            }

            $scope.$watch('currentSelectedPage.number', function (val) {
                if (val == 0) $scope.items = [];
                if (val > 0) $scope.retrieveItems(val);
            });

            $scope.selectItem = function (item) {
                $state.go('menu.forumthread', { id: item.id });
            }

        }]);