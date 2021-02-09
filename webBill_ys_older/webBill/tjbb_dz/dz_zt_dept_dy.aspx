<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dz_zt_dept_dy.aspx.cs" Inherits="webBill_tjbb_dz_dz_zt_dept_dy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //$("#btn_zd").click(function () {
            //    location.href = "chbxd_ImportExcel.aspx";
            //});
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            function SelectAll(aControl) {
                var chk = document.getElementById("GridView1").getElementsByTagName("input");
                for (var s = 0; s < chk.length; s++) {
                    chk[s].checked = aControl.checked;
                }
            }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 5px; margin-left:5px;">
            <table cellpadding="1" cellspacing="0" width="100%">
                <tr>
                    <td>帐套：<asp:DropDownList ID="ddlZhangTao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnddlZhangTao_SelectedIndexChanged">
                    </asp:DropDownList>

                        <%--<input type="button" value="关闭" id="btn_zd" class="baseButton" />--%>
                        <asp:Button ID="Button1" runat="server" Font-Size="9pt" Text="保存设置" CssClass="baseButton"
                            OnClick="Button1_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="header">
                        </div>
                        <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 400px; height: 430px;">
                            <asp:GridView EmptyDataText="暂时没有数据" ID="GridView1" runat="server" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="0" Font-Size="9pt" BackColor="White"
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" OnRowDataBound="GridView1_RowDataBound"
                                CssClass="myGrid" Style="table-layout: fixed" Width="100%">
                                <HeaderStyle CssClass=" myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <RowStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" Height="30" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="45" HeaderStyle-Width="45" ItemStyle-Wrap="true">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                                Text="全选" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-Width="90" HeaderStyle-Width="90" />
                                    <asp:BoundField DataField="deptname" HeaderText="部门名称" HtmlEncode="False" ItemStyle-Wrap="true" />

                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
