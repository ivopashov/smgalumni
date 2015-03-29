app.service('searchService', ['commonService', function (commonService) {

    return ({
        getUsersByYearAndDivision: getUsersByYearAndDivision,
        getUserByUserName: getUserByUserName,
        getUserById: getUserById
    });

    function getUsersByYearAndDivision(vm) {
        return commonService.$http.post('api/search/bydivisionandyear', vm);
    }
    
    function getUserByUserName(username) {
        return commonService.$http.get('api/search/byusername?username='+username);
    }

    function getUserById(id) {
        return commonService.$http.get('api/search/byid?id=' + id);
    }

}])