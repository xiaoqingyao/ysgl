<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LrbBmfjLeft.aspx.cs" Inherits="webBill_ysglnew_LrbBmfjLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
        td
        {
            text-align: left;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#find_txt_km").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    if (rybh != "" && rybh != undefined) {
                        $("#hdkm").val(rybh);
                        $("#btn_qd").click();
                    }
                }
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin-left: 5px">
            预算年度：
            <asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    <div style="margin-top: 2px; margin-left: 5px">
        快速检索：<asp:TextBox ID="find_txt_km" runat="server"></asp:TextBox>
        <asp:Button ID="btn_qd" runat="server" CssClass="hiddenbill" Text="确定" OnClick="Txtchange" />
    </div>
    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True"  EnableViewState="false">
    </asp:TreeView>
    <input type="hidden" id="hdkm" runat="server" />
    </div>
    </form>
</body>
</html>
