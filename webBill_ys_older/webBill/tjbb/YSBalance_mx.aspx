<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YSBalance_mx.aspx.cs" Inherits="webBill_tjbb_YSBalance_mx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算平衡表明细页面</title>
    <base target="_self" />
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        //高亮显示
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            gudingbiaotounew($("#myGrid"), 600);
            initMainTableClass("<%=myGrid.ClientID%>");

            gudingbiaotounew($("#DGxiangmu"), 600);
            initMainTableClass("<%=DGxiangmu.ClientID%>");
            
            $("#btn_toExcel").click(function() {
                if ($("#myGrid tr").size() <= 0) {
                    return false;
                }
            });
            $("#btn_close").click(function () {
                window.location.href = "YSBalance.aspx";
              //  window.self.close();
            });

        });


        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divOption" class="baseDiv">
            <input id="btn_close" type="button" value="返回" class="baseButton" />
            <asp:Button ID="btn_toExcel" runat="server" Text="导出Excel" OnClick="btn_excel_Click"
                CssClass="baseButton" />
            <label runat="server" id="msg" style="color: Red">
            </label>
        </div>
        <div class="baseDiv">
            显示内容：<asp:RadioButton ID="RdbMx" runat="server" Text="明细" GroupName="a" Checked="true"
                OnCheckedChanged="RdbMx_CheckedChanged" AutoPostBack="true" />
            <asp:RadioButton ID="RdbXmDangAn" runat="server" Text="预算项目档案" GroupName="a" OnCheckedChanged="RdbXmDangAn_CheckedChanged"
                AutoPostBack="true" />
        </div>
        <div class="baseDiv" style="position: relative; word-warp: break-word; word-break: break-all">
            <asp:GridView ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word" 
                AllowPaging="True" PageSize="99999">
                <Columns>
                    <asp:BoundField DataField="yskmmc" HeaderText="预算科目" HeaderStyle-Width="200" ItemStyle-Width="200">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="je" HeaderText="金额" HeaderStyle-Width="120" ItemStyle-Width="120"
                        DataFormatString="{0:N}">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" />
                    </asp:BoundField>
                </Columns>
                <%--<PagerStyle Visible="False" />--%>
            </asp:GridView>
            <asp:GridView ID="DGxiangmu" runat="server" AutoGenerateColumns="False" CellPadding="3"
                CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word" Width="320px"
                AllowPaging="True" PageSize="99999">
                <Columns>
                    <asp:BoundField DataField="proname" HeaderText="预算科目" HeaderStyle-Width="200" ItemStyle-Width="200">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="je" HeaderText="金额" HeaderStyle-Width="120" ItemStyle-Width="120"
                        DataFormatString="{0:N}">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" />
                    </asp:BoundField>
                </Columns>
                <%--<PagerStyle Visible="False" />--%>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
