<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lrb_result.aspx.cs" Inherits="webBill_tjbb_dz_lrb_result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <style>
        .baseTable tr td span {
            float: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="baseTable">
                <tr>
                    <td colspan="7">利润表</td>
                </tr>
                <tr>
                    <td colspan="7" style="text-align: right">报表时间区间：<asp:Label ID="lblDt" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>账套：</td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlZt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZt_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">单位：元</td>
                </tr>

                <tr>
                    <td>项 目</td>
                    <td>行数</td>
                    <td>本月数-预算</td>
                    <td>本月数-决算</td>
                    <td>本年累计数-预算</td>
                    <td>本年累计数-决算</td>

                </tr>
                <tr>
                    <td>一、主营业务收入
                    </td>
                    <td>1</td>
                    <td>
                        <asp:Label ID="bqys_1" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_1" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_1" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_1" runat="server" /></td>

                </tr>
                <tr>
                    <td>减：主营业务成本</td>
                    <td>2</td>
                    <td>
                        <asp:Label ID="bqys_2" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_2" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_2" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_2" runat="server" /></td>

                </tr>
                <tr>
                    <td>主营业务税金及附加</td>
                    <td>3</td>
                    <td>
                        <asp:Label ID="bqys_3" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_3" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_3" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_3" runat="server" /></td>
                </tr>
                <tr>
                    <td>二、主营业务利润</td>
                    <td>4</td>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>加：其他业务利润</td>
                    <td>5</td>
                    <td>
                        <asp:Label ID="bqys_5" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_5" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_5" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_5" runat="server" /></td>

                </tr>
                <tr>
                    <td>减：营业费用</td>
                    <td>6</td>
                    <td>
                        <asp:Label ID="bqys_6" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_6" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_6" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_6" runat="server" /></td>

                </tr>
                <tr>
                    <td>管理费用</td>
                    <td>7</td>
                    <td>
                        <asp:Label ID="bqys_7" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_7" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_7" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_7" runat="server" /></td>

                </tr>
                <tr>
                    <td>财务费用</td>
                    <td>8</td>
                    <td>
                        <asp:Label ID="bqys_8" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_8" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_8" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_8" runat="server" /></td>

                </tr>
                <tr>
                    <td>三、营业利润</td>
                    <td>9</td>
                    <td colspan="4"></td>

                </tr>
                <tr>
                    <td>加：投资收益</td>
                    <td>10</td>
                    <td>
                        <asp:Label ID="bqys_10" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_10" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_10" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_10" runat="server" /></td>

                </tr>
                <tr>
                    <td>补贴收入</td>
                    <td>11</td>
                    <td>
                        <asp:Label ID="bqys_11" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_11" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_11" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_11" runat="server" /></td>

                </tr>
                <tr>
                    <td>营业外收入</td>
                    <td>12</td>
                    <td>
                        <asp:Label ID="bqys_12" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_12" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_12" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_12" runat="server" /></td>

                </tr>
                <tr>
                    <td>减：营业外支出</td>
                    <td>13</td>
                    <td>
                        <asp:Label ID="bqys_13" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_13" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_13" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_13" runat="server" /></td>

                </tr>
                <tr>
                    <td>四、利润总额</td>
                    <td>14</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>减：所得税</td>
                    <td>15</td>
                    <td>
                        <asp:Label ID="bqys_15" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_15" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_15" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_15" runat="server" /></td>

                </tr>
                <tr>
                    <td>五、净利润</td>
                    <td>16</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
