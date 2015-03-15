app.factory('authHelper', [function () {
    var obj = {}

    var getAuth = function () {
        if (sessionStorage.authenticationData) {
            return JSON.parse(sessionStorage.authenticationData);
        }
        return null;
    };

    var clearAuth = function () {
        sessionStorage.authenticationData = '';
    };

    var getUserName = function (update) {
        switch (update) {
            case 'undefined':
                obj.val = '';
                break;
            case 'login':
                if (sessionStorage.authenticationData) {
                    var authData = JSON.parse(sessionStorage.authenticationData);
                    obj.val = authData.username;
                }
                break;
            default: obj.val = update;
                break;
        }
        return obj;
    }

    return {
        getUserName: getUserName,
        getAuth: getAuth,
        clearAuth: clearAuth
    };
}])