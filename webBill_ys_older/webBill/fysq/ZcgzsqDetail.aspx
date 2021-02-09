<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZcgzsqDetail.aspx.cs" Inherits="webBill_fysq_ZcgzsqDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title></title>
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="js/Jscript.js"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function clo() {
            window.close();
            // self.close();
        }
        $(function () {

            $("#txt_sqsj").datepicker();

            //审核单据
            $("#btn_ok").click(function () {
                if (confirm("是否确定审核？")) {
                    var billcode = '<%=Request["billcode"] %>';
                    var mind = $("#txt_shyj").val();
                    billcode = billcode + "*" + mind + ",";
                    billcode = escape(billcode);
                    if (billcode == undefined || billcode == "") {
                        alert("请先选择单据!");
                    }
                    else {
                        //if (confirm("确定要审批该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                        //}
                    }
                }

            });

            $("#btn_cancel").click(function () {
                var billcode = '<%=Request["Code"] %>';
                var mind = $("#txt_shyj").val();
                if (billcode == "") {
                    alert("请选择驳回的记录。");
                    return;
                }
                window.showModalDialog("../MyWorkFlow/DisAgreeToSpecial.aspx?billCode=" + billcode + "&mind=" + mind, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes')
                window.close();
            });
        });
    </script>
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table class="baseTable">
                <tr style="background-color: #F0E2F3">
                    <td style="text-align: left; height: 30px;" colspan="4">
                        <strong>
                            <asp:Label ID="lbl_bd" runat="server" Text="新建总部固定资产申购单">
                            </asp:Label>
                        </strong></td>
                </tr>
                <tr>
                    <td style="text-align: right; background-color: #F3F3F3">
                        <asp:Label runat="server" Text="*" ForeColor="Red" />事务编号
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txt_swbh" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td style="text-align: right; background-color: #F3F3F3">重要等级
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="drp_zydj" runat="server" Width="150">
                            <asp:ListItem Value="0">一般</asp:ListItem>
                            <asp:ListItem Value="1">加急</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; background-color: #F3F3F3">
                        <asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                        申请事由
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txt_sqsy" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td style="text-align: right; background-color: #F3F3F3">特殊备注
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txt_tsbz" runat="server" Width="150"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr style="background-color: #F0E2F3">
                    <td style="text-align: left; height: 30px;" colspan="4"><strong>表单信息</strong></td>
                </tr>
                <tr>
                    <th colspan="4" style="height: 35px">
                        <asp:Label ID="lbl_title" runat="server" Text="固定资产申购单"></asp:Label></th>
                </tr>
                <tr>
                    <td style="text-align: right">编号
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox runat="server" ID="txt_bh" Width="150"></asp:TextBox>
                    </td>
                    <td style="text-align: right">申请时间
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txt_sqsj" runat="server" Width="150"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table border="0">
                            <tr>
                                <td style="text-align: center">物品名称
                                </td>
                                <td style="text-align: center">规格数量
                                </td>
                                <td style="text-align: center">用途
                                </td>
                                <td style="text-align: center">使用部门
                                </td>
                                <td style="text-align: center">需用日期
                                </td>
                                <td style="text-align: center">估计价值
                                </td>
                                <td style="text-align: center">购置备注
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_wpmc" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_ggsl" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_yt" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_sybm" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_xyrq" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_gjjz" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox runat="server" ID="txt_gzbz" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: left;">共申请办公用品 
                                    <asp:TextBox ID="txt_sqjs" runat="server" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                    件
                                </td>
                                <td colspan="4" style="text-align: left;">总金额
                                    <asp:TextBox ID="txt_zje" runat="server" BorderColor=" #FBF7CB"></asp:TextBox>
                                    <asp:Label runat="server" Text="*" ForeColor="Red" />
                                    元
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="7">
                                    <table width="100%" style="border-color: #000000" border="0">
                                        <tr>
                                            <td>申购部门负责人 
                                    <div>
                                        <asp:TextBox ID="txt_sgbmfzr" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                            <td>内勤部意见 
                                     <div>
                                         <asp:TextBox ID="txt_nqbyj" runat="server" Width="150" Height="50"></asp:TextBox>
                                     </div>
                                            </td>
                                            <td>财务部意见 
                                     <div>
                                         <asp:TextBox ID="txt_cwbyj" runat="server" Width="150" Height="50"></asp:TextBox>
                                     </div>
                                            </td>
                                            <td>人资行政部意见 
                                     <div>
                                         <asp:TextBox ID="txt_rzxzbyj" runat="server" Width="150" Height="50"></asp:TextBox>
                                     </div>
                                            </td>
                                            <td>行政部意见 
                                     <div>
                                         <asp:TextBox ID="txt_xzbyj" runat="server" Width="150" Height="50"></asp:TextBox>
                                     </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>日期
                                    <div>
                                        <asp:TextBox ID="txt_sgbrq" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                            <td>日期
                                    <div>
                                        <asp:TextBox ID="txt_nqbrq" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                            <td>日期
                                    <div>
                                        <asp:TextBox ID="txt_cwbrq" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                            <td>日期
                                    <div>
                                        <asp:TextBox ID="txt_rzxrrq" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                            <td>日期
                                    <div>
                                        <asp:TextBox ID="txt_xzbrq" runat="server" Width="150" Height="50"></asp:TextBox>
                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tr_shyj" runat="server">
                    <td style="text-align: right">审核意见：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_shxx_history" runat="server">
                    <td style="text-align: right">审核详细：
                    </td>
                    <td colspan="3">
                        <span id="txt_shxx_history" runat="server"></span>
                    </td>
                </tr>

                <tr id="tr_shyj_history" runat="server">
                    <td style="text-align: right">历史驳回意见：
                    </td>
                    <td colspan="3">
                        <span id="txt_shyj_History" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: left">
                        <asp:Button ID="btn_save" CssClass="baseButton" Text="保存" runat="server" OnClick="btn_save_Click" />
                        <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />
                        <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />
                        <asp:Button ID="btn_close" CssClass="baseButton" runat="server" OnClientClick="clo()" Text="取消" />
                    </td>
                </tr>
            </table>
        </form>
    </center>

</body>
</html>
