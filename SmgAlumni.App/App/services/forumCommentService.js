app.factory('forumCommentService', ['commonService',
    function (commonService) {

        function deleteItem(item) {
            return commonService.$http.get('api/forumcomment/delete?id=' + item.id);
        }

        function getById(id) {
            return commonService.$http.get('api/forumcomment/forumcommentbyid?id=' + id);
        }

        function update(vm) {
            return commonService.$http.post('api/forumcomment/update', vm);

        }

        function addNew(vm) {
            return commonService.$http.post('api/forumcomment/create', vm);
        }

        return ({
            deleteItem: deleteItem,
            getById: getById,
            update: update,
            addNew: addNew
        });
    }])