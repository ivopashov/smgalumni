app.factory('funService', ['commonService',
    function (commonService) {

        function convert(text) {
            return commonService.$http.get('api/fun/bgtogreek?text=' + text);
        }


        return ({
            convert: convert
        });
    }])