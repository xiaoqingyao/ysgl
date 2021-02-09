<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptDuiYingList.aspx.cs"
    Inherits="webBill_pingzheng_DeptDuiYingRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">
        .Div
        {
            margin-top: 5px;
            margin-left: 5px;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            initMainTableClass("<%=gridView.ClientID%>");
            $("#btnEdit").click(function() {
                var row = $(".highlight td");
                if (row == null || row == undefined || row.length == 0) {
                    alert("请先选择行！");
                    return;
                }

                var currentDeptName = row.get(1).innerHTML;
                var osDeptName = row.get(2).innerHTML;
                openDetail("DeptDuiYingEdit.aspx?currentdeptname=" + currentDeptName + "&osdeptname=" + osDeptName);
                document.getElementById("btnRefresh").click();
            });
            $("#btnAdd").click(function() {
                var currentDeptName = $("#hdCurrentDeptName").val();
                if (currentDeptName == undefined || currentDeptName == '') {
                    alert("请先在左侧选择本系统中的部门！"); return;
                }
                openDetail("DeptDuiYingEdit.aspx?currentdeptname=" + currentDeptName);
                document.getElementById("btnRefresh").click();
            });
        });
        function openDetail(openUrl) {
            openUrl += '&par=' + Math.random();
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:180px;dialogWidth:400px;status:no;scroll:yes');
            return returnValue;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="Div">
        <input type="button" id="btnRefresh" value="刷 新" class="baseButton" onclick="javascript:window.location.replace(window.location.href);" />
        <input type="button" id="btnAdd" value="添 加" class="baseButton" />
        <input type="button" id="btnEdit" value="编 辑" class="baseButton" />
        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
    </div>
    <div class="Div">
        <asp:GridView ID="gridView" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
            Width="80%">
            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                 HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                Height="30" />
            <RowStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                CssClass="myGridItem" Width="20" />
            <Columns>
                <asp:BoundField DataField="OSDeptCode" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" />
                <asp:BoundField HeaderText="本系统部门名称" DataField="Note1" />
                <asp:BoundField HeaderText="NC系统部门名称" DataField="OSDeptName" />
                <%-- <asp:BoundField HeaderText="上级部门名称" DataField="ParentName"/>--%>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:HiddenField runat="server" ID="hdCurrentDeptName" />
    </div>
    </form>
</body>
</html>
