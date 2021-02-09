function initMainTableClass(mainTableClientID) {
    if (mainTableClientID == null || mainTableClientID == '' || mainTableClientID == undefined) {
        return;
    }
    $("#" + mainTableClientID + " tr:even").addClass("trodd");
    $("#" + mainTableClientID + " tr:odd").addClass("treven");

    $("#" + mainTableClientID + " tr").children().filter(" .myGridItem,.myGridItemRight,.myGridItemCenter").parent().click(function() {
        $(this).removeClass("trodd");
        $(this).removeClass("treven");
        $(this).removeClass("trmouseover");
        $("#" + mainTableClientID + " tr").removeClass("highlight");
        $(this).addClass("highlight");
        $("#" + mainTableClientID + " tr:even").not(this).addClass("trodd");
        $("#" + mainTableClientID + " tr:odd").not(this).addClass("treven");
    });
    //当行有鼠标飘过的时候
    $("#" + mainTableClientID + " tr").children().filter(" .myGridItem,.myGridItemRight,.myGridItemCenter").parent().mouseover(function() {
        $(this).removeClass("trodd");
        $(this).removeClass("treven");
        $("#" + mainTableClientID + " tr").removeClass("trmouseover");
        if (!$(this).hasClass("highlight")) {
            $(this).addClass("trmouseover");
        }
        $("#" + mainTableClientID + " tr:even").not(".highlight").addClass("trodd");
        $("#" + mainTableClientID + " tr:odd").not(".highlight").addClass("treven");
    });
}


function SubCode(str) {

    if (str.indexOf("[") != -1 && str.indexOf("]") != -1) {
        str = str.substring(str.indexOf("[") + 1, str.indexOf("]"))
    }
    return str;
}