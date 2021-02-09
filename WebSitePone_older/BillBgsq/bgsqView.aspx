<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bgsqView.aspx.cs" Inherits="BillBgsq_bgsqView" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>报告申请详细信息</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
      
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="dialog" data-theme="b" method="post">
    <div>
        <div data-role="header">
            <h1>
                报告申请详细信息</h1>
        </div>
        <div data-role="content">
            <table style="color: Black; font-family: 微软雅黑;">
                <tr>
                    <td style="width: 70px" class='tdborder'>
                        采购单号:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_cgbh" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        申请日期:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_sj" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        承办人:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_cbr" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        预计费用:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_yjfy" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        部门:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_dept" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        申请类别:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_cglb" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        内容:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_zynr" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class='tdborder'>
                        说明:
                    </td>
                    <td class='tdborder'>
                        <asp:Label ID="lb_sm" runat="server" Text="Label"></asp:Label>
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
    </div>
    </form>
</body>
</html>
