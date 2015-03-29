'use strict';

app.controller('adminNewsController',
    ['$scope', 'commonService', 'newsCauseService',
        function ($scope, commonService, newsCauseService) {

            $scope.news = [];

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };

            newsCauseService.getAll('news').then(function (success) {
                $scope.news = success.data;
            }, function (err) {
                commonService.notification.error(err.data.message);
            });

            $scope.updateNews = function (news) {
                newsCauseService.getById('news',news.id).then(function (success) {
                    $scope.selectedItem = success.data;
                    commonService.ngDialog.openConfirm({
                        templateUrl: '/App/templates/dialog/editCauseNews.html',
                        scope: $scope
                    }).then(function (success) {
                        newsCauseService.update($scope.selectedItem,'news').then(function (success) {
                            commonService.notification.success("Успешно обновихте новината");
                            $scope.selectedItem = {};
                            newsCauseService.getAll('news').then(function (success) {
                                $scope.news = success.data;
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
                    newsCauseService.addNew($scope.selectedItem, 'news').then(function (success) {
                        commonService.notification.success("Успешно добавихте новината");
                        $scope.selectedItem = {};
                        newsCauseService.getAll('news').then(function (success) {
                            $scope.news = success.data;
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