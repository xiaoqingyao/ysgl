<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zjfsDetail.aspx.cs" Inherits="ZiChan_ZiChanGuanLi_zjfsDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>增减方式编辑页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script language="javascript" type="Text/javascript">

        function check()
        {
               
            var reptname = $("#txtreportname").val();
            if (reptname.length < 1) {
                alert("增减名称不能为空！");
                $("#txtreportname").focus();
                return false;
            }
            else
            {
                return true;
            
            }

        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table class="baseTable" width="95%" style="background-color: #EBF2F5;">
        <tr>
            <td colspan="6" class="billtitle" style="text-align: center; height: 26px; font-family: @华文宋体; font-size: medium;
                    font-weight: 800; background-color:#EDEDED;" >
                增减方式
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                增减编号：
            </td>
            <td>
                <asp:Label ID="lblcode" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: right" class="InputLabel">
                增减名称：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtreportname" runat="server"></asp:TextBox>
            </td>
       
            <td style="text-align: right" class="InputLabel">
                增减方式：
            </td>
            <td class="baseText">
                <asp:DropDownList ID="dropzjfs" runat="server">
                    <asp:ListItem Value="1">增</asp:ListItem>
                    <asp:ListItem Value="0">减</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                备注：
            </td>
            <td colspan="5" class="baseText">
                <asp:TextBox ID="txtreportexplain" runat="server" TextMode="MultiLine" Height="250px"
                    Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: center">
                <asp:Button ID="btnsave" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();"
                    OnClick="btnsave_Click" />
                <asp:Button ID="btnclose" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
