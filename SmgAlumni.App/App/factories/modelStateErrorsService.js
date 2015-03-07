app.factory('modelStateErrorsService', [
    function () {
        return {
            parseErrors: parseErrors
        };

        function parseErrors(error) {
            var errors = [];
            if (error.hasOwnProperty('modelState')) {
                for (var key in error.modelState) {
                    for (var i = 0; i < error.modelState[key].length; i++) {
                        errors.push(error.modelState[key][i]);
                    }
                }
                return errors;
            }
            return errors;
        }
    }
]);