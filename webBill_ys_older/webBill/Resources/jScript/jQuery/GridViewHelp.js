

function GetRowCode() {
    var checkrow = $(".highlight");
    if (checkrow.val() == undefined) {
        alert("请先选择行");
        return "";
    }
    var billcode = checkrow.find("td")[0].innerHTML;
    return billcode;
}


function DataDelete(type) {
    var code = GetRowCode();
    if (code == "") {
        return;
    }
    $.post("../MyAjax/DeleteBill.ashx", { "type": "message", "billcode": code }, function(data, status) {
        if (status == "success") {
            if (data == "1") {
                alert("删除成功");
                $(".highlight").remove();
            }
            else {
                alert("删除失败");
            }
        }
        else {
            alert("删除失败");
        }
    });
}