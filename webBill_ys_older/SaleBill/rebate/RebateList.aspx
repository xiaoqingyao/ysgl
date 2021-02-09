<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateList.aspx.cs" Inherits="SaleBill_rebate_RebateList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>部门销售预算明细</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script type="text/javascript">
        var status = "none";
        $(function() {

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[1] != null && $(this).find("td")[1].innerHTML != "") {
                    var bh = $(this).find("td")[1].innerHTML;
                    $("#hdbillcode").val(bh);
                }

            });


            $("#btnSelect").click(function() {
             $("#trSelect").toggle();             
            });
            $("#btrefresh").click(function() {
                location.replace(location.href);
            });

            //车辆类型
            $("#txtcartype").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var carTypeCode = ui.item.value;
                    // $("#lbmcar").text("车辆类型：" + carTypeCode);
                }

            });

            //部门选择

            $("#txtdept").autocomplete({
                source: availableTagsdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;

                    //根据部门改变费用类别的可选项
                    var fylbsource = '';
                    $.post("../../webBill/MyAjax/GetYSKMByDept.ashx", { "deptCode": deptCode }, function(data, status) {

                        if (status == "success") {
                            fylbsource = data;

                        }
                    });

                    if (fylbsource != '') {
                        availableTagsfy = fylbsource;
                    }

                }
            });

            //费用类别
            $("#txtfeename").autocomplete({

                source: availableTagsfy,
                select: function(event, ui) {
                    var rybh = ui.item.value;

                }
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });


        });

        function istrue() {

            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行！");
                return false;
            }
            //                 var varstatus = checkrow.find("td")[10].innerHTML;
            //                 if (varstatus=="已批复")
            //                 {
            //                     alert("该记录已批复！");
            //                     return false;
            //                 }
            if (!confirm("您确定要执行该操作吗?")) {
                return false;
            }



        }

        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:200px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {

                location.replace(location.href);
            }
        }
        function goToSelectSaleBill(url) {
            window.location.href = url;
        }
        function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
            if (t == null || t.rows.length < 1) {
                return;
            }
            var t2 = t.cloneNode(true);
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

    </script>

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <input type="button" id="btrefresh" value="刷 新" class="baseButton" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <%--                <input type="button" value="增 加" runat="server" id="Button2" class="baseButton" visible="false"
                    onclick="goToSelectSaleBill('../select/SelectBillToRebate.aspx')" />--%>
                <asp:Button runat="server" Text="修 改" ID="btn_edit" CssClass="baseButton" OnClick="btn_edit_Click" />
                <asp:Button runat="server" Text="删 除" ID="btn_delete" CssClass="baseButton" OnClientClick=" return istrue()"
                    OnClick="btn_delete_Click" />
                <%--<asp:Button ID="btn_pf" runat="server" Text="确认批复" CssClass="baseButton" OnClientClick=" return istrue()"
                    Visible="false" OnClick="btn_pf_Click" />--%>
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <div style="float: left">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                日期从
                            </td>
                            <td>
                                <asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                至：
                            </td>
                            <td>
                                <asp:TextBox ID="txb_sqrqend" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                申请单位：
                            </td>
                            <td>
                                <asp:TextBox ID="txtdept" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                车架号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtTruckCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                车辆类型：
                            </td>
                            <td>
                                <asp:TextBox ID="txtcartype" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                费用科目;
                            </td>
                            <td>
                                <asp:TextBox ID="txtfeename" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                类别：
                            </td>
                            <td>
                                <asp:DropDownList ID="txttype" runat="server" Width="120px">
                                    <asp:ListItem Value="">--全部--</asp:ListItem>
                                    <asp:ListItem Value="0">期初分配</asp:ListItem>
                                    <asp:ListItem Value="1">销售提成</asp:ListItem>
                                    <asp:ListItem Value="2">配置项</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                状态：
                            </td>
                            <td>
                                <asp:DropDownList ID="txtstatus" runat="server" Width="120px">
                                    <asp:ListItem Value="">--全部--</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">正常</asp:ListItem>
                                    <asp:ListItem Value="D">已删除</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" style="text-align: center">
                                <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_sel_Click" />
                                <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="header">
                </div>
                <div class="baseDiv" id="main" style="overflow-y: scroll; margin-top: -1px; width: 1220px;
                    height: 380px;">
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="17" Style="table-layout: fixed" Width="100%" ShowFooter="true"
                        OnItemDataBound="myGrid_ItemDataBound">
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择" ItemStyle-Width="38">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Nid" HeaderText="编号" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ActionDate" HeaderText="费用发生日期" DataFormatString="{0:D}"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemCenter" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="申请单位" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="150" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" Width="150" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" Width="150" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TruckCode" HeaderText="车架号" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="trucktypename" HeaderText="车辆类型" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Width="150" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="150" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="feename" HeaderText="费用科目" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="150" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItemRight" Width="150" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" Width="150" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="feekz" HeaderText="费用控制" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="150" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="150" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Fee" HeaderText="金额" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="rtype" HeaderText="类别" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <%-- <asp:BoundColumn DataField="username" HeaderText="批复人" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                        </asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="astatus" HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="True" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ActionNote" HeaderText="费用变动" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="True" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="True" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="备注" DataField="Remark" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
                    </asp:DataGrid>
                    <asp:HiddenField ID="hdbillcode" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton><asp:LinkButton
                    ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton><asp:LinkButton
                        ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton><asp:LinkButton
                            ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾 页</asp:LinkButton>第<asp:DropDownList
                                ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                            </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                        </td>
                        <td id="wf">
                            <asp:Label ID="lblShlc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
