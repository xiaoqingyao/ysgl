<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YsAuditView.aspx.cs" Inherits="BillYs_YsAuditView" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>预算追加单详细页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />
    <style>
        .div-yskm
        {
            margin-top: 10px;
            border-bottom: 2px solid red;
        }
        .tab-yskm
        {
            color: Blue;
            font-size: 16px;
        }
        .div-hs
        {
            margin-top: 10px;
            border-bottom: 1px solid gray;
            text-align: left;
        }
        .tab-hs
        {
            color: Black;
            font-size: 14px;
            background-color:Green;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b">
    <div>
        <div data-role="header">
            <%-- <a href="" data-rel="back" data-icon="back" data-ajax="false">返回</a>--%>
            <h1>
                 <asp:Literal ID="ltrTitle" runat="server" Text="预算审批详细"></asp:Literal></h1>
        </div>
        <div data-role="content">
            <table style="color: Black; font-family: 微软雅黑;">
                <tr style="display:none">
                    <td class='tdborder'>
                        单据编号：
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lbBillCode" runat="server"></asp:Label>
                    </td >
                </tr>
                <tr>
                    <td class='tdborder'>
                        制单日期：
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lbBillData" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        制单人：
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lbBillUser" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        所在部门：
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lbBillDept" runat="server"></asp:Label>
                    </td >
                </tr>
                <tr>
                    <td class='tdborder'>
                        单据金额：
                    </td>
                    <td class='tdborder'>
                        ￥<asp:Label ID="lbBillje" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class='tdborder'>
                        <asp:Label ID="lbMx" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" >
                        <div id="wf">
                        </div>
                    </td>
                </tr>
                <tr id="aduittr" runat="server">
                    <td>
                        审批意见:
                    </td>
                    <td>
                        <input type="text" data-role="none" class="myInput" id='txt_mymind' />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <input id="btn_audit" type="button" value="通过" data-inline="true" onclick="AuditChildren();"
                            runat="server" />
                        <input id="btn_cancel" type="button" value="驳回" data-inline="true" onclick="CancelChildren();"
                            runat="server" />
                        <input id="btn_submit" type="button" value="提交" data-inline="true" onclick="SubmitChildren();"
                            runat="server" />
                        <input id="btn_delete" type="button" value="删除" data-inline="true" onclick="DeleteChildren();"
                            runat="server" />
                        <input id="btn_revoke" type="button" value="撤销" data-inline="true" onclick="RevorkChildren();"
                            visible="false" runat="server" />
                        <%-- <input id="btn_close" type="b
                        utton" value="关闭" data-inline="true" onclick="test();" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <%-- <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>--%>
    </div>
    </form>
</body>
</html>
