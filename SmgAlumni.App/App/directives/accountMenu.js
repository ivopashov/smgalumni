app.directive('accountMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/accountMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Редактирай Си Профила", sref: "account.manageaccount", statename: "account.manageaccount" },
                    { title: "Смени Си Паролата", sref: "account.changepassword", statename: "account.changepassword" }
                ];
            }
        ]
    }
}]);