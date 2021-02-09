<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ysyj.aspx.cs" Inherits="webBill_cwgl_Ysyj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
  
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .highlight{ background:#EBF2F5;}
        .hiddenbill{ display:none;}
    </style>
    <title></title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
     <script language="javascript" type="Text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight"); 
                 var billCode = $(this).find("td")[0].innerHTML;
                $("#hd_billCode").val(billCode);
             });
         });
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        选择年份:
        <asp:DropDownList ID="ddl_year" runat="server">
            <asp:ListItem>2012</asp:ListItem>
            <asp:ListItem>2013</asp:ListItem>
            <asp:ListItem>2014</asp:ListItem>
            <asp:ListItem>2015</asp:ListItem>
            <asp:ListItem>2016</asp:ListItem>
            <asp:ListItem>2017</asp:ListItem>
            <asp:ListItem>2018</asp:ListItem>
            <asp:ListItem>2019</asp:ListItem>
            <asp:ListItem>2020</asp:ListItem>
            <asp:ListItem>2021</asp:ListItem>
            <asp:ListItem>2022</asp:ListItem>
            <asp:ListItem>2023</asp:ListItem>
            <asp:ListItem>2024</asp:ListItem>
            <asp:ListItem>2025</asp:ListItem>
            <asp:ListItem>2026</asp:ListItem>
            <asp:ListItem>2027</asp:ListItem>
            <asp:ListItem>2028</asp:ListItem>
            <asp:ListItem>2029</asp:ListItem>
            <asp:ListItem>2030</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;
        <asp:Button ID="btn_select" runat="server" Text="查询" CssClass="baseButton" 
            onclick="btn_select_Click"   />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_save" runat="server" Text="月结算" CssClass="baseButton" 
            onclick="btn_save_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_qx" runat="server" Text="取消月结" CssClass="baseButton" 
            onclick="btn_qx_Click"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
    </div>
    <div>
    <%--    <asp:GridView ID="myGrid" runat="server" AutoGenerateColumns="False" CssClass="myGrid">
            <Columns>
                <asp:BoundField DataField="yf" HeaderText="月份" />
                <asp:BoundField DataField="yjbj" HeaderText="是否月结" />
                <asp:BoundField DataField="userCode" HeaderText="操作人" />
                <asp:BoundField DataField="yjsj" HeaderText="结算时间" />
                
            </Columns>
            <HeaderStyle CssClass="myGridHeader" />
        </asp:GridView>--%>
        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" 
                        CellPadding="3" CssClass="myGrid" PageSize="17" Width="611px"  AllowPaging="True">
                        <PagerStyle Visible="False" />
                        <Columns>
                            <asp:BoundColumn DataField="yf" HeaderText="月份">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yf" HeaderText="月份">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yjbj" HeaderText="是否月结">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yjsj" HeaderText="结算时间" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userCode" HeaderText="操作人" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
        <asp:HiddenField ID="hd_billCode" runat="server" />
    </div>
    </form>
</body>
</html>
