<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyConfig.aspx.cs" Inherits="webBill_xtsz_MyConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
     $(function(){
      $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                  parent.helptoggle();
                }
            });
     });
         //刷新页面     
         function Refresh() {
             location.replace(location.href);
         }

     function CloseWithParam(file, fileName, type) {  
      if (type == "logo") {
                document.getElementById("<%=lb0.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f0").val(file);
                $("fj0").val(fileName);

            }       
            if (type == "f1") {
                document.getElementById("<%=lb1.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f1").val(file);
                $("fj1").val(fileName);
            }
            
             if (type == "f2") {
                document.getElementById("<%=lb2.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f2").val(file);
                $("fj2").val(fileName);

            }
             if (type == "f3") {
                document.getElementById("<%=lb3.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f3").val(file);
                $("fj3").val(fileName);

            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin-left: 5px; margin-top: 5px;">
            <input id="btn_reFresh" type="button" value="刷  新" class="baseButton" onclick="Refresh();" />
            <asp:Button ID="btn_save" runat="server" Text="保  存" CssClass="baseButton" OnClick="btn_save_Click" />
            <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
        </div>
        <div style="background-color: #E0EEE0; margin-left: 5px; margin-top: 5px;">
            <h4>
                启用设置</h4>
            <table>
                <tr>
                    <td>
                        是否启用预算管理模块
                    </td>
                    <td>
                        <asp:RadioButton ID="rdb_BudgetControl_yes" runat="server" Checked="true" Text="需要"
                            GroupName="IsNeedBudgetControl" /><asp:RadioButton ID="rdb_BudgetControl_no" runat="server"
                                Text="不需要" GroupName="IsNeedBudgetControl" />
                    </td>
                </tr>
                <tr>
                    <td>
                        是否启用归口继续分解
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbIsGoOnGkYes" runat="server" Checked="true" Text="需要" GroupName="IsGoOnGk" /><asp:RadioButton
                            ID="rdbIsGoOnGkNo" runat="server" Text="不需要" GroupName="IsGoOnGk" />
                    </td>
                </tr>
                <tr>
                    <td>
                        一般报销单税额是否进总金额
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbIsInAllYes" runat="server" Checked="true" Text="是" GroupName="IsInAll" /><asp:RadioButton
                            ID="rdbIsInAllNo" runat="server" Text="否" GroupName="IsInAll" />
                    </td>
                </tr>
                <tr>
                    <td>
                        凭证是记到使用部门上 还是归口部门上
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbBM" runat="server" Checked="true" Text="使用部门" GroupName="GkOrShiYong" /><asp:RadioButton
                            ID="rdbGK" runat="server" Text="归口部门" GroupName="GkOrShiYong" />
                    </td>
                </tr>
                <tr>
                    <td>
                        是否控制点数 1为控制 0或不设置都是不控制
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbIsCPYes" runat="server" Checked="true" Text="是" GroupName="IsControlPoint" /><asp:RadioButton
                            ID="rdbIsCPNo" runat="server" Text="否" GroupName="IsControlPoint" />
                    </td>
                </tr>
                <tr>
                    <td>
                        是否检测到狗
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbCheckDogYes" runat="server" Checked="true" Text="是" GroupName="IsCheckDog" /><asp:RadioButton
                            ID="rdbCheckDogNo" runat="server" Text="否" GroupName="IsCheckDog" />
                    </td>
                </tr>
                <tr>
                    <td>
                        预算调整时目标预算过程必须是已开启预算
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbAllowTzUodoYes" runat="server" Checked="true" Text="是" GroupName="AllowTzUodo" /><asp:RadioButton
                            ID="rdbAllowTzUodoNo" runat="server" Text="否" GroupName="AllowTzUodo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        核算部门是否必须是末级部门
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbHsDeptIsLastYes" runat="server" Text="是" GroupName="HsDeptIsLast" />
                        <asp:RadioButton ID="rdbHsDeptIsLastNo" runat="server" Text="否" GroupName="HsDeptIsLast"
                            Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                       报销单核算项目模式
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbHsxmLowModel" runat="server" Text="简单" GroupName="YbbxHsxmMode" />
                        <asp:RadioButton ID="rdbHsxmHighModel" runat="server" Text="高级" GroupName="YbbxHsxmMode"
                            Checked="true" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="background-color: #E0EEE0; margin-left: 5px; margin-top: 5px;">
            <h4>
                费用申请启用设置</h4>
            是否启用报告申请单模块&nbsp;
            <asp:RadioButton ID="rdb_BGSQ_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedBGSQ" />
            <asp:RadioButton ID="rdb_BGSQ_no" runat="server" Text="不需要" GroupName="IsNeedBGSQ" />
            &nbsp; &nbsp; &nbsp; &nbsp; 是否启用采购审批单模块&nbsp;
            <asp:RadioButton ID="rdb_CGSP_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedCGSP" />
            <asp:RadioButton ID="rdb_CGSP_no" runat="server" Text="不需要" GroupName="IsNeedCGSP" />
            &nbsp; &nbsp; &nbsp; &nbsp; 是否启用项目支付申请单模块&nbsp;
            <asp:RadioButton ID="rdb_XMZF_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedXMZF" />
            <asp:RadioButton ID="rdb_XMZF_no" runat="server" Text="不需要" GroupName="IsNeedXMZF" />
            <br />
            是否启用出差申请单模块&nbsp;
            <asp:RadioButton ID="rdb_CCSQ_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedCCSQ" />
            <asp:RadioButton ID="rdb_CCSQ_no" runat="server" Text="不需要" GroupName="IsNeedCCSQ" />
            &nbsp; &nbsp; &nbsp; &nbsp; 是否启用费用提成该模块&nbsp;
            <asp:RadioButton ID="rdbIsSaleRebateYes" runat="server" Checked="true" Text="需要"
                GroupName="IsSaleRebate" />
            <asp:RadioButton ID="rdbIsSaleRebateNo" runat="server" Text="不需要" GroupName="IsSaleRebate" />
            &nbsp; &nbsp; &nbsp; &nbsp; 是否开启滚动费用控制&nbsp;
            <asp:RadioButton ID="rdbRollNo" runat="server" Checked="true" Text="否" GroupName="IsRoll" />
            <asp:RadioButton ID="rdbRollYes" runat="server" Text="是" GroupName="IsRoll" />
            &nbsp; &nbsp; &nbsp; &nbsp;
        </div>
        <div style="background-color: #E0EEE0; margin-left: 5px; margin-top: 5px;">
            <h4>
                地址配置
            </h4>
            <table>
                <tr>
                    <td>
                        在桌面上回车 打开的报销单的地址
                    </td>
                    <td>
                        <asp:TextBox ID="txtEnterForBxURL" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        nc系统的平台路径
                    </td>
                    <td>
                        <asp:TextBox ID="txtToNcURL" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        凭证制作页面的地址
                    </td>
                    <td>
                        <asp:TextBox ID="txtpingzhengdetailurl" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        凭证数据库连接名称
                    </td>
                    <td>
                        <asp:TextBox ID="txtpingzhengdblinkname" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        程序错误提示信息
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ErrorMsg" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="background-color: #E0EEE0; margin-left: 5px; margin-top: 5px;">
            <h4>
                系统显示信息配置</h4>
            <table>
                <tr>
                    <td>
                        企业名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_CompanyName" runat="server" CssClass="baseText" Width="450px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        企业Logo：
                    </td>
                    <td runat="server" id="td3" colspan="5" style="height: 100%" valign="top">
                        <iframe id="Iframe4" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=logo&UseName=false"
                            width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                        </iframe>
                        <asp:Label ID="lb0" runat="server" Text="" Width="100%"></asp:Label>
                        <asp:Label runat="server" ID="lbfj0" />
                        <asp:HiddenField ID="f0" runat="server" />
                        <asp:HiddenField ID="fj0" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        登录页面图片一：
                    </td>
                    <td runat="server" id="tdiframe" colspan="5" style="height: 100%" valign="top">
                        <iframe id="Iframe2" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f1&UseName=false"
                            width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                        </iframe>
                        <asp:Label ID="lb1" runat="server" Text="" Width="100%"></asp:Label>
                        <asp:Label runat="server" ID="lbfj1" />
                        <asp:HiddenField ID="f1" runat="server" />
                        <asp:HiddenField ID="fj1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        登录页面图片二：
                    </td>
                    <td runat="server" id="td1" colspan="5" style="height: 100%" valign="top">
                        <iframe id="Iframe1" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f2&UseName=false"
                            width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                        </iframe>
                        <asp:Label ID="lb2" runat="server" Text="" Width="100%"></asp:Label>
                        <asp:Label runat="server" ID="lbfj2" />
                        <asp:HiddenField ID="f2" runat="server" />
                        <asp:HiddenField ID="fj2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        登录页面图片三：
                    </td>
                    <td runat="server" id="td2" colspan="5" style="height: 100%" valign="top">
                        <iframe id="Iframe3" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f3&UseName=false"
                            width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                        </iframe>
                        <asp:Label ID="lb3" runat="server" Text="" Width="100%"></asp:Label>
                        <asp:Label runat="server" ID="lbfj3" />
                        <asp:HiddenField ID="f3" runat="server" />
                        <asp:HiddenField ID="fj3" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
