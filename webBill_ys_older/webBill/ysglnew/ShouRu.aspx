<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShouRu.aspx.cs" Inherits="webBill_ysglnew_ShouRu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收入录入</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <meta http-equiv="content-type" content="application/ms-excel; charset=UTF-8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
        });
        function importExcel() {
            if ($("#ddlNd").val() == '' || $("#ddlKm").val() == '') {
                alert('请先选择年度和预算科目。');
                return;
            }
            var returnValue = window.showModalDialog('ShouRu_InExcel.aspx' + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "")
            { }
            else {
                $("#btn_reload").click();
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="baseDiv" style="margin:10px">
        年度：<asp:DropDownList ID="ddlNd" runat="server" AutoPostBack="True" 
            OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
        </asp:DropDownList>
        月份:
        <asp:DropDownList runat="server" ID="ddlMonths" >
            <asp:ListItem Value="1">01</asp:ListItem>
            <asp:ListItem Value="2">02</asp:ListItem>
            <asp:ListItem Value="3">03</asp:ListItem>
            <asp:ListItem Value="4">04</asp:ListItem>
            <asp:ListItem Value="5">05</asp:ListItem>
            <asp:ListItem Value="6">06</asp:ListItem>
            <asp:ListItem Value="7">07</asp:ListItem>
            <asp:ListItem Value="8">08</asp:ListItem>
            <asp:ListItem Value="9">09</asp:ListItem>
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="11">11</asp:ListItem>
            <asp:ListItem Value="12">12</asp:ListItem>
        </asp:DropDownList>
        部门：
        <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true" 
            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
        </asp:DropDownList>
        <input type="button" id="importExcelForTab" value="导入Excel" onclick="importExcel();"
            class="baseButton" />
        <asp:Button runat="server" ID="btn_excelout" CssClass="baseButton" Text="导出Excel模板"
            OnClick="btnExcelOut_Onclick" />
        <asp:Button runat="server" ID="btnSave" CssClass="baseButton" Text="保 存" OnClick="btnSave_Onclick" />
        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
        <asp:Button ID="btn_reload" runat="server" OnClick="btn_reload_Click" CssClass="hiddenbill" />
        <span style="color: Red">
            <asp:Label runat="server" ID="lblMsg"></asp:Label></span>
    </div>
    <div style="margin:10px;overflow: auto;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
            ShowHeader="true" ShowFooter="true" EmptyDataText="暂无数据" OnRowDataBound="GridView1_OnRowDataBound">
            <Columns>
                <asp:BoundField DataField="deptcode" HeaderText="部门编号"  ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader"  />
                <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField DataField="yskmcode" HeaderText="科目编号" ItemStyle-CssClass="myGridItemRight"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField DataField="yskmname" HeaderText="科目名称" ItemStyle-CssClass="myGridItemRight"
                    HeaderStyle-CssClass="myGridHeader" />
                <%-- DataFormatString="{0:F2}"--%>
                <asp:TemplateField HeaderText="实际发生额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtje" Width="100"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="je" HeaderText="金额" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"
                    FooterStyle-CssClass="hiddenbill" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
