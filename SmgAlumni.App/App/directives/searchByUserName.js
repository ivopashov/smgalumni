app.directive('searchByUserName', ['commonService', 'searchService', 'ngDialog', function (commonService, searchService, ngDialog) {

    var searchByUserNameCtrl = function ($scope,commonService, searchService) {

        $scope.students = [];
        $scope.username = '';

        $scope.getUsers = function () {

            $scope.trySentInvalidForm = true;

            if ($scope.studentSearchForm.$invalid) {
                return;
            };

            searchService.getUserByUserName($scope.username).then(function (success) {
                $scope.students.push(success.data);
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
        templateUrl: '/App/templates/directives/searchByUserName.html',
        scope: {},
        controller: ['$scope', 'commonService', 'searchService', 'ngDialog', searchByUserNameCtrl]
    }
}]);