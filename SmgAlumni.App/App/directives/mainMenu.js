app.directive('mainMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/mainMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Начало", sref: "homeauth", statename: "homeauth" },
                    { title: "Моят Акаунт", sref: "account", statename: "account" },
                    { title: "Търсене", sref: "homeauth.search", statename: "homeauth.search" },
                    { title: "Новини", sref: "homeauth.news", statename: "homeauth.news" },
                    { title: "Каузи", sref: "homeauth.causes", statename: "homeauth.causes" },
                    { title: "Малки Обяви", sref: "homeauth.listings", statename: "homeauth.listings" },
                    { title: "Форум", sref: "homeauth.forum", statename: "homeauth.forum" },
                ];
            }
        ]
    }
}]);