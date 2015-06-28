app.service('searchService', ['commonService', function (commonService) {

    function getUsersByYearAndDivision(vm) {
        return commonService.$http.post('api/search/bydivisionandyear', vm);
    }
    
    function getUserByUserName(username) {
        return commonService.$http.get('api/search/short/byusername?username=' + username);
    }

    //returns long vm of users whose usernames contain the searched criteria
    function getUserByUserNameContains(username) {
        return commonService.$http.get('api/search/long/byusernamecontains?username=' + username);
    }

    function getUserById(id) {
        return commonService.$http.get('api/search/byid?id=' + id);
    }

    return ({
        getUsersByYearAndDivision: getUsersByYearAndDivision,
        getUserByUserName: getUserByUserName,
        getUserById: getUserById,
        getUserByUserNameContains: getUserByUserNameContains
    });

}])