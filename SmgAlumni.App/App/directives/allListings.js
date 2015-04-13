app.directive('allListings', ['newsCauseListingService', 'commonService', function (newsCauseListingService, commonService) {

    var allListingsController = function ($scope, newsCauseListingService, commonService) {

        $scope.params = {};
        $scope.items = [];
        $scope.kind = "listing";
        $scope.currentSelectedPage = { number: -1 };

        newsCauseListingService.getCount($scope.kind).then(function (success) {
            $scope.totalCount = success.data;
            $scope.params = newsCauseListingService.itemsPerPage();
        })

        $scope.selectItem = function (item) {
            if (!item.selected) item.selected = true;
            else {
                item.selected = false;
            }
            if (!item.body) {
                newsCauseListingService.getById($scope.kind, item.id).then(function (success) {
                    item.body = success.data.body;
                }, function (err) {
                    commonService.notification.error(err.data.message);
                })
            }
        }

        $scope.retrieveItems = function (pageNumber) {
            var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
            newsCauseListingService.skipAndTake($scope.kind, skip, $scope.params.itemsPerPage).then(function (success) {
                $scope.items = success.data;
            }, function (error) {
                commonService.notification.error('Каузите не можаха да бъдат заредени');
            })
        }

        $scope.$watch('currentSelectedPage.number', function (val) {
            if (val == 0) $scope.items = [];
            if (val > 0) $scope.retrieveItems(val);
        })
    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/allListings.html',
        scope: {},
        controller: ['$scope', 'newsCauseListingService', 'commonService', allListingsController]
    }
}]);