<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuHelpDetail.aspx.cs" Inherits="webBill_sysMenu_MenuHelpDetail" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>帮助信息编辑</title>
     <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:5px;padding:0px;">
    <div id="div_top">
    <asp:Label ID="lblUserInfo" runat="server" Text="显示当前待设置权限人员信息" ForeColor="Red"></asp:Label>
         &nbsp;<asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />
    </div>
    <div>
      <strong><h2 style=" text-align:center;"><span style="font-size: 14pt;color:Red;" id="h_title" runat="server"></span><span style="font-size: 14pt">帮助信息编辑</span></h2></strong>
    </div>
    <div id="div_edit">
     <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" DefaultLanguage="zh-cn" Height="600">
                            </FCKeditorV2:FCKeditor>
    
    </div>
    </div>
    </form>
</body>
</html>
