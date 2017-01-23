

var FtpControl = function(config) {
    var publicMembers = {
        downLoad: function() {
            return privateMembers.downloadFile();
        },

        reset: function() {
            privateMembers.resetUploader();
        }
    };

    var privateMembers = {
        downloadLocation: config.downLoaction,
        rowId: config.rowId,
        fileName: config.fileName,

        initialise: function() {
            var $currentIFrame = $('#iFrameLoader');
            $currentIFrame.contents().find("body #uploadRowId").val(privateMembers.rowId);
        },

        downloadFile: function() {
            if (privateMembers.downloadLocation.length > 1) {
                var x = ('/FTP/Download?id=' + encodeURI(privateMembers.rowId) + '&type=' + encodeURI(privateMembers.fromType) + '&location=' + encodeURI(privateMembers.downloadLocation));
                var result;
                $.ajax({
                    type: "GET",
                    async: false,
                    url: x,

                    success: function(res) {
                        result = res;
                    }
                });

                return result;
            }
            return "";
        },

        resetUploader: function() {
            var url = '/FTP/Uploader';
            $('#iFrameLoader').prop('src', url);
            privateMembers.initialise();
        }
    };

    privateMembers.initialise();

    return publicMembers;

};