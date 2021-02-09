<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptFrame.aspx.cs" Inherits="Dept_deptFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择部门</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="deptLeft.aspx?mxGuid=<%=mxGuid %>"></frame>
<frame id="list" name="list" src="deptList.aspx?deptCode=&mxGuid=<%=mxGuid %>" /></frameset>
</html>
