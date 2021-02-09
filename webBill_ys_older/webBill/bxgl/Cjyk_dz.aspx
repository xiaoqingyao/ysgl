<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cjyk_dz.aspx.cs" Inherits="webBill_bxgl_Cjyk_dz" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>用款回冲预算</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        var status = "none";
        var sqdbillCode = "";
        var bxdbillcode = "";
        var sqdbillname = "";
        var bxdbillname = "";
        var hcje = "";
        $(function () {


            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[1] != null) {
                    sqdbillCode = $(this).find("td")[0].innerHTML;
                    bxdbillcode = $(this).find("td")[1].innerHTML;
                    sqdbillname = $(this).find("td")[2].innerHTML;
                    bxdbillname = $(this).find("td")[3].innerHTML;
                    hcje = $(this).find("td")[6].innerHTML;
                    //alert(billCode);
                }
            });
            //回冲
            $("#btn_hc").click(function () {

                if (bxdbillcode == undefined || bxdbillcode == "") {
                    alert("请选择需要回冲的用款单");
                    return;

                }
                if (!confirm("您确定要回冲该用款申请单的占用预算吗？")) {
                    return;
                }
                var usercode= $("#hidusercode").val();//
              
                $.post("../MyAjax/Ykyshc.ashx", { "bxdbillcode": bxdbillcode, "billuser": usercode, "hcje": hcje }, function (data, status) {
                    if (status == "success") {
                        if (data == "ok") {
                            alert("回冲成功");
                            $(".highlight").remove();
                        }


                    }
                });

            });

            //申请单详细信息
            $("#btn_look").click(function () {
                var url = 'bxDetailForDz.aspx?type=look&billCode=' + sqdbillname;
                url += '&isDZ=1&dydj=02';
                openDetail(url);
            });

            //报销单信息
            $("#btn_look_bxd").click(function () {
                var url = 'bxDetailForDz.aspx?type=look&billCode=' + bxdbillname;
                url += '&isDZ=1&dydj=06';
                openDetail(url);
            });

        });


        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:990px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btnRefresh").click();
            }
        }

        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {

            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");

        });



    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <input id="btnRefresh" type="button" class="baseButton" value="刷 新" onclick="javascript: location.replace(location.href);" />
                    <input type="button" value="申请单详细信息" id="btn_look" class="baseButton" />
                    <input type="button" value="报销单详细信息" id="btn_look_bxd" class="baseButton" />
                    <input type="button" value="回冲预算" id="btn_hc" class="baseButton" />

                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid">
                            <Columns>
                                <asp:BoundColumn DataField="billcode_sqd" HeaderText="billcode_sqd" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billcode_bxd" HeaderText="billcode_bxd" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billname_sqd" HeaderText="申请单据编号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billname_bxd" HeaderText="报销单据编号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sqdje" HeaderText="申请单金额" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bxdje" HeaderText="报销金额" DataFormatString="{0:F2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItemRight" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="kcjje" HeaderText="可回冲金额" ItemStyle-Width="350" HeaderStyle-Width="350">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>

                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                        <asp:HiddenField ID="hidusercode" runat="server" />
                    </div>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
