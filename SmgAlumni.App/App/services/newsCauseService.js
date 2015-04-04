app.service('newsCauseService', ['commonService', function (commonService) {

    return ({
        getAll: getAll,
        update: update,
        getById: getById,
        addNew: addNew,
        getCount: getCount,
        skipAndTake: skipAndTake,
        itemsPerPage: itemsPerPage
    });

    function itemsPerPage() {
        var params = {};
        params.itemsPerPage = 10;
        params.visiblePages = 5;
        return params;
    }

    function getAll(kind) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/allnews');
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/allcauses');
        }
    }

    function getById(kind, id) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/newsbyid?id=' + id);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/causebyid?id=' + id);
        }
    }

    function update(vm, kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/news/updatenews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/cause/updatecause', vm);
        }
    }

    function addNew(vm, kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/news/createnews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/cause/createcause', vm);
        }
    }

    function getCount(kind) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/count');
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/count');
        }
    }

    function skipAndTake(kind, skip, take) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/skiptake?take=' + take + '&skip=' + skip);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/skiptake?take=' + take + '&skip=' + skip);
        }
    }
}])