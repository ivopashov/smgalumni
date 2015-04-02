app.service('accountService', ['commonService', function(commonService) {

    return ({
        forgotPassword: forgotPassword,
        changePassword: changePassword,
        checkGuid: checkGuid,
        resetPassword: resetPassword,
        register: register,
        saveAccountData: saveAccountData,
        getAccountData: getAccountData,
        getUser: getUserName,
        isUserLoggedIn: isUserLoggedIn,
        getUserAccount: getUserAccount,
        updateUser: updateUser
    });

    function getUserName() {
        if (sessionStorage.authenticationData) {
            var result = JSON.parse(sessionStorage.authenticationData);
            return result.username;
        }
        return '';
    }

    function isUserLoggedIn() {
        if (sessionStorage.authenticationData) return true;
        return false;
    }

    function getAccountData() {
        return commonService.$http.get('api/manage/data');
    }

    function saveAccountData(managedata) {
        return commonService.$http.post('api/account/saveaccountmanage', managedata);
    }

    function forgotPassword(email) {
        return commonService.$http.post('api/account/forgotpassword', { email: email });
    }

    function changePassword(password) {
        return commonService.$http.post('api/account/changepassword', password);
    }

    function checkGuid(guid,email) {
        return commonService.$http.get('api/account/checkguid', {
            params: { guid: guid, email: email }
        });
    }

    function resetPassword(resetPassword) {
        return commonService.$http.post('api/account/resetpassword', resetPassword);
    }

    function getUserAccount() {
        return commonService.$http.get('api/account/useraccount');
    }

    function updateUser(user) {
        return commonService.$http.post('api/account/updateuser',user);
    }

    function register(user) {
        return commonService.$http.post('api/account/register', user);
    }
}])