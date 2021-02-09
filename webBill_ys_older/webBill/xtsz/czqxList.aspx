<%@ Page Language="C#" AutoEventWireup="true" CodeFile="czqxList.aspx.cs" Inherits="xtsz_czqxList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 33%">
            <tr>
                <td style="height: 21px">
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                    <asp:Label ID="lblUserInfo" runat="server" Text="显示当前待设置权限人员信息" ForeColor="Red"></asp:Label>
                    &nbsp;<asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <table class="baseTable">
                        <tr>
                            <th style="width: 97px">一级菜单</th>
                            <th style="width: 167px">二级菜单</th>
                            <th style="width: 7px"></th>
                            <th style="width: 97px">一级菜单</th>
                            <th style="width: 167px">二级菜单</th>
                        </tr>
                        <tr>
                            <td rowspan="5" style="width: 97px">基础设置</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0101" runat="server" Text="部门设置" /></td>
                            <td rowspan="27" style="width: 7px"></td>
                            <td rowspan="10" style="width: 97px">预算管理</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0301" runat="server" Text="预算过程" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0102" runat="server" Text="角色设置" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0302" runat="server" Text="财务填报" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0103" runat="server" Text="人员设置" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0303" runat="server" Text="预算填报" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0104" runat="server" Text="数据字典" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0310" runat="server" Text="项目预算填报" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0105" runat="server" Text="项目设置" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0304" runat="server" Text="预算追加" /></td>
                        </tr>
                        <tr>
                            <td rowspan="5" style="width: 97px">科目设置</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0201" runat="server" Text="财务科目" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0305" runat="server" Text="预算调整" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0202" runat="server" Text="预算科目" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0306" runat="server" Text="预算审核" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0203" runat="server" Text="部门预算科目" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0311" runat="server" Text="项目预算审核" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0204" runat="server" Text="预算科目对照" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0307" runat="server" Text="追加审核" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">&nbsp;</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0308" runat="server" Text="预算调整审核" /></td>
                        </tr>
                        <tr>
                            <td rowspan="4" style="width: 97px">费用申请</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0803" runat="server" Text="报告申请单" /></td>
                            <td rowspan="4" style="width: 97px">报销管理</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0402" runat="server" Text="一般报销" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0804" runat="server" Text="采购审批单" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0403" runat="server" Text="一般报销审核" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0805" runat="server" Text="报告申请单审核" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0412" runat="server" Text="其他报销" /></td>
                        </tr>
                        <tr>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0806" runat="server" Text="采购审批单审核" /></td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0413" runat="server" Text="其他报销审核" /></td>
                        </tr>
                        <tr>
                            <td rowspan="6" style="width: 97px">财务管理</td>
                            <td style="width: 167px">
                                <asp:CheckBox ID="chk0521" runat="server" Text="报销财务免审" /></td>
                        </tr>
                    </table>
                </td>
                <td rowspan="13" style="width: 97px">查询统计</td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0901" runat="server" Text="预算查询" /></td>

            </tr>
            <tr>

                <td style="width: 167px; height: 20px;">
                    <asp:CheckBox ID="chk0507" runat="server" Text="一般报销给付" /></td>
                <td style="width: 167px; height: 20px;">
                    <asp:CheckBox ID="chk0902" runat="server" Text="预算追加查询" /></td>
            </tr>
            <tr>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0508" runat="server" Text="一般报销给付撤销" /></td>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0903" runat="server" Text="报告申请查询" /></td>
            </tr>
            <tr>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0517" runat="server" Text="其他报销给付" /></td>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0904" runat="server" Text="采购审批单查询" /></td>
            </tr>
            <tr>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0518" runat="server" Text="其他报销给付撤销" /></td>
                <td style="width: 167px; height: 20px">
                    <asp:CheckBox ID="chk0905" runat="server" Text="一般报销查询" /></td>
            </tr>
            <tr>
                <td style="width: 97px" rowspan="1">
                    <asp:CheckBox ID="chk0519" runat="server" Text="预算调整" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0701" runat="server" Text="流程设置" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0908" runat="server" Text="其他报销查询" /></td>
            </tr>
            <tr>
                <td rowspan="7" style="width: 97px">系统设置</td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0602" runat="server" Text="管理权限" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0906" runat="server" Text="预算调整查询" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0603" runat="server" Text="人员操作权限" /></td>
                <td>
                    <asp:CheckBox ID="chk0907" runat="server" Text="费用情况查询" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0604" runat="server" Text="角色操作权限" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0920" runat="server" Text="部门预算统计" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0605" runat="server" Text="采购查询权限" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0921" runat="server" Text="费用科目预算统计" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0611" runat="server" Text="参数设置" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0931" runat="server" Text="费用使用部门统计" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0610" runat="server" Text="友情提示发布" /></td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0932" runat="server" Text="项目预算与报销统计" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0609" runat="server" Text="系统初始化" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 97px">采购资金计划</td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk1101" runat="server" Text="采购资金计划单" /></td>
                <td>&nbsp;</td>
                <td rowspan="2" style="width: 97px">采购资金付款
                </td>
                <td>
                    <asp:CheckBox ID="chk1103" runat="server" Text="采购资金付款单" /></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk1102" runat="server" Text="采购资金计划单审核" /></td>
                <td>&nbsp;</td>
                <td>
                    <asp:CheckBox ID="chk1104" runat="server" Text="采购资金付款审批" />
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 97px">新增查询</td>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0520" runat="server" Text="各部门预算统计" /></td>
                <td>&nbsp;</td>
                <td rowspan="3" style="width: 97px"></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0933" runat="server" Text="部门各科目汇总" /></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 167px">
                    <asp:CheckBox ID="chk0934" runat="server" Text="项目各科目汇总" /></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

        </table>

    </form>
</body>
</html>
