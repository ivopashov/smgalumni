'use strict';

app.controller('userCausesController',
    ['$scope', 'commonService', 'newsCauseService',
        function ($scope, commonService, newsCauseService) {

            newsCauseService.getCount('cause').then(function (success) {
                $scope.totalCount = success.data;
            })

        }]);