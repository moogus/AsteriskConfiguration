var addQueueMember = function (config) {

  var addQueueMember = config.table.dataTable({
    "bPaginate": false,
    "bLengthChange": false,
    "iDisplayLength": 100,
    "aoColumns": config.columns
  });

  var publicMembers = {

    addToQueue: function () {
      if ($(config.checkBox).length > 0) {
        $(config.valueType).attr('action', config.addUrl);
        $(config.valueType).submit();

      } else {
        alert("you have not made a valid selection.");
      }
    }
  };

  var privateMembers = {};


  
  return publicMembers;

};