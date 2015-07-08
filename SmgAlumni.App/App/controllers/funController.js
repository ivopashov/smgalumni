'use strict';

app.controller('funController',
    ['$scope', 'funService','$sce',
        function ($scope, funService,$sce) {
            $scope.textToConvert = '';

            $scope.convert = function () {
                if ($scope.bgtoGreekForm.$valid) {
                    funService.convert($scope.textToConvert).then(function (success) {
                        $scope.convertedText = $sce.trustAsHtml(success.data);
                    });
                }
            }
        }]);