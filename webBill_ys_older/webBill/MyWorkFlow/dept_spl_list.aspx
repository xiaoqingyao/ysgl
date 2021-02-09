<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dept_spl_list.aspx.cs" Inherits="webBill_MyWorkFlow_dept_spl_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //部门选择
            $("#txt_dept").autocomplete({
                source: availableTags
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 5px">
            <asp:Label ID="lbl_massege" Text="【友情提示】:下面输入框内输入从哪个部门复制审批流程。" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div style="margin-top: 5px">
            <table>
                <tr>
                    <td>部门:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_dept" runat="server">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btn_fz" Text="复制" OnClick="btn_fz_Click" CssClass="baseButton" />
                    </td>
                </tr>
            </table>
            <%--部门：--%>
        </div>
    </form>
</body>
</html>
