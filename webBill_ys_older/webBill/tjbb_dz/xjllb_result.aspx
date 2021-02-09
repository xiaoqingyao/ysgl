<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xjllb_result.aspx.cs" Inherits="webBill_tjbb_dz_xjllb_result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <style>
        .baseTable tr td span {
            float:right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <table class="baseTable">
                <tr>
                    <td colspan="7">现金流量表</td>
                </tr>
                  <tr><td colspan="7" style="text-align:right">报表时间区间：<asp:Label ID="lblDt" runat="server"></asp:Label></td></tr>
                <tr>
                    <td>账套：</td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlZt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZt_SelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td colspan="2">单位：元</td>
                </tr>
                <tr>
                    <td>项 目</td>
                    <td>行次</td>
                    <td>本期金额-预算</td>
                    <td>本期金额-决算</td>
                    <td>本年累计-预算</td>
                    <td>本年累计-决算</td>
                </tr>
                <tr>
                    <td colspan="7">一、经营活动产生的现金流量：</td>
                </tr>
                <tr>
                    <td>销售商品、提供劳务收到的现金：</td>
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
                    <td>收到的税费返还：</td>
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
                    <td>收到的其他与经营活动有关的现金：</td>
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
                    <td>现金流入小计</td>
                    <td>4</td>
                    <td>
                        <asp:Label ID="bqys_4" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_4" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_4" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_4" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>购买商品接受劳务支付的现金
                    </td>
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
                    <td>支付给职工以及为职工支付的现金
                    </td>
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
                    <td>支付的各项税费
                    </td>
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
                    <td>支付的其他与经营活动有关的现金
                    </td>
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
                    <td>现金流出小计
                    </td>
                    <td>9</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>经营活动产生的现金流量净额
                    </td>
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
                    <td colspan="6">二、投资活动产生的现金流量：
                    </td>
                </tr>
                <tr>
                    <td>收回投资所收到的现金
                    </td>
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
                    <td>取得投资收益所收到的现金
                    </td>
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
                    <td>处置固定资产、无形资产和其他长期资产所收回的现金净额
                    </td>
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
                    <td>收到的其他与投资活动有关的现金
                    </td>
                    <td>14</td>
                    <td>
                        <asp:Label ID="bqys_14" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_14" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_14" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_14" runat="server" /></td>
                   
                </tr>
                <tr>
                    <td>现金流入小计
                    </td>
                    <td>15</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>购建固定资产、无形资产和其他长期资产所支付的现金
                    </td>
                    <td>16</td>
                    <td>
                        <asp:Label ID="bqys_16" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_16" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_16" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_16" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>投资所支付的现金
                    </td>
                    <td>17</td>
                    <td>
                        <asp:Label ID="bqys_17" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_17" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_17" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_17" runat="server" /></td>
                   
                </tr>
                <tr>
                    <td>支付的其他与投资活动有关的现金
                    </td>
                    <td>18</td>
                    <td>
                        <asp:Label ID="bqys_18" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_18" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_18" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_18" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>现金流出小计
                    </td>
                    <td>19</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    
                </tr>
                <tr>
                    <td>投资活动产生的现金流量净额
                    </td>
                    <td>20</td>
                    <td>
                        <asp:Label ID="bqys_20" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_20" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_20" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_20" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td colspan="7">三、筹资活动产生的现金流量：
                    </td>
                </tr>
                <tr>
                    <td>吸收投资所收到的现金
                    </td>
                    <td>21</td>
                    <td>
                        <asp:Label ID="bqys_21" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_21" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_21" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_21" runat="server" /></td>
                   
                </tr>
                <tr>
                    <td>取得借款所收到的现金
                    </td>
                    <td>22</td>
                    <td>
                        <asp:Label ID="bqys_22" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_22" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_22" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_22" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>收到的其他与筹资活动有关的现金
                    </td>
                    <td>23</td>
                    <td>
                        <asp:Label ID="bqys_23" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_23" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_23" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_23" runat="server" /></td>
                   

                </tr>
                <tr>
                    <td>现金流入小计
                    </td>
                    <td>24</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    
                </tr>
                <tr>
                    <td>偿还债务所支付的现金
                    </td>
                    <td>25</td>
                    <td>
                        <asp:Label ID="bqys_25" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_25" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_25" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_25" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>分配股利、利润和偿付利息所支付的现金
                    </td>
                    <td>26</td>
                    <td>
                        <asp:Label ID="bqys_26" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_26" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_26" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_26" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>支付的其他与筹资活动有关的现金
                    </td>
                    <td>27</td>
                    <td>
                        <asp:Label ID="bqys_27" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_27" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_27" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_27" runat="server" /></td>
                    
                </tr>
                <tr>
                    <td>现金流出小计
                    </td>
                    <td>28</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                  
                </tr>
                <tr>
                    <td>筹资活动产生的现金流量净额
                    </td>
                    <td>29</td>
                    <td>
                        <asp:Label ID="bqys_29" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_29" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_29" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_29" runat="server" /></td>
                   
                </tr>
                <tr>
                    <td>四、汇率变动对现金的影响
                    </td>
                    <td>30</td>
                    <td>
                        <asp:Label ID="bqys_30" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_30" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_30" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_30" runat="server" /></td>
                   
                </tr>
                <tr>
                    <td>五、现金及现金等价物净增加额
                    </td>
                    <td>31</td>
                    <td>
                        <asp:Label ID="bqys_31" runat="server" /></td>
                    <td>
                        <asp:Label ID="bqjs_31" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljys_31" runat="server" /></td>
                    <td>
                        <asp:Label ID="ljjs_31" runat="server" /></td>
                   
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
