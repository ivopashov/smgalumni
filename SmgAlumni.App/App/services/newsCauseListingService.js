app.service('newsCauseListingService', ['commonService', function (commonService) {

    return ({
        getAll: getAll,
        update: update,
        getById: getById,
        addNew: addNew,
        getCount: getCount,
        skipAndTake: skipAndTake,
        itemsPerPage: itemsPerPage,
        myListingsCount: myListingsCount,
        myListingsSkipAndTake: myListingsSkipAndTake,
        deleteItem:deleteItem

    });

    function deleteItem(kind,item) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/delete?id='+item.id);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/delete?id=' + item.id);
        } else if (kind == 'listing') {
            return commonService.$http.get('api/listing/delete?id=' + item.id);
        }
    }

    function myListingsCount() {
        return commonService.$http.get('api/listing/my/count');
    }

    function myListingsSkipAndTake(skip, take) {
        return commonService.$http.get('api/listing/my/skiptake?take=' + take + '&skip=' + skip);
    }

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
        } else if (kind == 'listing') {
            return commonService.$http.get('api/listing/alllistings');
        }
    }

    function getById(kind, id) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/newsbyid?id=' + id);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/causebyid?id=' + id);
        } else if (kind == 'listing') {
            return commonService.$http.get('api/listing/listingbyid?id=' + id);
        }
    }

    function update(vm, kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/news/updatenews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/cause/updatecause', vm);
        } else if (kind == 'listing') {
            return commonService.$http.post('api/listing/updatelisting', vm);
        }
    }

    function addNew(vm, kind) {
        if (kind == 'news') {
            return commonService.$http.post('api/news/createnews', vm);
        } else if (kind == 'cause') {
            return commonService.$http.post('api/cause/createcause', vm);
        } else if (kind == 'listing') {
            return commonService.$http.post('api/listing/createlisting', vm);
        }
    }

    function getCount(kind) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/count');
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/count');
        } else if (kind == 'listing') {
            return commonService.$http.get('api/listing/count');
        }
    }

    function skipAndTake(kind, skip, take) {
        if (kind == 'news') {
            return commonService.$http.get('api/news/skiptake?take=' + take + '&skip=' + skip);
        } else if (kind == 'cause') {
            return commonService.$http.get('api/cause/skiptake?take=' + take + '&skip=' + skip);
        } else if (kind == 'listing') {
            return commonService.$http.get('api/listing/skiptake?take=' + take + '&skip=' + skip);
        }
    }
}])