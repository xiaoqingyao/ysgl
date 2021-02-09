<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gongzi.aspx.cs" Inherits="webBill_makebxd_gongzi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>水电暖导入报销单</title>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script src="../Resources/jScript/windowScript.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {



            //验证总金额输入是否是数字
            $("#txttotalamount").blur(function() {
                replaceNaN(this);
            });
            $("#txttotalamount").keyup(function() {
                replaceNaN(this);
            });
            //选择费用科目
            $("#btn_selectfykm").click(function() {

                selectfykm();
                showshengyuyusuan();
            });
            $("#txtfykm").dblclick(function() {
                selectfykm();
            });

            //制单日期
            $("#txtDate").click(function() {
                setday(this);
            });
            $("#txtzdr").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#txtgkdept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            $("#txtgkdept").autocomplete({
                source: availableTagsdept,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                }
            });
            //选择费用科目
            $("#btn_bili").click(function() {
                var kemu = $("#txtfykm").val();
                if (kemu == "" || kemu == undefined) {
                    alert("请先选择费用科目。"); return;
                }
                kemu = kemu.substring(1, kemu.indexOf(']'));
                window.location.href = "feetodepts_bili.aspx?type=" + kemu;
            });
            $("#txtzdr").change(function() {
                $("#btn_make").removeAttr("disabled");
            });
            $("#txtgkdept").change(function() {
                $("#btn_make").removeAttr("disabled");
            });
            $("#txtfykm").change(function() {
                $("#btn_make").removeAttr("disabled");
            });
        });
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function selectfykm() {
            var rel = window.showModalDialog("selectyskm.aspx", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:150px;status:no;scroll:auto');
            if (rel != undefined && rel != '') {
                $("#txtfykm").val(rel);
            }
        }
        function doenable(obj) {
            document.getElementById("divOver").style.visibility = "visible";
            //$("#btn_make").attr("disabled", "false");
            return true;
        }
        //显示预算余额
        function showshengyuyusuan() {
            var strdeptcode = $("#txtgkdept").val();
            if (strdeptcode == "" || strdeptcode == undefined) {
                return;
            }
            strdeptcode = strdeptcode.substring(1, strdeptcode.indexOf("]"));
            var strzdrq = $("#txtDate").val();
            if (strzdrq == "" || strzdrq == undefined) {
                return;
            }
            var strfykm = $("#txtfykm").val();
            if (strfykm == "" || strfykm == undefined) {
                return;
            }
            strfykm = strfykm.substring(1, strfykm.indexOf("]"));
            $.post("getys.ashx", { "date": strzdrq, "yskm": strfykm, "dept": strdeptcode }, OnApproveSuccess);
        }
        function OnApproveSuccess(data, status) {
            if (status == "success") {
                if (data == "-1") {
                    alert("日期格式不正确");
                } else {
                    $("#lblsyje").text(data);
                }
            }
        }
        function openDetail(billcode) {
            window.showModalDialog("bxDetailForGK.aspx?type=edit&billCode=" + billcode + "&dydj=02", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
        }
        function showsuccess(msg) {
            alert(msg);
            $("#btn_make").attr("disabled", "disabled");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table class="baseTable" width="95%">
        <tr>
            <td>
                制单日期：
            </td>
            <td>
                <input runat="server" id="txtDate" type="text" />
            </td>
            <td>
                制单人：
            </td>
            <td>
                <input type="text" runat="server" id="txtzdr" />
            </td>
            <td>
                归口部门:
            </td>
            <td>
                <input runat="server" type="text" id="txtgkdept" />
            </td>
            <td colspan="2">
                <input type="button" class="baseButton" value="设置科室分解比例" id="btn_bili" />
            </td>
        </tr>
        <tr>
            <td>
                费用科目：
            </td>
            <td>
                <input runat="server" type="text" id="txtfykm" readonly="readonly" class="baseTextReadOnly" />
                <input id="btn_selectfykm" type="button" class="baseButton" value="选" />
            </td>
            <td>
                预算剩余金额：
            </td>
            <td>
                <label id="lblsyje" runat="server">
                </label>
            </td>
            <td>
                总金额:
            </td>
            <td>
                <input runat="server" type="text" id="txttotalamount" />
            </td>
            <td colspan="2">
                <asp:Button ID="btn_showfenjie" runat="server" Text="显示分解情况" CssClass="baseButton"
                    OnClick="btn_showfenjie_click" />
                <asp:Button ID="btn_make" runat="server" Text="制 单" CssClass="baseButton" OnClientClick=" return doenable(this);"
                    OnClick="btn_make_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="10">
                <asp:GridView runat="server" ID="GridView1" ShowHeader="true" CssClass="myGrid" EmptyDataText="请先填写费用科目、总金额等信息，然后单击‘显示分解情况’按钮。"
                    AutoGenerateColumns="false" ShowFooter="true" Width="90%" OnRowDataBound="Gridview_OnrowdataBound">
                    <HeaderStyle CssClass="myGridHeader" />
                    <RowStyle CssClass="myGridItem" />
                    <Columns>
                        <asp:BoundField HeaderText="编号" DataField="deptcode" />
                        <asp:BoundField HeaderText="名称" DataField="deptname" />
                        <asp:BoundField HeaderText="分解比例" DataField="bili" ItemStyle-CssClass="myGridItemRight" />
                        <asp:TemplateField HeaderText="分解金额" ItemStyle-CssClass="myGridItemRight">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtje" Text='<%#Eval("je") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="科目名称" DataField="yskmname" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div id="divOver" runat="server" style="z-index: 1200; left: 30%; width: 160; cursor: wait;
        position: absolute; top: 25%; height: 100; visibility: hidden;">
        <table style="width: 17%; height: 10%;">
            <tr>
                <td>
                    <table style="width: 316px; height: 135px;">
                        <tr align="center" valign="middle">
                            <td>
                                <img src="../Resources/Images/Loading/pressbar2.gif" alt="" /><br />
                                <b>正在处理中，请稍后....<br />
                                </b>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
