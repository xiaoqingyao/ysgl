<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RollYsjzList.aspx.cs" Inherits="webBill_cwgl_RollYsjzList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算结转列表页</title>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <script type="text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
             
            });
            $("#btn_back").click(function() {
                window.location.href = "RollYsgcList.aspx";
            });
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function doenable(obj) {
            document.getElementById("divOver").style.visibility = "visible";
            return true;
        }
        
        
          //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value) || obj.value > 1 || obj.value < 0) {
                obj.value = '';
                alert("比例必须是0~1之间的小数！");
            };
        }
    </script>

<style type="text/css">
.num
{
	text-align:right;
	}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin: 10px">
           结转比例： <asp:TextBox ID="txt_percentage" runat="server" Text="1.0" Width="50" CssClass="num"  onkeyup="replaceNaN(this);"></asp:TextBox>
            <asp:Button ID="btn_ok" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_ok_Click" OnClientClick="return doenable();" />
            <input id="btn_back" type="button" value="返 回" class="baseButton" />
            <label style="color: Red">
                【友情提示】：预算结转将把本月的预算剩余金额转移至下一预算过程，同时系统将自动驳回当前预算过程的未审核通过的预算调整类的单据，并将未审核通过的报销单结转至下月的一号。</label>
        </div>
        <div>
            <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                CssClass="myGrid" Width="1200">
                <Columns>
                    <asp:BoundColumn DataField="ysgcmc" HeaderText="当前预算过程" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="nextysgcmc" HeaderText="结转入预算过程" HeaderStyle-Width="100"
                        ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="deptname" HeaderText="部门名称" HeaderStyle-Width="110" ItemStyle-Width="110">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="yskmmc" HeaderText="预算科目名称" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ysje" HeaderText="预算金额" HeaderStyle-Width="100" ItemStyle-Width="100"
                        DataFormatString="{0:N2}">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" CssClass="myGridItemRight" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="jsje" HeaderText="决算金额" HeaderStyle-Width="100" ItemStyle-Width="100"
                        DataFormatString="{0:N2}">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="syje" HeaderText="剩余金额（结转金额）" HeaderStyle-Width="100"
                        ItemStyle-Width="100" DataFormatString="{0:N2}">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" CssClass="myGridItemRight" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="username" HeaderText="制单人" HeaderStyle-Width="80" ItemStyle-Width="80">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                </Columns>
                <PagerStyle Visible="False" />
            </asp:DataGrid>
        </div>
    </div>
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
