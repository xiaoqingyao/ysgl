<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yd_ystzDetail.aspx.cs" Inherits="webBill_ysgl_yd_ystzDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--这个页面是用于预算调整的，具有普遍适用性。调整金额不得超过科目总金额，总体调整不会超过年度总预算--%>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
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
    </style>
    <script type="text/javascript">
        $(function () {
            $("#<%=GridView1.ClientID%>  tr").filter(":not(:has(table,th))").click(function () {
                var pinghengshu = $(this).find("td").eq(28).children().eq(0).html();
                var km = $(this).find("td").eq(1).html();
                km += "调整差额：";
                $("#phs").html(km + pinghengshu);
            });
            //让GridView2靠左对齐
            $("#GridView2").css("margin-left", "0px");

        });
        function calculatePingHeng(obj) {
            if (obj.value != "-" && isNaN(obj.value)) {
                obj.value = "";
                alert("请输入阿拉伯数字。");
                return;
            }
            var totalTiaoZheng = 0;//调整金额总计
            var inputs = $(obj).parent().parent().find("input:text");
            var inputsCount = inputs.length;//获取的文本框的个数
            for (var i = 0; i < inputsCount - 1; i++) {
                if (i % 2 != 0) {
                    var eveTiaoZheng = inputs[i].value;
                    if (!isNaN(eveTiaoZheng) && eveTiaoZheng.length > 0) {
                        totalTiaoZheng += parseFloat(inputs[i].value);
                    }
                }
            }
            //计算出年调整数
            inputs[inputsCount - 1].value = totalTiaoZheng;
            //获取年预算数
            var yearYs = inputs[inputsCount - 2].value;
            //计算调整差额数
            var pinghengshu = yearYs - totalTiaoZheng;
            var inHtml = "<span style='color:red;font-weight:bold'>" + pinghengshu + "</span>";
            if (pinghengshu > 0) {
                inHtml = "<span style='color:green;font-weight:bold'>" + pinghengshu + "</span>";
            } else if (pinghengshu == 0) {
                inHtml = "<span style='font-weight:bold'>" + pinghengshu + "</span>";
            }
            //插入调整差额数列
            $(obj).parent().parent().find("td").eq(28).children().eq(0).html(inHtml);
            //放入隐藏域
            $(obj).parent().parent().find("td").eq(28).children().eq(1).val(pinghengshu);
            //显示调整差额
            var km = $(obj).parent().parent().find("td").eq(1).html();//科目
            km += "调整差额：";
            $("#phs").html(km + inHtml);
        }
        function EnbleTxt() {
            $("body td[class='rightReadOnly'] input").attr("readonly", "readonly");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <div>

                <input type="button" value="返回列表页" id="btn_fh" runat="server" onclick="window.location.href = 'ystz.aspx'" class="baseButton" />
                <asp:Button ID="btnTzMx" runat="server" Text="查看调整明细" OnClick="btnTzMx_Click" CssClass="baseButton" />
                <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" CssClass="baseButton" />

                财年：<asp:DropDownList ID="ddlNd" runat="server"></asp:DropDownList>
                预算类型：<asp:DropDownList
                    runat="server" ID="ddlYsType" AutoPostBack="True" OnSelectedIndexChanged="ddlYsType_SelectedIndexChanged">
                </asp:DropDownList>
                部门：<%=deptname %>
                <span id="phs"></span>
            </div>
            <div style="float: right">
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
                        <td>(预算过程未开始，已经结束。)
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
                                            <asp:TextBox ID="txtJanuary" runat="server" Width="70" Text='<%#Eval("January") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="一月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJanuary_to" runat="server" Width="70" Text='<%#Eval("January") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="二月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFebruary" runat="server" Width="70" Text='<%#Eval("February") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="二月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFebruary_to" runat="server" Width="70" Text='<%#Eval("February") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="三月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmarch" runat="server" Width="70" Text='<%#Eval("march") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="三月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmarch_to" runat="server" Width="70" Text='<%#Eval("march") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="四月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtApril" runat="server" Width="70" Text='<%#Eval("April") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="四月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtApril_to" runat="server" Width="70" Text='<%#Eval("April") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="五月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMay" runat="server" Width="70" Text='<%#Eval("May") %>' CssClass="rightReadOnly"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="五月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMay_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("May") %>'
                                                onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="六月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJune" runat="server" Width="70" Text='<%#Eval("June") %>' CssClass="rightReadOnly"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="六月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJune_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("June") %>'
                                                onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="七月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJuly" runat="server" Width="70" Text='<%#Eval("July") %>' CssClass="rightReadOnly"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="七月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJuly_to" runat="server" Width="70" CssClass="righttxt" Text='<%#Eval("July") %>'
                                                onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="八月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAugust" runat="server" Width="70" Text='<%#Eval("August") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="八月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAugust_to" runat="server" Width="70" Text='<%#Eval("August") %>'
                                                onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);" CssClass="righttxt"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="九月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSeptember" runat="server" Width="70" Text='<%#Eval("September") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="九月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSeptember_to" runat="server" Width="70" Text='<%#Eval("September") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOctober" runat="server" Width="70" Text='<%#Eval("October") %>'
                                                ReadOnly="true" CssClass="rightReadOnly"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOctober_to" runat="server" Width="70" Text='<%#Eval("October") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十一月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNovember" runat="server" Width="70" Text='<%#Eval("November") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十一月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNovember_to" runat="server" Width="70" Text='<%#Eval("November") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十二月预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDecember" runat="server" Width="70" Text='<%#Eval("December") %>'
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="十二月预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDecember_to" runat="server" Width="70" Text='<%#Eval("December") %>'
                                                CssClass="righttxt" onblur="calculatePingHeng(this);" onkeyup="calculatePingHeng(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="年预算金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtYear" runat="server" Width="70" Text=""
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="年预算调整金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtYear_to" runat="server" Width="70" Text=""
                                                CssClass="rightReadOnly" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="调整差额">
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
                        <td>
                            <div>调整明细如下：</div>
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="baseTable" ShowHeader="true">
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
                                    <asp:BoundField DataField="Ysje" HeaderText="调整金额" ItemStyle-CssClass="myGridItemRight" />
                                    <asp:TemplateField HeaderText="调整说明">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="tzShuoMing" Text='<%#Eval("sm") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Gcbh" HeaderText="过程编号" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="Yskm" HeaderText="预算科目" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                预算调整单信息提示：
                <ul style="margin: 0; padding: 0;">
                    <li>1.调整差额=本行预算总额-本行调整后预算总额；调整差额数等于0时，该行调整记录有效。</li>
                    <li>2.单击“查看调整明细”按钮查看调整明细情况。</li>
                    <li>3.审批通过后生效。</li>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
