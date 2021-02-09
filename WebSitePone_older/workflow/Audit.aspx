<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Audit.aspx.cs" Inherits="workflow_Audit" %>
<!DOCTYPE>
<html>
<head runat="server">
    <title>审核</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
            <a href="../Index.aspx" data-icon="home" data-role="button" data-ajax="false">主页</a>
            <h1>
                我的待审核单据</h1>
        </div>
        <div data-role="content">
            <ul data-role="listview" data-inset="true" runat="server" id="remainUL">
              <%--  <li><a href='ybbxAuditList.aspx' data-ajax='false'>一般报销单<span class='ui-li-count'>9</span></a></li>
                <li><a href='ybbxAuditList.aspx' data-ajax='false'>出差申请单<span class='ui-li-count'>5</span></a></li>--%>
            </ul>
        </div>
        <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>
</body>
</html>
