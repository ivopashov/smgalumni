app.factory('forumAnswerService', ['commonService',
    function (commonService) {

        function deleteItem(item) {
            return commonService.$http.get('api/forumanswer/delete?id=' + item.id);
        }

        function count() {
            return commonService.$http.get('api/forumanswer/count');
        }

        function skipandtake(skip, take, forumthreadid) {
            return commonService.$http.get('api/forumanswer/skiptake?take=' + take + '&skip=' + skip + '&forumthreadid=' + forumthreadid);
        }

        function itemsPerPage() {
            var params = {};
            params.itemsPerPage = 10;
            params.visiblePages = 5;
            return params;
        }

        function getById(id) {
            return commonService.$http.get('api/forumanswer/forumanswerbyid?id=' + id);
        }

        function update(vm) {
            return commonService.$http.post('api/forumanswer/update', vm);

        }

        function addNew(vm) {
            return commonService.$http.post('api/forumanswer/create', vm);
        }

        return ({
            deleteItem: deleteItem,
            count: count,
            skipandtake: skipandtake,
            getById: getById,
            update: update,
            addNew: addNew
        });
    }])