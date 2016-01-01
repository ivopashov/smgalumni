'use strict';

app.controller('adminNewsController',
    ['$scope', 'commonService', 'newsCauseListingService',
        function ($scope, commonService, newsCauseListingService) {

            $scope.params = {};
            $scope.items = [];
            $scope.kind = "news";
            $scope.currentSelectedPage = { number: -1 };

            $scope.init = function () {
                newsCauseListingService.getCount($scope.kind).then(function (success) {
                    $scope.totalCount = success.data;
                    $scope.params = newsCauseListingService.itemsPerPage();
                })
            }
            $scope.init();

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };

            $scope.updateItem = function (item) {
                newsCauseListingService.getById($scope.kind, item.id).then(function (success) {
                    $scope.selectedItem = success.data;
                    commonService.ngDialog.openConfirm({
                        templateUrl: '/App/templates/dialog/editCauseNews.html',
                        scope: $scope
                    }).then(function (success) {
                        newsCauseListingService.update($scope.selectedItem, $scope.kind).then(function (success) {
                            commonService.notification.success("Успешно обновихте новината");
                            var temp = $scope.items.filter(function (x) { return x.id == $scope.selectedItem.id })[0];
                            var tempIndex = $scope.items.indexOf(temp);
                            $scope.items[tempIndex].heading = $scope.selectedItem.heading;
                            $scope.items[tempIndex].body = $scope.selectedItem.body;
                            $scope.selectedItem = {};
                        }, function (err) {
                            commonService.notification.error(err.data.message);
                        });
                    });
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            }

            $scope.createNew = function () {
                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editCauseNews.html',
                    scope: $scope
                }).then(function (success) {
                    newsCauseListingService.addNew($scope.selectedItem, $scope.kind).then(function (success) {
                        commonService.notification.success("Успешно добавихте новината");
                        $scope.selectedItem = {};
                        $scope.getCurrentPageItems();
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    });
                });
            }

            $scope.deleteItem = function (item) {
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                    scope: $scope
                }).then(function (success) {
                    newsCauseListingService.deleteItem($scope.kind, item).then(function () {
                        commonService.notification.success("Новината беше изтрита успешно");
                        $scope.getCurrentPageItems();
                    }, function (error) {
                        commonService.notification.error(error.data.message);
                    });
                });
            }

            $scope.retrieveItems = function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
                newsCauseListingService.skipAndTake($scope.kind, skip, $scope.params.itemsPerPage).then(function (success) {
                    $scope.items = success.data;
                }, function (error) {
                    commonService.notification.error('Новините не можаха да бъдат заредени');
                })
            }

            $scope.getCurrentPageItems = function () {
                $scope.retrieveItems($scope.currentSelectedPage.number);
            }

            $scope.$watch('currentSelectedPage.dateChange', function () {
                if ($scope.currentSelectedPage.number == 0) {
                    $scope.items = [];
                };
                if ($scope.currentSelectedPage.number > 0) {
                    $scope.retrieveItems($scope.currentSelectedPage.number);
                };
            });

            $scope.selectItem = function (item) {
                if (!item.selected) item.selected = true;
                else {
                    item.selected = false;
                }
                if (!item.body) {
                    newsCauseListingService.getById('news', item.id).then(function (success) {
                        item.body = success.data.body;
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }
            }

        }]);