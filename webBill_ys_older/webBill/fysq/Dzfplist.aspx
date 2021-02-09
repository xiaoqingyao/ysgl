<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dzfplist.aspx.cs" Inherits="webBill_fysq_Dzfplist" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>电子发票列表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">

        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txb_sqrqbegin").datepicker();
            $("#txb_sqrqend").datepicker();

        });

        $(function () {
            //gudingbiaotounew($("#myGrid"), 380);
            // var status = "none";
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_cx").click(function () {
                $("#trSelect").toggle();
            });
            //修改
            $("#btn_edit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("DzfpDetail.aspx?Ctrl=Edit&Code=" + billcode);
            });
       
            $("#btn_delete").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
              
                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                if (checkrow.find("td")[0] != null && checkrow.find("td")[0].innerHTML != "") {
                    var billcode = checkrow.find("td")[0].innerHTML;
                }
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "dzfp" }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败!");
                        }
                    }
                    else {
                        alert("删除失败!");
                    }
                });
            });
            $("#btn_look").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("DzfpDetail.aspx?Ctrl=View&Code=" + billcode);
            });
           
           
          

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    var billCode = $(this).find("td")[0].innerHTML;
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
     
        });

        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:1000px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                //document.getElementById("Button4").click();
                $("#Button4").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }


        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="height: 30px">
                    <input type="button" value="查 询" id="btn_cx" class="baseButton" />
                    <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('DzfpDetail.aspx?Ctrl=Add&par=' + Math.random());" />
                    <input type="button" value="修 改" id="btn_edit" class="baseButton" runat="server" />
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" runat="server" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" runat="server" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr id="trSelect" style="display: none;">
                <td>
                    <div style="float: left">
                        <table class="baseTable" style="text-align: left;">
                            <tr>
                                <td>申请日期从：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>至:
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_sqrqend" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>单据编号：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>申请部门：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppDept" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>报告单状态：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server">
                                        <asp:ListItem Value="">--全部--</asp:ListItem>
                                        <asp:ListItem Value="1">已附</asp:ListItem>
                                        <asp:ListItem Value="0">未附</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="8" style="text-align: center">
                                    <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_cx_Click" />
                                    <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="position: relative; overflow-x: auto; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                            ItemStyle-HorizontalAlign="Center" AutoGenerateColumns="False" OnItemDataBound="myGrid_ItemDataBound">
                            <ItemStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundColumn DataField="billCode" HeaderText="单据编号" HeaderStyle-Width="150px"
                                    ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="deptName" HeaderText="申请单位" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUser" HeaderText="申请人" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}"
                                    HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                             
                              
                                <asp:BoundColumn DataField="billje" HeaderText="单据总额" DataFormatString="{0:N2}"
                                    HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>        
                                <asp:BoundColumn HeaderText="部门" DataField="deptName" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
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
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
