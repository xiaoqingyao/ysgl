<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jkDetailForDz.aspx.cs" Inherits="webBill_bxgl_jkDetailForDz" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--  <%=title %>--%>
    <title>退费申请单</title>

    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="ajaxfileupload.js"></script>
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

        table input {
            width: 150px;
        }

        table select {
            width: 158px;
        }

        table input[type=button] {
            width: 100px;
        }

        table input[type=radio] {
            width: 20px;
        }
        /*审核展示table*/
        .auditTable {
            border-spacing: 0;
        }

            .auditTable td {
                border-right: 0px solid;
                border-bottom: 1px dashed;
                height: 22px;
            }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        var selectCode = ''; //单据撤销和单据详细信息用到的选择单据的编号
        $(function () {
            htjeChange();
            //  change();
            var isgk = $("#rb_ok").attr("checked");
            if (!isgk) {
                $("#txt_gk").addClass("basehidden");
            }

            $("#tab_fykm tbody tr").live("click", function () {

                $("#tab_fykm tbody tr").removeClass("highlight");
                $(this).addClass("highlight");
                var thisid = this.id.split("_")[1];

                $("#td_dept ul").addClass("hiddenbill");
                $("#td_xm ul").addClass("hiddenbill");
                $("#bm_" + thisid).removeClass("hiddenbill");
                $("#xm_" + thisid).removeClass("hiddenbill");
            });

            //审核单据
            $("#btn_ok").click(function () {
                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();
                billcode = billcode + "*" + mind + ",";
                billcode = escape(billcode);
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                        parent.closeDetail();
                    }
                }
            });

            $("#btn_cancel").click(function () {
                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();


                if (billcode == "") {
                    alert("请选择驳回的记录。");
                    return;
                }
                //window.showModalDialog(, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes')
                $("#prodcutDetailSrc").attr("src", "../MyWorkFlow/DisAgreeToSpecial.aspx?billCode=" + billcode + "&mind=" + mind);
                $("#dialog-confirm").dialog(
                    {
                        modal: true,             // 创建模式对话框
                        autoOpen: true,//是否自动打开
                        height: 400, //高度
                        width: 600, //宽度
                        title_html: true,
                        title: '审批驳回'
                    }
                );
            });
          <%--  //否决单据
            $("#btn_cancel").click(function () {
                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();
                billcode = billcode + "*" + mind + ",";
                billcode = escape(billcode);
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                } else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": "", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });--%>
            //保存
            $("#btn_test").click(function () {
                var type = '<%=Request["type"] %>';
                var dydj = '<%=Request["dydj"]%>';
                if (type == null || type == undefined || type == "") {
                    alert("未获取到操作状态，无法保存！"); return;
                }
                //检测必填项
                var teacher = $("#txt_teacher").val();
                if (teacher.length == 0) {
                    alert("请填写任课讲师");
                    return;
                }
                var djs = $("#txt_djs").val();

                if (djs == undefined || djs == "" || djs == "0") {
                    if (!confirm("确定附件数为0吗?")) {
                        $("#txt_djs").focus();
                        return;
                    }
                }
                var json = jsonMaker();

                if (json == "") {
                    alert("部门分配金额与科目金额不相等!");
                    return;
                }
                //var dictype = $("#hddictype").val();
                var dydj = '<%=Request["dydj"]%>';
                if (dydj == null && dydj == '' && dydj == undefined) {
                    dydj = "02";
                }

                $.post("../MyAjax/JkbxBillSave_Dz.ashx?type=" + type + "&dydj=" + dydj, json, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("保存成功");
                            parent.closeDetail();
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
                //var fjname = $("#hidfilnename").val();
                //var fjurl = $("#hiddFileDz").val();
                //var fujian = "";
                var fjname = "";
                $(".fujianname").each(function (i, d) {
                    fjname += $(this).val() + ";";
                });
                var fjurl = "";
                $(".fujianurl").each(function (i, d) {
                    fjurl += $(this).val() + ";";
                });
                var fujian = "";
                if (fjname.length > 0 && fjurl.length > 0)
                    fujian = escape(fjname + "|" + fjurl);
                //构造提交服务器的字符串
                var djtype = $("#hd_djtype").val();
                var billname = $("#HiddenField1").val();
                var pd_test = true;
                var billcode = $("#HiddenField1").val();
                var usercode = $("#txtBxr").val();
                usercode = usercode.split(']')[0];
                usercode = usercode.substring(1, usercode.length);
                var varzh = $("#txtbxzh").val(); //报销人账号
                var varkhh = $("#txt_khh").val(); //开户行

                varzh = varkhh + "|&|" + varzh;

                var bxzy = $("#txtBxzy").val();
                var bxsm = $("#txtBxsm").val();
                var isgk = $("#rb_ok").attr("checked");
                var bxDate = $("#txtSqrq").val();
                if (isgk) {
                    gkbmbh = $("#txt_gk").val();
                } else {
                    gkbmbh = $("#txtDept").val();
                }
                gkbmbh = gkbmbh.split(']')[0];
                gkbmbh = gkbmbh.substring(1, gkbmbh.length);

                var djs = $("#txt_djs").val();
                var szfx = $("#txt_stuschool").val();
                var xyxm = $("#txt_stuname").val();
                var sznj = $("#txt_stuclass").val();
                var xybh = $("#txt_stucode").val();
                var xdsj = $("#txt_qdtime").val();
                var tfxx = szfx + "|&|" + xyxm + "|&|" + sznj + "|&|" + xybh + "|&|" + xdsj;
                var xyfdfy = $("#txt_xyfdfy").val();
                var yxfks = $("#txt_yxfks").val();
                var dyksdj = $("#txt_dyksdj").val();
                var yxffy = $("#txt_yxffy").val();
                var ykqtfy = $("#txt_ykqtfy").val();
                var teacher = $("#txt_teacher").val();
                var xfqk = xyfdfy + "|&|" + yxfks + "|&|" + dyksdj + "|&|" + yxffy + "|&|" + ykqtfy;
                var ret = '{"billcode":"' + billcode + '","fujian":"' + fujian + '","djlx":"' + djtype + '","billuser":"' + usercode + '","bxr":"' + usercode + '","bxrzh":"' + varzh + '","zy":"' + bxzy + '","sm":"' + bxsm + '","bxlx":"03","tfxx":"' + tfxx + '","xfqk":"' + xfqk + '","gkbmbh":"' + gkbmbh + '","bxDate":"' + bxDate + '","sfgf":"0","gfje":"0","djs":"' + djs + '","isgk":"' + isgk + '","sqlx":"","ykfs":"","isxkfx":"是","bxr2":"","note2":"' + teacher +'","bxrphone":"","list":[';
                //预算科目明细
                $("#tab_fykm tbody tr").each(function (i) {
                    var tempkm = $(this).find("td:eq(0)").html();//科目
                    tempkm = tempkm.split(']')[0];
                    tempkm = tempkm.substring(1, tempkm.length);
                    var tempbm = $(this).find("td:eq(1)").html(); //部门
                    tempbm = tempbm.split(']')[0];
                    tempbm = tempbm.substring(1, tempbm.length);
                    var tempje = $(this).find("input:eq(0)").val(); //金额
                    var tempse = $(this).find("input:eq(1)").val(); //税额
                    ret += '{"km":"' + tempkm + '","bxbm":"' + tempbm + '","je":"' + tempje + '","se":"' + tempse + '","bm":[';
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
            $("#txt_qdtime").datepicker();

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
                    alert("如果是归口报销，请先选择归口部门");
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
            //删除费用科目
            $("#btnDelFykm").click(function () {
                $("#tab_fykm tbody tr ").each(function (e) {
                    if ($(this).hasClass("highlight")) {
                        //删除 报销总金额
                        //var tempje = $(this).find("input:eq(0)").val(); //金额
                        //var zje=$("#txtHjjeXx").val();
                        // alert(zje);
                        //alert(tempje);
                        // return;
                        $(this).remove();
                        htjeChange();

                    }
                });
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
            $("#btn_lookpic").click(function () {
                var billcode = '<%=Request["billCode"]%>';
                window.open("lookpic.aspx?billcode=" + billcode);
            });

            //部门选择 归口
            $("#txt_gk").autocomplete({
                source: availableTagsDept
            });

            //审核详细页
            $("#btn_sh").click(function () {

                var billcode = '<%=Request["billCode"]%>';

                window.showModalDialog("../MyWorkFlow/BillShDetail.aspx?billCode=" + billcode, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:700px;status:no;scroll:yes');
            });
        });

        function lookyksq(billcode) {
            window.showModalDialog("bxDetailForDz.aspx?type=look&dydj=02&billCode=" + billcode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:970px;status:no;scroll:yes');
            // location.href = " bxDetailForDz.aspx?type=look&dydj=02&billCode=" + billcode;
        }

        //function change() {
        //    var zt = $("#drp_xcn").val();
        //    if (zt == "是") {
        //        $("#btn_yksq").attr("disabled", "");
        //    }
        //    if (zt == "否") {
        //        $("#btn_yksq").attr("disabled", "true");
        //    }
        //}
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

        function del(obj) {
            $(obj).parent().parent().remove();
        }

        function openKm(deptCode, isGk) {
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function (i) {
                kmArray[i] = $(this).html() + "-" + $(this).next().html();//科目加部门
            });
            kmcode = kmArray.join();
            var billDate = $("#txtSqrq").val();
            var hashdxmhs = $("#hdxmhs").val();
            var dydj = '<%=Request["dydj"]%>';
            if (dydj == null && dydj == '' && dydj == undefined || dydj == '06') {
                dydj = "02";
            }
            //var str = window.showModalDialog("YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate + "&dydj=" + dydj + "&tfsq=tfsq", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            //insertKm(str);
            $("#prodcutDetailSrc").attr("src", "YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate + "&dydj=" + dydj);
            $("#dialog-confirm").dialog(
                {
                    modal: true,             // 创建模式对话框
                    autoOpen: true,//是否自动打开
                    height: 520, //高度
                    width: 300, //宽度
                    title_html: true
                }
            );
        }
        //传入一个json 然后插入科目明细
        function insertKm(str) {
            $("#dialog-confirm").dialog('close');
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function (i) {
                kmArray[i] = $(this).html() + "-" + $(this).next().html();//科目加部门
            });
            kmcode = kmArray.join();
            var kmIndex = $("#tab_fykm tbody tr").last().attr("id");
            if (kmIndex == undefined || kmIndex == "") {
                kmIndex = 1;
            }
            else {
                kmIndex = (kmIndex.split("_")[1]) * 1 + 1;
            }
            if (str != undefined && str != "") {

                var json = $.parseJSON(str);
                //显示报销摘要和报销说明
                if ($("#txtBxsm").val().length == 0) {
                    $("#txtBxsm").val(json[0].bxsm);
                }
                if ($("#txtBxzy").val().length == 0) {
                    $("#txtBxzy").val(json[0].bxzy);
                }
                var inner = "";
                var checkArray = new Array();

                for (var i = 0; i < json.length; i++) {
                    if (kmcode.indexOf(json[i].Yscode + "-" + json[i].dept) < 0) {
                        inner += '<tr id="tr_' + kmIndex + '">';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        inner += "<td>" + json[i].dept + "</td>";
                        //if (hashdxmhs == "1") {
                        //    inner += "<td>" + json[i].XiangMuHeSuan + "</td>";
                        //}
                        inner += "<td>" + json[i].Ysje + "</td>";
                        inner += "<td>" + json[i].Syje + "</td>";
                        inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="' + json[i].je + '" /></td>';
                        inner += '<td><input type="text" class="baseText ysse" onblur="htjeChange();" value="' + json[i].se + '" /></td>';
                        //inner += "<td><input type='button' style='text-align:center' class='baseButton' value='删除' onclick='del(this)'/></td>";
                        inner += "</tr>";
                        $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"><li><span>' + json[i].dept + ':</span><input type="text" value="' + json[i].je + '" /></li></ul>');
                        $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                        kmIndex++;
                    }
                    checkArray[i] = json[i].Yscode;
                }

                $("#tab_fykm tbody").append(inner);
                //选中一下最后一行
                $("#tab_fykm tbody tr:last").click();
            }
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
        function shangchuan() {
            //var filepath = document.getElementById("upLoadFiles").value;
            //var position = filepath.lastIndexOf("\\");
            //var filename = filepath.substring(position + 1);

            $.ajaxFileUpload
                (
                    {
                        url: 'uploadFile.ashx', //用于文件上传的服务器端请求地址
                        secureuri: false, //一般设置为false
                        fileElementId: 'upLoadFiles', //文件上传空间的id属性  <input type="file" id="file" name="file" />
                        dataType: 'json', //返回值类型 一般设置为json
                        success: function (data, status)  //服务器成功响应处理函数
                        {
                            // $("#img1").attr("src", data.imgurl);
                            if (typeof (data.error) != 'undefined') {
                                if (data.error != '') {
                                    alert(data.error);
                                } else {
                                    //成功
                                    //alert(decodeURI(unescape(data.filename)));
                                    //alert(decodeURI(unescape(data.fileurl)));
                                    $("#filenames").html($("#filenames").html() + "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件：" + decodeURI(unescape(data.filename)) + "：</span><a onclick='delfj(this);'>删除</a><span style='display:none'><input type='text' class='fujianurl' value='" + decodeURI(unescape(data.fileurl)) + "'/><input type='text' class='fujianname' value='" + decodeURI(unescape(data.filename)) + "'/></span></div>");
                                    alert(data.msg);
                                }
                            }
                        },
                        error: function (data, status, e)//服务器响应失败处理函数
                        {
                            alert(e);
                        }
                    }
                )//清空上传控件
            document.getElementById("upLoadFiles").outerHTML = document.getElementById("upLoadFiles").outerHTML;
            return false;


        }
        function delfj(obj) {
            $(obj).parent().remove();

        }
        function closeDetail() {
            $("#dialog-confirm").dialog("close");
            parent.closeDetail();
        }

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
        <div style="margin: 0 auto; width: 95%">
            <table cellpadding="1" id="taball" cellspacing="0" class="style1" width="100%" border="0" style="margin: 0 auto;">
                <tr>
                    <td style="text-align: center; height: 36px;">
                        <strong>退费申请单
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">

                        <table border="0" cellpadding="1" cellspacing="0" class="myTable" style="width: 100%;">
                            <tr>
                                <td colspan="5">
                                    <div style="float:right;margin-right: 10px;">
                                        制单人：&nbsp;&nbsp;<input id="txtBxr" type="text" runat="server" />
                                        制单部门：&nbsp;&nbsp;<input id="txtDept" readonly="readonly" type="text" runat="server" style="width: 120px" /><input id="txtbxdept"
                                            readonly="readonly" style="display: none; width: 0px" type="text" runat="server" />
                                        受理时间：&nbsp;&nbsp;<asp:TextBox ID="txtSqrq" runat="server" Style="width: 150px"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">所在分校
                                </td>
                                <td style="text-align: center">学员姓名
                                </td>
                                <td style="text-align: center">所在年级
                                </td>
                                <td style="text-align: center">协议编号
                                </td>
                                <td style="text-align: center">签单时间
                                </td>
                            </tr>
                            <tr>

                                <td style="text-align: center;">&nbsp;<asp:TextBox runat="server" ID="txt_stuschool"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_stuname"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_stuclass"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_stucode"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_qdtime"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2">退费原因
                                </td>
                                <td colspan="2">
                                    <div style="text-align: left">
                                        <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine"  Style="width: 350px; margin-left:5px;"></asp:TextBox><span style="color: red">*</span>
                                    </div>
                                </td>
                                  <td class="tableBg2">任课导师：</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_teacher"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>

                                <td class="tableBg2">协议中学员<br />
                                    监护人银行账号
                                </td>
                                <td colspan="4">
                                    <div style="margin-left: 2px">
                                        户&nbsp;&nbsp;&nbsp;&nbsp;名：
                                        <input type="text" style="width: 638px" runat="server" id="txt_khh" /><span style="color: red">*</span>
                                    </div>
                                    <hr />
                                    <div style="margin-left: 2px">
                                        建行账号：
                                        <asp:TextBox ID="txtbxzh" Style="width: 629px" runat="server"></asp:TextBox><span style="color: red">*</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">协议辅导费用
                                </td>
                                <td style="text-align: center">已消费课时
                                </td>
                                <td style="text-align: center">对应课时单价
                                </td>
                                <td style="text-align: center">已消费费用
                                </td>
                                <td style="text-align: center">应扣其他费用
                                </td>
                            </tr>
                            <tr>

                                <td style="text-align: center;">&nbsp;&nbsp;<asp:TextBox runat="server" ID="txt_xyfdfy"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_yxfks"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_dyksdj"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_yxffy"></asp:TextBox><span style="color: red">*</span>
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox runat="server" ID="txt_ykqtfy"></asp:TextBox><span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2">附件：</td>
                                <td colspan="2" style="text-align: left">
                                    <div style="border-bottom: 1px dashed #CDCDCD; height: 20px;">
                                        &nbsp;&nbsp;&nbsp;
                                          <input type="file" id="upLoadFiles" name="upLoadFiles" style="100px" />
                                        <input id="csfj" value="上传" class="baseButton" onclick="shangchuan();" type="button" />
                                        <input type="button" id="btn_lookpic" runat="server" value="查看图片附件" class="baseButton" />
                                        <div id="divBxdj" runat="server">
                                        </div>
                                    </div>
                                    <%--存放用于显示附件的名字--%>
                                    <div id="filenames" runat="server">
                                    </div>
                                </td>
                                <td class="tableBg2" style="text-align: right">
                                    <asp:Label ID="lbl_djs" runat="server" Text="单据数：" class="tableBg2"></asp:Label>
                                </td>
                                <td>
                                    <div style="margin-left: 5px">
                                        <asp:TextBox ID="txt_djs" runat="server"></asp:TextBox>
                                    </div>
                                </td>

                            </tr>
                            <tr>
                                <td style="background-color: red; height: 4px" colspan="5"></td>
                            </tr>

                            <tr id="trGk" runat="server">
                                <td class="tableBg2">归口预算
                                </td>
                                <td>
                                    <div style="float:left">
                                        <asp:RadioButton ID="rb_ok" runat="server" Text="是" GroupName="is_gk" />
                                        <asp:RadioButton ID="rb_can" runat="server" Text="否" GroupName="is_gk" Checked="true" />
                                    </div>
                                </td>
                                <td class="tableBg2">归口部门
                                </td>
                                <td colspan="2" id="gkbm">
                                    <div id='dv_gk' style="margin-left: 2px; width: 100px;">
                                        <input type='text' id='txt_gk' runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2">
                                    <asp:Label ID="lbl_yskm" runat="server" Text="预算科目"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <div style="margin-left: 2px; text-align: left">
                                        <input type="button" value="选择预算科目" id="btnAddFykm" runat="server" class="baseButton" />
                                        <input type="button" value="删除预算科目" id="btnDelFykm" runat="server" class="baseButton" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2">单据明细
                                </td>
                                <td colspan="4">
                                    <div style="margin-left: 2px">
                                        <table id="tab_fykm" class="myTable" style="width: 95%">
                                            <thead class="myGridHeader">
                                                <tr>
                                                    <th>科目
                                                    </th>
                                                    <th>部门
                                                    </th>
                                                    <th id="thxmhs" runat="server">项目核算
                                                    </th>
                                                    <th>预算金额
                                                    </th>
                                                    <th id="thLeavingAmount" runat="server">剩余金额
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
                                <td colspan="4">
                                    <div style="margin-left: 2px; text-align: left">
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
                                <td colspan="4" id="td_dept" runat="server" style="text-align: left"></td>
                            </tr>
                            <tr id="trXm1" runat="server" style="display: none">
                                <td class="tableBg2">项目选择
                                </td>
                                <td colspan="4">
                                    <div style="margin-left: 2px">
                                        <input type="button" value="选择项目" id="btn_choosexm" runat="server" class="baseButton" />
                                    </div>
                                </td>
                            </tr>
                            <tr id="trXm2" runat="server" style="display: none">
                                <td class="tableBg2">核算项目
                                </td>
                                <td colspan="4" id="td_xm" runat="server"></td>
                            </tr>
                            <tr class="fykmRow">
                                <td class="tableBg2">应退费金额
                                </td>
                                <td colspan="4">
                                    <div style="margin-left: 2px; text-align: left">
                                        ￥：<input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px; text-align: right; background-color: #cccccc;" />
                                    </div>
                                    <hr />
                                    <div style="margin-left: 2px; text-align: left">
                                        大写：
                                        <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2">

                                    <asp:Label runat="server" ID="lbl_bxzy" Text="备注"></asp:Label>
                                </td>
                                <td colspan="4" style="text-align: left">
                                    <asp:TextBox ID="txtBxzy" runat="server" Width="80%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: red; height: 4px" colspan="5"></td>
                            </tr>
                            <tr id="tr_shyj" runat="server">
                                <td style="text-align: right">审核意见：
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_shxx_history" runat="server">
                                <td style="text-align: right">审核详细：
                                </td>
                                <td colspan="4">
                                    <div id="txt_shxx_history" runat="server"></div>
                                </td>
                            </tr>
                            <tr id="tr_shyj_history" runat="server">
                                <td style="text-align: right">历史驳回意见：
                                </td>
                                <td colspan="4">
                                    <div id="txt_shyj_History" runat="server"></div>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 30px;">
                        <asp:HiddenField ID="hf_kmselect" runat="server" />
                        <asp:HiddenField ID="hd_djtype" runat="server" />
                        <asp:HiddenField ID="HiddenField1" runat="server" />

                        <asp:Button ID="btn_kmselect" runat="server" Text="Button" OnClick="btn_kmselect_Click"
                            CssClass="hiddenbill" />
                        <input type="button" value="保 存" id="btn_test" class="baseButton" runat="server" />
                        &nbsp;
                <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                <input type="button" onclick="javascript:parent.closeDetail();" value="关 闭" class="baseButton" />&nbsp;
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
                        <asp:HiddenField ID="hdxmhs" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
         <div id="dialog-confirm" style="display: none; overflow: hidden;">
            <iframe frameborder="no" border="0" marginwidth="0" marginheight="0" id="prodcutDetailSrc" scrolling="auto" width="100%" height="100%"></iframe>
        </div>
    </form>
</body>
</html>
