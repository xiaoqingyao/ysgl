<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LrbBmfjList_ImportExcel.aspx.cs"
    Inherits="webBill_ysglnew_LrbBmfjList_ImportExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 74px;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="baseDiv">
        <label id="msg" style="color: Red">
            【友情提示】：请导入标准Excel文件且文档的列必须与模板完全一致，否则将导入不成功！</label>
        <table class="myTable" width="100%">
            <tr>
                <td class="style1">
                    导入：
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="baseButton" Width="85%" />
                    <asp:Button ID="btnImport" runat="server" Text="导 入" CssClass="baseButton" OnClick="btnImport_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
