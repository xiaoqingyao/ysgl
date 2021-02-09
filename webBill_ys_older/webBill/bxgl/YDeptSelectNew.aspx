<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YDeptSelectNew.aspx.cs" Inherits="webBill_bxgl_YDeptSelectNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择部门</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type="text/javascript">
         $(function() {
             $("#find_txt_km").autocomplete({
                 source: availableTags,
                 select: function(event, ui) {
                     var rybh = ui.item.value;
                     if (rybh != "" && rybh != undefined) {
                         $("#hddept").val(rybh);
                         $("#btn_qd").click();
                     }
                 }
             });
         });
    </script>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                快速检索：<asp:TextBox ID="find_txt_km" runat="server"></asp:TextBox>
                <asp:Button ID="btn_qd" runat="server" CssClass="hiddenbill" Text="确定" OnClick="Txtchange" />
                        <input type="hidden" id="hddept" runat="server" />

            </td>
        </tr>
        <tr>
            <td>
                <div style="overflow: auto; height: 380px;">
                    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2">
                        <Nodes>
                            <asp:TreeNode ImageUrl="../Resources/Images/treeView/treeHome.gif" SelectAction="None"
                                Text="使用部门" Value=""></asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </div>
            </td>
        </tr>
    </table>
    <div style="bottom: 5px; position: fixed; width: 100%">
        <div style="width: 180px; margin: 0 auto;">
            <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="确 定" OnClick="Button1_Click" />
            <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />
        </div>
    </div>
    </form>
</body>
</html>
