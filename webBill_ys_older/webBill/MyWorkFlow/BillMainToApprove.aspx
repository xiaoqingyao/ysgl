<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillMainToApprove.aspx.cs"
    Inherits="webBill_MyWorkFlow_BillMainToApprove" %>

<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>
<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html" charset="gb2312" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">

        function openDetailHd(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:130px;dialogWidth:280px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                $("#btn_summit").click();
            }
        }

        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            var flowid = '<%=Request["flowid"] %>';
            if (flowid == "jksq") {
                $("#btn_hdje").show();
            }
            $("#btn_hdje").click(function () {
                var trs = $("#<%=GridView1.ClientID%> tr:gt(0)");


                var billcode = "";
                var iChecked = 0;
                trs.each(function (index, obj) {
                    if ($(obj).find("td").eq(0).children().eq(0).attr("checked")) {
                        billcode = $(obj).find("td").eq(1).html();
                        iChecked++;
                    }
                });
                if (iChecked == 0) {
                    alert("请选择要核定金额的行.");
                    return;
                }
                if (iChecked > 1) {
                    alert("您选择了" + iChecked + "行，请选择一条数据");
                    return;
                }
                openDetailHd("../../SaleBill/BorrowMoney/FundBorrowUpdateDj.aspx?Ctrl=Audit&billCode=" + billcode);

            });

            initWindowHW();
            initMainTableClass("<%=GridView1.ClientID%>");
            $("#TextBox5").datepicker();
            $("#txtDateTo").datepicker();
            var isdz = '<%=Request["isdz"] %>';
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                var billCode = "";
                if (flowid == 'gkbx' || flowid == "tfsq" || (isdz == "1" && (flowid == 'ybbx' || flowid == "yksq_dz"))) {
                    if ($(this).find("td")[2] != null) {
                        billCode = $(this).find("td")[2].innerHTML;

                    }
                } else {
                    if ($(this).find("td")[1] != null) {
                        billCode = $(this).find("td")[1].innerHTML;

                    }
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //双击打开
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").dblclick(function () {
                $("#btn_detail").click();
            });
            //驳回选择   大智
            $("#btn_cancel_xz").click(function () {
                //var billcode = "";//$(".highlight td:eq(1)").html();

                //if (flowid == 'gkbx' || flowid == "tfsq" || (isdz != null && isdz != undefined && isdz == "1" && (flowid == "yksq_dz" || flowid == "ybbx"))) {
                //    billcode = $(".highlight td:eq(2)").html();
                //} else {
                //    billcode = $(".highlight td:eq(1)").html(); //varGridView.rows[i].cells[1].innerHTML;
                //}

                //var mind = $(".highlight td:eq(12)").find("input").val();//.html().children();


                //if (billcode == ""||billcode==null) {
                //    alert("请选择驳回的记录。");
                //    return;
                //}





                var i = 0;
                var iChecked = 0;
                var billcode = "";
                var mind = "";
                $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").filter(":has(input)").each(function (index, obj) {
                    if ($(obj).find("td").eq(0).children().eq(0).attr("checked")) {
                        var evebillcode = "";
                        if (flowid == 'gkbx' || flowid == "tfsq" || (isdz != null && isdz != undefined && isdz == "1" && (flowid == "yksq_dz" || flowid == "ybbx"))) {
                            evebillcode = $(obj).find("td").eq(2).html();
                        } else {
                            evebillcode = $(obj).find("td").eq(1).html(); //varGridView.rows[i].cells[1].innerHTML;
                        }
                        if (evebillcode != undefined && evebillcode != "" && evebillcode != "编号") {
                            mind = escape($(obj).find("td").eq(12).children().eq(0).val());
                            billcode = evebillcode;
                            iChecked++;
                        }
                    }
                });

                if (billcode == "" || billcode == null) {
                    alert("请选择驳回的记录。");
                    return;
                }
                if (iChecked != 1) {
                    alert("驳回只能选择一条记录。");
                    return;
                }

                openDetail("DisAgreeToSpecial.aspx?billCode=" + billcode + "&mind=" + mind);
                $("#btnRefresh").click();
            });


            //驳回  其他客户
            $("#btn_cancel").click(function () {
                $(this).attr("disabled", "disabled");
                //                var billcode = $(".highlight td:eq(1)").html();
                //                if (billcode == undefined || billcode == "") {
                //                    alert("请先选择单据!");
                //                }
                //                else {
                //                    if (confirm("确定要否决选中的单据吗?")) {
                //                        var mind = $(".highlight td input").val();
                //                        $.post("WorkFlowApprove.ashx", { "billcode": billcode, "mind": mind, "action": "disagree" }, OnApproveSuccess);
                //                    }
                //                }
                var varGridView = document.getElementById("<%=GridView1.ClientID %>");
                var flowid = '<%=Request["flowid"] %>';
                var isdz = '<%=Request["isdz"] %>';
                var iGridViewRow = varGridView.rows.length;
                if (iGridViewRow >= 18) { iGridViewRow = iGridViewRow - 1; }
                var i = 0;
                var iChecked = 0;
                var billcode = "";
                var mind = "";
                $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").filter(":has(input)").each(function (index, obj) {
                    if ($(obj).find("td").eq(0).children().eq(0).attr("checked")) {
                        var evebillcode = "";
                        if (flowid == 'gkbx' || flowid == "tfsq" || (isdz != null && isdz != undefined && isdz == "1" && (flowid == "yksq_dz" || flowid == "ybbx"))) {
                            evebillcode = $(obj).find("td").eq(2).html();
                        } else {
                            evebillcode = $(obj).find("td").eq(1).html(); //varGridView.rows[i].cells[1].innerHTML;
                        }
                        if (evebillcode != undefined && evebillcode != "" && evebillcode != "编号") {
                            mind = escape($(obj).find("td").eq(12).children().eq(0).val());
                            billcode += evebillcode + "*" + mind + ",";
                            iChecked++;
                        }
                    }
                });

                //                for (var i = 0; i < iGridViewRow; i++) {
                //                    if (varGridView.rows[i].cells[0].getElementsByTagName("input")[0].checked) {
                //                        var evebillcode = "";
                //                        if (flowid == 'gkbx') {
                //                            evebillcode = varGridView.rows[i].cells[2].innerHTML;
                //                        } else {
                //                            evebillcode = varGridView.rows[i].cells[1].innerHTML;
                //                        }
                //                        if (evebillcode == undefined || evebillcode == "" || evebillcode == "编号") { continue; }
                //                        var mind = escape(varGridView.rows[i].cells[12].children[0].value);
                //                        billcode += evebillcode + "*" + mind + ",";
                //                        iChecked++;
                //                    }
                //                }
                if (iChecked == 0) { alert("请勾选复选框选择记录！"); $(this).removeAttr("disabled"); return; }
                billcode = billcode.substring(0, billcode.length - 1);
                if (confirm("确定要否决选中的单据吗?")) {
                    $.post("WorkFlowApprove.ashx", { "billcode": billcode, "mind": mind, "action": "disagree" }, function (data, status) {

                        if (data == -1) {
                            alert("该订单流程已经通过，不允许驳回。");
                            return;
                        }
                        if (data < 0 && status != "success") {
                            alert("审核失败！"); $(this).removeAttr("disabled");
                            return;
                        }
                    });
                    alert("成功审核通过" + iChecked + "条记录！");

                }
                $("#btn_summit").click();
            });
            //部门选择
            $("#TextBox2").autocomplete({
                source: availableTags
            });
            //人员选择
            $("#TextBox3").autocomplete({
                source: avaiusertb
            });

            //详细信息
            $("#btn_detail").click(function () {
                var flowid = '<%=Request["flowid"] %>';

                var isdz = '<%=Request["isdz"]%>';
                var checking = $("#rdoStatusNow").attr("checked");
                var billcode = $(".highlight td:eq(1)").html();
                var billname = $(".highlight td:eq(2)").html();
                if (flowid == 'gkbx' || flowid == "tfsq" || (isdz == "1" && (flowid == 'ybbx' || flowid == "yksq_dz"))) {//如果是大智的用款申请和费用报销
                    billcode = billname;
                }
                var xmcode = $(".highlight td:eq(17)").html();
                var yskmtype = '<%=Request["yskmtype"]%>';//预算科目类型  01 收入类 02 费用类 03资产 04存货 05往来  在数据字典中配置
                var tbtype = '<%=Request["tbtype"]%>';//填报方式  zxer 自下而上  zsex自上而下
                var limittotal = '<%=Request["limittotal"]%>';//如果是自下而上填报  是否控制填报总金额
                var jecheckflg = '<%=Request["jecheckflg"]%>';//如果是自上而下填报  保存时检查方式 0分解金额必须等于总金额  1分解金额小于等于总金额

                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {

                    $.post("GetBillType.ashx", { "billcode": billcode, "yskmtype": yskmtype, "tbtype": tbtype, "limittotal": limittotal, "jecheckflg": jecheckflg, "xmcode": xmcode, "isdz": isdz, "flowid": flowid }, function (data, status) {

                        if (status == "success") {
                            if (flowid == "yshz" || flowid == "xmyshz") {
                                window.location.href = data + "&checking=" + checking;
                                return;
                            } else if (flowid == 'ys' || flowid == 'xmys') {
                                window.location.href = data + "&checking=" + checking;
                                return;
                            }
                            //window.location.href = data + "&checking=" + checking;

                            openDetail(data + "&checking=" + checking);

                            //   window.showModalDialog(data + "&checking=" + checking, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
                            //location.replace(location.href);
                            //$("#btn_summit").click();
                            $("#btn_summit").click();
                        }
                    });
                }

            });
            //审核通过
            $("#btn_ok").click(function () {
                if (confirm("是否确定审核?")) {
                    $(this).attr("disabled", "disabled");
                    var varGridView = document.getElementById("<%=GridView1.ClientID %>");
                    var flowid = '<%=Request["flowid"] %>';
                    var isdz = '<%=Request["isdz"] %>';
                    var iGridViewRow = varGridView.rows.length;
                    var i = 0;
                    if (iGridViewRow >= 18) { iGridViewRow = iGridViewRow - 1; }
                    var iChecked = 0;
                    var billcode = "";
                    $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").filter(":has(input)").each(function (index, obj) {
                        if ($(obj).find("td").eq(0).children().eq(0).attr("checked")) {
                            var evebillcode = "";
                            if (flowid == 'gkbx' || flowid == "tfsq" || (isdz == "1" && (flowid == "yksq_dz" || flowid == "ybbx"))) {
                                evebillcode = $(obj).find("td").eq(2).html();
                            } else {
                                evebillcode = $(obj).find("td").eq(1).html(); //varGridView.rows[i].cells[1].innerHTML;
                            }


                            if (evebillcode != undefined && evebillcode != "" && evebillcode != "编号") {
                                var mind = escape($(obj).find("td").eq(12).children().eq(0).val());
                                billcode += evebillcode + "*" + mind + ",";
                                iChecked++;
                            }
                        }
                    });
                    //                return;
                    //                for (var i = 0; i < iGridViewRow; i++) {
                    //                    if (varGridView.rows[i].cells[0].getElementsByTagName("input")[0].checked) {
                    //                        var evebillcode = "";
                    //                        if (flowid == 'gkbx') {
                    //                            evebillcode = varGridView.rows[i].cells[2].innerHTML;
                    //                        } else {
                    //                            evebillcode = varGridView.rows[i].cells[1].innerHTML;
                    //                        }
                    //                    }
                    //                }
                    if (iChecked == 0) { alert("请勾选复选框选择记录！"); $(this).removeAttr("disabled"); return; }
                    billcode = billcode.substring(0, billcode.length - 1);


                    $.post("WorkFlowApprove.ashx", { "billcode": billcode, "mind": "", "action": "approve" }, function (data, status) {
                        if (data < 0 && status != "success") { alert("审核失败！"); $(this).removeAttr("disabled"); }
                    });
                    alert("成功审核通过" + iChecked + "条记录！");
                    $("#btn_summit").click();
                }

            });

            $('#dialog').dialog({
                autoOpen: false,
                width: 400,
                buttons: {
                    "确定": function () {
                        $(this).dialog("close");
                        $("#btn_summit").click();
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#btn_find").click(function () {
                $("#trSelect").toggle();
                //                var stu = "none";

                //                stu = document.getElementById("trSelect").style.display;

                //                document.getElementById("trSelect").style.display = stu == "" ? "none" : "";


            });
            //修改

            $("#btn_edit").click(function () {

                var billcode = $(".highlight td:eq(1)").html();
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    openDetail("../../SaleBill/RemitTance/RemitTanceDetails.aspx?Ctrl=Atal&Code=" + billcode);
                }


            });


        });


        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                //alert("审批成功！");
                $(".highlight").remove();
            }
            else {
                alert("失败");
            }
        }
        function OnSuccess(result, context, methodName) {
            document.getElementById("wf").innerHTML = result;
        }
        function OnError(error, context, methodName) {
            alert("失败");
        }
        function openDetail(openUrl) {
            //window.location.href = openUrl;
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = "";
            var flowid = '<%=Request["flowid"] %>';
            if (flowid == "ys" || flowid == "xmys" || flowid == "zjys" || flowid == "srys" || flowid == "zcys" || flowid == "chys" || flowid == "wlys" || flowid == "yszj") {
                returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:1300px;status:no;scroll:yes');

            } else {
                returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes');

            }

            if (returnValue == undefined || returnValue == "")
            { }
            else {

            }
        }
        function submitData(oCheckbox) {
            var code = oCheckbox.name.substr(13, 2);

            var gvList = document.getElementById("GridView1");
            var yj;
            yj = gvList.rows[code - 2].cells[1].innerText;
            for (i = 1; i < gvList.rows.length; i++) {
                if (gvList.rows[i].cells[1].innerText > 2) {
                    if (gvList.rows[i].cells[1].innerText.substr(0, 2) == yj) {
                        gvList.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
                    }
                }
            }
        }
        function SelectAll(aControl) {
            var chk = document.getElementById("GridView1").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }


        //替换非数字
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
        }
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 10));
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="return initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="90%" style="margin-left: 5px">
            <tr>
                <td style="height: 30px">
                    <input id="btnRefresh" type="button" class="baseButton" value="刷 新" onclick="javascript: location.replace(location.href);" />
                    <input id="btn_find" type="button" value="查 询" class="baseButton" />
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />
                    <input id="btn_cancel" type="button" value="驳回制单人" class="baseButton" runat="server" />
                    <asp:Button ID="btn_bh" runat="server" CssClass="baseButton" Text="驳回上一步" OnClick="btn_bh_Click" />
                    <input id="btn_cancel_xz" type="button" value="选择节点驳回" class="baseButton" runat="server" />
                    <input id="btn_detail" type="button" value="详细信息" class="baseButton" />
                    <input id="btn_hdje" type="button" value="修改核定金额" class="baseButton" style="display: none;"
                        runat="server" />
                    <asp:Button ID="btn_edit" runat="server" Visible="false" CssClass="baseButton" Text="修 改" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div title="查询" style="display: none; float: left" id="trSelect">
                        <table class="baseTable" style="text-align: left; width: 900px">
                            <tr>
                                <td>日期从:
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                </td>
                                <td>至：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                                </td>
                                <td>报销人:
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                </td>
                                <td>所属部门:
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>单据编号：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillCode" runat="server"></asp:TextBox>
                                </td>
                                <td>金额:
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                </td>
                                <td>状态：
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdoStatusNow" runat="server" GroupName="rdstatus" Text="正在进行"
                                        Checked="true" />
                                    <asp:RadioButton ID="rdoStatusEnd" runat="server" GroupName="rdstatus" Text="审核通过" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_summit" runat="server" CssClass="baseButton" Text="确 定" OnClick="btn_summit_Click" />
                                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="取 消" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="myGrid"
                            OnRowDataBound="GridView1_RowDataBound" ShowFooter="false" Width="1400">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="50" HeaderStyle-Width="50">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="billcode" HeaderText="编号" ItemStyle-CssClass="myGridItem"
                                    HeaderStyle-CssClass="myGridHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100"
                                    HeaderStyle-Width="100"></asp:BoundField>
                                <asp:BoundField DataField="tbillName" HeaderText="单据编号" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader"
                                        Width="130" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="True" CssClass="myGridItem" Width="130" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="单据类型" ItemStyle-CssClass="myGridItem" DataField="flowid">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="100" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="制单人" ItemStyle-CssClass="myGridItem" DataField="billuser">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="110" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="110" />
                                </asp:BoundField>
                                <asp:BoundField DataField="billdept" HeaderText="部门" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                        Width="120" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="120" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="底盘号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField DataField="gkdept" HeaderText="归口部门" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                        Width="120" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="120" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="摘要说明" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="350" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="350" />
                                </asp:BoundField>
                                <asp:BoundField DataField="billje" HeaderText="单据金额" ItemStyle-CssClass="myGridItem"
                                    DataFormatString="{0:N}">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItemRight" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="制单时间" ItemStyle-CssClass="myGridItem" DataField="tbilldate">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItemCenter" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField DataField="billtype" HeaderText="billtype" Visible="False" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="80" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="80" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="备注" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="100" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="审批意见">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="100" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="相关人员" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="附加单据" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="True" CssClass="myGridItem" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="审批人" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader"
                                        Width="200" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="True" CssClass="myGridItem" Width="200" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="项目名称" DataField="" ItemStyle-CssClass="myGridItem">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="True" CssClass="myGridItem" Width="100" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="项目编号" DataField="note3" ItemStyle-CssClass="myGridItem hiddenbill">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader hiddenbill "
                                        Width="20" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="True" CssClass="myGridItem hiddenbill" Width="20" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="myGridHeader" />
                            <PagerStyle CssClass="hiddenbill" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <div style="height: 30px">
            <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
            </pager:UcfarPager>
            <input type="hidden" runat="server" id="hdwindowheight" />
        </div>
        <div>
            <table>
                <tr>
                    <td style="height: 10px">审核流程：
                    </td>
                    <td id="wf"></td>
                </tr>
            </table>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/webBill/MyWorkFlow/WorkFlowService.asmx" />
            </Services>
        </asp:ScriptManager>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
