'use strict';

app.controller('userNewsController',
    ['$scope', 'commonService', 'newsCauseService',
        function ($scope, commonService, newsCauseService) {

            $scope.params = {};
            $scope.items = [];
            $scope.kind = "news";

            newsCauseService.getCount('news').then(function (success) {
                $scope.totalCount = success.data;
                $scope.params = newsCauseService.itemsPerPage();
            })
        }]);