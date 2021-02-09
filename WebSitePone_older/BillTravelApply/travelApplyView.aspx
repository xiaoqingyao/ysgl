<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplyView.aspx.cs"
    Inherits="BillTravelApply_travelApplyView" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>出差申请单详细信息</title>
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
                <h1>出差申请单详细信息</h1>
            </div>
            <div data-role="content">
                <table style="color: Black; font-family: 微软雅黑;">
                    <tr>
                        <td style="width: 90px" class='tdborder'>出差派遣部门:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_bm" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class='tdborder'>制单日期:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_billDate" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder'>申请人:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_billUser" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder'>出差日期:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_travelDate" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class='tdborder'>地点:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_address" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class='tdborder'>出差事由:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_reasion" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder'>日程安排:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_plan" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder' colspan="2">申请交通工具:<asp:Label ID="lb_jtgj" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder'>是否超标准:
                        </td>
                        <td class='tdborder'>
                            <asp:Label ID="lb_isbz" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder'>预计总金额:
                        </td>
                        <td class='tdborder'>￥<asp:Label ID="lb_zje" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdborder' colspan="2">
                            <div id="chr" runat="server" style="border-top: 1px solid gray;"></div>
                        </td>

                    </tr>


                    <tr>
                        <td colspan="2">
                            <div style="border-top: 1px solid gray">
                                <h5>预计费用明细</h5>
                                <table class='tab-hs ItemTable' style='color: Black; font-family: 微软雅黑;'>
                                    <tr>
                                        <td class='tdOdd'>交通费:&nbsp;&nbsp;￥<asp:Label ID="lb_jtf" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='tdOdd'>住宿费:&nbsp;&nbsp;￥<asp:Label ID="lb_zsf" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='tdOdd'>业务招待费:&nbsp;&nbsp;￥<asp:Label ID="lb_zdf" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='tdOdd'>会议费:&nbsp;&nbsp;￥<asp:Label ID="lb_hyf" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='tdOdd'>印刷费:&nbsp;&nbsp;￥<asp:Label ID="lb_ysf" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='tdOdd'>其他等:&nbsp;&nbsp;￥<asp:Label ID="lb_qt" runat="server" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <div id="wf">
                            </div>
                        </td>
                    </tr>
                    <tr id="aduittr" runat="server">
                        <td>审批意见:
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
