﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwkmLeft.aspx.cs" Inherits="cwkm_cwkmLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <style type="Text/css">
        .Hidden{
            overflow-x:hidden 
        }
        
    </style>
</head>
<body class="Hidden">
    <form id="form1" runat="server">
   
   <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="1" EnableViewState="false">
        <SelectedNodeStyle BackColor="#EBF2F5" />
    </asp:TreeView>
   
    </form>
</body>
</html>
