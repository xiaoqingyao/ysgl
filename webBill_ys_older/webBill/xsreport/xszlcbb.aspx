<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xszlcbb.aspx.cs" Inherits="webBill_xsreport_xszlcbb" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes'); 
        }
        function openLookSpStep(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="查 询" CssClass="baseButton"  /></td>
            </tr>
            <tr>
                <td>
                   
                        <tr>
                            <td width="33%"></td>
                            <td  width="33%">质 量 成 本 表</td>
                            <td  width="33%"></td>
                        </tr>
                        <tr>
                            <td  width="33%" height="20px"></td>
                            <td  width="33%" height="20px"></td>
                            <td  width="33%" height="20px">重汽财25表</td>
                        </tr>
                        <tr>
                            <td  width="33%" height="20px">编制单位：中国重汽集团济南橡塑件有限公司</td>
                            <td  width="33%" height="20px">2012年03月31日</td>
                            <td width="33%" height="20px">金额单位：人民币元</td>
                        </tr>
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="90%">
                        <tr>
                            <td class="tableBg">科目</td>
                            <td class="tableBg">成本项目</td>
                            <td class="tableBg">本年计划累计</td>
                            <td class="tableBg">上年同期累计</td>
                            <td class="tableBg">本月实际</td>
                            <td class="tableBg">本年实际累计</td>
                            <td class="tableBg">科目</td>
                            <td class="tableBg">成本项目</td>
                            <td class="tableBg">本年计划累计</td>
                            <td class="tableBg">上年同期累计</td>
                            <td class="tableBg">本月实际</td>
                            <td class="tableBg">本年实际累计</td>
                        </tr>
                        <tr>
                            <td rowspan="9" class="tableBg">内部故障成本</td>
                            <td class="tableBg">内部废品损失</td>
                            <td >&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td rowspan="9" class="tableBg">预防成本</td>
                            <td class="tableBg">质量工作费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">外部废品损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">质量培训费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:Label ID="Lbyfpxdy" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td><asp:Label ID="Lbyfpxdn" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tableBg">内部返修损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">质量奖励费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">外部返修损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">产品评审费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">停工损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">质量改进措施费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">事故分析处理费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">工资及附加费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:Label ID="Lbyfgzdy" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td><asp:Label ID="Lbyfgzdn" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tableBg">产品降级损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">小计</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">小计</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                        </tr>
                        <tr>
                            <td rowspan="9" class="tableBg">外部故障成本</td>
                            <td class="tableBg">索赔费用</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:Label ID="Lbspdy" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td><asp:Label ID="Lbspdn" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td rowspan="9" class="tableBg">鉴定成本</td>
                            <td class="tableBg">检验试验费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:Label ID="Lbjdjydy" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td><asp:Label ID="Lbjdjydn" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tableBg">退货损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">工资及附加费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><asp:Label ID="Lbjdgzdy" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                            <td><asp:Label ID="Lbjdgzdn" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=100%></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="tableBg">保修费用</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">办公费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">诉讼费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">检测设备大修费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">产品降价损失</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">检测手段购置费</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">小计</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg">小计</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">合计</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                            <td class="tableBg">&nbsp;</td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
