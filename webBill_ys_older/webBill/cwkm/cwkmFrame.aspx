<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwkmFrame.aspx.cs" Inherits="cwkm_cwkmFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="cwkmLeft.aspx?wdheight=<%=Request["wdheight"] %>" />
<frame id="list" name="list" src="cwkmList.aspx?wdheight=<%=Request["wdheight"] %>&kmCode=" /></frameset>
</html>
