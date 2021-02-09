<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yksqList.aspx.cs" Inherits="webBill_bxgl_yksqList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用款审批的列表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
           
               //查询
            $("#btnSelect").click(function() {
                $("#trSelect").toggle();
            });
            $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                 if(confirm("删除本条记录会清除本条记录与外部系统入库单的对应，您确定要删除吗？"))
                 {
                var billcode = checkrow.find("td")[0].innerHTML;
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "yksq" }, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败1!");
                        }
                    }
                    else {
                        alert("删除失败2!");
                    }
                });
                }
            });
            $("#btn_look").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("yksqDetail.aspx?type=view&billCode=" + billcode);
            });
            
            
            $("#btn_print").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                window.open("yksqDetailPrint.aspx?billCode=" + billcode);
            });
            
        });
          function OpenAdd() {
            var url = 'yksqDetail.aspx?type=add&par=' + Math.random();
            openDetail(url);
        }

        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
              $("#btnRefresh").click();
            }
        }
     
   
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
  


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 30px">
                <input id="btnRefresh" type="button" class="baseButton" value="刷 新" onclick="javascript:location.replace(location.href);" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="OpenAdd();" />
                <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                <input type="button" value="打印" id="btn_print" class="baseButton" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left" colspan="3">
                <div style="float: left">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                申请日期从：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                到：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                单据编号：
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td colspan="6">
                                <asp:Button ID="Button2" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <PagerStyle Visible="False" />
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="编号" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="jbr" HeaderText="经办人" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="用款日期" DataFormatString="{0:D}"
                                HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="用款部门" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="je" HeaderText="申请金额" DataFormatString="{0:F2}" HeaderStyle-Width="150"
                                ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                </pager:UcfarPager>
                <input type="hidden" runat="server" id="hdwindowheight" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
