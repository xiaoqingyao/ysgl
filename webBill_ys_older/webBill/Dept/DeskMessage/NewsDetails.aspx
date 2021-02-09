<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsDetails.aspx.cs" Inherits="webBill_DeskMessage_NewsDetails" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function() {
            $("#btn_choose").click(function() {
                var str = window.showModalDialog('../select/SelectMoreUserFrame.aspx', 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:750px;status:no;scroll:yes');
                if (str != undefined && str != "") {
                    var tzr = $("#txt_tzr").val();
                    if (tzr == "") {
                        $("#txt_tzr").val(str);
                    }
                    else {
                        $("#txt_tzr").val(tzr + ":" + str);
                    }

                }
            });
            $("#btn_reset").click(function() {
                $("#txt_tzr").val("");
            });
        });
    </script>
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="myGrid style1">
            <tr>
                <td colspan="4" style="text-align:center; font-size:12pt; font-weight:bolder" class="tableBg">
                    通知发布
                </td>
            </tr>
            <tr>
                <td class="tableBg">
                    标题:</td>
                <td colspan="3">
                    <asp:TextBox ID="txt_title" Width="90%" runat="server" CssClass="baseText"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="tableBg">
                    内容</td>
                <td colspan="3">
                    <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" DefaultLanguage="zh-cn" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                    </td>
            </tr>
            <tr>
                <td class="tableBg">
                    附件:</td>
                <td colspan="3">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:HiddenField ID="hf_upfile" runat="server" />
                    <asp:HiddenField ID="hf_code" runat="server" />
                    </td>
            </tr>
            <tr id="info_tzr" >
                <td class="tableBg">通知人:</td>
                <td colspan="3">
                    <asp:TextBox ID="txt_tzr" runat="server" CssClass="baseText" 
                        Width="500px"></asp:TextBox>            

                    <input type="button" value="选 择" class="baseButton" id="btn_choose" />&nbsp;
                    <input type="button" value="重 置" class="baseButton" id="btn_reset" />
                </td>
            </tr>
            <tr>
                <td class="tableBg">
                    备注:</td>
                <td colspan="3">
                    <asp:TextBox ID="txt_memo" Width="90%" runat="server" CssClass="baseText"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align:center">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" 
                        onclick="btn_save_Click" />&nbsp;
                    <asp:Button ID="btn_return" runat="server" Text="取 消" CssClass="baseButton" 
                        onclick="btn_return_Click" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
