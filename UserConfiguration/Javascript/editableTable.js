function editableTable(config) {

  var tableId = config.table.attr("id");
  addEditColumn(config);
  addDeleteColumn(config);

  var dtColSetup = [];
  $.each(config.template, function () { dtColSetup.push(null); });
  dtColSetup.push({ "bSortable": false });
  dtColSetup.push({ "bSortable": false });
  config.datatable = config.table.dataTable({
    "bPaginate": false,
    "bLengthChange": false,
    "iDisplayLength": 100,
    "aoColumns": dtColSetup
  });

  // add the add item link
  var addLinkId = tableId + "_addItem";
  $('#' + tableId + '_filter').before("<a class='button editableTableAdd' id='" + addLinkId + "' href=''>Add Item</a>");
  $("#" + addLinkId).live("click", function (e) {
    e.preventDefault();
    addNewRow(config);
    editRow(config, $('#newrow'));
  });

  // add status bar
  $('#' + tableId + '_wrapper').after("<div class='editableTableStatus' id='" + tableId + "_statusBar'></div>");

  return {
    setColumnVisibility: function (column, vis) {
      config.datatable.fnSetColumnVis(column, vis);
    },

    setnumberPickerValue: function (value) {
      var a = $("span", config.numberPickerCell);
      a[0].innerHTML = value;
    },

    setDayValue: function (value) {
      var a = $("span", config.dayPickerCell);
      a[0].innerHTML = value;
    },

    setTimeValue: function (value) {
      var a = $("span", config.timePickerCell);
      a[0].innerHTML = value;
    },

    setMultiDateValue: function (value) {
      var a = $("span", config.MultiDateCell);
      a[0].innerHTML = value;
    }

  };

}



function addNewRow(config) {
  var dataCells = [];
  $.each(config.template, function () { dataCells.push(''); });
  var commands;

  if (!config.cannotUpdate) {
    commands = ['<a class="edit" href="">Edit</a>', '<a class="delete" href="">Delete</a>'];
  } else {
    commands = ['<a class="edit" href=""></a>', '<a class="delete" href="">Delete</a>'];
  }

  var newRow = config.datatable.fnAddData(dataCells.concat(commands));
  var row = config.datatable.fnGetNodes(newRow[0]);
  $(row).attr("id", "newrow");

  // add classes to each new td
  var to = $(row).children("td");
  var i;
  for (i = 0; i < config.template.length; i++) {
    $(to[i]).addClass(config.template[i]);
  }
}

function addEditColumn(config) {
  if (!config.cannotUpdate) {
    addColumnToTable(config.table, "<a href='' class='edit'>Edit</a>");
  } else {
    addColumnToTable(config.table, "<a href='' class='edit'></a>");
  }

  // add click events to each edit link
  //config.table.find("a.edit").each(function () {
  $("a.edit", config.table).live("click", function (e) {
    e.preventDefault();
    if (this.innerHTML == "Edit") {
      editRow(config, $(this).parents('tr'));
    } else {
      saveRow(config, $(this).parents('tr'));
    }
  });
}

function addDeleteColumn(config) {
  addColumnToTable(config.table, "<a href='' class='delete'>Delete</a>");
  // add click events to each edit link
  $("a.delete", config.table).live("click", function (e) {
    e.preventDefault();
    if (this.innerHTML == "Delete") {
      deleteRow(config, $(this).parents('tr'));
    } else {
      cancelRowEdit(config, $(this).parents('tr'));
    }
  });
}

function cancelRowEdit(config, row) {
  if (row.attr("id") == "newrow") {
    var thisrow = row[0];
    config.datatable.fnDeleteRow(thisrow);
    enableEdits(config);
  } else {
    unEditRow(config, row, "hidden");
    enableEdits(config);
    // change "Cancel" link to "Delete"
    row.find("a.delete")[0].innerHTML = 'Delete';
  }
}

function deleteRow(config, row) {
  var r = confirm('Are you sure you want to delete this item?');
  if (r) {
    statusLoad(config, config.deleteURL + row.attr("id"));
    var thisrow = row[0];
    config.datatable.fnDeleteRow(thisrow);
  }
  
}

function saveRow(config, row) {
  unEditRow(config, row, "new");

  var inputValues = $('td', row).map(function () { return this.innerHTML; });
  var id = row.attr("id");
  var saveString = "?" + config.getString(id, inputValues);

  if (id === "newrow") {
    $.ajax({
      url: config.addURL + saveString,
      success: function (data) {
        if (data != -1) {
          row.attr("id", data);
          addLinksToNewRow(config, row);

          statusDisplay(config, "Added");

          // change "Cancel" link to "Delete"....this is now in the method below
          enableEdits(config);

        } else {
          statusDisplay(config, "Add failed");
          editRow(config, row);
          alert("Add Failed. Check that you are not creating a duplicate entry.");
        }
      }
    });
  } else {
    statusLoad(config, config.updateURL + saveString);
    enableEdits(config);
    // change "Cancel" link to "Delete"
    row.find("a.delete")[0].innerHTML = 'Delete';

  }

}

function addLinksToNewRow(config, row) {
  var tds = $("td", row);
  var i;
  var id = row.attr("id");
  var linkIndex = 0;
  for (i = 0; i < config.template.length; i++) {
    if (config.template[i] == 'link') {
      tds[i].innerHTML = config.links[linkIndex](id);
      linkIndex++;
    }
  }
}

function statusLoad(config, url) {
  var tableId = config.table.attr("id");
  $("#" + tableId + "_statusBar").load(url);
}

function statusDisplay(config, content) {
  var tableId = config.table.attr("id");
  $("#" + tableId + "_statusBar")[0].innerHTML = content;
}

function editRow(config, row) {
  // change each td to contain a textbox
  row.children("td.editableText").each(function () {
    var data = this.innerHTML;
    this.innerHTML = '<input type="text" size=10 value="' + data + '">';
    this.innerHTML += '<input type="hidden" value="' + data + '">';
  });

  row.children("td.numberPicker").each(function () {
    config.numberPickerCell = this;

    var data = this.innerHTML;
    this.innerHTML = "<a id='numberPicker' href=''>Number Picker</a>";
    this.innerHTML += "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";
    this.innerHTML += '<input type="hidden" value="' + data + '">';
    $("#numberPicker").on("click", function (e) {
      e.preventDefault();
      $("#addNumber").show('fast');
    });
  });

  row.children("td.timePicker").each(function () {
    config.timePickerCell = this;

    var data = this.innerHTML;
    this.innerHTML = "<a id='timePicker' href=''>Time Picker</a>";
    this.innerHTML += "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";
    this.innerHTML += '<input type="hidden" id="hiddenTime" value="' + data + '">';
    $("#timePicker").on("click", function (e) {
      e.preventDefault();
      $("#addTime").show('fast');
      var separatedList = $('#hiddenTime').val();
      var list = separatedList.split('-');
      for (var t in list) {
        if (t == 0) {
          $('#timeFrom').val(list[t]);
        } else {
          $('#timeTo').val(list[t]);
        }
      }
    });
  });

  row.children("td.dayPicker").each(function () {
    config.dayPickerCell = this;

    var data = this.innerHTML;
    this.innerHTML = "<a id='dayPicker' href=''>Day Picker</a>";
    this.innerHTML += "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";
    this.innerHTML += '<input type="hidden" id="hiddenDate" value="' + data + '">';
    $("#dayPicker").on("click", function (e) {
      e.preventDefault();
      $("#addDays").show('fast');
      var heldData = $('#hiddenDate').val();
      setDayCheckBox(heldData, '#valueDaysMon', 'mon');
      setDayCheckBox(heldData, '#valueDaysTue', 'tue');
      setDayCheckBox(heldData, '#valueDaysWed', 'wed');
      setDayCheckBox(heldData, '#valueDaysThur', 'thur');
      setDayCheckBox(heldData, '#valueDaysFri', 'fri');
      setDayCheckBox(heldData, '#valueDaysSat', 'sat');
      setDayCheckBox(heldData, '#valueDaysSun', 'sun');
    });
  });

  row.children("td.editableDatePicker").each(function () {
    config.MultiDateCell = this;

    var data = this.innerHTML;
    this.innerHTML = "<a id='multiDatePicker' href=''>Date Picker</a>";
    this.innerHTML += "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";
    this.innerHTML += '<input type="hidden"  value="' + data + '">';
    $("#multiDatePicker").on("click", function (e) {
      e.preventDefault();
      $("#addFromMultiDate").show('fast');
    });
  });

  row.children("td.checkBox").each(function () {
    var data = this.innerHTML;
    var checked = data == "yes" ? "checked" : "";
    this.innerHTML = '<input type="checkbox" ' + checked + '>';
    this.innerHTML += '<input type="hidden" value="' + data + '">';
  });

  var dropdownIndex = 0;
  row.children("td.editableDropdown").each(function () {
    var data = this.innerHTML;
    var content = "";
    content = '<select>';
    var i;
    var found = 0;
    for (i = 0; i < config.dropdowns[dropdownIndex].length; i++) {
      var option = config.dropdowns[dropdownIndex][i];
      content += '<option' + (data == option ? " selected" : "") + '>' + option + "</option>";
      if (data == option) {
        found = 1;
      }
    }
    if (found == 0) {
      content += '<option selected>' + data + '</option>';
    }
    dropdownIndex++;

    content += '</select>';
    this.innerHTML = content;
    this.innerHTML += '<input type="hidden" value="' + data + '">';
  });

  //set the checkboxs on the day form based on the current held values
  function setDayCheckBox(commaSeparatedList, checkbox, day) {
    if (isInList(commaSeparatedList, ",", day) == 1) {
      $(checkbox).attr('checked', true);
    } else {
      $(checkbox).attr('checked', false);
    }
  }


  function isInList(separatedList, listDelimiter, valueToFind) {
    var list = separatedList.split(listDelimiter);
    var rtn = 0;
    for (var d in list) {
      if (list[d] == valueToFind) {
        rtn = 1;
      }
    }
    return rtn;
  }

  // change "Edit" link to "Save"
  row.find("a.edit")[0].innerHTML = 'Save';

  // change "Delete" link to "Cancel"
  row.find("a.delete")[0].innerHTML = 'Cancel';

  disableEdits(config.table);
}

function unEditRow(config, row, type) {
  if (type == "new") {
    row.children("td.editableText").each(function () {
      var val = $(this).children("input[type='text']")[0].value;
      this.innerHTML = val;
    });

    row.children("td.numberPicker").each(function () {
      $("#numberPicker").off();
      var val = $(this).children("span")[0].textContent;
      this.innerHTML = val;
    });
    row.children("td.timePicker").each(function () {
      $("#timePicker").off();
      var val = $(this).children("span")[0].textContent;
      this.innerHTML = val;
    });
    row.children("td.dayPicker").each(function () {
      $("#dayPicker").off();
      var val = $(this).children("span")[0].textContent;
      this.innerHTML = val;
    });
    row.children("td.editableDropdown").each(function () {
      var val = $(this).children("select")[0].value;
      this.innerHTML = val;
    });
    row.children("td.checkBox").each(function () {
      if ($(this).children("input[type='checkbox']").prop("checked")) {
        this.innerHTML = "yes";
      } else {
        this.innerHTML = "no";
      }
    });
    row.children("td.editableDatePicker").each(function () {
      var val = $(this).children("span")[0].textContent;
      this.innerHTML = val;
    });
  } else {
    var inputValues = $('input[type="' + type + '"]', row).map(function () { return this.value; });
    row.children("td.editableText").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.numberPicker").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.timePicker").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.dayPicker").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.editableDropdown").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.checkBox").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.editableDatePicker").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.editableDate").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
    row.children("td.editableDateDouble").each(function () {
      var val = $(this).children("input[type='hidden']")[0].value;
      this.innerHTML = val;
    });
  }
}

function disableEdits(table) {
  table.find("a.edit").each(function () {
    if (this.innerHTML == 'Edit') {
      this.innerHTML = '';
    }
  });

  var tableId = table.attr("id");
  $("#" + tableId + "_addItem").hide();
}

function enableEdits(config) {
  config.table.find("a.edit").each(function () {
    if (!config.cannotUpdate) {
      this.innerHTML = 'Edit';
    } else {
      this.innerHTML = '';
    }
  });
  config.table.find("a.delete").each(function () {
    this.innerHTML = 'Delete';
  });
  var tableId = config.table.attr("id");
  $("#" + tableId + "_addItem").show();
}

function addColumnToTable(table, columnHtml) {
  // add an edit item header to thead
  table.find("thead:first").children("tr").append("<th></th>");
  // add an edit item link to each row
  table.find("tbody:first").children("tr").append("<td>" + columnHtml + "</td>");
}
