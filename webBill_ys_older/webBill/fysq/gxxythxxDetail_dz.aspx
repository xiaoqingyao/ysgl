<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gxxythxxDetail_dz.aspx.cs" Inherits="webBill_fysq_gxxythxxDetail_dz" %>

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
            //var data = '<div>sdfdfwere<sdf><dfd>sfdsf</div>';
            //alert($('<p>' + data + '</p>').text());
            $("#txtdate").datepicker();
            $("#txtsqr").autocomplete({
                source: availableTags,
                select: function (event, ui) {
                    var rybh = ui.item.value;

                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": escape(rybh) }, function (data, status) {
                        if (status == "success") {
                            $("#txtdept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });

                }
            });
        });
    </script>
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table class="baseTable">
                <tr>
                    <th colspan="3" style="height: 35px">
                        <asp:Label ID="lbl_title" runat="server" Text=""></asp:Label>关系学员特惠信息表</th>
                </tr>
                  <tr>
                    <td style="text-align: right">申请人：
                        <asp:TextBox ID="txtsqr" runat="server" Width="100"></asp:TextBox>
                    </td>
                    <td style="text-align: right">申请部门：
                        <asp:TextBox ID="txtdept" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td style="text-align: right">申请时间：
                        <asp:TextBox ID="txtdate" runat="server" Width="150"></asp:TextBox></td>
                </tr>

                <tr>
                    <td style="text-align: center">分校
                    </td>
                    <td style="text-align: center">学员姓名
                    </td>
                    <td style="text-align: center">年级
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_fx" Width="100"></asp:TextBox>
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_xyxm" Width="100"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_nj" Width="100"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">报名课时/小时
                    </td>
                    <td style="text-align: center">应收费
                    </td>
                    <td style="text-align: center">现行优惠
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_bmkc" Width="100"></asp:TextBox>
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_ysf" Width="100"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox runat="server" ID="txt_xxyh" Width="100"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="text-align: left;">在现行优惠的基础上另申请：优惠 
                        <asp:TextBox ID="txt_youhui" runat="server"></asp:TextBox>元；
                        <asp:Label runat="server" Text="*" ForeColor="Red" />赠送
                        <asp:TextBox ID="txt_zengsong1" runat="server"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                        小时/科
                        <br />
                        或  
                        <asp:TextBox ID="txt_zengsong2" runat="server"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />优惠
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">备注：
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_beizhu" runat="server" Width="90%"></asp:TextBox>
                        <asp:Label runat="server" Text="*" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">附件：</td>
                    <td colspan="2">
                        <asp:Label ID="lblfj" runat="server" Text="上传附件："></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:FileUpload ID="upLoadFiles" runat="server" Width="100px" />
                        <asp:HiddenField ID="hidfilnename" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddFileDz" runat="server" />
                        <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />
                        <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <div id="divBxdj" runat="server">
                        </div>
                        <asp:Literal ID="Lafilename" runat="server" Text=""></asp:Literal>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </td>
                </tr>
                   <tr id="tr_shyj" runat="server">
                    <td style="text-align: right">审核意见：
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_shxx_history" runat="server">
                    <td style="text-align: right">审核详细：
                    </td>
                    <td colspan="2">
                        <span id="txt_shxx_history" runat="server"></span>
                    </td>
                </tr>

                <tr id="tr_shyj_history" runat="server">
                    <td style="text-align: right">历史驳回意见：
                    </td>
                    <td colspan="2">
                        <span id="txt_shyj_History" runat="server"></span>
                    </td>
                </tr>   
                <tr>
                    <td colspan="3" style="text-align: center">
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
