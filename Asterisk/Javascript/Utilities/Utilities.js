var utilities = {
    uniquePageId: 1,

    getNextUniqueID: function(str) {
        this.uniquePageId++;
        return str + this.uniquePageId;
    }
};

function toArray(e) {
    return Array.prototype.slice.call(e);
}

Function.prototype.curry = function() {
    if (arguments.length < 1) {
        return this; //nothing to curry with - return function
    }
    var method = this;
    var args = toArray(arguments);
    return function() {
        return method.apply(this, args.concat(toArray(arguments)));
    };
};