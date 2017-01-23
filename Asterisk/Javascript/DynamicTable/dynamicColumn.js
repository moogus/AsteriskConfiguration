
var abstractColumn = function(contentValue) {
    var publicMembers = {
        content: "",
        editable: true,
        useforsave: true
    };

    if (contentValue)
        publicMembers.content = contentValue;

    return publicMembers;
};

var abstractCommandColumn = function() {
    var publicMembers = abstractColumn();
    publicMembers.editable = false;
    publicMembers.useforsave = false;

    return publicMembers;
};

var editColumn = function() {
    var publicMembers = abstractCommandColumn();
    publicMembers.content = "<a href='' class='edit'>Edit</a>";
    publicMembers.thisIndex = '2';
    publicMembers.isLink = true;

    return publicMembers;
};

var editColumnNotVisable = function() {
    var publicMembers = editColumn();
    publicMembers.isInvisible = true;

    return publicMembers;
};

var deleteColumn = function() {
    var publicMembers = abstractCommandColumn();
    publicMembers.content = "<a href='' class='delete'>Delete</a>";
    publicMembers.thisIndex = '1';
    publicMembers.isLink = true;

    return publicMembers;
};

var deleteColumnNotVisable = function() {
    var publicMembers = deleteColumn();
    publicMembers.isInvisible = true;

    return publicMembers;
};

var linkColumn = function(link) {
    var publicMembers = abstractCommandColumn();
    publicMembers.isEdit = true;
    publicMembers.transform = link;
    publicMembers.thisIndex = '3';

    return publicMembers;
};

var idColumn = function() {
    var publicMembers = abstractColumn();
    publicMembers.editable = false;
    publicMembers.hidden = true;

    publicMembers.extractValue = function(td) {
        return td[0].innerHTML;
    };

    return publicMembers;
};

var nonEditColumn = function(contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.editable = false;
    publicMembers.useforsave = false;

    return publicMembers;
};

var editTextColumn = function(contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.extractValue = function(td) {
        return td.children("input[type='text']")[0].value;
    };
    publicMembers.editHTML = function(data) {
        var str = '<input type="text" size=10 value="' + data + '">';
        return str;
    };

    return publicMembers;
};

var editTextNoEditColumn = function() {
    var publicMembers = editTextColumn();
    publicMembers.stopUpdate = true;
    return publicMembers;
};

//to be used in conjucntion with the hidenColumns[] in the table 
//the array is the inverse index eg edit column is 2
var hidenValueColumn = function(contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.extractValue = function(td) {
        return td.children("input[type='hidden']")[0].value;
    };
    publicMembers.editHTML = function(data) {
        return "";
    };
    if (contentValue)
        publicMembers.content = contentValue;
    return publicMembers;
};

var dropdownColumn = function(dropdown, contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.dropdownValues = [];

    publicMembers.extractValue = function(td) {
        return td.children("select")[0].value;
    };

    publicMembers.editHTML = function(data) {
        var content = "";
        content = '<select>';
        var i;
        var found = 0;

        for (i = 0; i < publicMembers.dropdownValues.length; i++) {
            var option = publicMembers.dropdownValues[i];
            content += '<option' + (data === option ? " selected" : "") + '>' + option + "</option>";
            if (data === option) {
                found = 1;
            }
        }
        if (found === 0) {
            content += '<option selected>' + data + '</option>';
        }
        content += '</select>';
        return content;
    };

    if (dropdown !== null)
        publicMembers.dropdownValues = dropdown;

    return publicMembers;
};

var ajaxDropdownColumn = function(url, contentValue) {
    var publicMembers = dropdownColumn(url, contentValue);
    var superEditHtml = publicMembers.editHTML;

    publicMembers.editHTML = function(data) {
        // load ajax content into dropdown array
        publicMembers.dropdownValues = [''];

        $.ajax({
            async: false,
            url: url,
            dataType: "json",
            success: function(ajaxdata) {
                $.each(ajaxdata, function(index, item) { publicMembers.dropdownValues.push(item); });
            }
        });

        return superEditHtml(data);
    };

    return publicMembers;
};

var pickerColumn = function(pickerObject, contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.stopUpdate = pickerObject.removeUpdate;
    publicMembers.isPicker = true;

    publicMembers.extractValue = function(td) {
        return td.children("span")[0].textContent;
    };

    publicMembers.editHTML = function(data) {
        var str = "<a href=\"\">" + pickerObject.linkText + "</a>" +
            "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";

        return str;
    };

    publicMembers.init = function(cell) {
        $(cell).children("a").click(function(e) {
            e.preventDefault();
            var currentValue = $(cell).children("span")[0].innerHTML;
            pickerObject.show(currentValue);

            pickerObject.externalValueSetter = function(value) {
                $(cell).children("span")[0].innerHTML = value;
            };

        });
    };

    return publicMembers;
};

var pickerEditTextColumn = function(pickerObject, contentValue) {
    var publicMembers = abstractColumn(contentValue);
    publicMembers.isPicker = true;
    publicMembers.stopUpdate = pickerObject.removeUpdate;

    publicMembers.extractValue = function(td) {
        return td.children("span")[0].textContent;
    };

    publicMembers.editHTML = function(data) {
        var d = data.split('for');
        var time = d[1] === undefined ? 0 : d[1];
        var str = "<a href=\"\">" + pickerObject.linkText + "</a>" +
            "<span id='pickerSpan' style='margin-left: 8px;'>" + d[0] + "</span><br/>" +
            '<input type="text" size=7 value="' + time + '">' +
            "<span style='margin-left: 8px;'>time in seconds</span>";

        return str;
    };
    publicMembers.init = function(cell) {
        $(cell).children("a").click(function(e) {
            e.preventDefault();
            var currentValue = $(cell).children("span")[0].innerHTML;
            pickerObject.show(currentValue);

            pickerObject.externalValueSetter = function(value) {
                $(cell).children("span")[0].innerHTML = value;
            };

        });
    };
    return publicMembers;
};