jQuery.fn.FixHead = function() {


    var gvn = this.clone(true).removeAttr("id");
    $(gvn).find("tr:not(:first)").remove();
    $(gvn).css("margin-bottom", "0px");
    this.css("margin", "0 0 0 0");
    this.before(gvn);
    this.find("tr:first").remove();
    this.wrap("<div style='height:" + "300" + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(this.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");


}