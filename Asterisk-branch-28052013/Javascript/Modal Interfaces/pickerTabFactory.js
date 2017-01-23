var addTabAbstract = function(title) {
    var pub = {};
    pub.uniqueTabId = utilities.getNextUniqueID("NPtab");
    pub.currentTab = $('<div id="' + pub.uniqueTabId + '">');

    // virtual method button click
    pub.click = function() {
    };

    pub.addTabForThisTable = function(picker) {
        picker.newdiv.find('#tabs').find('ul').append($('<li>').append($('<a>').prop('href', '#' + pub.uniqueTabId).html(title)));

        pub.currentTab.find('input.button').click(function(e) {
            e.preventDefault();
            pub.click(picker);
        });

    };
    return pub;
};

var addRemoveTab = function() {

    var pub = addTabAbstract('Remove Value');

    pub.addMe = function(picker) {

        var html = "<div><p>To remove the value from the table press the button.</p></div><input type=\"button\" value=\"Remove Value\"  class=\"button tabButton\" onclick=\"  \" />";

        picker.newdiv.find('#tabs').append(pub.currentTab);
        pub.currentTab.append($(html));
        pub.addTabForThisTable(picker);
    };

    pub.click = function(picker) {
        picker.setValueInDataTable('');
    };

    return pub;
};

var addStaticTab = function(title) {

    var pub = addTabAbstract(title);

    //TODO:the ok button should only say ok if the value changes....

    pub.html = "<input type=\"button\" value=\"Ok\"  class=\"button tabButton\" disabled='disabled' onclick=\"  \" />";

    pub.addMe = function(picker) {
        picker.newdiv.find('#tabs').append(pub.currentTab);
        pub.currentTab.append($(pub.html));
        pub.addTabForThisTable(picker);
    };

    return pub;
};

var addStaticTabWithTextInput = function(title) {

    var pub = addStaticTab(title);
    var uniqueValueId = utilities.getNextUniqueID("VAL");

    pub.html = "<div><p>Type a number in the textbox provided.</p><label >Custom:</label><input type=\"text\" " +
        " id=\"" + uniqueValueId + "\" class=\"valueCustom\" onkeypress=\"$('#" + pub.uniqueTabId + "').find('input.button').removeAttr('disabled')\" /></div>" + pub.html;

    pub.makeValue = function(id) { return $('#' + id).val(); };
    pub.click = function(picker) {
        picker.setValueInDataTable(pub.makeValue(uniqueValueId));
    };

    return pub;
};

var addAjaxTabWithRadio = function(title, jsonLink, columns, heldValue) {

    var pub = addTabAbstract(title);

    pub.addMe = function(picker) {

        var fillTableWithJsonData = function() {
            var uniqueId = utilities.getNextUniqueID("ANP");
            var table = pub.currentTab.find('table').dataTable();

            $.getJSON(jsonLink, function(data) {
                $.each(data.aoData, function(lineindex, lineitem) {

                    if (columns[0][1](lineitem) === heldValue) {
                        table.fnAddData(
                            ["<input type=\"radio\" name='" + uniqueId + "' value=\"" + columns[0][1](lineitem) + "\" checked=\"checked\"  onclick=\"$('#" + pub.uniqueTabId + "').find('input.button').removeAttr('disabled')\" />"]
                                .concat(
                                    columns.slice(1).map(function(a) { return a[1](lineitem); })
                                )
                        );
                    } else {
                        table.fnAddData(
                            ["<input type=\"radio\" name='" + uniqueId + "' value=\"" + columns[0][1](lineitem) + "\"  onclick=\"$('#" + pub.uniqueTabId + "').find('input.button').removeAttr('disabled')\" />"]
                                .concat(
                                    columns.slice(1).map(function(a) { return a[1](lineitem); })
                                )
                        );
                    }
                });
            });
        };

        var createTable = function() {
            var theadRow = $('<tr>');
            $.each(columns, function(index, item) { theadRow.append($('<th>').html(item[0])); });
            var uniqueTableId = utilities.getNextUniqueID("ANPTab");

            pub.currentTab.append($('<div>').addClass('tabTableText tabTableSetButtonPosition')
                .append($('<table>').attr('id', uniqueTableId).addClass('dataTable')
                    .append($('<thead>')
                        .append(theadRow)
                    )));
        };

        picker.newdiv.find('#tabs').append(pub.currentTab);

        createTable();

        pub.currentTab.append($("<input type='button' value='Ok' name='setNumber' class='button' disabled='disabled' />"));

        fillTableWithJsonData();

        pub.addTabForThisTable(picker);

    };

    pub.click = function(picker) {
        picker.setValueInDataTable($('#' + pub.uniqueTabId).find("input[type=\"radio\"]:checked").val());
    };

    return pub;

};


var addAjaxTabWithCheckBox = function(title, jsonLink, columns, heldValue) {

    var pub = addTabAbstract(title);

    pub.addMe = function(picker) {

        var uniqueId = utilities.getNextUniqueID("CHX");

        var fillTableWithJSONData = function() {

            var table = pub.currentTab.find('table').dataTable();

            $.getJSON(jsonLink, function(data) {
                $.each(data.aoData, function(lineindex, lineitem) {
                    // alert(heldValue);
                    if (columns[0][1](lineitem) === heldValue) {
                        // alert(columns[0][1](lineitem));
                        table.fnAddData(
                            ["<input type=\"checkbox\" name='" + uniqueId + "' value=\"" + columns[0][1](lineitem) + "\" checked onclick=\"$('#" + pub.uniqueTabId + "').find('input.button').removeAttr('disabled')\" />"]
                                .concat(
                                    columns.slice(1).map(function(a) { return a[1](lineitem); })
                                ));
                    } else {
                        table.fnAddData(
                            ["<input type=\"checkbox\" name='" + uniqueId + "' value=\"" + columns[0][1](lineitem) + "\" onclick=\"$('#" + pub.uniqueTabId + "').find('input.button').removeAttr('disabled')\" />"]
                                .concat(
                                    columns.slice(1).map(function(a) { return a[1](lineitem); })
                                )
                        );
                    }
                });
            });
        };

        var createTable = function() {
            var theadRow = $('<tr>');
            $.each(columns, function(index, item) { theadRow.append($('<th>').html(item[0])); });
            var uniqueTableId = utilities.getNextUniqueID("ANPTab");

            pub.currentTab.append($('<div>').addClass('tabTableText tabTableSetButtonPosition')
                .append($('<table>').attr('id', uniqueTableId).addClass('dataTable')
                    .append($('<thead>')
                        .append(theadRow)
                    )));
        };

        picker.newdiv.find('#tabs').append(pub.currentTab);

        createTable();

        pub.currentTab.append($("<input type='button' value='Ok' name='setNumber' class='button' disabled='disabled' />"));

        fillTableWithJSONData();

        pub.addTabForThisTable(picker);

    };

    pub.click = function(picker) {
        var rtn = '';

        $('#' + pub.uniqueTabId).find("input[type=\"checkbox\"]:checked").each(function() {
            rtn = (rtn === '') ? rtn = $(this).val() : rtn = rtn + ',' + $(this).val();
        });

        picker.setValueInDataTable(rtn);
    };

    return pub;

};