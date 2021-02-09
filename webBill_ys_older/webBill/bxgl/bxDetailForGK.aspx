<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxDetailForGK.aspx.cs" Inherits="webBill_bxgl_bxDetailForGK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--市立医院专用--%>
    <title>费用报销单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        var selectCode = ''; //单据撤销和单据详细信息用到的选择单据的编号
        $(function() {
            //showhelpmsg();
            showworkflow();
            htjeChange();
            //是否归口-------------------------------------------------------------------------
            var isgk = $("#rb_ok").attr("checked");
            if (!isgk) {
                $("#fykmname").addClass("basehidden");
                $("#selectbxdept").addClass("basehidden");
            }
            //是归口
            $("#rb_ok").click(function() {
                $("#fykmname").removeClass();
                $("#selectbxdept").removeClass();
                $("#selectbxdept").addClass("baseButton");
                //                $("#fykm").accordion('destroy');
                //                $("#fykm").html("");

                $("#body_fykm").html("");
                $("#td_dept").html("");
                $("#td_xm").html("");


            });
            //非归口
            $("#rb_can").click(function() {
                //                $("#fykm").accordion('destroy');
                //                $("#fykm").html("");
                $("#fykmname").addClass("basehidden");
                $("#selectbxdept").addClass("basehidden");
                $("#body_fykm").html("");
                $("#td_dept").html("");
                $("#td_xm").html("");
            });
            //是否归口--------------------------------end--------------------------------------------

            $("#tab_fykm tbody tr").live("click", function() {
                $("#tab_fykm tbody tr").removeClass("highlight");
                $(this).addClass("highlight");
                var thisid = this.id.split("_")[1];
                $("#td_dept ul").addClass("hiddenbill");
                $("#td_xm ul").addClass("hiddenbill");
                $("#bm_" + thisid).removeClass("hiddenbill");
                $("#xm_" + thisid).removeClass("hiddenbill");
            });
            //查看信息 edit by lvcc
            $("#btnFysqXX").click(function() {
                var codeType = selectCode.substring(0, 4);
                if (codeType == 'ccsq') {
                    //出差申请
                    //window.showModalDialog("../fysq/travelApplicationDetails.aspx?Ctrl=View&Code=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
                    window.showModalDialog("../fysq/travelReportDetail.aspx?Ctrl=View&AppCode=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
                } else if (codeType == 'lscg') {
                    //报告申请
                    window.showModalDialog("../fysq/lscgDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:560px;dialogWidth:940px;status:no;scroll:yes');
                } else if (codeType == 'cgsp') {
                    //采购审批
                    window.showModalDialog("../fysq/cgspDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
                } else {
                }
            });
            //审核单据
            $("#btn_ok").click(function() {
                var billcode = '<%=Request["billCode"] %>';
                billcode = billcode + "*,"
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                    }
                }
            });
            //否决单据
            $("#btn_cancel").click(function() {
                var billcode = '<%=Request["billCode"] %>';
                billcode = billcode + "*,"
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                } else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });

            //保存
            $("#btn_test").click(function() {
                save(0);
            });
            //保存并提交
            $("#btn_save_commit").click(function() {
                save(1);
            });
            //保存  参数表示是否还提交 1是0否
            function save(iscommit) {
                var type = '<%=Request["type"] %>';
                if (type == null || type == undefined || type == "") {
                    alert("未获取到操作状态，无法保存！"); return;
                }
                //检测必填项

                var zy = $("#txtBxzy").val();
                if (zy == undefined || zy == '') {
                    alert("请填写报销摘要");
                    return;
                }
                var bxr2 = $("#txtbxr2").val();
                if (bxr2 == undefined || bxr2 == '') {
                    alert("请填写报销人或报销单位");
                    return;
                }
                var json = jsonMaker();
                if (json == "") {
                    alert("部门分配金额与科目金额不相等!");
                    return;
                } else if (json == "123") {
                    alert("经办人必须为系统内部人员，请输入经办人编号或姓名，然后选择系统自动提示的记录，正确的格式为‘[编号]名称’。");
                    return;
                }
                $.post("../MyAjax/YbbxBillSaveForGkfj.ashx?type=" + type + "&iscommit=" + iscommit, json, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("保存成功");
                            window.returnValue = "1";
                            window.close();
                        } else if (data == "-2") {
                            alert("有预算超支了,不能保存!");
                        } else if (data == "-3") {
                            alert("该报销类型需要附单据,请添加单据!");
                        }
                        else if (data == "-4") {
                            alert("修改单据日期不能跨月!");
                        }
                        else if (data == "-5") {
                            alert("该费用科目有部门核算，请选择使用部门!");
                        }
                        else if (data == "-6") {
                            alert("该费用科目有项目核算，请选择使用项目!");
                        }
                        else if (data == "-7") {
                            alert("该月份单据已做月结，不能再做本月份的单据!");
                        } else if (data == "-8") {
                            alert("申请人必须包括在附加的申请单据内！");
                        }
                        else {
                            alert(data);
                            alert("保存失败2");
                        }
                    }
                    else {
                        alert("保存失败");
                    }
                });
            }
            function jsonMaker() {
                //构造提交服务器的字符串
                var djtype = $("#hd_djtype").val();
                var pd_test = true;
                var billname = $("#HiddenField1").val();
                var usercode = $("#txtBxr").val(); //经办人
                if (usercode.indexOf(']') == -1) {

                    return "123";
                }
                usercode = usercode.split(']')[0];
                usercode = usercode.substring(1, usercode.length);
                var varphone = $("#txtbxrdh").val(); //报销人电话
                var varzh = $("#txtbxzh").val(); //报销人账号
                var varkhh = $("#txt_khh").val(); //开户行
                varzh = varkhh + "|&|" + varzh;
                var varbxr2 = $("#txtbxr2").val(); //报销人
                var bxr = $("#txtJbr").val(); //预算管理员
                bxr = bxr.split(']')[0];
                bxr = bxr.substring(1, bxr.length);
                var bxzy = $("#txtBxzy").val();
                var bxsm = $("#txtBxsm").val();
                var bxlx = $("#drpBxmxlx").val();
                var isgk = $("#rb_ok").attr("checked");
                var bxDate = $("#txtSqrq").val();
                gkbmbh = $("#txtDept").val();
                gkbmbh = gkbmbh.split(']')[0];
                gkbmbh = gkbmbh.substring(1, gkbmbh.length);
                var fysqCode = "";
                //                $("#cgdj tbody tr td:eq(0)").each(function() {
                //                    fysqCode += $(this).html() + "|"; alert();
                //                });
                var itbodytrCount = $("#cgdj tbody tr").length;
                for (var i = 0; i < itbodytrCount; i++) {
                    fysqCode += $("#cgdj tbody ").find('tr')[i].children[1].innerHTML + "|";
                }
                if (fysqCode.length > 0) {
                    fysqCode = fysqCode.substring(0, fysqCode.length - 1);
                }
                var djs = '0';
                var sqlx = "";//申请类型 大智用 为了用一个一般处理程序
                var ykfs = "";//用款方式  大智用 为了用一个一般处理程序
                var yksqcode = "";//用款申请单好 大智用 为了用一个一般处理程序
                var isxkfx = "1";//是否新财年  大智用 为了用一个一般处理程序
                var fujian = "";//附件
                var ret = '{"fujian":"' + fujian + '","djlx":"' + djtype + '","bxr":"' + usercode + '","billuser":"' + bxr + '","bxrzh":"' + varzh + '","bxr2":"' + varbxr2 + '","bxrphone":"' + varphone + '","zy":"' + bxzy + '","sm":"' + bxsm + '","bxlx":"' + bxlx + '","gkbmbh":"' + gkbmbh + '","billname":"' + billname + '","fysq":"' + fysqCode + '","bxDate":"' + bxDate + '","sfgf":"0","djs":"' + djs + '","isgk":"' + isgk + '","sqlx":"' + sqlx + '","ykfs":"' + ykfs + '","yksqcode":"' + fysqCode + '","isxkfx":"' + isxkfx + '","list":[';
                $("#tab_fykm tbody tr").each(function(i) {
                    var tempkm = $(this).find("td:eq(0)").html(); //科目  
                    tempkm = tempkm.split(']')[0];
                    tempkm = tempkm.substring(1, tempkm.length);
                    var tempbm = $(this).find("td:eq(1)").html(); //部门
                    tempbm = tempbm.split(']')[0];
                    tempbm = tempbm.substring(1, tempbm.length);

                    var tempje = $(this).find("input:eq(0)").val(); //金额
                    if (tempje == 0 || tempje == '') {
                        return true;
                    }
                    var tempse = $(this).find("input:eq(1)").val(); //税额
                    ret += '{"km":"' + tempkm + '","bxbm":"' + tempbm + '","je":"' + tempje + '","se":"' + tempse + '","syje":"0","bm":[';
                    var bmret = "";

                    var kmIndex = this.id.split("_")[1];
                    $("#bm_" + kmIndex + " li").each(function() {
                        var tempbmbh = $(this).find("span").html();
                        tempbmbh = tempbmbh.split(']')[0];
                        tempbmbh = tempbmbh.substring(1, tempbmbh.length);
                        var tempbmje = $(this).find("input").val();
                        bmret += '{"bmbh":"' + tempbmbh + '","bmje":"' + tempbmje + '"},';
                    });

                    bmret = bmret.substring(0, bmret.length - 1);
                    ret += bmret + '] ,"xm":[';
                    var xmret = "";
                    $("#xm_" + kmIndex + " li").each(function() {
                        var tempxmbh = $(this).find("span").html();
                        tempxmbh = tempxmbh.split(']')[0];
                        tempxmbh = tempxmbh.substring(1, tempxmbh.length);
                        var tempxmje = $(this).find("input").val();
                        xmret += '{"xmbh":"' + tempxmbh + '","xmje":"' + tempxmje + '"},';
                    });
                    xmret = xmret.substring(0, xmret.length - 1);
                    ret += xmret + ']},';
                });
                ret = ret.substring(0, ret.length - 1);
                ret += "]}";
                return ret;
            }
            function OnApproveSuccess(data, status) {
                if (data > 0 && status == "success") {
                    alert("操作成功！");
                    self.close();
                } else {
                    alert("审批失败！");
                }
            }
            $("#txtBxr").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#txtDept").val(data);
                            $("#reload").click();
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            //摘要
            $("#txtBxzy").autocomplete({
                source: allzhaiyao,
                select: function(event, ui) {
                    $("#txtBxzy").val(ui.item.value);
                }
            });
            //账号
            $("#txtbxzh").autocomplete({
                source: allzh,
                select: function(event, ui) {
                    $("#txtbxzh").val(ui.item.value);
                }
            });
            //电话
            $("#txtbxrdh").autocomplete({
                source: allphone,
                select: function(event, ui) {
                    $("#txtbxrdh").val(ui.item.value);
                }
            });
            //报销人或单位
            $("#txtbxr2").autocomplete({
                source: allbxr2,
                select: function(event, ui) {
                    $("#txtbxr2").val(ui.item.value);
                }
            });
            //开户行
            $("#txt_khh").autocomplete({
                source: allkhh,
                select: function(event, ui) {
                    $("#txt_khh").val(ui.item.value);
                }
            });
            $("#txtSqrq").datepicker();
            //选择部门
            $("#btn_choosedept").click(function() {
                var checkRowid = $("#tab_fykm .highlight").attr("id");
                if (checkRowid == undefined) {
                    alert("请选择行!");
                    return;
                }
                checkRowid = checkRowid.split("_")[1];
                openBm(checkRowid);
            });
            //批量导入部门
            $("#btn_importdept").click(function() {
                var checkRowid = $("#tab_fykm .highlight").attr("id");
                if (checkRowid == undefined) {
                    alert("请选择行!");
                    return;
                }
                checkRowid = checkRowid.split("_")[1];
                var kmcode = $("#tab_fykm .highlight").children().eq(0).html();
                var url = "BxDetail_HsDeptImportExcel.aspx?kmcode=" + kmcode;
                var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:240px;dialogWidth:860px;status:no;scroll:yes');
                if (str != null && str != undefined && str != '') {
                    $("#bm_" + checkRowid).html(str);
                }
            });
            $("#btn_choosexm").click(function() {
                var current = $("#tab_fykm .highlight");
                //edit by lvcc 控制预算科目核算项目的属性
                if (current.text().indexOf("是") == -1) {
                    alert("该费用科目没有启用项目核算，请先到预算科目菜单修改此科目的属性！");
                    return;
                }
                var checkRowid = $("#tab_fykm .highlight").attr("id");

                if (checkRowid == undefined) {
                    alert("请选择行!");
                    return;
                }
                var gkbmbh = "";
                gkbmbh = $("#txtDept").val();
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }

                checkRowid = checkRowid.split("_")[1];
                openXm(gkbmbh, checkRowid);
            });
            var gkbm_cache = {};
            $("#btnAddFykm").click(function() {
                var gkbmbh = $("#txtguikoukeshi").val();
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                var isgk = $("#rb_ok").attr("checked");
                openKm(gkbmbh, isgk);
            });
            //选择报销部门
            $("#selectbxdept").click(function() {
                var yskmname = $("#fykmname").val();
                if (yskmname == '' || yskmname == undefined) {
                    alert("请先选择要报销的费用科目。"); return;
                }
                var bxdate = $("#txtSqrq").val();
                if (bxdate == '' || bxdate == undefined) {
                    alert("请先选择报销日期。"); return;
                }
                var deptcode = $("#txtDept").val();
                if (deptcode == '' || deptcode == undefined) {
                    alert("请先选择报销人所在部门。"); return;
                }
                var url = "SelectBxDeptForGk.aspx?deptcode=" + deptcode + "&bxdate=" + bxdate + "&yskmname=" + yskmname;
                var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:800px;status:no;scroll:yes');
                //已存在的km
                var kmArray = new Array();
                $("#tab_fykm tbody tr td:nth-child(2)").each(function(i) {
                    kmArray[i] = $(this).html();
                });
                deptcode = kmArray.join();
                //index
                var kmIndex = $("#tab_fykm tbody tr").last().attr("id");
                if (kmIndex == undefined || kmIndex == "") {
                    kmIndex = 1;
                }
                else {
                    kmIndex = (kmIndex.split("_")[1]) * 1 + 1;
                }
                if (str == null || str == undefined || str == '') {
                    return;
                }
                var json = $.parseJSON(str);
                var inner = "";
                var checkArray = new Array();
                for (var i = 0; i < json.length; i++) {

                    inner += '<tr id="tr_' + kmIndex + '">';
                    inner += "<td>" + json[i].Yscode + "</td>";
                    inner += "<td>" + json[i].dept + "</td>";
                    inner += "<td>" + json[i].Ysje + "</td>";
                    inner += "<td>" + json[i].Syje + "</td>";
                    inner += "<td>" + json[i].sybl + "</td>";
                    inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="' + json[i].bxje + '" /></td>';
                    inner += '<td  style="display:none"><input type="text" class="baseText ysse" onblur="htjeChange();" value="' + json[i].se + '" /></td>';
                    inner += "</tr>";
                    $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"><li><span>' + json[i].dept + ':</span><input type="text" value="' + (json[i].bxje + json[i].se) + '" /></li></ul>');
                    $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                    kmIndex++;

                    checkArray[i] = json[i].dept;
                }
                //                var checkdept = checkArray.join();
                //                $("#tab_fykm tbody tr").each(function() {
                //                    var tempdeptCode = $(this).find("td:eq(1)").html();
                //                    if (checkdept.indexOf(tempdeptCode) < 0) {
                //                        var delIndex = this.id.split("_")[1];
                //                        $("#xm_" + delIndex).remove();
                //                        $("#bm_" + delIndex).remove();
                //                        $(this).remove();
                //                    }
                //                });
                $("#tab_fykm tbody").append(inner);
            });

            $("body").keypress(function(event) {
                var key = event.keyCode;
                if (key == null || key == undefined || key == "") {
                    return;
                }
                if (key == "27") {
                    if (confirm("单据尚未保存，确定要关闭吗？")) {
                        window.close();
                    }
                }
            });
        });

        function htjeChange() {
            var je = 0;
            $(".ysje").each(function() {
                je += Number($(this).val()) * 100;
            });
            $(".ysse").each(function() {
                je += Number($(this).val()) * 100;
            });
            $("#txtHjjeXx").val(je / 100);
            $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
        }



        function openBm(dvid) {
            var deptCode = "";
            $("#bm_" + dvid + " span").each(function() {
                var deptname = $(this).html();
                var depthsje = $(this).parent().children().eq(1).val();
                deptCode += depthsje + '$' + deptname;
            });

            var str = window.showModalDialog("YDeptSelectNew.aspx?deptCode=" + deptCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var depts = str.split('|');
                var innerval = '';
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li><span>" + depts[i].split('$')[1] + ":</span><input type='text' value='" + depts[i].split('$')[0] + "' /></li>";
                }
                innerval += "";
                $("#bm_" + dvid).html(innerval);
            }
        }

        function openXm(deptCode, dvid) {
            var xmCode = "";
            $("#xm_" + dvid + " span").each(function() { xmCode += $(this).html(); });
            var tempurl = "YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode;
            var str = window.showModalDialog("YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var xms = str.split('|');
                var innerval = "";
                for (var i = 0; i < xms.length; i++) {
                    innerval += "<li><span>" + xms[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                $("#xm_" + dvid).html(innerval);
            }
        }
        //附加采购单
        function openCgsp() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('selectCgsp.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                var num = $("#tb_fysq").find('tr').length;
                var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);;'/></td>" + strTemp;
                $("#cgdj tbody").append(endStr);
            }
        }

        //mxl 2012.04.09 添加报告单
        function openCgsp2() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('selectlscg.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            //alert(tempInner);
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                var num = $("#tb_fysq").find('tr').length;
                var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);;'/></td>" + strTemp;
                $("#cgdj tbody").append(endStr);
            }
        }
        //添加出差申请单 edit by Lvcc 2012.08.25 
        function openTravelApplication() {
            var url = 'selectTravelAppBill.aspx';
            if ($("#hdHsCCBG").val() == "1") {
                url += "?Status=HasRepBill";
            }
            var tempInner = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');

            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                var num = $("#tb_fysq").find('tr').length;
                var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);'/></td>" + strTemp;
                $("#tb_fysq").append(endStr);
            }
        }
        function openKm(deptCode, isGk) {
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function(i) {
                kmArray[i] = $(this).html();
            });
            kmcode = kmArray.join();

            var kmIndex = $("#tab_fykm tbody tr").last().attr("id");
            if (kmIndex == undefined || kmIndex == "") {
                kmIndex = 1;
            }
            else {
                kmIndex = (kmIndex.split("_")[1]) * 1 + 1;
            }

            var billDate = $("#txtSqrq").val();
            var dydj = "02";
            var rqdydj = '<%=Request["dydj"] %>';
            if (rqdydj != null && rqdydj != undefined && rqdydj != '') {
                dydj = rqdydj;
            }
            var str = window.showModalDialog("YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate + "&dydj=" + dydj, 'newwindow', 'center:yes;dialogHeight:540px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
                if (isGk) {//归口报销
                    if (json.length > 0) {
                        $("#fykmname").val(json[0].Yscode);
                    }
                } else {//本部门报销
                    var inner = "";
                    var checkArray = new Array();
                    for (var i = 0; i < json.length; i++) {
                        if (kmcode.indexOf(json[i].Yscode) < 0) {
                            inner += '<tr id="tr_' + kmIndex + '">';
                            inner += "<td>" + json[i].Yscode + "</td>";
                            inner += "<td>" + json[i].dept + "</td>";
                            inner += "<td>" + json[i].Ysje + "</td>";
                            inner += "<td>" + json[i].Syje + "</td>";
                            inner += "<td>" + json[i].sybl + "</td>"; //使用比例
                            inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="0.00" /></td>';
                            inner += '<td  style="display:none"><input type="text" class="baseText ysse" onblur="htjeChange();" value="0.00" /></td>';
                            inner += "</tr>";
                            $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"></ul>');
                            $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                            kmIndex++;
                        }
                        checkArray[i] = json[i].Yscode;
                    }
                    var checkKm = checkArray.join();
                    $("#tab_fykm tbody tr").each(function() {
                        var tempKmCode = $(this).find("td:eq(0)").html();
                        if (checkKm.indexOf(tempKmCode) < 0) {
                            var delIndex = this.id.split("_")[1];
                            $("#xm_" + delIndex).remove();
                            $("#bm_" + delIndex).remove();
                            $(this).remove();
                        }
                    });
                    $("#tab_fykm tbody").append(inner);
                }
            }
        }
        function openKm1(deptCode, isGk) {
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function(i) {
                kmArray[i] = $(this).html();
            });
            kmcode = kmArray.join();

            var kmIndex = $("#tab_fykm tbody tr").last().attr("id");
            if (kmIndex == undefined || kmIndex == "") {
                kmIndex = 1;
            }
            else {
                kmIndex = (kmIndex.split("_")[1]) * 1 + 1;
            }
            var billDate = $("#txtSqrq").val();
            var hasSaleRebateFlg = $("#hdHasSaleRebate").val();
            var str = window.showModalDialog("YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
                var inner = "";
                var checkArray = new Array();
                for (var i = 0; i < json.length; i++) {
                    if (kmcode.indexOf(json[i].Yscode) < 0) {
                        inner += '<tr id="tr_' + kmIndex + '">';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        inner += "<td>" + json[i].XiangMuHeSuan + "</td>";

                        inner += "<td>" + json[i].Ysje + "</td>";
                        if (hasSaleRebateFlg != "1") {
                            inner += "<td>" + json[i].Syje + "</td>";
                        } else {
                            inner += "<td>" + json[i].Tcje + "</td>";
                            inner += "<td>" + json[i].Kyje + "</td>";
                        }
                        inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="0.00" /></td>';
                        inner += '<td><input type="text" class="baseText ysse" onblur="htjeChange();" value="0.00" /></td>';
                        inner += "</tr>";
                        $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"></ul>');
                        $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                        kmIndex++;
                    }
                    checkArray[i] = json[i].Yscode;
                }
                var checkKm = checkArray.join();
                $("#tab_fykm tbody tr").each(function() {
                    var tempKmCode = $(this).find("td:eq(0)").html();
                    if (checkKm.indexOf(tempKmCode) < 0) {
                        var delIndex = this.id.split("_")[1];
                        $("#xm_" + delIndex).remove();
                        $("#bm_" + delIndex).remove();
                        $(this).remove();
                    }
                });
                $("#tab_fykm tbody").append(inner);
            }
        }
        //edit by lvcc 20120827 
        function onSelectBillChanged(strBillVal) {
            if (strBillVal == null || strBillVal == "") {
                return;
            } else if (strBillVal == "cc") {//出差
                openTravelApplication();
            } else if (strBillVal == "bg") {//报告单
                openCgsp2();
            } else if (strBillVal == "cg") {//采购单
                openCgsp();
            } else { }
            $("#selectBill").val("");
        }
        //edit by lvcc20120901  tableId是要添加到的table的id obj是要添加的内容
        function addRowsToBillGrid(tableId, obj) {
            alert(obj);
            var table = document.getElementById(tableId);
            var n = table.rows.length;
            r = table.insertRow(n);
            r.insertCell(0).innerHTML = obj;
        }
        //附件撤销 配合下面的 radCheck方法  上面的方法有的浏览器不支持edit by lvcc 20121012

        function cancelAttachment() {
            if (selectCode == '' || selectCode == undefined) {
                alert("请选中要操作的记录！");
            }
            var tbody = $("#tb_fysq");
            var trLength = tbody.children().length;
            var flg = false;
            var index = -1;
            for (var i = 0; i < trLength; i++) {
                var iRel = tbody.children()[i].children[1].innerHTML.indexOf(selectCode);
                if (iRel != '-1') {
                    flg = true;
                    index = i;
                    break;
                }
            }
            if (!flg) {
                alert("请选中要操作的记录！");
                return;
            }
            var tableMain = $("#cgdj");
            var nowTr = tbody.children()[index];
            nowTr.parentNode.removeChild(nowTr);
        }
        function radCheck(self) {
            selectCode = self.parentNode.parentNode.childNodes[1].innerHTML;
        }
        function exportExcel() {


        }
        function exportExcel2() {
            var checkRowid = $("#tab_fykm .highlight").attr("id");
            if (checkRowid == undefined) {
                alert("请选择行!");
                return;
            }
            checkRowid = checkRowid.split("_")[1];
            var kmcode = $("#tab_fykm .highlight").children().eq(0).html();
            var url = "exportsybmexcel.aspx?kmcode=" + kmcode;
            // showModalDialog(url, "newwindow", "center:yes;dialogHeight:240px;dialogWidth:860px;status:no;scroll:yes");
            window.open(url, "newwindow", 'height=100,width=400,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
        }
        function showworkflow() {
            var billcode = '<%=Request["billCode"] %>';
            $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billcode }, function(data, status) {
                if (status == "success") {
                    $("#wf").html(data);
                }
            });
        }
        //显示帮助信息
        function showhelpmsg() {
            window.showModalDialog("fykm_sm_show.aspx", 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:660px;status:no;scroll:auto;');
        }
    </script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
        }
        ul
        {
            list-style: none;
            margin-left: 0px;
            margin-top: 0px;
            padding-left: 5px;
        }
        ul li
        {
            margin: 5px 5px 5px 5px;
        }
        #tab_fykm
        {
            margin-top: 5px;
            margin-bottom: 5px;
        }
        #td_dept input
        {
            width: 100px;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 36px;">
                    <strong>
                        <asp:Label ID="lbdjmc" runat="server" Text="Label"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></strong>
                        
                </td>
            </tr>
            <tr>
                <td>
                    <div style="margin: 0 auto; width: 98%;">
                        <table border="0" cellpadding="2" cellspacing="0" class="myTable" width="100%">
                            <tr>
                                <td colspan="2" class="tableBg2" style="width: 80px">
                                    预算管理员
                                </td>
                                <td colspan="2" style="width: 200px">
                                    <asp:TextBox ID="txtJbr" runat="server" ReadOnly="True" class="baseTextReadOnly"></asp:TextBox>
                                </td>
                                <td class="tableBg2">
                                    归口科室
                                </td>
                                <td colspan="4">
                                    <input id="txtguikoukeshi" readonly="readonly" type="text" runat="server" class="baseTextReadOnly" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" colspan="2">
                                    经办人
                                </td>
                                <td colspan="2">
                                    <input id="txtBxr" type="text" runat="server" /><span style="color: Red">*</span>
                                </td>
                                <td class="tableBg2">
                                    经办科室
                                </td>
                                <td colspan="4">
                                    <input id="txtDept" readonly="readonly" type="text" runat="server" class="baseTextReadOnly" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    报销人/单位
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtbxr2" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button1" runat="server" Text="添加到常用" CssClass="baseButton" OnClick="btnAddbxrTOChangyong_Click" />
                                    <span style="color: Red">*</span>
                                </td>
                                <td class="tableBg2" colspan="2">
                                    报销人/单位电话
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtbxrdh" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnaddbxrdh" runat="server" Text="添加到常用" CssClass="baseButton" OnClick="btnaddbxrdh_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" colspan="2">
                                    报销人/单位开户行
                                </td>
                                <td colspan="2">
                                    <input type="text" runat="server" id="txt_khh" />
                                    <asp:Button ID="btnaddbxrkhh" runat="server" Text="添加到常用" CssClass="baseButton" OnClick="btnaddbxrkhh_Click" />
                                </td>
                                <td class="tableBg2" colspan="2">
                                    报销人/单位账号
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtbxzh" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnaddbxzh" runat="server" Text="添加到常用" CssClass="baseButton" OnClick="btnaddbxzh_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    报销明细类型
                                </td>
                                <td colspan="2" style="width: 200px">
                                    <asp:DropDownList ID="drpBxmxlx" runat="server" Width="142">
                                    </asp:DropDownList>
                                </td>
                                <td class="tableBg2" colspan="2">
                                    申请日期
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtSqrq" runat="server"></asp:TextBox><span style="color: Red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" colspan="2">
                                    报销摘要
                                </td>
                                <td colspan="7">
                                    <asp:TextBox ID="txtBxzy" runat="server" Width="720px"></asp:TextBox>
                                    <asp:Button ID="btnAddTOChangyong" runat="server" Text="添加到常用" CssClass="baseButton"
                                        OnClick="btnAddTOChangyong_Click" /><span style="color: Red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    报销说明
                                </td>
                                <td colspan="10">
                                    <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: red; height: 1px" colspan="9">
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" colspan="2">
                                    报销类型
                                </td>
                                <td colspan="7">
                                    <asp:RadioButton ID="rb_ok" runat="server" Text="归口报销" GroupName="is_gk" Checked="true" />
                                    <asp:RadioButton ID="rb_can" runat="server" Text="本部报销" GroupName="is_gk" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    费用科目
                                </td>
                                <td colspan="2">
                                    <input type="text" id="fykmname" readonly="readonly" class="baseTextReadOnly" runat="server" />
                                    <input type="button" value="选择费用科目" id="btnAddFykm" runat="server" class="baseButton" />
                                    <span style="color: Red">*</span>
                                </td>
                                <td colspan="5">
                                    <input type="button" value="选择归口部门" id="selectbxdept" runat="server" class="baseButton" /><span
                                        style="color: Red">*</span>
                                        <span  style="color: Red">如果您难以确定费用科目，请<a onclick="showhelpmsg();" href="#" style="color:Blue">点我<a/>。</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    费用科目
                                </td>
                                <td colspan="7">
                                    <table id="tab_fykm" class="myTable" style="width: 95%">
                                        <thead class="myGridHeader">
                                            <tr>
                                                <th>
                                                    科目
                                                </th>
                                                <th>
                                                    部门
                                                </th>
                                                <th>
                                                    预算金额
                                                </th>
                                                <th id="thLeavingAmount" runat="server">
                                                    剩余金额
                                                </th>
                                                <th>
                                                    已执行
                                                </th>
                                                <th id="thSaleAmount" runat="server">
                                                    销售提成金额
                                                </th>
                                                <th id="thAvailableAmount" runat="server">
                                                    可用金额
                                                </th>
                                                <th>
                                                    报销金额
                                                </th>
                                                <th style="display: none">
                                                    税额
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="body_fykm" runat="server">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    选择科室
                                </td>
                                <td colspan="7">
                                    <input type="button" value="选择核算科室" id="btn_choosedept" runat="server" class="baseButton" />
                                    <input class="baseButton" value="导出Excel模板" type="button" onclick="exportExcel2();" />
                                    <input type="button" value="Excel导入" id="btn_importdept" runat="server" class="baseButton" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    核算科室
                                </td>
                                <td colspan="7" id="td_dept" runat="server">
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2" class="tableBg2">
                                    项目选择
                                </td>
                                <td colspan="7">
                                    <input type="button" value="选择项目" id="btn_choosexm" runat="server" class="baseButton" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2" class="tableBg2">
                                    核算项目
                                </td>
                                <td colspan="7" id="td_xm" runat="server">
                                </td>
                            </tr>
                            <tr class="fykmRow">
                                <td colspan="2" class="tableBg2">
                                    合计金额小写
                                </td>
                                <td colspan="2" style="width: 200px">
                                    <input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px;
                                        text-align: right; background-color: #cccccc;" />
                                </td>
                                <td colspan="2" class="tableBg2" style="width: 200px">
                                    合计金额大写
                                </td>
                                <td colspan="3">
                                    <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" />
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: red; height: 1px" colspan="9">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    附加单据
                                </td>
                                <td colspan="7" style="text-align: right">
                                    <asp:DropDownList ID="selectBill" runat="server" CssClass="baseSelect" onchange="onSelectBillChanged(this.options[this.selectedIndex].value);">
                                        <asp:ListItem Value="">--选择附加单据--</asp:ListItem>
                                    </asp:DropDownList>
                                    <input id="btnFyspCx" value="附件撤销" type="button" class="baseButton" onclick="cancelAttachment();" />
                                    <input id="btnFysqXX" value="单据信息" type="button" class="baseButton" />
                                </td>
                            </tr>
                            <tr class="cgspInfo" runat="server" id="cgspInfo">
                                <td colspan="9">
                                    <table id="cgdj" width="100%">
                                        <thead>
                                            <tr class="myGridHeader">
                                                <td>
                                                    选择
                                                </td>
                                                <td>
                                                    单据编号
                                                </td>
                                                <td>
                                                    采购单位
                                                </td>
                                                <td>
                                                    承办人
                                                </td>
                                                <td>
                                                    申请日期
                                                </td>
                                                <td>
                                                    申请类别
                                                </td>
                                                <td>
                                                    采购总额
                                                </td>
                                                <td>
                                                    原因说明
                                                </td>
                                                <td>
                                                    单据状态
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody runat="server" id="tb_fysq">
                                        </tbody>
                                    </table>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 30px;">
                    <asp:HiddenField ID="hf_kmselect" runat="server" />
                    <asp:HiddenField ID="hd_djtype" runat="server" />
                    <asp:Button ID="btn_kmselect" runat="server" Text="Button" OnClick="btn_kmselect_Click"
                        CssClass="hiddenbill" />
                    <input type="button" value="保 存" id="btn_test" class="baseButton" runat="server" />
                    &nbsp;
                    <input type="button" value="保存并提交审核" id="btn_save_commit" class="baseButton" runat="server" />
                    &nbsp;
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <input type="button" onclick="javascript:window.close();" value="关 闭" class="baseButton" />&nbsp;
                    <asp:HiddenField ID="hdHsCCBG" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" />
                </td>
            </tr>
            <tr style="display: none;">
                <td style="text-align: center; height: 24px;">
                    <asp:Label ID="lblBillCode" runat="server"></asp:Label><asp:Label ID="lblType" runat="server"></asp:Label>
                    <asp:Button ID="btnRefresh" runat="server" Text="刷新脚本数据" />
                    <input type="button" id="btn_Print" runat="server" value="打印预览" class="baseButton" />
                    <asp:HiddenField ID="hdHasSaleRebate" runat="server" />
                    <asp:Button ID="reload" runat="server" CssClass="hiddenbill" />
                </td>
            </tr>
            <tr>
                <td>
                    <span style="color: Red">【友情提示】带*项目为必填项。</span>
                </td>
            </tr>
            <tr>
                <td>
                    审批流程：<span id="wf"></span>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
