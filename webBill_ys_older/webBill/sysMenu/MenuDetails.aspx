<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuDetails.aspx.cs" Inherits="webBill_sysMenu_MenuDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单信息修改页</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript">
        function checkForEdit() {
            var myName = document.getElementById("txtMyName").value;
            //alert(myName);
            if (myName == null || myName == "") {
                alert("菜单自定义名称不能为空！");
                return false;
            } else {
                return true;
            }
        }
        function cancle() {
            window.returnValue = "";
            window.close();
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">菜 &nbsp; 单&nbsp; &nbsp;信 &nbsp; 息</span></strong>
                </td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 449px">
                        <tr>
                            <td class="tableBg" style="text-align: right">
                                菜单名称：
                            </td>
                            <td>
                                <asp:Label ID="lbeName" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" style="text-align: right">
                                自定义菜单名称：
                            </td>
                            <td>
                                <asp:TextBox ID="txtMyName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" style="text-align: right">
                                序号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtMenuOrder" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; height: 35px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click"
                        OnClientClick="return checkForEdit();" />
                    &nbsp;&nbsp;&nbsp;
                    <input id="Button1" type="button" value="取 消" class="baseButton" onclick="cancle();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
