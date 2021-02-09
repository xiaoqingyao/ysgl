<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystzDetail.aspx.cs" Inherits="webBill_ysgl_ystzDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>预算调整</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function SaveCheck() {
            var i = 1;
            var btemp = true;
            $(".zjje").each(function() {
                var zjje = $(this).val();
                var syje = $(this).parent().parent().children("td").eq(5).html();
                if (zjje == "") {
                    zjje = "0";
                }
                if (syje == "" || syje == "&nbsp;") {
                    syje = "0";
                }
                syje = parseFloat(syje);
                zjje = parseFloat(zjje);
                if (zjje < 0) {
                    i = 0;
                    btemp = false;
                    return false;
                }
                if (syje < 0) {
                    if (syje > zjje) {
                        btemp = false;
                        return false;
                    }
                }
                else {
                    if (syje < zjje) {
                        btemp = false;
                        return false;
                    }
                }
                i++;
            });
            if (!btemp) {
                if (i == 0) {
                    alert("调整金额必须大于0");
                }
                else {
                    alert("第" + i + "行录入金额大于输入金额!");
                }
            }
            return btemp;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr id="trSelect" runat="server">
                <td style="height: 22px">
                    选择源预算过程：<asp:DropDownList ID="drpSource" runat="server">
                        <%--<asp:ListItem Value="-1">-=选择=-</asp:ListItem>--%>
                    </asp:DropDownList></td>
                <td style="height: 22px">
                </td>
                <td style="height: 22px">
                    选择目标预算过程：<asp:DropDownList ID="drpTarget" runat="server">
                       <%-- <asp:ListItem Value="-1">-=选择=-</asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:Button ID="Button2" runat="server" Text="获取预算数据" CssClass="baseButton" OnClick="Button2_Click" /></td>
            </tr>
            <tr id="trResult" runat="server" style="display:none;">
                <td style="height: 22px"><asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" OnClientClick="return SaveCheck();" />
                    &nbsp; <asp:Button ID="Button3"  Visible="false" runat="server" Text="取 消" CssClass="baseButton" OnClick="Button3_Click" />
                    &nbsp; 源预算过程：
                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td style="height: 22px">
                </td>
                <td style="height: 22px">目标预算过程：
                    <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:DataGrid ID="myGrid1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" onitemdatabound="myGrid1_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="yskm" HeaderText="科目编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ysDept" HeaderText="部门编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="源预算剩余金额" DataFormatString="{0:F2}" 
                                DataField="ysje">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" 
                                    HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="目标剩余金额" DataFormatString="{0:F2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" 
                                    HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="冻结金额" DataFormatString="{0:F2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" 
                                    HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="可调整金额" DataFormatString="{0:F2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" 
                                    HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="增加金额">
                                <ItemTemplate>
                                    &nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="zjje"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="gcbh" HeaderText="gcbh" Visible="False">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
        </table>
    </form>
</body>
</html>
