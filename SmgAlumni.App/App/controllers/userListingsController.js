'use strict';

app.controller('userListingsController',
    ['$scope','$state',
        function ($scope, $state) {

            $scope.showMyListings = false;
            $scope.showAllListings = true;

            $scope.myListings = function () {
                if (!sessionStorage.authenticationData) {
                    $state.go('menu.login', { returnUrl:'menu.listings' });
                    event.preventDefault();
                    return;
                }
                $scope.showMyListings = true;
                $scope.showAllListings = false;
            }
            
            $scope.allListings = function () {
                $scope.showMyListings = false;
                $scope.showAllListings = true;
            }

        }]);