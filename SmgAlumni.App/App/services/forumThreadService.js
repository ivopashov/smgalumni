app.factory('forumThreadService', ['commonService',
    function (commonService) {

        function deleteItem(item) {
            return commonService.$http.get('api/forumthread/delete?id=' + item.id);
        }

        function count() {
            return commonService.$http.get('api/forumthread/count');
        }

        function skipandtake(skip, take) {
            return commonService.$http.get('api/forumthread/skiptake?take=' + take + '&skip=' + skip);
        }

        function itemsPerPage() {
            var params = {};
            params.itemsPerPage = 10;
            params.visiblePages = 5;
            return params;
        }

        function getById(id) {
            return commonService.$http.get('api/forumthread/forumthreadbyid?id=' + id);
        }

        function update(vm) {
            return commonService.$http.post('api/forumthread/update', vm);

        }

        function addNew(vm) {
            return commonService.$http.post('api/forumthread/create', vm);
        }

        return ({
            deleteItem: deleteItem,
            count:count,
            skipandtake: skipandtake,
            getById: getById,
            update: update,
            addNew: addNew,
            itemsPerPage:itemsPerPage
        });
    }])