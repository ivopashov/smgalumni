'use strict';

app.controller('userNewsController',
    ['$scope', 'commonService', 'newsCauseService',
        function ($scope, commonService, newsCauseService) {

            $scope.pages = [];
            $scope.params = {};

            newsCauseService.getCount('news').then(function (success) {
                $scope.totalCount = success.data;
                $scope.params = newsCauseService.itemsPerPage();

                if ($scope.totalCount % $scope.params.itemsPerPage == 0) {
                    $scope.totalNumberOfPages = $scope.totalCount / $scope.params.itemsPerPage;
                } else {
                    $scope.totalNumberOfPages = ($scope.totalCount / $scope.params.itemsPerPage) + 1;
                }

                $scope.visiblePages = $scope.totalNumberOfPages > $scope.params.visiblePages ? $scope.params.visiblePages : $scope.totalNumberOfPages;
                for (var i = 1; i <= $scope.visiblePages; i++) {
                    if (i == 1) {
                        $scope.pages.push({ number: i, current: true });
                    }
                    else {
                        $scope.pages.push({ number: i});
                    }
                }
            })

            $scope.firstPage = function () {

            }
            $scope.lastPage = function () {

            }
            $scope.previousPage = function () {

            }
            $scope.nextPage = function () {

            }
            $scope.switchPage = function (page) {
                var currentPage = $scope.pages.filter(function (x) { return x.current })[0];
                var indexOfCurrentPage = $scope.pages.indexOf(currentPage);
                $scope.pages[indexOfCurrentPage].current = false;

                var selectedPage = $scope.pages.filter(function (x) { return x.number == page.number })[0];
                var indexOfSelectedPage = $scope.pages.indexOf(selectedPage);
                $scope.pages[indexOfSelectedPage].current = true;
                $scope.setCurrentPage(selectedPage);

                newsCauseService.skipAndTake('news', indexOfSelectedPage * $scope.params.itemsPerPage, $scope.params.itemsPerPage).then(function (success) {
                    $scope.items = success.data;
                }, function (error) {
                    commonService.notification.error('Новините не можаха да бъдат заредени');
                })
            }

            $scope.setCurrentPage = function (page) {
                $scope.currentSelectedPage = page;
            }

            

           

        }]);