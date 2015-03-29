app.directive('mainMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/mainMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Моят Акаунт", sref: "account", statename: "account" },
                    { title: "Търсене", sref: "homeauth.search", statename: "homeauth.search" },
                    { title: "Новини", sref: "news", statename: "news" },
                ];
            }
        ]
    }
}]);