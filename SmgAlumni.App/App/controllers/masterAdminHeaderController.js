app.controller('masterAdminHeaderController',
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
            { name: 'Роли', state: 'masteradmin.manageroles' },
            { name: 'Настройки', state: 'masteradmin.settings' },
        ];
    }
    ]);