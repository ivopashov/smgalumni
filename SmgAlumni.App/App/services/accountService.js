app.service('accountService', ['commonService', function(commonService) {

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
        var deferred = commonService.$q.defer();
        commonService.$http.get('api/manage/data')
                 .success(
                 function (data) {
                     deferred.resolve(data);
                 })
                 .error(
                 function (data, status) {
                     deferred.reject(data, status);
                 });
        return deferred.promise;
    }

    function saveAccountData(managedata) {
        var deferred = commonService.$q.defer();
        commonService.$http.post('api/account/saveaccountmanage', managedata)
                 .success(
                 function (data) {
                     deferred.resolve(data);
                 })
                 .error(
                 function (data, status) {
                     deferred.reject(data, status);
                 });
        return deferred.promise;
    }

    function forgotPassword(email) {
        var deferred = commonService.$q.defer();
        commonService.$http.post('api/account/forgotpassword', { email: email })
            .success(
            function (data) {
                commonService.notification.info('Success sending your request');
                deferred.resolve(data);
            })
            .error(
            function (data, status) {
                var errors = commonService.modelStateErrorsService.parseErrors(data);
                for (var i = 0; i < errors.length; i++) {
                    commonService.notification.error(errors[i]);
                }
                deferred.reject(data, status);
            });
        return deferred.promise;
    }

    function changePassword(password) {
        var deferred = commonService.$q.defer();
        commonService.$http.post('api/account/changepassword', password)
            .success(
            function (data) {
                commonService.notification.info('Successful password change');
                deferred.resolve(data);
            })
            .error(
            function (data, status) {
                var errors = commonService.modelStateErrorsService.parseErrors(data);
                for (var i = 0; i < errors.length; i++) {
                    commonService.notification.error(errors[i]);
                }
                deferred.reject(data, status);
            });
        return deferred.promise;
    }

    function checkGuid(values) {
        var deffered = commonService.$q.defer();
        commonService.$http.get('api/account/checkguid', {
            params: { guid: values.guid, email: values.email }
        })
            .success(
            function (data) {
                deferred.resolve(data);
            })
            .error(
            function (data, status) {
                var errors = commonService.modelStateErrorsService.parseErrors(data);
                for (var i = 0; i < errors.length; i++) {
                    commonService.notification.error(errors[i]);
                }
                deferred.reject(data, status);
            });
        return deffered.promise;
    }

    function resetPassword(resetPassword) {
        var deferred = commonService.$q.defer();
        commonService.$http.post('api/account/resetpassword', resetPassword)
           .success(
            function (data) {
                commonService.notification.info('Successful password reset');
                deferred.resolve(data);
            })
            .error(
            function (data, status) {
                var errors = commonService.modelStateErrorsService.parseErrors(data);
                for (var i = 0; i < errors.length; i++) {
                    commonService.notification.error(errors[i]);
                }
                deferred.reject(data, status);
            });
        return deferred.promise;
    }

    function register(user) {
        var deferred = commonService.$q.defer();
        commonService.$http.post('api/account/register', user)
           .success(
            function (data) {
                //commonService.notification.info('Successful registration');
                deferred.resolve(data);
            })
            .error(
            function (data, status) {
                var errors = commonService.modelStateErrorsService.parseErrors(data);
                for (var i = 0; i < errors.length; i++) {
                    commonService.notification.error(errors[i]);
                }
                deferred.reject(data, status);
            });
        return deferred.promise;
    }
}])