app.directive('customPager', ['newsCauseListingService', function (newsCauseListingService) {

    var pagerController = function ($scope, newsCauseListingService) {

        $scope.pages = [];

        $scope.$watch('totalCount', function () {
            if ($scope.totalCount % $scope.params.itemsPerPage == 0) {
                $scope.totalNumberOfPages = $scope.totalCount / $scope.params.itemsPerPage;
            } else {
                $scope.totalNumberOfPages = parseInt($scope.totalCount / $scope.params.itemsPerPage) + 1;
            }

            $scope.visiblePages = $scope.totalNumberOfPages > $scope.params.visiblePages ? $scope.params.visiblePages : $scope.totalNumberOfPages;
            for (var i = 1; i <= $scope.visiblePages; i++) {
                if (i == 1) {
                    $scope.pages.push({ number: i, current: true });
                    $scope.setCurrentPage($scope.pages[0]);
                }
                else {
                    $scope.pages.push({ number: i });
                }
            }
        })

        $scope.firstPage = function () {
            $scope.pages = [];
            for (var i = 1; i <= $scope.visiblePages; i++) {
                if (i == 1) {
                    $scope.pages.push({ number: i, current: true });
                }
                else {
                    $scope.pages.push({ number: i });
                }
            }
            $scope.setCurrentPage($scope.pages[0]);
        }

        $scope.lastPage = function () {
            $scope.pages = [];
            for (var i = $scope.totalNumberOfPages; i > ($scope.totalNumberOfPages - $scope.visiblePages) ; i--) {
                if (i == $scope.totalNumberOfPages) {
                    $scope.pages.unshift({ number: i, current: true });
                }
                else {
                    $scope.pages.unshift({ number: i });
                }
            }
            $scope.setCurrentPage($scope.pages[$scope.pages.length - 1]);
        }

        $scope.previousPage = function () {
            var indexOfCurrentPage = $scope.makeCurrentPageInactive();

            if (indexOfCurrentPage == 0) {
                $scope.pages.unshift({ number: $scope.pages[indexOfCurrentPage].number - 1, current: true });
                $scope.pages.splice($scope.pages.length - 1, 1);
                $scope.setCurrentPage($scope.pages[0]);
            } else {
                $scope.pages[indexOfCurrentPage - 1].current = true;
                $scope.setCurrentPage($scope.pages[indexOfCurrentPage - 1]);
            }
        }

        $scope.nextPage = function () {
            var indexOfCurrentPage = $scope.makeCurrentPageInactive();

            if (indexOfCurrentPage == $scope.pages.length - 1) {
                $scope.pages.push({ number: $scope.pages[indexOfCurrentPage].number + 1, current: true });
                $scope.pages.splice(0, 1);
                $scope.setCurrentPage($scope.pages[$scope.pages.length - 1]);
            } else {
                $scope.pages[indexOfCurrentPage + 1].current = true;
                $scope.setCurrentPage($scope.pages[indexOfCurrentPage + 1]);
            }
        }

        $scope.switchPage = function (page) {
            $scope.makeCurrentPageInactive();

            page.current = true;
            $scope.setCurrentPage(page);
        }

        $scope.setCurrentPage = function (page) {
            $scope.currentSelectedPage.number = page.number;
        }

        $scope.makeCurrentPageInactive = function () {
            var currentPage = $scope.pages.filter(function (x) { return x.current })[0];
            var indexOfCurrentPage = $scope.pages.indexOf(currentPage);
            $scope.pages[indexOfCurrentPage].current = false;
            return indexOfCurrentPage;
        }

        $scope.numberOfCurrentPage = function () {
            if ($scope.pages.length == 0) return -1;
            var currentPage = $scope.pages.filter(function (x) { return x.current })[0];
            return currentPage.number;
        }

        $scope.showNextButton = function () {
            if ($scope.currentSelectedPage.number < $scope.totalNumberOfPages && $scope.currentSelectedPage.number > 0) return true;
            return false;
        }

        $scope.showPreviousButton = function () {
            if ($scope.currentSelectedPage.number > 1) return true;
            return false;
        }

        $scope.showFirstPageButton = function () {
            var result = $scope.pages.filter(function (x) { return x.number == 1 });
            if (result.length == 0) return true;
            return false;
        }

        $scope.showLastPageButton = function () {
            var result = $scope.pages.filter(function (x) { return x.number == $scope.totalNumberOfPages });
            if (result.length == 0) return true;
            return false;
        }

    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/customPager.html',
        scope: {
            items: '=',
            params: '=',
            kind: '=',
            totalCount: '=',
            currentSelectedPage: '='
        },
        //scope:false,
        controller: ['$scope', 'newsCauseListingService', pagerController]
    }
}]);