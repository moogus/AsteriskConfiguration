var picker = function(spec) {
    var publicMembers = modalDialog(spec);

    publicMembers.putSpinnerInInner = function() {
        publicMembers.contentDiv().html($("<img style='margin-left: 220px;margin-top: 10px;' src='../../Content/Images/ajax-loader.gif'" +
            " class='partialSpinner' alt='' /><h1 style='background-color: #fff; text-align: center'>Picker Loading....</h1>"));
    };

    var baseHide = publicMembers.hide;
    publicMembers.hide = function() {
        baseHide();
        publicMembers.putSpinnerInInner();
    };

    var baseShow = publicMembers.show;
    publicMembers.show = function() {
        // show the actual dialog
        baseShow();

        //create a new div to hold our tabs
        publicMembers.newdiv = $('<div>');

        // add blank tabs holder to dialog
        publicMembers.newdiv.append($('<div id="tabs" style="margin-top: 5px;"><ul></ul></div><br /><p>Click off this window to close.</p>'));

        //run script to fill tabs
        publicMembers.showAddTabs(publicMembers.newdiv);
        publicMembers.newdiv.find('#tabs').tabs();
        publicMembers.contentDiv().html(publicMembers.newdiv);

    };

    // this is a virtual method that needs to be implemented by anything that inherits 
    //from this class. It is used to add the tabs to the picker
    publicMembers.showAddTabs = function() {
    };

    // this is a virtual method that needs to be implemented by anything that inherits 
    //from this class. It is used to set the base values in the picker picker
    publicMembers.setCurrentValues = function() {
    };

    //use the functionality from the dynamicColumn, passed through the pickerObject
    publicMembers.setValueInDataTable = function(val) {
        publicMembers.externalValueSetter(val);
        publicMembers.hide();
    };

    // this is a virtual method that will be overridden in the picker column
    // it is used to take the currently selected value and pass it to the datatable
    publicMembers.externalValueSetter = function(val) {
    };

    publicMembers.addTab = function(tab) {
        tab.addMe(publicMembers);
    };

    publicMembers.putSpinnerInInner();

    return publicMembers;
};