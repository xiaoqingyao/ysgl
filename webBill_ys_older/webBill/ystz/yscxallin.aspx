<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yscxallin.aspx.cs" Inherits="webBill_ystz_yscxallin"  EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    $(function(){
    $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
      $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
    });
    </script>
    <style type="text/css">
         .NoBreak
        {   
             white-space:pre;

        }
    </style>
     <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    
    <form id="form1" runat="server">
    
    <div>
    <table>
    <tr><td>
        年份
        </td><td>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </td><td>
        起始月份
        </td><td>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>01</asp:ListItem>
             <asp:ListItem>02</asp:ListItem>
             <asp:ListItem>03</asp:ListItem>
             <asp:ListItem>04</asp:ListItem>
             <asp:ListItem>05</asp:ListItem>
             <asp:ListItem>06</asp:ListItem>
             <asp:ListItem>07</asp:ListItem>
             <asp:ListItem>08</asp:ListItem>
             <asp:ListItem>09</asp:ListItem>
             <asp:ListItem>10</asp:ListItem>
             <asp:ListItem>11</asp:ListItem>
             <asp:ListItem>12</asp:ListItem>
        </asp:DropDownList>
        </td><td>
       终止月份</td><td>
        <asp:DropDownList ID="DropDownList2" runat="server">
        <asp:ListItem>01</asp:ListItem>
             <asp:ListItem>02</asp:ListItem>
             <asp:ListItem>03</asp:ListItem>
             <asp:ListItem>04</asp:ListItem>
             <asp:ListItem>05</asp:ListItem>
             <asp:ListItem>06</asp:ListItem>
             <asp:ListItem>07</asp:ListItem>
             <asp:ListItem>08</asp:ListItem>
             <asp:ListItem>09</asp:ListItem>
             <asp:ListItem>10</asp:ListItem>
             <asp:ListItem>11</asp:ListItem>
             <asp:ListItem>12</asp:ListItem>
        </asp:DropDownList>
        </td><td>
        <asp:Button ID="Button2" runat="server" Text="统计查询" onclick="Button2_Click" CssClass="baseButton"/>  
        </td><td>                               
        <asp:Button ID="Button1" runat="server" Text="导出Excel"  CssClass="baseButton"
            onclick="Button1_Click" />
            </td>
            <td>
              <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
            </td>
    </tr>
         </table>
    </div>
    <div>
         <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" 
             AutoGenerateColumns="False" onrowdatabound="GridView1_RowDataBound">
            <HeaderStyle CssClass="myGridHeader" />
            <RowStyle CssClass="myGridItem" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
