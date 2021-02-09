<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zfzxsqd_dzDetails.aspx.cs" Inherits="webBill_bxgl_zfzxsqd_dzDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <%--<script src="ajaxfileupload.js"></script>--%>
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        .item-help {
            border: 2px solid blue;
            height: auto;
            width: 300px;
            position: absolute;
            overflow-y: auto;
            font-size: 14px;
            padding: 15px;
            background-color: White;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        $(function () {
            SyjeChange();
            XbjjeChange();
            $("#txt_yksj").datepicker();
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
                var billcode = '<%=Request["billcode"] %>';
                var mind = $("#txt_shyj").val();
                if (billcode == "") {
                    alert("请选择驳回的记录。");
                    return;
                }
                window.showModalDialog("../MyWorkFlow/DisAgreeToSpecial.aspx?billCode=" + billcode + "&mind=" + mind, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes')
                window.close();
                // $("#btnRefresh").click();
            });
        });

        function SyjeChange() {
            $("#txt_syjedx").val(cmycurd($("#txt_syje").val()));

        }

        function XbjjeChange() {
            $("#txt_xbjjedx").val(cmycurd($("#txt_xbjje").val()));
        }
        function CheckNAN(obj) {

            if (parseFloat($(obj).val()).toString() == "NaN")
                $(obj).val("0");
        }
        function delfj(obj) {

            $(obj).parent().remove();
            $("#hidfilnename").val("");
            $("#hiddFileDz").val("");
            fj();//重新为隐藏域赋值
        }

        function fj() {
            var fjname = "";
            $(".fujianname").each(function (i, d) {
                fjname += $(this).val() + ";";
            });
            var fjurl = "";
            $(".fujianurl").each(function (i, d) {
                fjurl += $(this).val() + ";";
            });
            $("#hidfilnename").val(fjname);
            $("#hiddFileDz").val(fjurl);

            //alert($("#hidfilnename").val());
            //alert($("#hiddFileDz").val());
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <center>
             <div>
            <div style="text-align: center; font-weight: 800; margin-top: 10px; margin-bottom: 5px; font-size: large"><asp:Label ID="lbl_title" runat="server" Text="大智教育跨区域转费转校申请单"></asp:Label></div>
            <table class="myTable">

                <tr>
                    <td style="text-align: right">申请人：</td>
                    <td>
                        <asp:TextBox ID="txtsqr" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">申请部门：</td>
                    <td>
                        <asp:TextBox ID="txtdept" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">申请时间：</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtdate" runat="server" Width="95%"></asp:TextBox></td>
                </tr>

                <tr>
                    <td style="text-align: right"><asp:Label ID="lbl_zcfx" runat="server" Text="转出区域分校："></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txt_zcqyfx" runat="server" Width="95%"></asp:TextBox>
                    </td >
                    <td style="text-align: right"><asp:Label ID="lbl_zrfx" runat="server" Text="转入区域分校："></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_zrfx" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">学员姓名：</td>
                    <td>
                        <asp:TextBox ID="txt_xyxm" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">年级：</td>
                    <td>
                        <asp:TextBox ID="txt_nianji" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">原教育教学服务合同费用：</td>
                    <td>
                        <asp:TextBox ID="txt_yxyfdfy" Text="0" runat="server" Width="95%"></asp:TextBox></td>
                    <td style="text-align: right">原报名课程：</td>
                    <td>
                        <asp:TextBox ID="txt_ybmkc" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">原课程享受优惠：</td>

                    <td colspan="3">
                        <asp:TextBox ID="txt_ykcxsyh" Text="0" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">已消费课时/次：</td>
                    <td>
                        <asp:TextBox ID="txt_yxfks" Text="0" runat="server" onkeyup="CheckNAN(this)" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">对应课时单价：</td>
                    <td>
                        <asp:TextBox ID="txt_dyksdj" Text="0" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">已消费费用：</td>

                    <td colspan="3">
                        <asp:TextBox ID="txt_yxffy" Text="0" runat="server" onkeyup="CheckNAN(this)" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">应扣其他费用：</td>
                    <td>
                        <asp:TextBox ID="txt_ykqtfy" Text="0" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">剩余金额：</td>
                    <td>
                        <asp:TextBox ID="txt_syje"  Text="0" onblur="SyjeChange();" onkeyup="CheckNAN(this)" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">大写：</td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_syjedx" Enabled="false" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">转费原因：</td>
                    <td colspan="7">
                        <asp:TextBox ID="txtSm" runat="server" TextMode="MultiLine" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">新报小时/课程：</td>
                    <td>
                        <asp:TextBox ID="txt_xbxs" onkeyup="CheckNAN(this)" runat="server" Width="95%"></asp:TextBox>
                    </td>
                    <td style="text-align: right">须补交金额：</td>
                    <td>
                        <asp:TextBox ID="txt_xbjje" Text="0" onblur="XbjjeChange();" onkeyup="CheckNAN(this)" runat="server" Width="95%"></asp:TextBox>

                    </td>
                    <td style="text-align: right">大写：</td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_xbjjedx" Enabled="false" runat="server" Width="95%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right">附件：</td>
                    <td colspan="7">
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
                    <td colspan="7">
                        <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_shxx_history" runat="server">
                    <td style="text-align: right">审核详细：
                    </td>
                    <td colspan="7">
                        <span id="txt_shxx_history" runat="server"></span>
                    </td>
                </tr>

                <tr id="tr_shyj_history" runat="server">
                    <td style="text-align: right">历史驳回意见：
                    </td>
                    <td colspan="7">
                        <span id="txt_shyj_History" runat="server"></span>
                    </td>
                </tr>           
                <tr>                    
                    <td colspan="8" style="text-align: left">
                            <div>备注：</div>
                        <div>1.需上传教育教学服务合同、小票/收据/发票及学生卡；</div>
                        <div>2.转班费用不退，过期作废；</div>
                        <div>3.持此凭证到接收区域/分校办理报名手续，丢失不补办。</div>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align:center">
                        <asp:Button ID="btn_save" runat="server" Text="保  存" CssClass="baseButton" OnClick="btn_save_Click" />
                        &nbsp; &nbsp; &nbsp;
                              <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />
                        <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />
                        <input type="button" class="baseButton" value="关  闭" onclick="self.close();" />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    
        </center>

    </form>
</body>
</html>
