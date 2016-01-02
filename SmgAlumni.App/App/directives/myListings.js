app.directive('myListings', ['newsCauseListingService', 'commonService', 'ngDialog', 'Upload', function (newsCauseListingService, commonService, ngDialog, $upload) {

    var myListingsController = function ($scope, newsCauseListingService, commonService, $upload) {

        $scope.params = {};
        $scope.items = [];
        $scope.kind = "myListings";
        $scope.currentSelectedPage = { number: -1 };
        $scope.attachment = {};
        $scope.uploads = [];

        $scope.init = function () {
            newsCauseListingService.myListingsCount().then(function (success) {

                if (success.data == 0) { $scope.totalCount = -1; }
                else {
                    $scope.totalCount = success.data;
                }
                $scope.params = newsCauseListingService.itemsPerPage();
            });
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
                });
            }
        }

        $scope.createNew = function () {
            $scope.selectedItem = { attachments: [] };
            commonService.ngDialog.openConfirm({
                templateUrl: '/App/templates/dialog/editCauseNews.html',
                scope: $scope
            }).then(function (success) {
                $scope.selectedItem.tempKeys = _.map($scope.selectedItem.attachments, function (x) { return x.tempkey });
                newsCauseListingService.addNew($scope.selectedItem, 'listing').then(function (success) {
                    commonService.notification.success("Успешно добавихте обявата");
                    $scope.selectedItem = {};
                    $scope.init();
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            });
        }

        $scope.editItem = function (listing) {
            $scope.selectedItem = listing;
            commonService.ngDialog.openConfirm({
                templateUrl: '/App/templates/dialog/editCauseNews.html',
                scope: $scope
            }).then(function (success) {
                newsCauseListingService.update($scope.selectedItem, 'listing').then(function (success) {
                    commonService.notification.success("Успешно обновихте Обявата");
                    var temp = $scope.items.filter(function (x) { return x.id == $scope.selectedItem.id })[0];
                    var tempIndex = $scope.items.indexOf(temp);
                    $scope.items[tempIndex].heading = $scope.selectedItem.heading;
                    $scope.items[tempIndex].body = $scope.selectedItem.body;
                    $scope.selectedItem = {};
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            });
        }

        $scope.retrieveItems = function (pageNumber) {
            var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
            newsCauseListingService.myListingsSkipAndTake(skip, $scope.params.itemsPerPage).then(function (success) {
                $scope.items = success.data;
            }, function (error) {
                commonService.notification.error('Новините не можаха да бъдат заредени');
            });
        }

        $scope.$watch('currentSelectedPage.dateChange', function () {
            if ($scope.currentSelectedPage.number == 0) $scope.items = [];
            if ($scope.currentSelectedPage.number > 0) $scope.retrieveItems($scope.currentSelectedPage.number);
        });

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
                });
            });
        };

        $scope.upload = function (file) {
            if (file && file.length > 0) {
                $upload.upload({
                    url: 'api/attachment',
                    data: { file: file[0] }
                }).success(function (data, status, headers, config) {
                    $scope.selectedItem.attachments = $scope.selectedItem.attachments.concat(data);
                });
            }
        }

        $scope.deleteAttachment = function (item) {
            $scope.selectedItem.attachments.splice($scope.selectedItem.attachments.indexOf(item), 1);
        }

    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/myListings.html',
        scope: {},
        controller: ['$scope', 'newsCauseListingService', 'commonService', 'Upload', myListingsController]
    }
}]);