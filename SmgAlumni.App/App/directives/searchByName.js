app.directive('searchByName', ['commonService', 'searchService', 'ngDialog', function (commonService, searchService, ngDialog) {

    var searchByNameCtrl = function ($scope, commonService, searchService) {

        $scope.students = [];
        $scope.name = '';

        $scope.getUsers = function () {

            $scope.trySentInvalidForm = true;

            if ($scope.studentSearchForm.$invalid) {
                return;
            };

            searchService.getUserByName($scope.name).then(function (success) {
                $scope.students = [];
                $scope.students = [].concat(success.data);
            });
        }

        $scope.selectUser = function (id) {
            searchService.getUserById(id).then(function (success) {
                $scope.selectedUser = success.data;
                ngDialog.openConfirm({
                    templateUrl: '/App/templates/dialog/userDetails.html',
                    scope: $scope
                });
            });
        }

    }

    return {
        restrict: 'AE',
        templateUrl: '/App/templates/directives/searchByName.html',
        scope: {},
        controller: ['$scope', 'commonService', 'searchService', 'ngDialog', searchByNameCtrl]
    }
}]);