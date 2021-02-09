


var TableName = "GridView1";
var in_txtClass = "baseText";
var in_btnClass = "baseButton";

function page(model) {

    if (model == undefined) {
        model = new Object();
        this.id = TableName;
    }
    var d = TableName;

    if (model.id != undefined) {
        d = model.id;
    }

    this.id = d;

    if (model.RowsClick) {
        $("#" + d + " tr").filter(":not(:has(table, th))").click(function() {
            $("#" + d + " tr").removeClass("check");
            $(this).addClass("check");
            $(this).removeClass("selectk");
        });
    }
    if (model.RowsMouse) {
        $("#" + d + " tr").filter(":not(:has(table, th))").mouseover(function() {
            if (!$(this).hasClass("check")) {
                $("#" + d + " tr").removeClass("selectk");
                $(this).addClass("selectk");
            }
        });
    }
    if (model.RowsMouseOut) {
        $("#" + d + " tr").filter(":not(:has(table, th))").mouseout(function() {
            if (!$(this).hasClass("check")) {
                $("#" + d + " tr").removeClass("selectk");
            }
        });
    }
}
//刷新  无post提交提示  调用方法  page("刷新");
page.prototype.shuaxin = function() {
    location.replace(location.href);
}

page.prototype.RowsHtml = function(_index) {
    var checkRows = $("#" + this.id + " " + ".check");
    if (checkRows.val() == undefined) {
        return undefined;
    } else {
        return ClearNbsp(checkRows.find("td:eq(" + _index + ")").html());
    }
}
page.prototype.TxtCss = function(cssName) {
    $("input[type=text]").addClass(in_txtClass);
}
page.prototype.BtnCss = function() {
    $(":button").addClass(in_btnClass);
}
page.prototype.Sum = function(_index) {
    var gv = $("#" + this.id)[0];
    if (gv != undefined) {
        var hj = 0;
        for (var i = 1; i < gv.rows.length; i++) {
            hj += parseFloat(gv.rows[i].cells[_index].innerHTML);
        }
        return parseFloat(hj).toFixed(6);
    } else {
        return 0;
    }
}
page.prototype.SumInput = function(_index) {
    var gv = $("#" + this.id)[0];
    if (gv != undefined) {
        var hj = 0;
        for (var i = 1; i < gv.rows.length; i++) {
            if (isNaN($(gv.rows[i].cells[_index]).find("input").val()) || $(gv.rows[i].cells[_index]).find("input").val() == "") {
                continue;
            }
            hj += parseFloat($(gv.rows[i].cells[_index]).find("input").val());
        }
        return parseFloat(hj).toFixed(6);
    } else {
        return 0;
    }
}
page.prototype.Xh = function(_index) {
    var gv = $("#" + this.id)[0];
    if (gv != undefined) {
        if (_index == undefined) {
            _index = 0;
        }
        for (var i = 1; i < gv.rows.length; i++) {
            gv.rows[i].cells[_index].innerHTML = i;
        }
    }
}
page.prototype.ColorRightTd = function(_index, len, color) {

    var gv = $("#" + this.id)[0];

    if (color == undefined) {
        color = "red";
    }
    if (len == undefined) {
        len = 2;
    }
    if (gv != undefined) {
        if (_index == undefined) {
            _index = 0;
        }
        for (var i = 1; i < gv.rows.length; i++) {
            $(gv.rows[i].cells[_index]).css("text-align", "right");
            $(gv.rows[i].cells[_index]).css("color", color);


            if (!isNaN($(gv.rows[i].cells[_index]).html())) {
                $(gv.rows[i].cells[_index]).html(parseFloat($(gv.rows[i].cells[_index]).html()).toFixed(len));
            }
        }
    }

}
page.prototype.RowsClick = function() {
    var gay = new page({
        RowsClick: true
    });
}
page.prototype.RowsMouse = function() {
    var gay = new page({
        RowsMouse: true
    });
}
page.prototype.RowsMouseOut = function() {
    var gay = new page({
        RowsMouseOut: true
    });
}

page.prototype.Tc = function(url, w, h) {
    if (w == undefined) {
        w = 300;
    }
    if (h == undefined) {
        h = 300;
    }
    $('#topen').remove();
    $("form:eq(0)").append("<div id = 'topen'></div>");
    $('#topen').dialog({
        width: w,
        height: h,
        title: ".."
    });
    $('#topen').dialog("open");
    $('#topen').html('<iframe scrolling="yes" frameborder="0" id = "fmydlog"   style="width:100%;height:100%;"></iframe>');
    $("#fmydlog").attr("src", url);
}
page.prototype.Tcgb = function() {
    $('#topen').dialog("close");
    $('#topen').remove();
}
page.prototype.Wait = function() {
    var waitStr = "<table id='page_wait' style=' position: absolute; z-index: 1200; left: 0px; top: 0px; width: 100%;  height: 100%; background-color:White; cursor: wait;  visibility:visible;'>";
    waitStr += "<tr><td align='center' valign='middle'><img src='../Other/img/wait.gif' /><br />";
    waitStr += " <b>系统正在处理你的操作，请耐心等待....<br /></b></td> </tr></table>";
    $("form:eq(0)").append(waitStr);
}
page.prototype.WaitStop = function() {
    $("#page_wait").remove();
}

var $$ = new page();

function ClearNbsp(str) {
    return str.toString().replace("&nbsp;", "").toString();
}


function q(str) {
    try {
        return str.toString().substring(1, str.toString().indexOf("]"));
    } catch (e) {
        return str;
    }
}

