<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BenefityskmLeft.aspx.cs" Inherits="webBill_ysglnew_BenefityskmLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <style type="Text/css">
        .Hidden {
            overflow-x: hidden;
        }
    </style>
</head>
<body class="Hidden">
    <form id="form1" runat="server">
        &nbsp; &nbsp; 财年：
        <asp:DropDownList ID="drpSelectNd" runat="server"
            AutoPostBack="True"
            OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" EnableViewState="false">
            <SelectedNodeStyle BackColor="#EBF2F5" />
        </asp:TreeView>

    </form>
</body>
</html>
