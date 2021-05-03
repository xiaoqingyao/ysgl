<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mainFrame.aspx.cs" Inherits="main_mainFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用内控及网上报销系统 v1.0</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openWindow(url, width, height,title) { 
            window.frames["main"].openWindow(url, width, height, title);
        }
    </script>
</head>
<frameset rows="80,*" id="mainFrame" framespacing="0" frameborder="0">
    <frame id="top" name="top" src="top.aspx" noresize scrolling="no" />
    <frame id="main" name="main" src="MainNew.aspx" noresize scrolling="no"></frame>
</frameset>
</html>
