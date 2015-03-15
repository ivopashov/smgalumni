app.service('adminService', ['commonService', function (commonService) {

    return ({
        getUnverifiedUsers: getUnverifiedUsers,
        postVerifiedUsers: postVerifiedUsers
    });


    function getUnverifiedUsers() {
        return commonService.$http.get('api/admin/unverifiedusers');
    }

    function postVerifiedUsers(vm) {
        return commonService.$http.post('api/admin/verifyusers', vm);
    }
}])