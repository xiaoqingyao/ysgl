<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartmentBudgetRevert.aspx.cs"
    Inherits="webBill_ysgl_DepartmentBudgetRevert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门预算驳回</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript"> 
    $(function(){
    $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
    });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>年份：</td>
                            <td><asp:DropDownList ID="ddlDateYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnDDlDateYearSelectIndexChanged">
                                </asp:DropDownList></td>
                            <td>
                                部门：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnDDlDeptSelectIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnRevert" runat="server" CssClass="baseButton" Text="预算驳回" OnClientClick="confirm('让驳回该部门所有的预算金额到全年预算，该过程是不可逆的，确定要继续吗？');" OnClick="OnbtnRevertClick"/>
                                  <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" /></td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="myGridView" runat="server" AutoGenerateColumns="false" CssClass="myGrid" OnRowDataBound="OnMyGridViewRowDataBound">
                        <Columns>
                            <asp:BoundField DataField="gcbh" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField DataField="yskm" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField HeaderText="预算过程" DataField="gcbhShowName">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="科目" DataField="yskmShowName">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True"  Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="预算金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="花费金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="占用金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="可驳回金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="billCode" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
