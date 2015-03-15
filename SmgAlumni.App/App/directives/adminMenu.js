app.directive('adminMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/adminMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Одобри Потребители", sref: "admin.verifyusers", statename: "admin.verifyusers" },
                ];
            }
        ]
    }
}]);