<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yd_ystzDetail_dz.aspx.cs" Inherits="webBill_ysgl_yd_ystzDetail_dz" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--这个页面是用于大智定制的，预算调整金额不超过年度预算即可--%>
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <%--  <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />--%>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        body {
            font-size: 12px;
        }

        .righttxt {
            text-align: right;
            background-color: Transparent;
        }

        .rightReadOnly {
            text-align: right;
            background-color: #dedede;
        }

        .unEdit {
            background-color: #dedede;
        }

        #bottomNav {
            background-color: #D1E0FD;
            z-index: 999;
            position: fixed;
            bottom: 0;
            left: 0;
            width: 100%;
            _position: absolute;
            _top: expression_r(documentElement.scrollTop + documentElement.clientHeight-this.offsetHeight);
            overflow: visible;
            height: 21px;
            font-size: medium;
            font-weight: 700;
            line-height: 21px;
        }

        .basehidden {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //让GridView2靠左对齐
            $("#GridView2").css("margin-left", "0px");

            <%--$("#<%=GridView1.ClientID%>  tr td").filter(":not(:has(table,th))").click(function () {
                //var pinghengshu = $(this).find("td").eq(40).children().eq(0).html();
                //var km = $(this).find("td").eq(1).html();
                //km += "调整差额：";
                //$("#phs").html(km + pinghengshu);
                //显示出科目和对应月份
                var km = $(this).parent().find("td:eq(1)").html();
                km = km.replace(new RegExp("&nbsp;", "gm"), "");
                var index = $(this).index();
                if (index > 1) {
                    var factindex = Math.floor(((index - 2) / 3)) + 2;
                    var yf = $("#<%=GridView1.ClientID%>").find("tr").eq(0).children().eq(factindex).html();
                    var txt = $("#<%=GridView1.ClientID%>").find("tr").eq(1).children().eq(index - 1).html();
                    $("#bottomNav").html("&nbsp;&nbsp;当前选择项：" + yf + "-" + txt + "-" + km);
                }
            });--%>

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
                        if (confirm("确定要审批该单据吗?")) {
                            $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                        }
                    }
                }

            });

            //驳回选择
            $("#btn_cancel").click(function () {
                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();


                if (billcode == "") {
                    alert("请选择驳回的记录。");
                    return;
                }
                window.showModalDialog("../MyWorkFlow/DisAgreeToSpecial.aspx?billCode=" + billcode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes')

                $("#btnRefresh").click();
            });

     <%--       //否决单据
            $("#btn_cancel").click(function () {

                var billcode = '<%=Request["billCode"] %>';
                var mind = $("#txt_shyj").val();
                billcode = billcode + "*" + mind + ",";
                billcode = escape(billcode);
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                } else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": "", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });--%>
        });
        function showMsg(obj) {
            var km = $(obj).parent().parent().find("td:eq(1)").html();
            km = km.replace(new RegExp("&nbsp;", "gm"), "");
            var index = $(obj).parent().index();
            if (index > 1) {
                var factindex = Math.floor(((index - 2) / 3)) + 2;
                var yf = $("#<%=GridView1.ClientID%>").find("tr").eq(0).children().eq(factindex).html();
                var txt = $("#<%=GridView1.ClientID%>").find("tr").eq(1).children().eq(index - 1).html();
                $("#bottomNav").html("&nbsp;&nbsp;当前选择项：" + yf + "-" + txt + "-" + km);
            }
        }
        //验证调整后的金额不能小于报销金额
        function yzbxje(obj) {
            var tzhje = obj.value;//调整后金额
            var ysje = $(obj).parent().prev().children().eq(0).val();
            var bxje = $(obj).parent().next().children().eq(0).val();

            if (parseFloat(tzhje) < parseFloat(bxje)) {
                alert("调整后的金额不能小于已经报销的金额")
                obj.value = ysje;
                $(obj).focus();
                return;
            }
        }
        //计算平衡数
        function calculatePingHeng(obj) {
            if (obj.value != "-" && isNaN(obj.value)) {
                obj.value = "";
                alert("请输入阿拉伯数字。");
                return;
            }
            var totalTiaoZheng = 0;//调整金额总计
            var inputs = $(obj).parent().parent().find("input:text");
            var inputsCount = inputs.length;//获取的文本框的个数

            for (var i = 0; i < inputsCount - 3; i++) {
                if (i % 3 == 1) {
                    var eveTiaoZheng = inputs[i].value;
                    if (!isNaN(eveTiaoZheng) && eveTiaoZheng.length > 0) {
                        totalTiaoZheng += parseFloat(inputs[i].value);
                    }
                }
            }
            //计算出年调整数
            inputs[inputsCount - 2].value = totalTiaoZheng.toFixed(2);

            //获取年预算数
            var yearYs = inputs[inputsCount - 3].value;
            //////计算调整差额数
            ////var pinghengshu = yearYs - totalTiaoZheng;
            ////var inHtml = "<span style='color:red;font-weight:bold'>" + pinghengshu + "</span>";
            ////if (pinghengshu > 0) {
            ////    inHtml = "<span style='color:green;font-weight:bold'>" + pinghengshu + "</span>";
            ////} else if (pinghengshu == 0) {
            ////    inHtml = "<span style='font-weight:bold'>" + pinghengshu + "</span>";
            ////}
            //////插入调整差额数列
            ////$(obj).parent().parent().find("td").eq(28).children().eq(0).html(inHtml);
            //////放入隐藏域
            ////$(obj).parent().parent().find("td").eq(28).children().eq(1).val(pinghengshu);
            //////显示调整差额
            ////var km = $(obj).parent().parent().find("td").eq(1).html();//科目
            ////km += "调整差额：";
            ////$("#phs").html(km + inHtml);
        }
        function EnbleTxt() {
            $("body td[class='rightReadOnly'] input").attr("readonly", "readonly");
        }
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <div>
                <input type="button" value="返回列表页" id="btn_fh" runat="server" onclick="window.location.href = 'ystz.aspx?isdz=1'" class="baseButton" />
                <asp:Button ID="btnTzMx" runat="server" Text="查看调整明细" OnClick="btnTzMx_Click" CssClass="baseButton" />
                <asp:Button ID="btn_excelmx" runat="server" Text="导出调整明细" OnClick="btn_excelmx_Click" CssClass="baseButton" />
                <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" CssClass="baseButton" />
                <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                <%--<input type="button" onclick="javascript: window.close();" value="关 闭" class="baseButton" />--%>&nbsp;
                <asp:Label runat="server" Visible="false" ID="lblzys" Text="10000000"></asp:Label>
                财年：<asp:DropDownList ID="ddlNd" runat="server"></asp:DropDownList>
                预算类型：<asp:DropDownList
                    runat="server" ID="ddlYsType" AutoPostBack="True" OnSelectedIndexChanged="ddlYsType_SelectedIndexChanged">
                </asp:DropDownList>
                部门：<%=deptname %>
                <span id="phs"></span>
                <div runat="server" id="div_shyj" style="margin-top: 5px">
                    审核意见：
                      
                            <asp:TextBox ID="txt_shyj" runat="server" Width="90%"></asp:TextBox>
                </div>
                <div style="border-bottom: 1px dashed #CDCDCD; margin-top: 5px;">
                    <asp:Label ID="lblfj" runat="server" Text="附件："></asp:Label>
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
                </div>
            </div>
            <div style="float: right" id="msg" runat="server">
                <table>
                    <tr>
                        <td></td>
                        <td>
                            <div style="height: 15px; border: solid 2px red;">
                                可以编辑
                            </div>
                        </td>
                        <td>(预算过程开始)
                        </td>
                        <td>
                            <div style="height: 15px; border: solid 2px red; background-color: #DEDEDE">
                                不允许编辑
                            </div>
                        </td>
                        <td>(预算过程已经结束。)
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 5px; display: none">调整说明：<asp:TextBox ID="txtZy" runat="server" Width="500"></asp:TextBox></div>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable" ShowHeader="true" OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="序号" InsertVisible="False">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="km" HeaderText="预算科目" HtmlEncode="false" ItemStyle-Wrap="false" />
                                    <asp:TemplateField HeaderText="一月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJanuary" runat="server" Width="70" Text='<%#Eval("January") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="一月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJanuary_to" runat="server" Width="70" Text='<%#Eval("January") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="一月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJanuarybx" runat="server" Width="70" Text='<%#Eval("Januarybxje") %>' CssClass="rightReadOnly" onclick="showMsg(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="二月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFebruary" runat="server" Width="70" Text='<%#Eval("February") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidFebruary" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="二月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFebruary_to" runat="server" Width="70" Text='<%#Eval("February") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="二月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFebruarybx" runat="server" Width="70" Text='<%#Eval("Februarybxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="三月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmarch" runat="server" Width="70" Text='<%#Eval("march") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidmarch" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="三月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmarch_to" runat="server" Width="70" Text='<%#Eval("march") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="三月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmarchbx" runat="server" Width="70"  Text='<%#Eval("marchbxje") %>'  onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="四月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtApril" runat="server" Width="70" Text='<%#Eval("April") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidApril" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="四月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtApril_to" runat="server" Width="70" Text='<%#Eval("April") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="四月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAprilbx" runat="server" Width="70" Text='<%#Eval("Aprilbxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="五月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMay" runat="server" Width="70" Text='<%#Eval("May") %>' CssClass="rightReadOnly" onclick="showMsg(this);"
                                                ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidMay" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="五月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMay_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("May") %>' onclick="showMsg(this);"
                                                onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="五月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMaybx" runat="server" Width="70" Text='<%#Eval("Maybxje") %>'  onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="六月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJune" runat="server" Width="70" Text='<%#Eval("June") %>' CssClass="rightReadOnly" onclick="showMsg(this);"
                                                ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidJune" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="六月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJune_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("June") %>' onclick="showMsg(this);"
                                                onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="六月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJunebx" runat="server" Width="70" Text='<%#Eval("Junebxje") %>'  onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="七月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJuly" runat="server" Width="70" Text='<%#Eval("July") %>' CssClass="rightReadOnly" onclick="showMsg(this);"
                                                ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidJuly" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="七月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJuly_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("July") %>' onclick="showMsg(this);"
                                                onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="七月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJulybx" runat="server" Width="70" Text='<%#Eval("Julybxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="八月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAugust" runat="server" Width="70" Text='<%#Eval("August") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidAugust" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="八月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAugust_to" runat="server" Width="70" Text='<%#Eval("August") %>' onclick="showMsg(this);"
                                                onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);" CssClass="righttxt"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="八月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAugustbx" runat="server" Width="70" Text='<%#Eval("Augustbxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="九月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSeptember" runat="server" Width="70" Text='<%#Eval("September") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidSeptember" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="九月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSeptember_to" runat="server" Width="70" Text='<%#Eval("September") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="九月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSeptemberbx" runat="server" Width="70" Text='<%#Eval("Septemberbxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOctober" runat="server" Width="70" Text='<%#Eval("October") %>' onclick="showMsg(this);"
                                                ReadOnly="true" CssClass="rightReadOnly"></asp:TextBox>
                                            <asp:HiddenField ID="hidOctober" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOctober_to" runat="server" Width="70" Text='<%#Eval("October") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOctoberbx" runat="server" Width="70"  Text='<%#Eval("Octoberbxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十一月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNovember" runat="server" Width="70" Text='<%#Eval("November") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidNovember" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十一月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNovember_to" runat="server" Width="70" Text='<%#Eval("November") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十一月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNovemberbx" runat="server" Width="70" Text='<%#Eval("Novemberbxje") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十二月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDecember" runat="server" Width="70" Text='<%#Eval("December") %>' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                            <asp:HiddenField ID="hidDecember" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十二月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDecember_to" runat="server" Width="70" Text='<%#Eval("December") %>' onclick="showMsg(this);"
                                                CssClass="righttxt" onblur="calculatePingHeng(this);yzbxje(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十二月报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDecemberbx" runat="server" Width="70" Text='<%#Eval("Decemberbxje") %>'  onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="年预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtYear" runat="server" Width="70" Text="" onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="年预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtYear_to" runat="server" Width="70" Text="" onclick="showMsg(this);"
                                                CssClass="rightReadOnly"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="年报销金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtYearbx" runat="server" Width="70" Text='0.00' onclick="showMsg(this);"
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="科目调整额">
                                        <ItemTemplate>
                                            <div><span style="font-weight: bold">0</span></div>
                                            <asp:HiddenField ID="hdPingHengShu" runat="server" Value="0" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="basehidden" HeaderStyle-CssClass="basehidden">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <div style="float: left"><strong>调整明细如下：</strong> </div>
                            <div id="divMx">
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="baseTable" ShowHeader="true" OnRowDataBound="GridView2_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="预算过程">
                                            <ItemTemplate>
                                                [<%#Eval("Gcbh") %>]<%#Eval("GcMc") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="预算科目">
                                            <ItemTemplate>
                                                [<%#Eval("Yskm") %>]<%#Eval("YskmMc") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="类别" ItemStyle-CssClass="myGridItemCenter" />
                                        <asp:BoundField DataField="Ysje" HeaderText="调整出" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N}" />
                                        <asp:BoundField DataField="Ysje" HeaderText="调整入" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N}" />
                                        <asp:BoundField HeaderText="调整后预算金额" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N}" />
                                        <asp:TemplateField HeaderText="调整说明">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="tzShuoMing" Text='<%#Eval("sm") %>' Width="260"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Gcbh" HeaderText="过程编号" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                        <asp:BoundField DataField="Yskm" HeaderText="预算科目" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                        <asp:BoundField DataField="Ysje" HeaderText="调整金额" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                        <asp:BoundField DataField="sm" HeaderText="调整说明" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>

                    <tr id="tr_shxx_history" runat="server">

                        <td style="text-align: left">
                            <strong>审核详细：</strong>

                            <span id="txt_shxx_history" runat="server"></span>
                        </td>
                    </tr>
                    <tr id="tr_shyj_history" runat="server">
                        <td style="text-align: left">
                            <strong>历史驳回意见：</strong>

                            <span id="txt_shyj_History" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        <div>
            <strong>预算调整单信息提示：</strong>
            <ul style="margin: 0; padding: 0;">
                <li>1.在不超过当前部门财年预算总额的情况下，允许进行月度间、预算科目之间预算额度的调整。</li>
                <li>2.单击“查看调整明细”按钮查看调整明细情况。</li>
                <li>3.审批通过后生效。</li>
            </ul>
        </div>
        <div id="bottomNav">
            &nbsp;&nbsp;当前选择：
        </div>
      <%--  <asp:TextBox ID="txtHtml" runat="server" CssClass="hiddenbill"></asp:TextBox>--%>
        </div>
       <%-- <script type="text/javascript">
            $("#txtHtml").val($("#divMx").html());
        </script>--%>
    </form>
</body>
</html>
