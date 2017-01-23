var modalDialog = function(spec) {

    var privateMembers = {
        uniqueDivId: null,
        width: function() {
            return spec.width === undefined ? '400' : spec.width;
        },
        moveLeft: function() {
            return spec.moveLeft === undefined ? '' : spec.moveLeft;
        },
        moveTop: function() {
            return spec.moveTop === undefined ? '' : spec.moveTop;
        },

        createStyle: function() {

            var rtn = "style = 'width:" + privateMembers.width() + "px;";

            if (privateMembers.moveLeft() !== '') {
                rtn = rtn + " margin-left: -" + privateMembers.moveLeft() + "px;";
            }

            if (privateMembers.moveTop() !== '') {
                rtn = rtn + " margin-top: " + privateMembers.moveTop() + "px";
            }

            return rtn;

        },
        initialise: function() {
            privateMembers.uniqueDivId = utilities.getNextUniqueID("Modal");

            $('#content').append($("<div id='" + privateMembers.uniqueDivId +
                "' class='startHidden'><div class='modalBackground modalClose' ></div><div class='modalContent tabbedPopUp' " + privateMembers.createStyle() + " '><h1>" +
                spec.title + "</h1><div id='inner" + privateMembers.uniqueDivId + "' ></div></div></div>"));

            $('#' + privateMembers.uniqueDivId).find('div.modalClose').click(function(e) {
                e.preventDefault();
                publicMembers.hide();
            });
        }
    };

    var publicMembers = {
        show: function() {
            $('#' + privateMembers.uniqueDivId).show('fast');
        },
        hide: function() {
            $('#' + privateMembers.uniqueDivId).hide('fast');
        },
        contentDiv: function() { return $('#inner' + privateMembers.uniqueDivId); }
    };

    privateMembers.initialise();

    return publicMembers;
};