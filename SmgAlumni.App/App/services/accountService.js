﻿app.service('accountService', ['commonService', function(commonService) {

    return ({
        forgotPassword: forgotPassword,
        changePassword: changePassword,
        checkGuid: checkGuid,
        resetPassword: resetPassword,
        register: register,
        saveAccountData: saveAccountData,
        getAccountData: getAccountData
    });

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

    function checkGuid(values) {
        return commonService.$http.get('api/account/checkguid', {
            params: { guid: values.guid, email: values.email }
        });
    }

    function resetPassword(resetPassword) {
        return commonService.$http.post('api/account/resetpassword', resetPassword);
    }

    function register(user) {
        return commonService.$http.post('api/account/register', user);
    }
}])