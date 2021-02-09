<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RollYsgcList.aspx.cs" Inherits="webBill_cwgl_RollYsgcList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>年度预算过程列表</title>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <script language="javascript" type="Text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
               
            });
        });


        function SingleSelect(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }

            aControl.checked = true;
        }
        function Check() {
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin: 5px">
            年份：<asp:DropDownList ID="ddl_nd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_nd_selectedIndexChanged">
            </asp:DropDownList>
            <asp:Button ID="btn_js" runat="server" Text="进入结转详细页..." OnClientClick="return Check()"
                CssClass="baseButton" Height="25px" OnClick="btn_js_Click" />
            <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            <label style="color: Red">
                【操作提示】：请勾选一条未结转的预算过程然后单击“进入结转详细页”。</label>
            <%-- <input id="btn_yszj" type="button" value="预算转结"  onclick="ToYsjz()" class="baseButton" />--%>
        </div>
        <div>
            <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                CssClass="myGrid" Width="1200">
                <Columns>
                    <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="50" ItemStyle-Width="50">
                        <ItemTemplate>
                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" onclick="javascript:SingleSelect(this);"
                                Width="10" />
                        </ItemTemplate>
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="gcbh" HeaderText="过程编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="xmmc" HeaderText="过程名称" HeaderStyle-Width="150" ItemStyle-Width="150">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="kssj" HeaderText="预算开始日期" DataFormatString="{0:D}" HeaderStyle-Width="120"
                        ItemStyle-Width="120">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="jzsj" HeaderText="预算截止日期" DataFormatString="{0:D}" HeaderStyle-Width="120"
                        ItemStyle-Width="120">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="fqr" HeaderText="过程发起人" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="fqsj" HeaderText="过程发起时间" DataFormatString="{0:D}" HeaderStyle-Width="100"
                        ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="status" HeaderText="status" Visible="False"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ysType" HeaderText="预算类型" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="nian" HeaderText="预算年份" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="yue" HeaderText="预算季度/月份" HeaderStyle-Width="100" ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="statusName" HeaderText="过程状态" HeaderStyle-Width="100"
                        ItemStyle-Width="100">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Isjz" HeaderText="是否结转" HeaderStyle-Width="100" ItemStyle-Width="100">
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
    </form>
</body>
</html>
