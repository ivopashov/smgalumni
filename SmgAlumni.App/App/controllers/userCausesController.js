'use strict';

app.controller('userCausesController',
    ['$scope', 'commonService', 'newsCauseService',
        function ($scope, commonService, newsCauseService) {

            $scope.params = {};
            $scope.items = [];
            $scope.kind = "cause";

            newsCauseService.getCount('cause').then(function (success) {
                $scope.totalCount = success.data;
                $scope.params = newsCauseService.itemsPerPage();
            })

        }]);