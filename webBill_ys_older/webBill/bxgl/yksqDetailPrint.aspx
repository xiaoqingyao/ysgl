<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yksqDetailPrint.aspx.cs"
    Inherits="webBill_bxgl_yksqDetailPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用款审批单打印页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .class1
        {
            width: 200px;
            text-align: center;
        }
        .class2
        {
            text-align: left;
            padding-left: 5px;
        }
        .myTable td
        {
             height: 50px;
                border: 1px solid;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="float: right; width: 800px; margin-right: 0px; margin-top: 140px;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 36px; font-size: 35px;">
                    <strong>用&nbsp;款&nbsp;审&nbsp;批&nbsp;单 </strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 14px;">
                    <asp:Label ID="date" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <table border="1" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                        <tr>
                            <td class="class1" style="width: 15%; height:80px;">
                                用款部门
                            </td>
                            <td class="class2">
                                <asp:Label ID="dept" runat="server" Text="dept"></asp:Label>
                            </td>
                            <td class="class1" style="width: 15%;">
                                用&nbsp;&nbsp;途
                            </td>
                            <td class="class2">
                                <asp:Label ID="yt" runat="server" Text="yt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="class1">
                                申请金额
                            </td>
                            <td class="class2">
                                <asp:Label ID="je" runat="server" Text="je"></asp:Label>
                            </td>
                            <td class="class1">
                                入库单号
                            </td>
                            <td class="class2">
                                <asp:Label ID="dh" runat="server" Text="dh"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="class2" style="width: 50%">
                                财务负责人（签字）
                            </td>
                            <td colspan="2" class="class2">
                                部门负责人（签字）
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="class2">
                                院长（签字）
                            </td>
                            <td colspan="2" class="class2">
                                分管副院长
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height:5px">
                </td>
            </tr>
            <tr>
                <td>
                    <div style="margin-left: 600px;">
                        经办人&nbsp;&nbsp;<asp:Label ID="jbr" runat="server" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
