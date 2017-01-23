var dynamicTable = function (config) {

  //#region ** Class Variables **

  var tableId = config.table.attr("id");

  //#endregion ** Class Variables **

  var datatable = config.table.dataTable({
    "bPaginate": false,
    "bLengthChange": false,
    "iDisplayLength": 100,
    "aaSorting": [[1, "asc"]],
    "bProcessing": true,
    "sAjaxSource": config.source,
    "fnServerData": function (sSource, aaData, fnCallback) {
      $.ajax({
        "dataType": 'json',
        "type": "POST",
        "url": sSource,
        "data": aaData,
        "success": function (json) {
          // process each line 
          $.each(json.aaData, function (lineindex, lineitem) {
            $.each(lineitem, function (index, item) {
              if (privateMembers.columnType(index).transform) {
                //the transform function being called uses the value held in the json call for that column(in this case the id)
                //look at the controller to see what is being passed in
                json.aaData[lineindex][index] = privateMembers.columnType(index).transform(item, json.aaData[lineindex][0]);
              }
            });
          });

          fnCallback(json);
          privateMembers.setCrudUrls();
          privateMembers.addLinkColumns();
          privateMembers.hideExtraColumns(config.hidenColumns);

          // hide first column
          privateMembers.hideId();
         
        }
      });
    }
  });

  var tableDecorations = {
    updateStatus: function (text) {
      $("#" + tableId + "_statusBar")[0].innerHTML = text;
    },
    hideAddLink: function () {
      $("#" + addLinkId).hide();
    },
    showAddLink: function () {
      $("#" + addLinkId).show();
    }
  };

  // add status bar
  $('#' + tableId + '_wrapper').after("<div class='editableTableStatus' id='" + tableId + "_statusBar'></div>");

  //#region ** Public Objects **

  var publicMembers = {
    setColumnVisibility: function (column, vis) {
      datatable.fnSetColumnVis(column, vis);
    },
    refreshDataTable: function () {
      datatable.fnReloadAjax();
    }
  };

  //#endregion ** Public Objects **

  //#region ** Private Objects **

  var privateMembers = {
    addColumn: function () {
      config.table.find("thead:first").children("tr").append("<th></th>");
    },

    changeColumn: function (columnNumber, content) {
      // find last column
      var columns = config.table.find("tbody:first").children("tr:first").children("td").length;
      var rows = config.table.find("tbody:first").children("tr");
      rows.each(function () {
        var tds = $(this).children("td");
        tds[columns - columnNumber].innerHTML = content;
      });
    },

    hideShowColumn: function (column, action) {
      var columns = config.table.find("tbody:first").children("tr:first").children("td").length;
      var rows = config.table.find("tbody:first").children("tr");
      rows.each(function () {
        var tds = $(this).children("td");
        if (action === 'hide') {
          $(tds[columns - column]).hide();
        } else if (action === 'show') {
          $(tds[columns - column]).show();
        }
      });
      var headColumns = config.table.find("thead:first").children("tr:first").children("th").length;
      var headRows = config.table.find("thead:first").children("tr");
      headRows.each(function () {
        var ths = $(this).children("th");
        if (action === 'hide') {
          $(ths[headColumns - column]).hide();
        } else if (action === 'show') {
          $(ths[headColumns - column]).show();
        }
      });
    },

    hideId: function () {
      config.table.find("tbody:first").children("tr").children("td:first-child").hide();
    },

    deleteRow: function (row) {
      if (confirm('Are you sure you want to delete this item?')) {
        privateMembers.disableEditing();
        var id = $(row).children("td")[0].innerHTML;
        $.ajax({
          url: config.deleteURL + id,
          success: function (data) {
            if (data) {
              tableDecorations.updateStatus(data);
              // refresh table data
              datatable.fnReloadAjax();
            } else {
              alert("Couldn't delete record");
              privateMembers.enableEditing();
            }
          }
        });

      }
    },

    editRow: function (row) {
      var tds = row.children("td");
      for (var c = 0; c < tds.length; c++) {
        var td = tds[c];

        var isNewRow = ($(row).children("td:first-child")[0].innerHTML == "");

        if (privateMembers.columnType(c).editable && !(!isNewRow && privateMembers.columnType(c).stopUpdate)) {

          var data = td.innerHTML;
          var allContent = data;

          data = data.replace(/<.*>/, '');

          td.innerHTML = privateMembers.columnType(c).editHTML(data) + '<input type="hidden" value="' + encodeURI(allContent) + '">';

          if (privateMembers.columnType(c).init) {
            privateMembers.columnType(c).init(td);
          }
        }
      }
      privateMembers.disableEditing();
      $(row).children("td").children("a.edit")[0].innerHTML = 'Save';
      $(row).children("td").children("a.delete")[0].innerHTML = 'Cancel';
    },

    columnType: function (columnIndex) {
      return config.columnTypes[columnIndex];
    },

    cancelRowEdit: function (row) {
      var tds = row.children("td");
      for (var i = 0; i < tds.length; i++) {
        var td = tds[i];
        var isNewRow = ($(row).children("td:first-child")[0].innerHTML == "");

        if (privateMembers.columnType(i).editable && !(!isNewRow && privateMembers.columnType(i).stopUpdate)) {
          td.innerHTML = decodeURI($(td).children("input[type='hidden']")[0].value);
        }
      }

      $(row).children("td").children("a.delete")[0].innerHTML = 'Delete';
      privateMembers.enableEditing();

      //hide edits columns if required
      $.each(config.columnTypes, function (index, element) {
        if (element.isInvisible) {
          privateMembers.hideShowColumn(2, 'hide');
        }
      });
    },

    disableEditing: function () {
      config.table.find("tbody:first").children("tr").children("td").children("a.edit").text("");
      config.table.find("tbody:first").children("tr").children("td").children("a.delete").text("");
      config.table.find("tbody:first").children("tr").children("td").children("a.defaultVoiceMail").text("");
    },

    enableEditing: function () {
      config.table.find("tbody:first").children("tr").children("td").children("a.edit").text("Edit");
      config.table.find("tbody:first").children("tr").children("td").children("a.delete").text("Delete");
      config.table.find("tbody:first").children("tr").children("td").children("a.defaultVoiceMail").text("Add Default");
    },

    saveRow: function (row, command) {
      var tds = row.children("td");
      var columnValues = [];
      for (var i = 0; i < tds.length; i++) {
        var td = tds[i];
        var colType = privateMembers.columnType(i);

        if (colType.useforsave) {
          if (!colType.stopUpdate || row.children("td:first-child")[0].innerHTML == "") {
            var val = colType.extractValue($(td));
            if (!colType.validator || (colType.validator && colType.validator(val))) {
              columnValues[i] = val;
            } else {
              $(td).children().addClass('hasError');
              alert("Some of the data is not valid please check and re-submit.");
              return false;
            }
          } else {
            columnValues[i] = td.innerHTML;
          }
        }
      }
      var saveString = command + "?" + config.getString(columnValues);
      $.ajax({
        url: saveString,
        success: function (data) {
          if (data) {
            tableDecorations.updateStatus(data);
            // refresh table data
            datatable.fnReloadAjax();
            tableDecorations.showAddLink();
          } else {
            alert("Couldn't save record");
          }
        }
      });
    },


    addNewRow: function () {
      var dataCells = [];
      $.each(config.columnTypes, function (index, element) {
        if (element.isInvisible) {
          privateMembers.hideShowColumn(2, 'show');
        }
        if (element === '' || element === null) {
          dataCells.push('');
        } else {
          dataCells.push(this.content);
        }
      });
      var newRow = datatable.fnAddData(dataCells);
      tableDecorations.hideAddLink();
      privateMembers.hideId();
      return datatable.fnGetNodes(newRow[0]);
    },

    addLinkColumns: function () {
      $.each(config.columnTypes, function (index, element) {
        if (element.isLink) {
          privateMembers.changeColumn(element.thisIndex, element.content);
          if (element.isInvisible) {
            privateMembers.hideShowColumn(element.thisIndex, 'hide');
          }
        }
      });
    },

    hideExtraColumns: function (columns) {
     for (var i in columns) {
        privateMembers.hideShowColumn(columns[i], 'hide');
      }
    },

    showExtraHidenColumns: function (columns) {
      for (var i in columns) {
        privateMembers.hideShowColumn(columns[i], 'show');
      }
    },

    setCrudUrls: function () {
      if (!config.addURL) {
        config.addURL = "Add";
      }
      if (!config.updateURL) {
        config.updateURL = "Update";
      }
      if (!config.deleteURL) {
        config.deleteURL = "Delete?id=";
      }
    }


  };

  //#endregion ** Private Objects **

  //#region ** Added Handlers **

  // add the add item link
  var addLinkId = tableId + "_addItem";
  $('#' + tableId + '_filter').before("<a class='button editableTableAdd' id='" + addLinkId + "' href=''>Add Item</a>");
  $("#" + addLinkId).live("click", function (e) {
    e.preventDefault();
    var newrow = privateMembers.addNewRow();
    privateMembers.editRow($(newrow));
  });

  // Set up handlers for row links
  $("a.delete", config.table).live("click", function (e) {
    e.preventDefault();
    if (this.innerHTML == "Delete") {
      privateMembers.deleteRow($(this).parents('tr'));
    } else if (this.innerHTML == "Cancel") {
      privateMembers.cancelRowEdit($(this).parents('tr'));
      tableDecorations.showAddLink();
      if ($(this).parents('tr').children("td:first-child")[0].innerHTML == "") {
        datatable.fnDeleteRow($(this).parents('tr')[0]);
        tableDecorations.showAddLink();
      }
    }
  });


  // Set up handlers for row links
  $("a.edit", config.table).live("click", function (e) {
    e.preventDefault();
    if (this.innerHTML == "Edit") {
      privateMembers.editRow($(this).parents('tr'));
      tableDecorations.hideAddLink();
    } else if (this.innerHTML == "Save") {
      if ($(this).parents('tr').children("td:first-child")[0].innerHTML == "") {
        privateMembers.saveRow($(this).parents('tr'), config.addURL);
      } else {
        privateMembers.saveRow($(this).parents('tr'), config.updateURL);
      }
    }
  });

  //#endregion ** Added Handlers **

  return publicMembers;
};
