'use strict';

app.controller('userForumThreadController',
    ['$scope', 'commonService', 'forumThreadService', '$state', '$stateParams', 'forumAnswerService', 'forumCommentService','$sce',
        function ($scope, commonService, forumThreadService, $state, $stateParams, forumAnswerService, forumCommentService,$sce) {

            $scope.params = {};
            $scope.items = [];
            $scope.currentSelectedPage = { number: -1 };

            $scope.editorOptions = {
                language: 'bg',
                uiColor: '#000000'
            };

            $scope.gotoForum = function () {
                $state.go('menu.forum');
            }

            forumThreadService.getById($stateParams.id).then(function (success) {
                $scope.forumThread = success.data;
            });

            $scope.initAnswers = function () {
                forumAnswerService.count($stateParams.id).then(function(success) {
                    $scope.totalCount = success.data;
                    $scope.params = forumThreadService.itemsPerPage();
                });
            }
            $scope.initAnswers();

            $scope.$watch('currentSelectedPage.number', function (val) {
                if (val === 0) $scope.items = [];
                if (val > 0) $scope.retrieveItems(val);
            });

            $scope.retrieveItems = function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.params.itemsPerPage;
                forumAnswerService.skipandtake(skip, $scope.params.itemsPerPage, $stateParams.id).then(function(success) {
                    $scope.items = success.data;
                }, function() {
                    commonService.notification.error('Отговорите не можаха да бъдат заредени');
                });
            }

            $scope.modifyLikes = function (action,answer) {

                if (!sessionStorage.authenticationData) {
                    commonService.notification.warning("Трябва да влезете в системата за да гласувате");
                    return;
                }

                var temp = [];
                
                if (!sessionStorage.votes) {
                    sessionStorage.votes = JSON.stringify(temp);
                } else {
                    temp = JSON.parse(sessionStorage.votes);
                    if (temp.indexOf(answer.id) > -1) {
                        commonService.notification.error('Вече сте гласували');
                        return;
                    }
                }

                if (action === 'increase') {
                    forumAnswerService.modifyLikes(answer.id, 1).then(function(success) {
                        temp.push(answer.id);
                        sessionStorage.votes = JSON.stringify(temp);
                        commonService.notification.success('Успешно гласувахте');
                        answer.likes++;
                    });
                }
                if (action === 'decrease') {
                    forumAnswerService.modifyLikes(answer.id, -1).then(function(success) {
                        temp.push(answer.id);
                        sessionStorage.votes = JSON.stringify(temp);
                        commonService.notification.success('Успешно гласувахте');
                        answer.likes--;
                    });
                }
            }

            $scope.editAnswer = function (answer) {
                $scope.selectedItem = {body:answer.body};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editItemWithBodyOnly.html',
                    scope: $scope
                }).then(function() {
                    var vm = { body: $scope.selectedItem.body, id: answer.id }
                    forumAnswerService.update(vm).then(function() {
                        answer.body = $scope.selectedItem.body;
                        $scope.selectedItem = {};
                        commonService.notification.success("Отговорът беше редактиран успешно");
                    }, function(error) {
                        commonService.notification.error(error.data.message);
                    });
                });
            }

            $scope.editComment = function (comment) {
                $scope.selectedItem = { body: comment.body };
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editItemWithBodyOnly.html',
                    scope: $scope
                }).then(function() {
                    var vm = { body: $scope.selectedItem.body, id: comment.id }
                    forumCommentService.update(vm).then(function() {
                        comment.body = $scope.selectedItem.body;
                        $scope.selectedItem = {};
                        commonService.notification.success("Коментарът беше редактиран успешно");
                    }, function(error) {
                        commonService.notification.error(error.data.message);
                    });
                });
            }

            $scope.deleteAnswer = function (item) {
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                    scope: $scope
                }).then(function() {
                    forumAnswerService.deleteItem(item).then(function() {
                        $scope.items.splice($scope.items.indexOf(item), 1);
                        commonService.notification.success("Отговорът беше изтрит успешно");
                    }, function(error) {
                        commonService.notification.error(error.data.message);
                    });
                });
            }

            $scope.deleteComment = function (item,parentId) {
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/confirmDeleteDialog.html',
                    scope: $scope
                }).then(function() {
                    forumCommentService.deleteItem(item).then(function() {
                        var answer = $scope.items.filter(function(x) { return x.id == parentId })[0];
                        answer.comments.splice(answer.comments.indexOf(item), 1);
                        commonService.notification.success("Коментарът беше изтрит успешно");
                    }, function(error) {
                        commonService.notification.error(error.data.message);
                    });
                });
            }

            $scope.createAnswer = function () {
                if (!sessionStorage.authenticationData) {
                    $state.go('menu.login', { returnUrl: 'menu.forumthread' });
                    event.preventDefault();
                    return;
                }

                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editItemWithBodyOnly.html',
                    scope: $scope
                }).then(function () {
                    var vm = { body: $scope.selectedItem.body, parentId: $stateParams.id }
                    forumAnswerService.addNew(vm).then(function () {
                        $scope.selectedItem = {};
                        $scope.initAnswers();
                    },function(err) {
                        commonService.notification.error(err.data.message);
                    });
                });
            }

            $scope.createComment = function (answer) {
                $scope.selectedItem = {};
                commonService.ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/editItemWithBodyOnly.html',
                    scope: $scope
                }).then(function () {
                    var vm = { body: $scope.selectedItem.body, parentId: answer.id }
                    forumCommentService.addNew(vm).then(function (success1) {
                        $scope.selectedItem = {};
                        answer.comments.push(success1.data);
                    }, function (err) {
                        commonService.notification.error(err.data.message);
                    });
                });
            }

            $scope.$watch('currentSelectedPage.dateChange', function () {
                if ($scope.currentSelectedPage.number == 0) $scope.items = [];
                if ($scope.currentSelectedPage.number > 0) $scope.retrieveItems($scope.currentSelectedPage.number);
            });

        }]);