app.directive('masterAdminMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/masterAdminMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Управление на Роли", sref: "masteradmin.manageroles", statename: "masteradmin.manageroles" },
                    { title: "Настройки", sref: "masteradmin.settings", statename: "masteradmin.settings" }
                ];
            }
        ]
    }
}]);