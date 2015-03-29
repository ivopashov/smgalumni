app.service('masterAdminService', ['commonService', function (commonService) {

    return ({
        getAllRoles: getAllRoles,
        updateUserRoles:updateUserRoles
    });


    function getAllRoles() {
        return commonService.$http.get('api/masteradmin/getallroles');
    }

    function updateUserRoles(vm) {
        return commonService.$http.post('api/masteradmin/updateuserroles',vm);
    }
}])