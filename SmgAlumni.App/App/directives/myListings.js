app.directive('myListings', ['newsCauseListingService', 'commonService', 'ngDialog', function (newsCauseListingService, commonService, ngDialog) {

    var myListingsController = function ($scope, newsCauseListingService, commonService) {

        $scope.params = {};
        $scope.items = [];
        $scope.kind = "news";
        $scope.currentSelectedPage = { number: -1 };

        
        $scope.init = function () {
            newsCauseListingService.myListingsCount().then(function (success) {
                $scope.totalCount = success.data;
                $scope.params = newsCauseListingService.itemsPerPage();
            })
        }
        $scope.init();

        $scope.selectItem = function (item) {
            if (!item.selected) item.selected = true;
            else {
                item.selected = false;
            }
            if (!item.body) {
                newsCauseListingService.getById('listing', item.id).then(function (success) {
                    item.body = success.data.body;
                }, function (err) {
                    commonService.notification.error(err.data.message);
                })
            }
        }

        $scope.createNew = function () {
            $scope.selectedItem = {};
            commonService.ngDialog.openConfirm({
                templateUrl: '/App/templates/dialog/editCauseNews.html',
                scope: $scope
            }).then(function (success) {
                newsCauseListingService.addNew($scope.selectedItem, 'listing').then(function (success) {
                    commonService.notification.success("Успешно добавихте обявата");
                    $scope.selectedItem = {};
                    $scope.init();
                }, function (err) {
                    commonService.notification.error(err.data.message);
                })
            }, function (err) {
                commonService.notification.error(err.data.message);
            });
        }

        $scope.editItem = function (listing) {
            newsCauseListingService.getById('listing', listing.id).then(function (success) {
                $scope.selectedItem = success.data;
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editCauseNews.html',
                    scope: $scope
                }).then(function (success) {
                    newsCauseListingService.update($scope.selectedItem, 'listing').then(function (success) {
                        commonService.notification.success("Успешно обновихте Обявата");
                        var temp = $scope.items.filter(function (x) {return x.id == $scope.selectedItem.id })[0];
                        var tempIndex = $scope.items.indexOf(temp);
                        $scope.items[tempIndex].heading = $scope.selectedItem.heading;
                        $scope.items[tempIndex].body = $scope.selectedItem.body;
                        $scope.selectedItem = {};
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                });
            }, function (err) {
                commonService.notification.error(err.data.message);
            });
        }

        $scope.retrieveItems = function (pageNumber) {
            var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
            newsCauseListingService.myListingsSkipAndTake(skip, $scope.params.itemsPerPage).then(function (success) {
                $scope.items = success.data;
            }, function (error) {
                commonService.notification.error('Новините не можаха да бъдат заредени');
            })
        }

        $scope.$watch('currentSelectedPage.dateChange', function () {
            if ($scope.currentSelectedPage.number == 0) $scope.items = [];
            if ($scope.currentSelectedPage.number > 0) $scope.retrieveItems($scope.currentSelectedPage.number);
        })

        $scope.deleteItem = function (item) {
            ngDialog.openConfirm({
                templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                scope: $scope
            }).then(function (success) {
                newsCauseListingService.deleteItem('listing', item).then(function () {
                    commonService.notification.success("Обявата беше изтрита успешно");
                    $scope.init();
                }, function (error) {
                    commonService.notification.error(error.data.message);
                })
            })
        }

    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/myListings.html',
        scope: {},
        controller: ['$scope', 'newsCauseListingService', 'commonService', myListingsController]
    }
}]);