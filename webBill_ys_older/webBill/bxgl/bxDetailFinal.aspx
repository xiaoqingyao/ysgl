<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxDetailFinal.aspx.cs" Inherits="webBill_bxgl_bxDetailFinal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=title %></title>
    <%--特冲车 海西 橡塑 --%>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        .item-help {
            border: 2px solid blue;
            height: auto;
            width: 300px;
            position: absolute;
            overflow-y: auto;
            font-size: 14px;
            padding: 15px;
            background-color: White;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        var selectCode = ''; //单据撤销和单据详细信息用到的选择单据的编号
        $(function () {
            htjeChange();
            change();
            var isgk = $("#rb_ok").attr("checked");
            if (!isgk) {
                $("#txt_gk").addClass("basehidden");
            }
            //附加单据的行单击
            //            $("#tb_fysq tr").click(function() {// tr
            //                alert($("#tb_fysq").text());
            //                //alert($("#tb_fysq").find('tr')[0].innerHTML);
            //                alert($("#tb_fysq tr").length);
            //                $("#tb_fysq tr").removeClass("highlight");
            //                $(this).addClass("highlight");
            //            });

            $("#tab_fykm tbody tr").live("click", function () {
                $("#tab_fykm tbody tr").removeClass("highlight");
                $(this).addClass("highlight");
                var thisid = this.id.split("_")[1];
                $("#td_dept ul").addClass("hiddenbill");
                $("#td_xm ul").addClass("hiddenbill");
                $("#bm_" + thisid).removeClass("hiddenbill");
                $("#xm_" + thisid).removeClass("hiddenbill");
            });
            //附件撤销
            //            $("#btnFyspCx").click(function() {
            //                var checkrow = $(".highlight");
            //                if (checkrow.val() == undefined) {
            //                    alert("请先选择行");
            //                    return;
            //                }
            //                $(".highlight").remove();
            //            });
            //查看信息 edit by lvcc
            $("#btnFysqXX").click(function () {
                //                var tbody = $("#tb_fysq");
                //                var trLength = tbody.children().length;
                //                var flg = false;
                //                var index = -1;
                //                for (var i = 0; i < trLength; i++) {
                //                    var iRel = tbody.children()[i].children[0].innerHTML.indexOf("CHECKED");
                //                    if (iRel != '-1') {
                //                        flg = true;
                //                        index = i;
                //                        break;
                //                    }
                //                }
                //                if (!flg) {
                //                    alert("请选中要操作的记录！");
                //                    return;
                //                }
                //                var billcode = tbody.children()[index].children[1].innerHTML;

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
            $("#btn_ok").click(function () {
                if (confirm("是否确定审核")) {
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
                }

            });
            //否决单据
            $("#btn_cancel").click(function () {
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
            $("#btn_test").click(function () {
                var type = '<%=Request["type"] %>';

                var dydj = '<%=Request["dydj"]%>';


                if (type == null || type == undefined || type == "") {
                    alert("未获取到操作状态，无法保存！"); return;
                }
                //检测必填项

                var zy = $("#txtBxzy").val();
                if (zy == undefined || zy == '') {
                    if (dydj == "02") {
                        alert("请填写款项用途");
                        return;

                    } else {
                        alert("请填写报销摘要");
                        return;
                    }

                }

                var json = jsonMaker();


                if (json == "") {
                    alert("部门分配金额与科目金额不相等!");
                    return;
                }

                var dictype = $("#hddictype").val();


                var dydj = '<%=Request["dydj"]%>';

                if (dydj == null && dydj == '' && dydj == undefined) {
                    dydj = "02";
                }

                //如果是新财年，判断费用报销单的金额不能大于用款申请单的总金额
                if (dydj != null && dydj != undefined && dydj != '' && dydj == '06') {


                    var xcn = $("#drp_xcn").val();

                    if (xcn == "是") {

                        //获取费用报销单的金额
                        var bxdje = $("#txtHjjeXx").val();
                        //获取用款申请单//tab_yksq tbody
                        var bills = $("#tab_yksq tbody tr").length;
                        if (bills == 0) {
                            alert("如果是新财年报销，请选择对应的用款申请单");
                            return;
                        }
                        var yksqje = 0;
                        $("#tab_yksq tbody tr").each(function () {
                            yksqje += parseFloat($(this).find("td").eq(1).html());
                        });


                        //获取用款申请单的金额

                        if (parseFloat(bxdje) > parseFloat(yksqje)) {
                            alert("费用报销单的金额不能大于用款申请单总金额");
                            return;
                        }
                    }

                }
                $.post("../MyAjax/YbbxBillSave.ashx?type=" + type + "&dictype=" + dictype + "&dydj=" + dydj, json, function (data, status) {
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
                        } else if (data == "yksq") {
                            alert("用款申请单明细保存失败");
                        }
                        else if (data == "smbt") {
                            alert("请填写报销说明！");
                        } else if (data == "wrong") {
                            alert("该时间段没有设置财年，请选择正确的财年时间。");
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
            });
            function jsonMaker() {
                var fjname = $("#Lafilename").text();
                var fjurl = $("#hiddFileDz").val();
                var fujian = "";
                if (fjname.length > 0 && fjurl.length > 0)
                    fujian = escape(fjname + "|" + fjurl);
                //构造提交服务器的字符串
                var djtype = $("#hd_djtype").val();

                var pd_test = true;
                var billcode = $("#HiddenField1").val();
                var usercode = $("#txtBxr").val();
                usercode = usercode.split(']')[0];
                usercode = usercode.substring(1, usercode.length);
                var varzh = $("#txtbxzh").val(); //报销人账号
                var varkhh = $("#txt_khh").val(); //开户行
                varzh = varkhh + "|&|" + varzh;

                var bxr = $("#txtJbr").val(); //报销人
                bxr = bxr.split(']')[0];
                bxr = bxr.substring(1, bxr.length);
                var bxzy = $("#txtBxzy").val();
                var bxsm = $("#txtBxsm").val();
                var bxlx = $("#drpBxmxlx").val();
                var isgk = $("#rb_ok").attr("checked");
                var bxDate = $("#txtSqrq").val();
                if (isgk) {
                    gkbmbh = $("#txt_gk").val();
                } else {
                    gkbmbh = $("#txtDept").val();
                }
                gkbmbh = gkbmbh.split(']')[0];
                gkbmbh = gkbmbh.substring(1, gkbmbh.length);
                var isxkfx = $("#drp_xcn").val();

                var sqlx = $("#ddl_ykdlx").val();
                var ykfs = $("#ddl_ykfs").val();
                var fysqCode = "";
                $("#tab_yksq tbody tr").each(function () {
                    fysqCode += $(this).find("td").eq(0).html() + ',';
                })
                var itbodytrCount = $("#cgdj tbody tr").length;
                for (var i = 0; i < itbodytrCount; i++) {
                    fysqCode += $("#cgdj tbody ").find('tr')[i].children[1].innerHTML + "|";
                }
                if (fysqCode.length > 0) {
                    fysqCode = fysqCode.substring(0, fysqCode.length - 1);
                }
                var djs = $("#txt_djs").val();
                var ret = '{"fujian":"' + fujian + '","djlx":"' + djtype + '","billuser":"' + usercode + '","bxr":"' + bxr + '","bxrzh":"' + varzh + '","zy":"' + bxzy + '","sm":"' + bxsm + '","bxlx":"' + bxlx + '","gkbmbh":"' + gkbmbh + '","billcode":"' + billcode + '","fysq":"' + fysqCode + '","bxDate":"' + bxDate + '","sfgf":"0","djs":"' + djs + '","isgk":"' + isgk + '","sqlx":"' + sqlx + '","ykfs":"' + ykfs + '","yksqcode":"' + fysqCode + '","isxkfx":"' + isxkfx + '","list":[';
                //var ret = '{"fujian":"' + fujian + '","djlx":"' + djtype + '","billuser":"' + usercode + '","bxr":"' + bxr + '","bxrzh":"' + varzh + '","zy":"' + bxzy + '","sm":"' + bxsm + '","bxlx":"' + bxlx + '","gkbmbh":"' + gkbmbh + '","billcode":"' + billcode + '","fysq":"' + fysqCode + '","bxDate":"' + bxDate + '","sfgf":"0","djs":"' + djs + '","isgk":"' + isgk + '","list":[';
                //预算科目明细
                $("#tab_fykm tbody tr").each(function (i) {
                    var tempkm = $(this).find("td:eq(0)").html();
                    tempkm = tempkm.split(']')[0];
                    tempkm = tempkm.substring(1, tempkm.length);
                    var tempje = $(this).find("input:eq(0)").val(); //金额
                    var tempse = $(this).find("input:eq(1)").val(); //税额
                    ret += '{"km":"' + tempkm + '","je":"' + tempje + '","se":"' + tempse + '","bm":[';
                    var bmret = "";

                    var kmIndex = this.id.split("_")[1];
                    $("#bm_" + kmIndex + " li").each(function () {
                        var tempbmbh = $(this).find("span").html();
                        tempbmbh = tempbmbh.split(']')[0];
                        tempbmbh = tempbmbh.substring(1, tempbmbh.length);
                        var tempbmje = $(this).find("input").val();
                        bmret += '{"bmbh":"' + tempbmbh + '","bmje":"' + tempbmje + '"},';
                    });

                    bmret = bmret.substring(0, bmret.length - 1);
                    ret += bmret + '] ,"xm":[';
                    var xmret = "";

                    //2014-07-01 edit by zyl 项目核算模式配置项添加后的修改
                    var hsxmModel = '<%=hsxmModel%>';
                    if (hsxmModel == "1") {
                        $("#xm_" + kmIndex + " li table tbody tr").each(function () {
                            var tempxmbh = $(this).find("td:eq(0)").html();
                            tempxmbh = tempxmbh.split(']')[0];
                            tempxmbh = tempxmbh.substring(1, tempxmbh.length);
                            var tempxmje = $(this).find("input").val();
                            xmret += '{"xmbh":"' + tempxmbh + '","xmje":"' + tempxmje + '"},';
                        });
                    }
                    else {
                        $("#xm_" + kmIndex + " li").each(function () {
                            var tempxmbh = $(this).find("span").html();
                            tempxmbh = tempxmbh.split(']')[0];
                            tempxmbh = tempxmbh.substring(1, tempxmbh.length);
                            var tempxmje = $(this).find("input").val();
                            xmret += '{"xmbh":"' + tempxmbh + '","xmje":"' + tempxmje + '"},';
                        });
                    }
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
                select: function (event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": escape(rybh) }, function (data, status) {
                        if (status == "success") {
                            $("#txtDept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            $("#txtSqrq").datepicker();

            $("#btn_choosedept").click(function () {
                var checkRowid = $("#tab_fykm .highlight").attr("id");
                if (checkRowid == undefined) {
                    alert("请选择行!");
                    return;
                }
                checkRowid = checkRowid.split("_")[1];
                openBm(checkRowid);
            });
            //批量导入部门
            $("#btn_importdept").click(function () {
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
            $("#btn_choosexm").click(function () {
                var current = $("#tab_fykm .highlight");

                if (current.attr("id") == undefined) {
                    alert("请选择行!");
                    return;
                }
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
                var isgk = $("#rb_ok").attr("checked");
                if (isgk) {
                    //                    gkbmbh = $("#txt_gk").val();20120716修改，无论什么费用都需要选择制单部门的项目
                    gkbmbh = $("#txt_gk").val(); //20131105修改   根据特种车要求  归口的选择归口部门的项目
                    //gkbmbh = $("#txtDept").val();

                } else {
                    gkbmbh = $("#txtDept").val();
                }
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }

                checkRowid = checkRowid.split("_")[1];
                var hsxmModel = '<%=hsxmModel%>';
                if (hsxmModel == "1")
                    openXmHigh(gkbmbh, checkRowid);
                else
                    openXmLow(gkbmbh, checkRowid);
            });

            $("#rb_can").click(function () {
                $("#fykm").accordion('destroy');
                $("#fykm").html("");
                $("#txt_gk").addClass("basehidden");
            });
            var gkbm_cache = {};
            //选择费用科目
            $("#btnAddFykm").click(function () {
                var gkbmbh = "";
                var isgk = $("#rb_ok").attr("checked");
                if (isgk) {
                    gkbmbh = $("#txt_gk").val();
                } else {
                    gkbmbh = $("#txtDept").val();
                }
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openKm(gkbmbh, isgk);
            });
            //选择用款申请单
            $("#btn_yksq").click(function () {
                openyksq()
            });
            $("#rb_ok").click(function () {
                $("#txt_gk").removeClass();
                $("#fykm").accordion('destroy');
                $("#fykm").html("");
                $.post("../MyAjax/GetDept.ashx", { "action": "gk", "code": "" }, function (data, status) {
                    if (status == "success") {
                        gkbm_cache = $.parseJSON(data);
                        $("#txt_gk").autocomplete({
                            source: gkbm_cache
                        });
                    }
                    else {
                        alert("获取部门失败");
                    }
                });
            });
            $("body").keypress(function (event) {
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

            //部门选择 归口
            $("#txt_gk").autocomplete({
                source: availableTagsDept
            });
        });
        //
        function change() {
            var zt = $("#drp_xcn").val();

            if (zt == "是") {
                $("#btn_yksq").attr("disabled", "");
            }
            if (zt == "否") {
                $("#btn_yksq").attr("disabled", "true");
            }
        }
        function htjeChange() {
            var je = 0;
            $(".ysje").each(function () {
                je += Number($(this).val()) * 100;
            });
            $(".ysse").each(function () {
                je += Number($(this).val()) * 100;
            });
            je = je.toFixed();
            $("#txtHjjeXx").val(je / 100);
            $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
        }
        function openBm(dvid) {
            var deptCode = "";
            $("#bm_" + dvid + " span").each(function () {
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
        function openXmHigh(deptCode, dvid) {
            var xmCode = "";
            var billDate = $("#txtSqrq").val();
            $("#xm_" + dvid + " table tbody tr").each(function () { xmCode += $(this).find("td:eq(0)").html() + ":"; });
            var tempurl = "YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode;
            var str = window.showModalDialog("YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode + "&billDate=" + billDate, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            //alert(str);
            if (str != undefined && str != "") {
                var xms = $.parseJSON(str);

                var innerval = "";
                innerval += "<li class='li_xm' ><table  class='myTable'  style='width: 95%;margin:0px;'>";
                innerval += "<thead class='myGridHeader'   style='height:0px;'><tr><th>核算项目</th><th>最大核算金额</th><th>控制项目金额</th><th>可用金额</th><th>金额</th><tr></thead><tbody  >";
                for (var i = 0; i < xms.length; i++) {
                    //innerval += "<li><span>" + xms[i].xmmc + "&nbsp剩余金额:<input type='text' id='txt_xm_syje' value='"+xms[i].syje+"' /></span><input type='text' value='0.00' id='txt_xm_tbje' />  <input type='hidden' name='hf_dept' id='hf_dept'  value='"+xms[i].syje+"'/></li>";                   
                    var jje = xms[i].ctrl == "否" ? "0.00" : xms[i].syje;
                    innerval += "<tr><td>" + xms[i].xmmc + "</td><td>" + xms[i].je + "</td><td>" + xms[i].ctrl + "</td><td>" + jje + "</td><td><input type='text' class='baseText '  value='0.00' /></td></tr>";
                }
                innerval += "</tbody></table></li>";
                $("#xm_" + dvid).html(innerval);
            }
        }
        function openXmLow(deptCode, dvid) {
            var xmCode = "";
            $("#xm_" + dvid + " span").each(function () { xmCode += $(this).html(); });
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
        //添加电子发票单 edit by wpp 2017 .03.31(如果需要完善可以找我 :)  )
        function openfp() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('selectfp.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            //alert(tempInner);
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                var num = $("#tb_fysq").find('tr').length;
                var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);;'/></td>" + strTemp;
                alert(endStr);
                //return;
                $("#cgdj tbody").append(endStr);
            }
        }
        //添加
        function openyksq() {
            var inner = "";
            var str = window.showModalDialog("../select/selectYksq.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:900px;status:no;scroll:yes');
            var zje = 0;
            var ykcode = "";
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);

                var checkArray = new Array();
                var ykIndex = $("#tab_yksq tbody tr").last().attr("id");
                if (ykIndex == undefined || ykIndex == "") {
                    ykIndex = 1;
                }
                else {
                    ykIndex = (ykIndex.split("_")[1]) * 1 + 1;
                }

                for (var i = 0; i < json.length; i++) {

                    if (json[i].Yksqcode.length > 0) {
                        inner += '<tr id="tr_' + ykIndex + '">';
                        inner += "<td  style='text-align:center'>" + json[i].Yksqcode + "</td>";
                        inner += "<td>" + json[i].yksqje + "</td>";
                        inner += "<td><input type='button' style='text-align:center' class='baseButton' value='删除' onclick='del(this)'/></td>";
                        inner += "</tr>";

                        zje += parseFloat(json[i].yksqje);
                        ykcode += json[i].Yksqcode + ",";
                        ykIndex++;
                    }

                    //var checkyk = checkArray.join();
                    //$("#tab_yksq tbody tr").each(function () {
                    //    var tempykCode = $(this).find("td:eq(0)").html();
                    //    if (checkyk.indexOf(tempykCode) < 0) {
                    //        var delIndex = this.id.split("_")[1];
                    //        //$("#xm_" + delIndex).remove();
                    //        //$("#bm_" + delIndex).remove();
                    //        $(this).remove();
                    //    }
                    //});

                }

                $("#tab_yksq tbody").append(inner);

            }
        }
        function del(obj) {
            $(obj).parent().parent().remove();
        }

        function openKm(deptCode, isGk) {
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function (i) {
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
            var hashdxmhs = $("#hdxmhs").val();
            var dydj = '<%=Request["dydj"]%>';
            if (dydj == null && dydj == '' && dydj == undefined || dydj == '06') {
                dydj = "02";
            }
            var str = window.showModalDialog("YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate + "&dydj=" + dydj, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
                var inner = "";
                var checkArray = new Array();

                for (var i = 0; i < json.length; i++) {
                    if (kmcode.indexOf(json[i].Yscode) < 0) {
                        inner += '<tr id="tr_' + kmIndex + '">';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        if (hashdxmhs == "1") {
                            inner += "<td>" + json[i].XiangMuHeSuan + "</td>";
                        }

                        inner += "<td>" + json[i].Ysje + "</td>";
                        if (hasSaleRebateFlg != "1") {
                            inner += "<td>" + json[i].Syje + "</td>";
                        } else {
                            inner += "<td style='display:none'>" + json[i].Tcje + "</td>";
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
                $("#tab_fykm tbody tr").each(function () {
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
            } else if (strBillVal == "fp") {//发票
                openfp();
            }
            else { }
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
        //附件撤销 edit by lvcc 20120906  终于搞定了O(∩_∩)O~
        //        function cancelAttachment() {
        //            var tbody = $("#tb_fysq");
        //            var trLength = tbody.children().length;
        //            var flg = false;
        //            var index = -1;
        //            for (var i = 0; i < trLength; i++) {
        //                var iRel = tbody.children()[i].children[0].innerHTML.indexOf("CHECKED");
        //                if (iRel != '-1') {
        //                    flg = true;
        //                    index = i;
        //                    break;
        //                }
        //            }
        //            if (!flg) {
        //                alert("请选中要操作的记录！");
        //                return;
        //            }
        //            var tableMain = $("#cgdj");
        //            var nowTr = tbody.children()[index];
        //            nowTr.parentNode.removeChild(nowTr);
        //        }
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
            window.open(url, "newwindow2", 'height=100,width=400,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
        }


        //显示标记的备注信息
        $(function () {
            $('#markImg').mouseover(function (e) {
                var xx = e.originalEvent.x || e.originalEvent.layerX || 0;
                var yy = e.originalEvent.y || e.originalEvent.layerY || 0;
                $('#item-help').css({ top: yy, left: xx }); //注意这是用css的top和left属性来控制div的。
                var mark = '<%=altMark %>';
                if (mark.length > 0) {
                    $("#item-help").html(mark);
                    $("#item-help").show();
                }
            }).mouseout(function () {
                $("#item-help").hide();
            });
        });
    </script>

    <style type="text/css">
        .style1 {
            background-color: #EDEDED;
        }

        ul {
            list-style: none;
            margin-left: 0px;
            margin-top: 0px;
            padding-left: 5px;
        }

            ul li {
                margin: 5px 5px 5px 5px;
            }

        #tab_fykm {
            margin-top: 5px;
            margin-bottom: 5px;
        }

        #td_dept input {
            width: 100px;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%" border="0">
                <tr>
                    <td style="text-align: center; height: 36px;">
                        <strong>
                            <asp:Label ID="lbdjmc" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

                        </strong><span style="margin-left: 20px" runat="server" id="markImg" visible="false">
                            <img src="../Resources/Images/mark.png" width="80" height="25" alt='' />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="margin: 0 auto; width: 99%;">
                            <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                                <tr>
                                    <td class="tableBg2" style="width: 80px;">
                                        <asp:Label runat="server" ID="lbl_Jbr" Text="经办人"></asp:Label>
                                    </td>
                                    <td style="width: 100px">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtJbr" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </td>
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lbl_bxr" Text="报销人"></asp:Label>

                                    </td>
                                    <td>
                                        <div style="margin-left: 2px">
                                            <input id="txtBxr" style="width: 100px" type="text" runat="server" />
                                        </div>
                                    </td>
                                    <td class="tableBg2">
                                        <asp:Label ID="lbl_dept" runat="server" Text="所在部门"></asp:Label>

                                    </td>
                                    <td>
                                        <div style="margin-left: 2px">
                                            <input id="txtDept" style="width: 100px" readonly="readonly" type="text" runat="server" /><input id="txtbxdept"
                                                readonly="readonly" style="display: none; width: 1px" type="text" runat="server" />
                                        </div>
                                    </td>

                                </tr>


                                <tr>
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lblDate" Text="申请日期"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtSqrq" runat="server" Style="width: 100px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lb_bxmxlx" Text="报销明细类型"></asp:Label>
                                    </td>
                                    <td style="width: 100px">
                                        <div style="margin-left: 2px">
                                            <asp:DropDownList ID="drpBxmxlx" Width="100px" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td style="text-align: right">支付方式
                                    </td>
                                    <td style="margin-left: 2px">
                                        <div style="margin-left: 2px;">
                                            <asp:DropDownList ID="ddl_ykfs" runat="server" Width="151px">
                                                <asp:ListItem Value="现金">现金</asp:ListItem>
                                                <asp:ListItem Value="转账">转账</asp:ListItem>
                                                <asp:ListItem Value="支票">支票</asp:ListItem>
                                                <asp:ListItem Value="电汇">电汇</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td style="display: none" id="yklxtd1" class="tableBg2" runat="server">
                                        <asp:Label ID="lbl_djlx" runat="server"></asp:Label>
                                    </td>
                                    <td id="yklxtd2" runat="server" style="display: none">
                                        <div style="margin-left: 2px">
                                            <asp:DropDownList ID="ddl_ykdlx" runat="server" Width="151px">
                                                <asp:ListItem Value="fxl">付现类</asp:ListItem>
                                                <asp:ListItem Value="jkfl">讲课费类</asp:ListItem>
                                                <asp:ListItem Value="fdl">返点类</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>

                                </tr>
                                <tr id="tr_yh" runat="server">
                                    <td class="tableBg2">开户银行
                                    </td>
                                    <td>
                                        <div style="margin-left: 2px">
                                            <input type="text" runat="server" id="txt_khh" />
                                        </div>
                                    </td>
                                    <td class="tableBg2">银行账号 

                                    </td>
                                    <td colspan="3">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtbxzh" runat="server"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tableBg2">

                                        <asp:Label runat="server" ID="lbl_bxzy" Text="报销摘要"></asp:Label>

                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtBxzy" runat="server" Width="810"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>


                                <tr>
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lbl_bxsm" Text="报销说明"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine" Width="810"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lbl_bxdj" Text="报销单据"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div style="border-bottom: 1px dashed #CDCDCD; height: 20px;">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:FileUpload ID="upLoadFiles" runat="server" Width="100px" />
                                            <asp:Label ID="Lafilename" runat="server" Text=""></asp:Label>
                                            <asp:HiddenField ID="hiddFileDz" runat="server" />
                                            <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />
                                            <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            <div id="divBxdj" runat="server">
                                            </div>
                                        </div>
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    </td>
                                    <td class="tableBg2">
                                        <asp:Label ID="lbl_djs" runat="server" Text="单据数："></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txt_djs" Width="90" runat="server"></asp:TextBox>
                                        </div>
                                    </td>

                                </tr>

                                <tr>
                                    <td style="background-color: red; height: 4px" colspan="6"></td>
                                </tr>
                                <tr id="trGk" runat="server">
                                    <td class="tableBg2">归口预算
                                    </td>
                                    <td colspan="2">
                                        <div style="margin-left: 2px">
                                            <asp:RadioButton ID="rb_ok" runat="server" Text="是" GroupName="is_gk" />
                                            <asp:RadioButton ID="rb_can" runat="server" Text="否" GroupName="is_gk" Checked="true" />
                                        </div>
                                    </td>
                                    <td class="tableBg2">归口部门
                                    </td>
                                    <td id="gkbm" colspan="2">
                                        <div id='dv_gk' style="margin-left: 2px">
                                            <span>
                                                <input type='text' id='txt_gk' runat="server" /></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBg2">
                                        <asp:Label ID="lbl_yskm" runat="server" Text="预算科目"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <input type="button" value="选择预算科目" id="btnAddFykm" runat="server" class="baseButton" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBg2">单据明细
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <table id="tab_fykm" class="myTable" style="width: 95%">
                                                <thead class="myGridHeader">
                                                    <tr>
                                                        <th>科目
                                                        </th>
                                                        <th id="thxmhs" runat="server">项目核算
                                                        </th>
                                                        <th>预算金额
                                                        </th>
                                                        <th id="thLeavingAmount" runat="server">剩余金额
                                                        </th>
                                                        <th style="display: none" id="thSaleAmount" runat="server">销售提成金额
                                                        </th>
                                                        <th id="thAvailableAmount" runat="server">可用金额
                                                        </th>
                                                        <th>金额
                                                        </th>
                                                        <th>税额
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="body_fykm" runat="server">
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trbm1" runat="server">
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lblHsbm1" Text="选择核算部门"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <input type="button" value="选择部门" id="btn_choosedept" runat="server" class="baseButton" />
                                            <input class="baseButton" value="导出Excel模板" type="button" onclick="exportExcel2();" />
                                            <input type="button" value="Excel导入" id="btn_importdept" runat="server" class="baseButton" />
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trbm2" runat="server">
                                    <td class="tableBg2">
                                        <asp:Label runat="server" ID="lblHsbm2" Text="核算部门"></asp:Label>
                                    </td>
                                    <td colspan="5" id="td_dept" runat="server"></td>
                                </tr>
                                <tr id="trXm1" runat="server">
                                    <td class="tableBg2">项目选择
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <input type="button" value="选择项目" id="btn_choosexm" runat="server" class="baseButton" />
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trXm2" runat="server">
                                    <td class="tableBg2">核算项目
                                    </td>
                                    <td colspan="5" id="td_xm" runat="server"></td>
                                </tr>
                                <tr class="fykmRow">
                                    <td class="tableBg2">合计金额小写
                                    </td>
                                    <td colspan="2" style="width: 200px">
                                        <div style="margin-left: 2px">
                                            <input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px; text-align: right; background-color: #cccccc;" />
                                        </div>
                                    </td>
                                    <td class="tableBg2" style="width: 200px">合计金额大写
                                    </td>
                                    <td colspan="2">
                                        <div style="margin-left: 2px">
                                            <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" />
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trMark" visible="false">
                                    <td class="tableBg2">单据标记</td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <asp:TextBox ID="txtMark" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnMark" runat="server" Text="标记" OnClick="btnMark_Click" CssClass="baseButton" />
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trfgx3" runat="server">
                                    <td style="background-color: red; height: 4px" colspan="9"></td>
                                </tr>
                                <tr runat="server" id="fujiadj">
                                    <td class="tableBg2">附加单据
                                    </td>
                                    <td colspan="5" style="text-align: right">
                                        <div style="margin-left: 2px">
                                            <asp:DropDownList ID="selectBill" runat="server" CssClass="baseSelect" onchange="onSelectBillChanged(this.options[this.selectedIndex].value);">
                                                <asp:ListItem Value="">--选择附加单据--</asp:ListItem>
                                            </asp:DropDownList>
                                            <input id="btnFyspCx" value="附件撤销" type="button" class="baseButton" onclick="cancelAttachment();" />
                                            <input id="btnFysqXX" value="单据信息" type="button" class="baseButton" />
                                        </div>
                                    </td>
                                </tr>
                                <tr class="cgspInfo" runat="server" id="cgspInfo">
                                    <td colspan="6">
                                        <table id="cgdj" width="100%">
                                            <thead>
                                                <tr class="myGridHeader">
                                                    <td>选择
                                                    </td>
                                                    <td>单据编号
                                                    </td>
                                                    <td>采购单位
                                                    </td>
                                                    <td>承办人
                                                    </td>
                                                    <td>申请日期
                                                    </td>
                                                    <td>申请类别
                                                    </td>
                                                    <td>采购总额
                                                    </td>
                                                    <td>原因说明
                                                    </td>
                                                    <td>单据状态
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody runat="server" id="tb_fysq">
                                            </tbody>
                                        </table>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                    </td>
                                </tr>

                               <%-- <tr class="cgspInfo" runat="server" id="trfp">
                                    <td colspan="6">
                                        <table id="tdfp" width="100%">
                                            <thead>
                                                <tr class="myGridHeader">
                                                    <td>选择
                                                    </td>
                                                    <td>发票单号
                                                    </td>
                                                    <td>部门
                                                    </td>
                                                    <td>申请人
                                                    </td>
                                                    <td>发票日期
                                                    </td>
                                                    <td>总金额
                                                    </td>
                                                    <td>备注
                                                    </td>                                                                                                    
                                                </tr>
                                            </thead>
                                            <tbody runat="server" id="body_fp">
                                            </tbody>
                                        </table>
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                    </td>
                                </tr>--%>
                                <tr id="tr_dysqd" runat="server" style="display: none">
                                    <td style="text-align: right; width: 100px" colspan="2">对应用款申请单
                                     
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px;">
                                            <asp:DropDownList ID="drp_xcn" runat="server" AutoPostBack="false" onchange="change()">
                                                <asp:ListItem Value="是" Selected="True">新财年报销</asp:ListItem>
                                                <asp:ListItem Value="否">老财年报销</asp:ListItem>
                                            </asp:DropDownList>
                                            <input type="button" id="btn_yksq" value="选择用款申请单" runat="server" class="baseButton" />
                                        </div>

                                    </td>
                                </tr>
                                <tr id="tr_sqdmx" runat="server" style="display: none">
                                    <td style="text-align: right" colspan="2">申请单明细
                                    </td>
                                    <td colspan="5">
                                        <div style="margin-left: 2px">
                                            <table id="tab_yksq" class="myTable" style="width: 95%">
                                                <thead class="myGridHeader">
                                                    <tr>
                                                        <th>申请单编号
                                                        </th>

                                                        <th>单据金额
                                                        </th>
                                                        <th>操作
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="body_yksqmx" runat="server">
                                                </tbody>
                                            </table>
                                        </div>

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
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <input type="button" onclick="javascript: window.close();" value="关 闭" class="baseButton" />&nbsp;
                    <asp:HiddenField ID="hdHsCCBG" runat="server" />
                        <asp:HiddenField ID="hddictype" runat="server" />
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
                        <asp:HiddenField ID="hdxmhs" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <%--用于显示鼠标悬浮提示的信息--%>
        <div class="item-help" id="item-help" style="display: none;">
        </div>
    </form>
</body>
</html>
