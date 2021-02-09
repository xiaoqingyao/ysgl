<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LrbBmfjDetails.aspx.cs" Inherits="webBill_ysglnew_LrbBmfjDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>增加预算内金额</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
      //替换非数字
        function replaceNaN(obj) {
            if (obj.value!="-"&&isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        //非空验证
        function checkisNaN(obj) {
            if (obj.value!="-"&&isNaN(obj.value)) {
                obj.value = '';
                alert("不能为空！");
                obj.focus();
                return false;
            };
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="baseTable" width="98%">
            <tr>
                <td class="tableBg2" style="text-align:right">
                    原预算金额：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblysmoney"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align:right">
                    追加预算金额：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtymoney" Width="98%" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align:right">
                    可分配金额：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblwfmoney"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center">
                    <asp:Button ID="btsave" runat="server" Text="确 定" CssClass="baseButton"  OnClick="btsave_Click" />
                    <asp:Button ID="btclose" runat="server" Text="取 消" CssClass="baseButton" OnClientClick="javascript:window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
