var queueMember = function(config) {

    var nEditing = null;

    var queueMemberTable = config.table.dataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "iDisplayLength": 100,
        "aoColumns": config.columns
    });

    var publicMembers = {
        removeExtensions: function() {
            if ($("#QueueMs :checkbox[name='queueMemeberId'][checked]").length > 0) {
                var r = confirm('Are you sure you want to remove these extensions?');
                if (r) {
                    $('#QueueMs').attr('action', config.deleteUrl);
                    $('#QueueMs').submit();
                }
            } else {
                alert("you have not made a valid selection.");
            }
        }
    };

    var privateMembers = {
        initialise: function() {
            privateMembers.setCustomButtons();
        },

        setCustomButtons: function() {
            $('#queueMembers_filter').before('<input type="button" value="Remove Checked"' +
                'name="removeExtensions" onclick="' + config.tableVariableName + '.removeExtensions()" class="removeButton editableTableAdd"' +
                ' /><a id="tableUpdateStatus" class="positionLeft" style="margin-top: 10px; margin-left: 12px;">');

            $('#membersAddQueue_filter').before('<input type="button" value="Add Queues" name="addEQueue"' +
                ' onclick="' + config.addQueueName + '.addToQueue()" class="button editableTableAdd" />');

            $('#membersAddExtension_filter').before('<input type="button" value="Add Extensions" name="addExtensions" ' +
                'onclick="' + config.addExtensionName + '.addToQueue()" class="button editableTableAdd" />');
        },

        editRow: function(nRow) {
            var aData = queueMemberTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);
            jqTds[5].innerHTML = '<input type="text" value="' + aData[5] + '">';
            jqTds[6].innerHTML = '<input type="checkbox" id="editCheck" >';
            jqTds[7].innerHTML = '<a class="edit" href="">Save</a>';
            if (aData[6] == 'yes') {
                $('#editCheck').attr('checked', true);
            }
        },

        setRowBackToText: function(jqInputs, nRow) {
            var val = 'no';
            if ($('#editCheck:checked').val() != undefined) {
                val = 'yes';
            }
            queueMemberTable.fnUpdate(jqInputs[1].value, nRow, 5, false);
            queueMemberTable.fnUpdate(val, nRow, 6, false);
            queueMemberTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 7, false);
            queueMemberTable.fnDraw();
        }
    };

    // Set up event Handlers :

    $("a.edit", config.table).live("click", function(e) {
        e.preventDefault();

        /* Get the row as a parent of the link that was clicked on */
        var nRow = $(this).parents('tr')[0];

        if (nEditing != null && nEditing != nRow) {
            /* A different row is being edited - the edit should be cancelled and this row edited */
            window.restoreRow(queueMemberTable, nEditing);
            privateMembers.editRow(nRow);
            nEditing = nRow;
        } else if (nEditing == nRow && this.innerHTML == "Save") {
            /* This row is being edited and should be saved */
            var jqInputs = $('input', nRow);
            var val = 'no';
            if ($('#editCheck:checked').val() != undefined) {
                val = 'yes';
            }

            $('#tableUpdateStatus').load("/QueueMembers/UpdateQueueMember?queueNumber=" + config.queueNumber +
                "&queueMemeberId=" + $(nRow).attr("id") + "&penalty=" + encodeURI(jqInputs[1].value) + "&paused=" + val);

            privateMembers.setRowBackToText(jqInputs, nRow);
            nEditing = null;

        } else {
            /* No row currently being edited */
            privateMembers.editRow(nRow);
            nEditing = nRow;
        }
    });

    $("#showPenalty").click(function(e) {
        $("#penaltyInfo").show('fast');
        e.preventDefault();
    });
    $(".modalClose").click(function(e) {
        $("#penaltyInfo").hide('fast');
        e.preventDefault();
    });
    $("#showPaused").click(function(e) {
        $("#pausedInfo").show('fast');
        e.preventDefault();
    });
    $(".modalClose").click(function(e) {
        $("#pausedInfo").hide('fast');
        e.preventDefault();
    });

    privateMembers.initialise();

    return publicMembers;


};