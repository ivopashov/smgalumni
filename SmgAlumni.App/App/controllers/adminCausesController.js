'use strict';

app.controller('adminCausesController',
    ['$scope', 'commonService', 'newsCauseListingService',
        function ($scope, commonService, newsCauseListingService) {

            $scope.causes = [];

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };
            
            newsCauseListingService.getAll('cause').then(function (success) {
                $scope.causes = success.data;
            }, function (err) {
                commonService.notification.error(err.data.message);
            });

            $scope.updateNews = function (cause) {
                newsCauseListingService.getById('cause',cause.id).then(function (success) {
                    $scope.selectedItem = success.data;
                    commonService.ngDialog.openConfirm({
                        templateUrl: '/App/templates/dialog/editCauseNews.html',
                        scope: $scope
                    }).then(function (success) {
                        newsCauseListingService.update($scope.selectedItem, 'cause').then(function (success) {
                            commonService.notification.success("Успешно обновихте каузата");
                            $scope.selectedItem = {};
                            newsCauseListingService.getAll('cause').then(function (success) {
                                $scope.causes = success.data;
                            }, function (err) {
                                commonService.notification.error(err.data.message);
                            });
                        }, function (err) {
                            commonService.notification.error(err.data.message);
                        })
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    });
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            }

            $scope.createNew = function () {
                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editCauseNews.html',
                    scope: $scope
                }).then(function (success) {
                    newsCauseListingService.addNew($scope.selectedItem, 'cause').then(function (success) {
                        commonService.notification.success("Успешно добавихте каузата");
                        $scope.selectedItem = {};
                        newsCauseListingService.getAll('cause').then(function (success) {
                            $scope.causes = success.data;
                        }, function (err) {
                            commonService.notification.error(err.data.message);
                        });
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    })
                }, function (err) {
                    commonService.notification.error(err.data.message);
                });
            }

        }]);