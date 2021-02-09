<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundBorrowUpdateDj.aspx.cs"
    Inherits="SaleBill_BorrowMoney_FundBorrowUpdateDj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    </script>
    <style>
        .r
        {    text-align:right;
        	}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="250px" class="myTable"  style=" margin:10px;">
            <tr>
                <td class="tableBg2" style="text-align: right">
                    借款金额：
                </td>
                <td style="text-align: right" >
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td> 
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right" >
                    核定金额：
                </td>
                <td style="text-align: right" >
                  <asp:TextBox ID="TextBox1" runat="server"   CssClass="r" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   colspan="2"  align="center">
                    <asp:Button ID="Button1" runat="server" Text="确 定"  CssClass="basebutton" OnClick="Button1_click"/>
                </td>
            </tr>
        </table>
        
       
    </div>
    </form>
</body>
</html>
