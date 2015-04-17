app.directive('accountMenu', [function () {
    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/accountMenu.html',
        controller: [
            '$scope', '$state', function ($scope, $state) {
                $scope.$state = $state;

                $scope.menuItems = [
                    { title: "Начало", sref: "homeauth", statename: "homeauth" },
                    { title: "Редакция на профил", sref: "account.manageaccount", statename: "account.manageaccount" },
                    { title: "Смяна на парола", sref: "account.changepassword", statename: "account.changepassword" },

                ];
            }
        ]
    }
}]);