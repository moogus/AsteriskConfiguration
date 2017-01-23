var userHomePageSetUp = function (config) {
  var publicMembers = {
   setMessagePositions: function (viewBagMesage) {
     $('#messageh1').css('margin-right', privateMembers.setMessageSize(viewBagMesage));
     $('#profileh1').css('margin-right', privateMembers.setSize(250));
     $('#voiceMailh1').css('margin-right', privateMembers.setSize(275));
     $('#forwardingh1').css('margin-right', privateMembers.setSize(575));
     $('#passwordh1').css('margin-right', privateMembers.setSize(275));
  }
  };

  var privateMembers = {
    initialise: function () {
      if (config.isGood === 'False') {
        $('#content').css('background-color', '#F1F1F1');
        $('#header').css('border-bottom', '0px');
      } else {
        $('#content').css('background-color', '#ffffff');
        $('#header').css('border-bottom', '1px');
      }

      $('#sideUserDetails').click(function (e) {
        e.preventDefault();
        privateMembers.ShowUserDetails();
      });

      $('#sideVoiceMail').click(function (e) {
        e.preventDefault();
        privateMembers.showVoiceMail();
      });

      $('#sideForwarding').click(function (e) {
        e.preventDefault();
        privateMembers.showForwarding();
      });

      $('#sidePassword').click(function (e) {
        e.preventDefault();
        privateMembers.showPassword();
      });

      $(".modalClose").click(function (e) {
        e.preventDefault();
        if (config.isGood === 'False') {
          window.location.replace("/Account/Index");
        }
      });
    },

    setMessageSize: function (viewMessage) {
      var amount = 50;
      var num = parseInt(viewMessage);
      if (num > 50 && num < 100) {
        amount = (120 - num);
      } else if (num < 50 && num > 15) {
        amount = (185 - num);
      } else if (num < 15) {
        amount = (220 - num);
      }
      return amount + 'px';
    },

    setSize: function (amount) {
      return amount + 'px';
    },

    ShowUserDetails: function () {
      privateMembers.setCurrentPageOnSide("UserDetails");
      $('#message').hide();
      $('#voiceMailPartial').hide();
      $('#forwardingPartial').hide();
      $('#passwordPartial').hide();

      $('#profilePartial').load('/UserDetails/Index');
      $('#profilePartial').show();
    },

    showVoiceMail: function () {
      privateMembers.setCurrentPageOnSide("VoiceMail");
      $('#message').hide();
      $('#profilePartial').hide();
      $('#forwardingPartial').hide();
      $('#passwordPartial').hide();

      $('#voiceMailPartial').load('/UserVoiceMail/Index');
      $('#voiceMailPartial').show();
    },

    showForwarding: function () {
      privateMembers.setCurrentPageOnSide("Forwarding");
      $('#message').hide();
      $('#profilePartial').hide();
      $('#voiceMailPartial').hide();
      $('#passwordPartial').hide();

      $('#forwardingPartial').load('/Forwarding/Index');
      $('#forwardingPartial').show();
    },

    showPassword: function () {
      privateMembers.setCurrentPageOnSide("Password");
      $('#message').hide();
      $('#profilePartial').hide();
      $('#voiceMailPartial').hide();
      $('#forwardingPartial').hide();
      $('#passwordPartial').load('/Account/ChangePassword');
      $('#passwordPartial').show();
    },

    setCurrentPageOnSide: function (menuItem) {
      switch (menuItem) {
        case 'UserDetails':
          $("#sideVoiceMail").removeClass("current");
          $("#sideForwarding").removeClass("current");
          $("#sidePassword").removeClass("current");

          $("#sideUserDetails").addClass("current");
          break;
        case 'VoiceMail':
          $("#sideUserDetails").removeClass("current");
          $("#sideForwarding").removeClass("current");
          $("#sidePassword").removeClass("current");

          $("#sideVoiceMail").addClass("current");
          break;
        case 'Forwarding':
          $("#sideUserDetails").removeClass("current");
          $("#sideVoiceMail").removeClass("current");
          $("#sidePassword").removeClass("current");

          $("#sideForwarding").addClass("current");
          break;
        case 'Password':
          $("#sideUserDetails").removeClass("current");
          $("#sideVoiceMail").removeClass("current");
          $("#sideForwarding").removeClass("current");

          $("#sidePassword").addClass("current");
          break;
        default:
      }
    }
  };

  privateMembers.initialise();
  
  return publicMembers;
};