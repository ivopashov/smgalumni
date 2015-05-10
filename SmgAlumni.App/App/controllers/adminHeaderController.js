app.controller('adminHeaderController',
    ['$scope', 'authHelper', 'authService', '$http', 'accountService', '$state', '$rootScope',
    function ($scope, authHelper, authService, $http, accountService, $state, $rootScope) {

        (function () {
            $scope.username = accountService.getUser();
        }());

        $rootScope.$on('login', function () {
            $scope.username = accountService.getUser();
        });

        $scope.logout = function () {
            authService.logout();
            $state.transitionTo('menu.login');
        }
        $scope.isLoggedIn = function () {
            return accountService.isUserLoggedIn();
        }

        $scope.menuItems = [
            { name: 'Новини', state: 'admin.news' },
            { name: 'Благотворителност', state: 'admin.causes' },
            { name: 'Одобри Юзери', state: 'admin.verifyusers' },
            { name: 'Управлявай Юзери', state: 'admin.manageusers' },
        ];
    }
    ]);