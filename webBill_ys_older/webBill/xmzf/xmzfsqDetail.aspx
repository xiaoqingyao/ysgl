<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmzfsqDetail.aspx.cs" Inherits="webBill_xmzf_xmzfsqDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目支付申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    
    <script language="javascript" type="text/javascript" src="../fysq/js/Jscript.js"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript" charset="UTF-8"></script>
    <script language="javascript" type="Text/javascript" >
        $(function() {
        $("#txtCgrq").datepicker();
        });
        function bcclick()
        {
            var dropDownList = document.getElementById("<%=ddl_cglb.ClientID %>");
            var index=dropDownList.selectedIndex;
            if (index<0){
                alert("请到项目管理中增加项目信息！"); 
                return false;
            }
            var dropDownListValue = dropDownList.options[index].value;
            if (dropDownListValue == 0) { 
                alert("请选择支付项目！"); 
                dropDownList.focus(); 
                return false;
            }
            var sqnr=document.getElementById("<%=txtZynr.ClientID %>");
            if (sqnr.value=="")            {
                alert("请输入申请内容！"); 
                sqnr.focus(); 
                return false;
            }
            var sqsm=document.getElementById("<%=txtSm.ClientID %>");
            if (sqsm.value=="")            {
                alert("请输入申请说明！"); 
                sqsm.focus(); 
                return false;
            }
            var Yjfy=document.getElementById("<%=txtYjfy.ClientID %>");
            if (Yjfy.value=="" || Yjfy.value==0)            {
                alert("请输入申请金额！"); 
                Yjfy.focus(); 
                return false;
            }
            return true;
        }
        </script>
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table cellpadding="0" cellspacing="0" class="style1" width="100%">
                <tr>
                    <td style="text-align: center; height: 26px;">
                        <strong><span style="font-size: 12pt">项目支付申请单</span></strong></td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                            <tr>
                                <td class="tableBg" colspan="2">
                                    申请单号</td>
                                <td colspan="2">
                                    <asp:Label ID="lblCgbh" runat="server" Text="No.201109120001"></asp:Label>
                                    <asp:Button ID="Button1" runat="server" Text="生成编号" CssClass="baseButton"
                                        Visible="False" /></td>
                                <td class="tableBg">
                                    申请日期</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCgrq" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg" colspan="2">
                                    <asp:Label ID="Label1" runat="server" Text="申请部门" Width="79px"></asp:Label></td>
                                <td colspan="2">
                                    <asp:Label ID="lblDept" runat="server" Text="[000001]采购蛇皮但单位" Width="100%" style="display:none"></asp:Label>
                                    <asp:Label ID="lblxsDept" runat="server" Text="[000001]采购蛇皮但单位" Width="100%"></asp:Label>
                                    
                                    </td>
                                <td class="tableBg">
                                    支付项目</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_cglb" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Selected="True" Value="0">-请选择-</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg" colspan="2">
                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label2" runat="server" Text="申请内容"></asp:Label></td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtZynr" runat="server" Rows="2" TextMode="MultiLine" Width="681px"
                                        Height="82px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg" colspan="2">
                                    申请说明</td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtSm" runat="server" Rows="2" TextMode="MultiLine" Width="681px"
                                        Height="60px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg" colspan="2">
                                    承办人</td>
                                <td colspan="2">
                                    <asp:Label ID="lblCbr" runat="server" Text="[000001]系统挂丽媛"></asp:Label></td>
                                <td class="tableBg">
                                    支付费用</td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtYjfy" runat="server" onblur="clearNoNum(this,'Y');"></asp:TextBox>元</td>
                            </tr>
                            
                            
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" onclick="btn_bc_Click" OnClientClick=" return bcclick()" />
                        &nbsp;
                        <asp:Button ID="btn_fh" runat="server" Text="返 回" CssClass="baseButton" 
                            onclick="btn_fh_Click" /></td>
                </tr>
                <tr style="display: none">
                    <td colspan="6" style="height: 37px; text-align: center">
                        <asp:Label ID="lbl_BillCode" runat="server"></asp:Label>
                        <asp:Button ID="btn_print" runat="server" Text="打印" CssClass="baseButton"  /></td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
