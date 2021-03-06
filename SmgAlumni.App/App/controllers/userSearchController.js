﻿'use strict';

app.controller('userSearchController',
    ['$scope', 'searchService', 'commonService', 'ngDialog',
        function ($scope, searchService, commonService, ngDialog) {

            $scope.showSearchByYearAndDivision = true;
            $scope.showSearchByUsername = false;
            $scope.showSearchByName = false;

            $scope.searchByYearAndDivision=function() {
                $scope.showSearchByYearAndDivision = true;
                $scope.showSearchByUsername = false;
                $scope.showSearchByName = false;
            }

            $scope.searchByUsername=function() {
                $scope.showSearchByYearAndDivision = false;
                $scope.showSearchByUsername = true;
                $scope.showSearchByName = false;
            }

            $scope.searchByName = function () {
                $scope.showSearchByYearAndDivision = false;
                $scope.showSearchByUsername = false;
                $scope.showSearchByName = true;
            }

        }]);