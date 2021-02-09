<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zlcbb.aspx.cs" Inherits="webBill_xsreport_zlcbb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" charset="utf-8">
        $(function() {
            $("#TextBox1").datepicker();
            $("#TextBox2").datepicker();
        });
    </script>
    <style type="text/css">

.font6
	{color:windowtext;
	font-size:12.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:宋体;
	}
        .style1
        {
            text-align: center;
        }
        .style2
        {
            width: 75px;
        }
        .style3
        {
            width: 55px;
        }
        .style5
        {
            width: 42px;
        }
        .style6
        {
            width: 60px;
        }
        .style8
        {
            width: 72px;
        }
        .style9
        {
            width: 61px;
        }
    </style>
    </head>
<body >
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 28px">开始时间<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;
                                截止时间<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>&nbsp;<asp:Button 
                        ID="Button3" runat="server" Text="查 询" CssClass="baseButton" onclick="Button3_Click" 
                        />
            </tr>
            <tr>
                <td>
                    
                    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse:
 collapse;width:881pt;" class="myTable" width="1171" x:str="">
                        <colgroup>
                            <col style="mso-width-source:userset;mso-width-alt:2752;width:65pt" 
                                width="86" />
                            <col style="mso-width-source:userset;mso-width-alt:3360;width:79pt" 
                                width="105" />
                            <col style="mso-width-source:userset;mso-width-alt:2048;" />
                            <col style="mso-width-source:userset;mso-width-alt:3616;" />
                            <col style="mso-width-source:userset;mso-width-alt:3360;" />
                            <col style="mso-width-source:userset;mso-width-alt:3616;" />
                            <col style="mso-width-source:userset;mso-width-alt:2048;width:48pt" 
                                width="64" />
                            <col style="mso-width-source:userset;mso-width-alt:3360;" />
                            <col style="mso-width-source:userset;mso-width-alt:2048;" />
                            <col style="mso-width-source:userset;mso-width-alt:3616;" />
                            <col style="mso-width-source:userset;mso-width-alt:3360;" />
                            <col style="mso-width-source:userset;mso-width-alt:4288;width:101pt" 
                                width="134" />
                        </colgroup>
                        <tr height="30">
                            <td class="style1" colspan="12" height="30">
                                <a href="#RANGE!A1">
                                <span style="color:windowtext;font-size:
  18.0pt;font-weight:700;text-decoration:none;font-family:宋体;mso-generic-font-family:
  auto;mso-font-charset:134">质<span style="mso-spacerun:yes">&nbsp; </span>量<span style="mso-spacerun:yes">&nbsp; </span>成<span 
                                    style="mso-spacerun:yes">&nbsp; </span>本<span style="mso-spacerun:yes">&nbsp; </span>表</span></a></td>
                        </tr>
                        <tr height="19">
                            <td class="style6" height="19">
                                　</td>
                            <td class="xl68">
                                　</td>
                            <td class="xl68">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style5">
                                　</td>
                            <td class="xl68">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                重汽财25表</td>
                        </tr>
                        <tr height="16">
                            <td class="xl70" colspan="3" height="16">
                                编制单位：中国重汽集团济南橡塑件有限公司</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="xl90" colspan="2">
                                <asp:Label ID="LbRq" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="xl73">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                金额单位：</td>
                            <td class="style9">
                                人民币元</td>
                        </tr>
                        <tr height="32">
                            <td class="style6" height="32">
                                科目</td>
                            <td class="xl75">
                                成本项目</td>
                            <td class="xl76" width="64">
                                本年计划累计</td>
                            <td class="style2">
                                上年同期累计</td>
                            <td class="style3">
                                本月实际</td>
                            <td class="style8">
                                本年实际累计</td>
                            <td class="style5">
                                科目</td>
                            <td class="xl76" width="105">
                                成本项目</td>
                            <td class="style8">
                                本年计划累计</td>
                            <td class="style8">
                                上年同期累计</td>
                            <td class="style9">
                                本月实际</td>
                            <td class="style9">
                                本年实际累计</td>
                        </tr>
                        <tr height="24">
                            <td class="style6" dir="LTR" height="216" rowspan="9">
                                内部故障成本</td>
                            <td class="xl77">
                                内部废品损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style5" rowspan="9">
                                预防成本</td>
                            <td class="xl77">
                                质量工作费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                外部废品损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                质量培训费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                内部返修损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                质量奖励费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                外部返修损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                产品评审费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                停工损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                质量改进措施费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                事故分析处理费</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                工资及附加费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　<asp:Label ID="LbYfgzdy" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                产品降级损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                　</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl79" height="24">
                                小计</td>
                            <td class="xl80">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl79">
                                小计</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="style6" height="216" rowspan="9">
                                外部故障成本</td>
                            <td class="xl77">
                                索赔费用</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　<asp:Label ID="Lbwbspdy" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style8">
                                　</td>
                            <td class="style5" rowspan="9">
                                鉴定成本</td>
                            <td class="xl77">
                                检验试验费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　<asp:Label ID="Lbjdjydy" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                退货损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                工资及附加费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　<asp:Label ID="Lbjdgzdy" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                保修费用</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                办公费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                诉讼费</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                检测设备大修费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                产品降价损失</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                检测手段购置费</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                　</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                　</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl79">
                                小计</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl77" height="24">
                                　</td>
                            <td class="xl78">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl77">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                        <tr height="24">
                            <td class="xl79" height="24">
                                小计</td>
                            <td class="xl80">
                                　</td>
                            <td class="style2">
                                　</td>
                            <td class="style3">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="xl79">
                                合计</td>
                            <td class="style8">
                                　</td>
                            <td class="style8">
                                　</td>
                            <td class="style9">
                                　</td>
                            <td class="style9">
                                　</td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
