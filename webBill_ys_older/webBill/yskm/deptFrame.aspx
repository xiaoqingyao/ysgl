<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptFrame.aspx.cs" Inherits="webBill_yskm_deptFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="deptLeft.aspx" />
<frame id="list" name="list" src="deptList.aspx?deptCode=" /></frameset>
</html>
