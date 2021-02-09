<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gongzi_mingxi.aspx.cs" Inherits="webBill_makebxd_gongzi_mingxi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工资明细</title>
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

                window.location.href = "gongzi_xiangmuduiying.aspx";
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

        function InportExcel() {
            var returnValue = window.showModalDialog('GzExcelInport.aspx', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1000px;status:no;scroll:yes');
            //            if (returnValue == '1') {
            //                $("#btn_make").removeAttr("disabled");
            //            }
        }
        function showsuccess(msg) {
            alert(msg);
            $("#btn_make").attr("disabled", "disabled");
        }
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table class="baseTable" width="95%">
        <tr>
            <td colspan="2">
                <input type="button" class="baseButton" value="项目科目对照" id="btn_bili" />
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
            <td>
                制单日期：
            </td>
            <td>
                <input runat="server" id="txtDate" type="text" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <input type="button" class="baseButton" value="导入Excel" id="btn_inportExcel" onclick="InportExcel()" />
            </td>
            <td colspan="5">
                <asp:Button ID="showlist" runat="server" Text="显示分解情况" OnClick="showlist_Click" CssClass="baseButton" />
                <asp:Button ID="btn_make" runat="server" Text="制 单" CssClass="baseButton" OnClientClick=" return doenable(this);"
                    OnClick="btn_make_Click" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td colspan="10">
                <asp:GridView runat="server" ID="GridView1" ShowHeader="true" CssClass="myGrid" EmptyDataText="请先填写项目科目对照、从Excel导入数据，即可制单"
                    AutoGenerateColumns="false" ShowFooter="true" Width="90%" OnRowDataBound="GridView1_OnRowDataBound">
                    <HeaderStyle CssClass="myGridHeader" />
                    <RowStyle CssClass="myGridItem" />
                    <Columns>
                        <asp:TemplateField HeaderText="行号">
                            <ItemTemplate>
                                <%#  Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="科目编号" DataField="yskmcode" />
                        <asp:BoundField HeaderText="科目名称" DataField="yskmname" />
                        <%-- <asp:BoundField HeaderText="预算金额" DataField="ysje" />
                        <asp:BoundField HeaderText="剩余金额" DataField="syje" />--%>
                        <asp:BoundField HeaderText="核算科室编号" DataField="deptcode" />
                        <asp:BoundField HeaderText="核算科室名称" DataField="deptname" />
                        <asp:TemplateField HeaderText="核算金额">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtje" Text='<%#Eval("je") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="所含工资项" DataField="coursename" />
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
