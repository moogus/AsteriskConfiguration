var forwardingControlPageSetUp = function (config) {

  var publicMembers = {

    currentNumber: config.currentExtension,

    initalize: function () {
      privateMembers.hideAllPartials();
      privateMembers.showPartial(config.allTypes[0]);
    },

    showPartial: function (value) {
      for (var thisType in config.allTypes) {
        if (value === config.allTypes[thisType]) {
          privateMembers.showChoosenPartial(value);
        }
      }
    },

    setPartialforSave: function (myNum, enabledVal, routeType, number) {
      var saveString = enabledVal + ',' + routeType + ',' + number + ',' + myNum;
      privateMembers.savePartial(routeType, saveString);
    }
  };

  var privateMembers = {

    hideAllPartials: function () {
      for (var thisType in config.allTypes) {
        privateMembers.setPartial(config.allTypes[thisType]);
      }
    },

    setPartial: function (type) {
      $('#partial' + type).html('<img src="' + config.ajaxLoadImage + '" style="margin-left: 37px; margin-bottom: 10px;"  /><h1 id="profileh1" style="font-size: 15px;">' + type + '&nbspLoading....</h1>');
      $('#partial' + type).hide();
    },

    showChoosenPartial: function (typeIn) {
      var current;
      for (var t in config.allTypes) {

        var thisType = config.allTypes[t];

        if (typeIn === thisType) {
          privateMembers.showPartial(typeIn);
          current = typeIn;
        }
        if (typeIn !== thisType) {
          if (current === thisType) {
            return;
          }
          privateMembers.setPartial(thisType);
        }
      }
    },

    showPartial: function (type) {
      $('#partial' + type).load(config.getForwardingBaseUrl + type + '&extension=' + publicMembers.currentNumber);
      $('#partial' + type).show();
    },

    savePartial: function (type, saveString) {
      $('#partial' + type).load(config.getSaveBaseUrl + saveString);
      $('#partial' + type).show();
    }
  };

  return publicMembers;

};