app.directive('htmlContainer', ['$compile', function ($compile) {

    return {
        restrict: 'AE',
        link: function (scope, element, attrs) {
            element.append(attrs.html);
        }
    }
}]);