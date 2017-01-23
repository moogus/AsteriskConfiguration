
var editColumn = function () {
    var publicMembers = {
        content: "<a href='' class='edit'>Edit</a>",
        editable: false,
        useforsave: false,
        isLink:true,
        thisIndex: '2'
    };
    return publicMembers;
};

var editColumnNotVisable = function () {
    var publicMembers = {
        content: "<a href='' class='edit'></a>",
        editable: false,
        useforsave: false,
        isLink: true,
        isInvisible: true,
        thisIndex: '2'
    };
    return publicMembers;
};

var deleteColumn = function () {
    var publicMembers = {
        content: "<a href='' class='delete'>Delete</a>",
        editable: false,
        useforsave: false,
        isLink: true,
//        isDelete: true,
        thisIndex: '1'
    };
    return publicMembers;
  };

  var editTextColumn = function (contentValue) {
    var publicMembers = {
      content: "",
      editable: true,
      useforsave: true,
      extractValue: function (td) {
        return td.children("input[type='text']")[0].value;
      },
      editHTML: function (data) {
        var str = '<input type="text" size=10 value="' + data + '">';
        return str;
      }
    };
    if (contentValue!==null)
      publicMembers.content = contentValue;
    return publicMembers;
  };

  var editTextNoEditColumn = function () {
    var publicMembers = {
      content: "",
      editable: true,
      useforsave: true,
      stopUpdate: true,
      extractValue: function (td) {
        return td.children("input[type='text']")[0].value;
      },
      editHTML: function (data) {
        var str = '<input type="text" size=10 value="' + data + '">';
        return str;
      }
    };
    return publicMembers;
  };


  //to be used in conjucntion with the hidenColumns[] in the table 
  //the array is the inverse index eg edit column is 2
  var hidenValueColumn = function (contentValue) {
    var publicMembers = {
      content: "",
      editable: true,
      useforsave: true,
      extractValue: function (td) {
        return td.children("input[type='hidden']")[0].value;
      },
      editHTML: function (data) {
      return "";
      }
  };
  if (contentValue)
    publicMembers.content = contentValue;
    return publicMembers;
  };

  var nonEditColumn = function (contentValue) {
    var publicMembers = {
      content: "",
        editable: false,
        useforsave: false
      };
      if (contentValue)
        publicMembers.content = contentValue;
    return publicMembers;
};

var linkColumn = function (link) {
    var publicMembers = {
        content: "",
        editable: false,
        useforsave: false,
        isEdit: true,
        transform: link,
        thisIndex: '3'
    };
    return publicMembers;
};

var idColumn = function () {
    var publicMembers = {
        content: "",
        editable: false,
        useforsave: true,
        extractValue: function (td) {
            return td[0].innerHTML;
        },
        hidden: true
    };
    return publicMembers;
};
var dropdownColumn = function (dropdown, contentValue) {
  var publicMembers = {
    content: "",
    dropdownValues: [],
    editable: true,
    useforsave: true,
    extractValue: function (td) {
      return td.children("select")[0].value;
    },
    editHTML: function (data) {
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
    }
  };
  if (contentValue)
    publicMembers.content = contentValue;
  if (dropdown !== null)
    publicMembers.dropdownValues = dropdown;
  return publicMembers;
};
var ajaxDropdownColumn = function (url, contentValue) {
  var publicMembers = {
    content: "",
    editable: true,
    useforsave: true,
    transform: false,
    extractValue: function (td) {
      return td.children("select")[0].value;
    },
    editHTML: function (data) {
      // load ajax content into dropdown array
      var dropdown = [''];
      $.ajax({
        async: false,
        url: url,
        dataType: "json",
        success: function (ajaxdata) {
          $.each(ajaxdata, function (index, item) { dropdown.push(item); });
        }
      });
      var content = "";
      content = '<select>';
      var i;
      var found = 0;

      for (i = 0; i < dropdown.length; i++) {

        var option = dropdown[i];
        content += '<option' + (data == option ? " selected" : "") + '>' + option + "</option>";
        if (data == option) {
          found = 1;
        }
      }
      if (found == 0) {
        content += '<option selected>' + data + '</option>';
      }

      content += '</select>';


      return content;

    }

  };
  if (contentValue)
    publicMembers.content = contentValue;
  return publicMembers;
};

var pickerColumn = function (pickerObject, contentValue) {
  var publicMembers = {
    content: "",
    editable: true,
    stopUpdate: pickerObject.removeUpdate,
    useforsave: true,
    extractValue: function (td) {
      return td.children("span")[0].textContent;
    },
    isPicker: true,
    editHTML: function (data) {
      var str = "<a href=\"\">" + pickerObject.linkText + "</a>" +
                "<span id='pickerSpan' style='margin-left: 8px;'>" + data + "</span>";

      return str;
    },
    init: function (cell) {
      $(cell).children("a").click(function (e) {
        e.preventDefault();
        var currentValue = $(cell).children("span")[0].innerHTML;
        pickerObject.showPicker(currentValue);

        pickerObject.externalValueSetter = function (value) {
          $(cell).children("span")[0].innerHTML = value;
        };

      });
    }
  };
  //TODO does this need to be here?
  if (contentValue)
    publicMembers.content = contentValue;
  return publicMembers;
};
