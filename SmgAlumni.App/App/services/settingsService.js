app.service('settingsService', ['commonService', function (commonService) {

    return ({
        getAllSettings: getAllSettings,
        updateSetting: updateSetting,
        addSetting: addSetting,
        deleteSetting: deleteSetting
    });

    function getAllSettings() {
        return commonService.$http.get('api/settings/getall');
    }

    function updateSetting(vm) {
        return commonService.$http.post('api/settings/update', vm);
    }

    function addSetting(vm) {
        return commonService.$http.post('api/settings/add', vm);
    }

    function deleteSetting(vm) {
        return commonService.$http.post('api/settings/delete', vm);
    }

}])