<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TSFLSZlist.aspx.cs" Inherits="SaleBill_TSFLSZ_TSFLSZlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特殊返利申请报告</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
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

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                 if( $(this).find("td")[0]!=null&& $(this).find("td")[0].innerHTML!="")
                {
                    $(this).addClass("highlight");
                    var billCode = $(this).find("td")[0].innerHTML;
                }
                
                $("#hd_billCode").val(billCode);
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //查看
            $("#btn_look").click(function() {

                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var SqCode = checkrow.find("td")[0].innerHTML;
                var CarCode = checkrow.find("td")[1].innerHTML;
                var DeptCode = checkrow.find("td")[4].innerHTML;//部门
                var statu = checkrow.find("td")[5].innerHTML;//批复状态
//                var vybfee = checkrow.find("td")[3].innerHTML;//正常返利
//                var vExceedStandardPoint=checkrow.find("td")[4].innerHTML;//超出正常返利
                openDetail("SpecialRebatesStandardView.aspx?Ctrl=View&SqCode=" + SqCode + "&Carcode=" + CarCode + "&deptcode=" + DeptCode + "&stu=" + statu );


            });
            //制定
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请选择记录！");
                    return;
                }
                var Sqtype = checkrow.find("td")[5].innerHTML;

                if (Sqtype == "已批复") {
                    alert("该记录已批复，请选择其它未批复的记录！");
                    return;
                }
                var SqCode = checkrow.find("td")[0].innerHTML;
                var CarCode = checkrow.find("td")[1].innerHTML;
               
                var DeptCode = checkrow.find("td")[4].innerHTML;
                
               

                openDetail("TsflDetail.aspx?Ctrl=Edit&SqCode=" + SqCode + "&Carcode=" + CarCode + "&deptcode=" + DeptCode);
            });

            $("#btsp").click(function() {

                var checkrow = $(".highlight");

                if (checkrow.val() != undefined) {
                    var Sqtype = checkrow.find("td")[5].innerHTML;
                    if(Sqtype!="已批复"){
                     $("#hdpfzt").val(Sqtype);
                    }else{
                        return false;
                    }
                   

                }
            });

            //查询
            $("#btnSelect").click(function() {
                 $("#trSelect").toggle();
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
        });


        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:900px;dialogWidth:900px;status:no;scroll:no');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }

        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function istrue() {

            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行！");
                return false;
                
            }
           var Sqtype = checkrow.find("td")[5].innerHTML;
                   
                      if (Sqtype == "已批复") {
                    alert("该记录已批复！");
                    return;
                }
            if (!confirm("您确定要批复该单据?")) {
                return false;
            }
            var billcode = checkrow.find("td")[1].innerHTML;
        }
        function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
              if(t==null||t.rows.length<1){
                return;
            }
            var t2 = t.cloneNode(true);
            t2.id = "clonemyGrid";
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
<body>
    <%-- onload="gudingbiaotou();"--%>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                    Text="刷 新" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <input type="button" value="制定特殊返利" id="btn_edit" class="baseButton" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                <asp:Button ID="btsp" runat="server" Text="确认批复" class="baseButton" OnClick="btsp_Click"
                    OnClientClick=" return istrue()" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <div style="float: left">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                申请单号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                申请日期从：
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateFrm" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                到：
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateTo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                车架号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtcarcode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <%-- <td>
                            批复状态：
                        </td>--%>
                            <%-- <td>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="1">已批复</asp:ListItem>
                              
                            </asp:DropDownList>
                        </td>--%>
                            <td colspan="4">
                                <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
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
                <div class="baseDiv" id="main" style="overflow: scroll; margin-top: -1px; width: 1000px;
                    height: 380px; word-wrap: break-word;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Style="table-layout: fixed" Width="100%" CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound"
                        AllowPaging="True" PageSize="20">
                        <Columns>
                            <%----%>
                            <asp:BoundColumn DataField="billCode" HeaderText="申请单号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="车架号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="TruckCount" HeaderText="辆数" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem"  HorizontalAlign="Left"/>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="StandardSaleAmount" HeaderText="正常返利"  DataFormatString="{0:N2}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight"  HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ExceedStandardPoint" HeaderText="超出正常标准点数"  DataFormatString="{0:N2}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" HorizontalAlign="Right"/>
                        </asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <%--  <asp:BoundColumn DataField="EffectiveDateFrm" HeaderText="有效期始" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemCenter"  HorizontalAlign="Center"/>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EffectiveDateTo" HeaderText="有效期末" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemCenter" HorizontalAlign="Center"/>
                        </asp:BoundColumn>--%>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="部门名称" DataField="deptName" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="批复状态" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <%-- <asp:BoundColumn DataField="Explain" HeaderText="申请说明"  ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left"/>
                        </asp:BoundColumn>--%>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hd_billCode" runat="server" />
                            <asp:HiddenField ID="hdpfzt" runat="server" />
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                                            </td>
                                            <td id="wf">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
