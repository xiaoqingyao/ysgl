<%@ Page Language="C#" AutoEventWireup="true" CodeFile="feetodepts_bili.aspx.cs"
    Inherits="webBill_makebxd_feetodepts_bili" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用核算到部门比例设置</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            window.location.go
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btn_save" runat="server" CssClass="baseButton" Text="保 存" OnClick="btn_save_click" />
        <input id="aa" type="button" value="返 回" onclick="javascript:window.history.go(-1);"
            class="baseButton" />
        <label id="lblmsg" runat="server" style="color: Red">
        </label>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
            ShowHeader="true" ShowFooter="true" EmptyDataText="暂无数据" OnRowDataBound="GridView1_OnRowDataBound">
            <Columns>
                <asp:BoundField DataField="deptcode" HeaderText="科室" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField DataField="deptname" HeaderText="科室名称" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:TemplateField HeaderText="分解比例" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtbili" Text="0.00"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="yskmmc" HeaderText="类别" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField DataField="bili" HeaderText="比例" ItemStyle-CssClass="hiddenbill"
                    HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                <asp:BoundField DataField="yskmcode" HeaderText="类别" ItemStyle-CssClass="hiddenbill"
                    HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
