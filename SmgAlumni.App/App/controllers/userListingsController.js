'use strict';

app.controller('userListingsController',
    ['$scope', 'commonService', 'newsCauseListingService',
        function ($scope, commonService, newsCauseListingService) {

            $scope.showMyListings = true;
            $scope.showAllListings = false;

            $scope.myListings = function () {
                $scope.showMyListings = true;
                $scope.showAllListings = false;
            }
            
            $scope.allListings = function () {
                $scope.showMyListings = false;
                $scope.showAllListings = true;
            }

        }]);