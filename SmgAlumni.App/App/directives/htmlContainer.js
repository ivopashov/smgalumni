app.directive('htmlContainer', [function () {

    return {
        restrict: 'AE',
        link: function (scope, element, attrs) {
            element.append(attrs.html);
        }
    }
}]);