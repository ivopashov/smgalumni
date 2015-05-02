app.directive('htmlContainer', [function () {

    return {
        restrict: 'AE',
        scope: {
          html:'='  
        },
        link: function (scope, element, attrs) {
            element.append(scope.html);
        }
    }
}]);