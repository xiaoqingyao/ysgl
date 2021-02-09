<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleDeptFeeLeft.aspx.cs" Inherits="SaleBill_SaleFee_SaleDeptFeeLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
        <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <asp:TreeView ID="treeViewFeeType" runat="server" ShowLines="True">
            <SelectedNodeStyle BackColor="#EBF2F5" />
            <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="Select"
                        Text="费用科目" Value=""></asp:TreeNode>
                </Nodes>
        </asp:TreeView>
    </div>
    </form>
</body>
</html>
