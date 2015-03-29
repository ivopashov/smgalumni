app.service('newsCauseService', ['commonService', function (commonService) {

    return ({
        getAll: getAll,
        update: update,
        getById: getById,
        addNew:addNew
    });


    function getAll(kind) {
        if (kind == 'news') {
            return commonService.$http.get('api/admin/allnews');
        } else if (kind == 'cause') {
            return commonService.$http.get('api/admin/allcauses');
        }
    }

    function getById(kind,id) {
        if (kind == 'news') {
            return commonService.$http.get('api/admin/newsbyid?id='+id);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/admin/causebyid?id=' + id);
        }
    }

    function update(vm,kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/admin/updatenews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/admin/updatecause',vm);
        }
    }

    function addNew(vm, kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/admin/createnews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/admin/createcause', vm);
        }
    }
}])