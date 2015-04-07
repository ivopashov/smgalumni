app.directive('allListings', [function () {

    var allListingsController = function ($scope, newsCauseListingService) {

        $scope.items = [1,2,3];
        $scope.kind = "listing";
        $scope.totalCount = 100;
        $scope.params = newsCauseListingService.itemsPerPage();

        //newsCauseListingService.getCount('listing').then(function (success) {
        //    $scope.totalCount = success.data;
        //    $scope.params = newsCauseListingService.itemsPerPage();
        //})
    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/allListings.html',
        scope: {},
        controller: ['$scope', 'newsCauseListingService', allListingsController]
    }
}]);