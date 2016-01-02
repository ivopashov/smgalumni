app.controller('headerController',
    ['$scope', 'authHelper', 'authService', '$http', 'accountService', '$state', '$rootScope',
    function ($scope, authHelper, authService, $http, accountService, $state, $rootScope) {

        (function() {
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
            { name: 'Новини',state:'menu.news' },
            { name: 'Обяви',state:'menu.listings' },
            { name: 'Благотворителност', state: 'menu.causes' },
            { name: 'Търсене', state: 'menu.search' },
            { name: 'Форум',state:'menu.forum' },
            //{ name: 'Документи',state:'menu.documents' },
        ];
    }
    ]);