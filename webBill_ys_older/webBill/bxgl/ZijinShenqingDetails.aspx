<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZijinShenqingDetails.aspx.cs" Inherits="webBill_fysq_ZijinShenqingDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <base target="_self" />
    <script src="ajaxfileupload.js"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script type="text/javascript">
        $(function () {
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
                    var billcode = '<%=Request["billCode"] %>';
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
                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();


                if (billcode == "") {
                    alert("请选择驳回的记录。");
                    return;
                }
                window.showModalDialog("../MyWorkFlow/DisAgreeToSpecial.aspx?billCode=" + billcode+"&mind="+mind, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes')
                window.close();
                // $("#btnRefresh").click();
            });

        });
        function delfj(obj) {

            $(obj).parent().remove();


        }
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }
        function shangchuan() {
            //var filepath = document.getElementById("upLoadFiles").value;
            //var position = filepath.lastIndexOf("\\");
            //var filename = filepath.substring(position + 1);

            $.ajaxFileUpload
            (
                {
                    url: 'uploadFile.ashx', //用于文件上传的服务器端请求地址
                    secureuri: false, //一般设置为false
                    fileElementId: 'upLoadFiles', //文件上传空间的id属性  <input type="file" id="file" name="file" />
                    dataType: 'json', //返回值类型 一般设置为json
                    success: function (data, status)  //服务器成功响应处理函数
                    {
                        // $("#img1").attr("src", data.imgurl);
                        if (typeof (data.error) != 'undefined') {
                            if (data.error != '') {
                                alert(data.error);
                            } else {
                                //成功

                                $("#filenames").html($("#filenames").html() + "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件：" + decodeURI(unescape(data.filename)) + "：</span><a onclick='delfj(this);'>删除</a><span style='display:none'><input type='text' class='fujianurl' value='" + decodeURI(unescape(data.fileurl)) + "'/><input type='text' class='fujianname' value='" + decodeURI(unescape(data.filename)) + "'/></span></div>");
                                //alert(data.msg);

                            }
                        }
                    },
                    error: function (data, status, e)//服务器响应失败处理函数
                    {
                        alert(e);
                    }
                }
            )//清空上传控件
            document.getElementById("upLoadFiles").outerHTML = document.getElementById("upLoadFiles").outerHTML;
            return false;


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

        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div>
            <div style="text-align: center; font-weight: 800; margin-top: 10px; margin-bottom: 5px; font-size: large">区域经费用款申请单</div>
            <table class="baseTable">

                <tr>
                    <td style="text-align: right">申请人：</td>
                    <td>
                        <asp:TextBox ID="txtsqr" runat="server"></asp:TextBox><span style="color: red">*</span>
                    </td>
                    <td style="text-align: right">用款部门：</td>
                    <td>
                        <asp:TextBox ID="txtdept" runat="server" OnTextChanged="txt_yksj_TextChanged" AutoPostBack="true"></asp:TextBox><span style="color: red">*</span>
                    </td>
                    <td>申请时间：</td>
                    <td>
                        <asp:TextBox ID="txtdate" runat="server"></asp:TextBox><span style="color: red">*</span></td>
                </tr>

                <tr>
                    <td>用款方式：</td>
                    <td>
                        <asp:DropDownList ID="drp_ykfs" runat="server" Width="90%">
                            <asp:ListItem Value="转账">转账</asp:ListItem>
                            <asp:ListItem Value="支票">支票</asp:ListItem>
                            <asp:ListItem Value="现金">现金</asp:ListItem>
                            <asp:ListItem Value="电汇">电汇</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">用款时间：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_yksj" runat="server" OnTextChanged="txt_yksj_TextChanged" AutoPostBack="true"></asp:TextBox><span style="color: red">*</span>
                    </td>
                    <td>可用额度：</td>
                    <td>
                        <asp:Label ID="lblkyed" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>款项用途：</td>
                    <td colspan="3">
                        <asp:TextBox ID="TextBox1" runat="server" Width="97%"></asp:TextBox><span style="color: red">*</span></td>
                    <td style="text-align: right">申请金额：</td>
                    <td>
                        <asp:TextBox ID="txtje" runat="server" Width="95%"></asp:TextBox></td>

                </tr>
                <tr>
                    <td style="text-align: right">申请说明：</td>
                    <td colspan="5">
                        <asp:TextBox ID="txtSm" runat="server" TextMode="MultiLine" Width="95%" Height="300"></asp:TextBox><span style="color: red">*</span>
                    </td>
                </tr>
                <tr>
                    <td>附件：</td>
                    <td colspan="5">
                        <asp:Label ID="lblfj" runat="server" Text="上传附件："></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                    <%--<asp:FileUpload ID="upLoadFiles" runat="server" Width="100px" />--%>
                        <input type="file" id="upLoadFiles" name="upLoadFiles" style="100px" />

                        <asp:HiddenField ID="hidfilnename" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddFileDz" runat="server" />
                        <input id="csfj" value="上传" class="baseButton" onclick="shangchuan();" type="button" />
                        <%--<asp:Button ID="btn_sc" runat="server" Visible="false" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />--%>
                        <%--<asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>--%>
                        <div id="divBxdj" runat="server">
                        </div>
                        <%-- <asp:Literal ID="Lafilename" runat="server" Text=""></asp:Literal>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>
                        <div id="filenames" runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label runat="server" ForeColor="Red" ID="lbl_je"></asp:Label>
                    </td>
                </tr>
                <tr id="tr_shyj" runat="server">
                    <td style="text-align: right">审核意见：
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_shxx_history" runat="server">
                    <td style="text-align: right">审核详细：
                    </td>
                    <td colspan="5">
                        <span id="txt_shxx_history" runat="server"></span>
                    </td>
                </tr>

                <tr id="tr_shyj_history" runat="server">
                    <td style="text-align: right">历史驳回意见：
                    </td>
                    <td colspan="5">
                        <span id="txt_shyj_History" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: right">
                        <asp:Button ID="btn_save" runat="server" Text="保  存" OnClientClick="fj()" CssClass="baseButton" OnClick="btn_save_Click" />
                        &nbsp; &nbsp; &nbsp;
                       <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />
                        <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />
                        <input type="button" class="baseButton" value="关  闭" onclick="self.close();" />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
