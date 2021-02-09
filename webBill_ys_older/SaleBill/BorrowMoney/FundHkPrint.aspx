<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundHkPrint.aspx.cs" Inherits="SaleBill_BorrowMoney_FundHkPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>还款详细信息</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
     <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
        <style type="text/css">
        .title
        {
            font-size: 18px;
            font-family: 微软雅黑;
            font-weight: 500;
            text-align: center;
            border: none;
        }
    </style>
    <script>
        function Print() {
            window.print();
        }
        $(function() {
            $(".baseTable td").addClass("tableBg2");
            $(".baseTable tr").find("td:even").css({ "text-align": "right","width":"150" });
            $(".baseTable tr").find("td:odd").css({ "text-align": "left" ,"min-width":"150"});
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div runat="server" id="divprint" style="float: right; width: auto;">
            <a href="#" onclick="Print();">[打印]</a></div>
              <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="title">
                   <strong><span style="font-size: 12pt">还款明细
                        <asp:Label ID="lbjkcode" runat="server"></asp:Label></span></strong>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" width="100%" class="baseTable">
           
            <tr>
                <td>
                    借款人：
                </td>
                <td>
                    <asp:Label ID="txtloanName" runat="server"></asp:Label>
                </td>
                <td>
                    借款部门：
                </td>
                <td>
                    <asp:Label ID="txtdeptname" runat="server"></asp:Label>
                </td>
                
            </tr>
            <tr>
               <td>
                    还款单号：
                </td>
                <td>
                    <asp:Label ID="txtjkdh" runat="server"></asp:Label>
                </td>
                <td>
                    借款天数：
                </td>
                <td>
                    <asp:Label ID="txtjkts" runat="server"></asp:Label>
                    (天)
                </td>
            </tr>
            <tr>             
                <td>
                    借款日期：
                </td>
                <td>
                    <asp:Label ID="txtjksj" runat="server"></asp:Label>
                </td>
                <td>
                    填报时间：
                </td>
                <td>
                    <asp:Label ID="txtaddtime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款类别：
                </td>
                <td>
                    <asp:Label ID="txtlb" runat="server"></asp:Label>
                </td>
                <td>
                    借款金额：
                </td>
                <td>
                    <asp:Label ID="txtmoney" runat="server"></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td>
                    已冲减金额：
                </td>
                <td>
                    <asp:Label ID="lbycj" runat="server"></asp:Label>
                </td>
                 <td>
                    还款金额：
                </td>
                <td>
                    <asp:Label ID="txt_hkje" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
               
                <td>
                    还款原因：
                </td>
                <td colspan="3">
                    <asp:Label ID="txtReason" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="trPz" runat="server" visible="false">
                <td>
                    凭证：
                </td>
                <td colspan="5">
                    <input type="text" runat="server" id="txt_pz" style="width: 70%" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
