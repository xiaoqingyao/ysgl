<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CopyDeptXm.aspx.cs" Inherits="webBill_xmsz_CopyDeptXm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>复制部门项目</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#txtFromDept").autocomplete({
                source: availableTagsDept
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table class="baseTable" width="90%">
            <tr>
                <td>
                    复制到部门：
                </td>
                <td>
                    <asp:Label ID="lblToDept" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    参照部门：
                </td>
                <td>
                    <asp:TextBox ID="txtFromDept" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    <asp:Button ID="btn_sure" runat="server" Text="确 定" OnClientClick="return confirm('将会删除之前该部门对应的项目，是否继续？');"
                        OnClick="btn_sure_Click"  CssClass="baseButton"/>
                    <input id="btn_cancle" type="button" class="baseButton" value="取 消" onclick="javascript:self.close();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
