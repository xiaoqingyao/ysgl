﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="glqxFrame.aspx.cs" Inherits="xtsz_glqxFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="glqxLeft.aspx" />
<frame id="list" name="list" src="glqxList.aspx?userCode=" /></frameset>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
        </script>
</html>
