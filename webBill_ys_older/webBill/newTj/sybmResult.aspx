<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sybmResult.aspx.cs" Inherits="webBill_newTj_sybmFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>部门预算统计</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
         .NoBreak{white-space:pre;
                padding: 3px;
                line-height: 20px;
                border-bottom:1px solid; border-top:1px solid; border-color:#1855C6; border-right:1px solid;border-left:1px solid;
                }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
<%--    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>--%>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        function openDetail(kssj,jzsj,dept)
        {
           window.showModalDialog('sybmResult_mx.aspx?kssj='+kssj+'&jzsj='+jzsj+'&deptCode='+dept , 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes'); 
        }
        $(function() {
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $(".highlight").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#<%=GridView1.ClientID%> tr td").click(function() {
                var je = $(this).html();
                var lie = $(this).prevAll().length;
                if (je != "0.00" && lie > 2) {
                    var km = $(this).parent().find("td")[0].innerHTML;
                    km = km.split("]")[0];
                    km = km.substring(1, km.length);
                    var bm = $("#<%=GridView1.ClientID%> th:eq(" + lie + ")").html();
                    bm = bm.split("/")[0];
                    var kssj = $("#Label1").html().split(":")[1];
                    var jssj = $("#Label2").html().split(":")[1];
                    self.location.href = "sybmResult_mx.aspx?deptCode=" + bm + "&kmCode=" + km + "&kssj=" + kssj + "&jzsj=" + jssj;
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
                   <asp:GridView ID="GridView1" runat="server" CssClass="myGrid"
                    AutoGenerateColumns="False" onprerender="GridView1_PreRender" EnableViewState="true" >
                        <HeaderStyle CssClass="myGridHeader" />
                        <RowStyle CssClass="myGridItem" />
                    </asp:GridView>
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
