app.factory('attachmentService', ['commonService',
    function (commonService) {

        function downloadFile(tempKey) {
            commonService.$http({ method: 'GET', url: 'api/attachment?tempkey=' + tempKey, responseType: 'arraybuffer' })
            .success(function (data, status, headers, config) {
                var blob = new Blob([data], { type: headers()['content-type'] });
                var blobURL = URL.createObjectURL(blob);
                var anchor = document.createElement("a");
                anchor.download = headers()['content-disposition'].split(';')[1].split('=')[1].replace(/"/g,"");
                anchor.href = blobURL;
                anchor.click();
                anchor.remove(anchor)
            });
        }

        return ({
            downloadFile: downloadFile,
        });
    }])