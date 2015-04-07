'use strict';

app.controller('userCausesController',
    ['$scope', 'commonService', 'newsCauseListingService',
        function ($scope, commonService, newsCauseListingService) {

            $scope.params = {};
            $scope.items = [];
            $scope.kind = "cause";

            newsCauseListingService.getCount('cause').then(function (success) {
                $scope.totalCount = success.data;
                $scope.params = newsCauseListingService.itemsPerPage();
            })

            $scope.selectItem = function (item) {
                if (!item.selected) item.selected = true;
                else {
                    item.selected = false;
                }
                if (!item.body) {
                    newsCauseListingService.getById('cause', item.id).then(function (success) {
                        item.body = success.data.body;
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }
            }

        }]);