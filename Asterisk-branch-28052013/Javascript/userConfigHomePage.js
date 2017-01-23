var userHomePageSetUp = function(config) {
    var publicMembers = {
        setCurrentExtension: function(value) {
            privateMembers.extension = value;
        },

        showCallPhoneForAudio: function() {
            config.callForAudio.showInterface();
        }
    };

    var privateMembers = {      
        adminEdit: false,
        extension: {},
        routePicker: config.routePicker,
        callForAudio: config.callForAudio,

        initialise: function() {
            if (config.isGood === 'False') {
                $('#content').css('background-color', '#F1F1F1');
                $('#header').css('border-bottom', '0px');
            } else {
                $('#content').css('background-color', '#ffffff');
                $('#header').css('border-bottom', '1px');
            }

            $("#setExtension").click(function(e) {
                e.preventDefault();
                publicMembers.setCurrentExtension($('#extension').val());
                privateMembers.clearAllPartials();
            });

            $('#sideUserDetails').click(function(e) {
                e.preventDefault();
                privateMembers.showPartial("UserDetails");
            });

            $('#sideVoiceMail').click(function(e) {
                e.preventDefault();
                privateMembers.showPartial("VoiceMail");
            });

            $('#sideVoiceMessages').click(function(e) {
                e.preventDefault();
                privateMembers.showPartial("VoiceMessages");
            });

            $('#sideForwarding').click(function(e) {
                e.preventDefault();
                privateMembers.showPartial("Forwarding");
            });

            $('#sidePassword').click(function(e) {
                e.preventDefault();
                privateMembers.showPartial("Password");
            });

            $(".modalClose").click(function(e) {
                e.preventDefault();
                if (config.isGood === 'False') {
                    window.location.replace("/ExtensionAdmin/Index");
                }
            });

            //forwarding initalize

            $('.showRoutePickerLink').live('click', function(e) {
                e.preventDefault();
                var dp = $(this).attr('id');
                privateMembers.routePicker.externalValueSetter = function(val) {
                    $('#' + dp + 'Dest').html(val);
                    $('#addRouteTo').hide('fast');

                };
                privateMembers.routePicker.show();
            });

            //voice messsages initalize

            $("#taber").tabs();

        },

        clearAllPartials: function() {
            privateMembers.setProfilePartial();
            privateMembers.setVoiceMailPartial();
            privateMembers.setVoiceMessagePartial();
            privateMembers.setForwardingPartial();
            privateMembers.setPasswordPartial();

            privateMembers.showPartial("NoPartial");
        },

        showPartial: function(partial) {
            privateMembers.setCurrentPageOnSide(partial);
            switch (partial) {
            case "UserDetails":
                privateMembers.resetUnusedPartials('VoiceMail');
                privateMembers.resetUnusedPartials('VoiceMessages');
                privateMembers.resetUnusedPartials('Forwarding');
                privateMembers.resetUnusedPartials('Password');
                privateMembers.resetUnusedPartials('NoPartial');

                $('#profilePartial').load('/UserDetails/Index?extn=' + privateMembers.extension + '&isAdminEdit=' + privateMembers.adminEdit);
                $('#profilePartial').show();
                break;
            case "VoiceMail":
                privateMembers.resetUnusedPartials('UserDetails');
                privateMembers.resetUnusedPartials('VoiceMessages');
                privateMembers.resetUnusedPartials('Forwarding');
                privateMembers.resetUnusedPartials('Password');
                privateMembers.resetUnusedPartials('NoPartial');

                $('#voiceMailPartial').load('/UserVoiceMail/Index?extn=' + privateMembers.extension + '&isAdminEdit=' + privateMembers.adminEdit);
                $('#voiceMailPartial').show();
                break;
            case "VoiceMessages":
                privateMembers.resetUnusedPartials('UserDetails');
                privateMembers.resetUnusedPartials('VoiceMail');
                privateMembers.resetUnusedPartials('Forwarding');
                privateMembers.resetUnusedPartials('Password');
                privateMembers.resetUnusedPartials('NoPartial');

                $('#voiceMessagePartial').load('/VoiceMessages/Index?extn=' + privateMembers.extension + '&isAdminEdit=' + privateMembers.adminEdit + '&lastFolder=' + 'INBOX');
                $('#voiceMessagePartial').show();
                break;
            case "Forwarding":
                privateMembers.resetUnusedPartials('UserDetails');
                privateMembers.resetUnusedPartials('VoiceMessages');
                privateMembers.resetUnusedPartials('VoiceMail');
                privateMembers.resetUnusedPartials('Password');
                privateMembers.resetUnusedPartials('NoPartial');

                $('#forwardingPartial').load('/Forwarding/Index?extn=' + privateMembers.extension + '&isAdminEdit=' + privateMembers.adminEdit);
                $('#forwardingPartial').show();
                break;
            case "Password":
                privateMembers.resetUnusedPartials('UserDetails');
                privateMembers.resetUnusedPartials('VoiceMessages');
                privateMembers.resetUnusedPartials('Forwarding');
                privateMembers.resetUnusedPartials('VoiceMail');
                privateMembers.resetUnusedPartials('NoPartial');

                if (privateMembers.extension === config.userLoggedOn) {
                    $('#passwordPartial').load('/Account/ChangePassword');
                    $('#passwordPartial').show();
                } else {
                    var extn = privateMembers.extension;
                    $('#passwordPartial').load('/Account/PasswordReset?extn=' + extn);
                    $('#passwordPartial').show();
                }
                break;
            case "NoPartial":
                privateMembers.resetUnusedPartials('UserDetails');
                privateMembers.resetUnusedPartials('VoiceMessages');
                privateMembers.resetUnusedPartials('Forwarding');
                privateMembers.resetUnusedPartials('VoiceMail');
                privateMembers.resetUnusedPartials('Password');

                if (config.userLoggedOn !== privateMembers.extension) {
                    privateMembers.adminEdit = true;
                    $('#noPartialh1').html('Currently editing extension ' + privateMembers.extension + '. Please select an option from the side.');
                } else {
                    privateMembers.adminEdit = false;
                    $('#noPartialh1').html('Currently your extension, ' + privateMembers.extension + '. Please select an option from the side.');
                }

                $('#noPartial').show();
                break;
            default:
            }
        },

        resetUnusedPartials: function(partial) {
            switch (partial) {
            case 'UserDetails':
                privateMembers.setProfilePartial();
                $('#profilePartial').hide();
                break;
            case 'VoiceMail':
                privateMembers.setVoiceMailPartial();
                $('#voiceMailPartial').hide();
                break;
            case 'VoiceMessages':
                privateMembers.setVoiceMessagePartial();
                $('#voiceMessagePartial').hide();
                break;
            case 'Forwarding':
                privateMembers.setForwardingPartial();
                $('#forwardingPartial').hide();
                break;
            case 'Password':
                privateMembers.setPasswordPartial();
                $('#passwordPartial').hide();
                break;
            case 'NoPartial':
                $('#noPartial').hide();
                break;
            default:
            }
        },

        setCurrentPageOnSide: function(menuItem) {
            switch (menuItem) {
            case 'UserDetails':
                $("#sideVoiceMail").removeClass("current");
                $("#sideForwarding").removeClass("current");
                $("#sidePassword").removeClass("current");
                $("#sideVoiceMessages").removeClass("current");

                $("#sideUserDetails").addClass("current");
                break;
            case 'VoiceMail':
                $("#sideUserDetails").removeClass("current");
                $("#sideForwarding").removeClass("current");
                $("#sidePassword").removeClass("current");
                $("#sideVoiceMessages").removeClass("current");

                $("#sideVoiceMail").addClass("current");
                break;
            case 'VoiceMessages':
                $("#sideVoiceMail").removeClass("current");
                $("#sideUserDetails").removeClass("current");
                $("#sideForwarding").removeClass("current");
                $("#sidePassword").removeClass("current");

                $("#sideVoiceMessages").addClass("current");
                break;
            case 'Forwarding':
                $("#sideUserDetails").removeClass("current");
                $("#sideVoiceMail").removeClass("current");
                $("#sidePassword").removeClass("current");
                $("#sideVoiceMessages").removeClass("current");

                $("#sideForwarding").addClass("current");
                break;
            case 'Password':
                $("#sideUserDetails").removeClass("current");
                $("#sideVoiceMail").removeClass("current");
                $("#sideForwarding").removeClass("current");
                $("#sideVoiceMessages").removeClass("current");

                $("#sidePassword").addClass("current");
                break;
            case 'NoPartial':
                $("#sideUserDetails").removeClass("current");
                $("#sideVoiceMail").removeClass("current");
                $("#sideForwarding").removeClass("current");
                $("#sidePassword").removeClass("current");
                $("#sideVoiceMessages").removeClass("current");
                break;
            default:
            }
        },

        setProfilePartial: function() {
            $('#profilePartial').html('<img src="../../Content/Images/ajax-loader.gif" style="margin-left: 37px; margin-bottom: 10px;" alt="" /><h1 id="profileh1" style="font-size: 15px;">User&nbspProfile&nbspLoading....</h1>');
        },

        setVoiceMailPartial: function() {
            $('#voiceMailPartial').html(' <img src="../../Content/Images/ajax-loader.gif" style="margin-left: 37px; margin-bottom: 10px;" alt="" /><h1 id="voiceMailh1" style="font-size: 15px;">Voice-Mail&nbspLoading....</h1>');
        },

        setVoiceMessagePartial: function() {
            $('#voiceMessagePartial').html(' <img src="../../Content/Images/ajax-loader.gif" style="margin-left: 37px; margin-bottom: 10px;" alt="" /><h1 id="voiceMessageh1" style="font-size: 15px;">Messages&nbspLoading....</h1>');
        },

        setForwardingPartial: function() {
            $('#forwardingPartial').html('<img src="../../Content/Images/ajax-loader.gif" style="margin-left: 37px; margin-bottom: 10px;" alt="" /><h1 id="forwardingh1" style="font-size: 15px;">Forwarding&nbspLoading....</h1>');
        },

        setPasswordPartial: function() {
            $('#passwordPartial').html('<img src="../../Content/Images/ajax-loader.gif" style="margin-left: 37px; margin-bottom: 10px;" alt="" /><h1 id="passwordh1" style="font-size: 15px;">Password&nbspLoading....</h1>');
        },
    };

    privateMembers.initialise();

    return publicMembers;
};