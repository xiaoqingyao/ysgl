<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectCwkm.aspx.cs" Inherits="webBill_yskm_selectCwkm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择财务科目</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="selectCwkmLeft.aspx?deptCode=<%=deptCode %>&yskmCode=<%=yskmCode %>" />
<frame id="list" name="list" src="selectCwkmList.aspx?kmCode=&deptCode=<%=deptCode %>&yskmCode=<%=yskmCode %>"" />
<%--<frame id="list" name="list" src="selectCwkmList.aspx?deptCode=<%=deptCode %>&yskmCode=<%=yskmCode %>"" />--%>
</frameset>
</html>
