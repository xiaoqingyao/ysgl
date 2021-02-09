<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysxmResult.aspx.cs" Inherits="webBill_newTj_ysxmFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>部门预算统计</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <%--<script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="Text/javascript">
        function openDetail1(deptCode,billCode)
        {//cwtbDetail.aspx?from=ystbFrame&billCode=" + billCode + "&deptCode=" + deptCode
           window.showModalDialog('../xmsz/cwtbDetail.aspx?deptCode='+deptCode+'&from=ystbFrame&billCode='+billCode , 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes'); 
        }
         function openDetail2(xmCode,kssj,jzsj,dept)
        {
           window.showModalDialog('ysxmResult_mx.aspx?xmCode='+xmCode+'&kssj='+kssj+'&jzsj='+jzsj+'&deptCode='+dept , 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes'); 
        }
    </script>--%>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
       
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $(".highlight").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#<%=myGrid.ClientID%> tr td").click(function() {
                var je = $(this).html();
                var lie = $(this).prevAll().length;
                var hang =$(this).parent().prevAll().length;
                if (hang>1){
                    if ( lie == 3) {
                        var bm = $(this).parent().find("td")[0].innerHTML;
                        bm = bm.split("]")[0];
                        bm = bm.substring(1, bm.length);
                        var xm = $(this).parent().find("td")[1].innerHTML;
                       
                        var kssj = $("#Label1").html().split(":")[1];
                        var jssj = $("#Label2").html().split(":")[1];
                        self.location.href = "ysxmResult_mx.aspx?deptCode=" + bm + "&xmCode=" + xm + "&kssj=" + kssj + "&jzsj=" + jssj + "&cxlx=zf";
                    }
                    if ( lie == 4) {
                        var bm = $(this).parent().find("td")[0].innerHTML;
                        bm = bm.split("]")[0];
                        bm = bm.substring(1, bm.length);
                        var xm = $(this).parent().find("td")[1].innerHTML;
                       
                        var kssj = $("#Label1").html().split(":")[1];
                        var jssj = $("#Label2").html().split(":")[1];
                        self.location.href = "ysxmResult_mx.aspx?deptCode=" + bm + "&xmCode=" + xm + "&kssj=" + kssj + "&jzsj=" + jssj + "&cxlx=bx";
                    }
                }
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
             <tr><td style="height: 25px">
               <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>&nbsp;
               <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>&nbsp;
               <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>&nbsp;
               <asp:Button ID="btn_excel" runat="server" Text="导出excel" CssClass="baseButton" 
            onclick="btn_excel_Click" />
            <asp:Button
                   ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click" Text="返 回" /></td></tr>
            <tr>
                <td align="left">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" PageSize="17" Width="80%" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="bm" HeaderText="部门">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="xmbh" HeaderText="项目编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="xmmc" HeaderText="项目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ysje" HeaderText="支付金额(不进费用)" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bxje" HeaderText="报销金额(进费用)" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hjje" HeaderText="合计金额" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>                    
                </td>
            </tr>
            <tr style="display: none;">
                <td align="left">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            </tr>
        </table>
    </form>
</body>
</html>
