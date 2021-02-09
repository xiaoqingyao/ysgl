<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bmfjjeqr_AddAttachment.aspx.cs"
    Inherits="webBill_ysglnew_Bmfjjeqr_AddAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>附件</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="340px" />
                    <asp:Label ID="Lafilename" runat="server" Text="" Width="440px"></asp:Label>
                    <asp:HiddenField ID="hiddFileDz" runat="server" />
                </td>
                <td style="text-align: left; padding-left: 30px; width: 200px">
                    <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btn_sc_Click" />
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right">
                    <asp:Button runat="server" ID="btn_save" CssClass="baseButton" Text="保 存" OnClick="btn_save_Click" />
                    <input type="button" id="btn_cancle" onclick="javascript:self.close();" value="关 闭"
                        class="baseButton" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
