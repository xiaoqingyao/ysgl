<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszjAddhz.aspx.cs" Inherits="ysgl_yszjAddhz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算追加</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="Text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script language="javascript" type="Text/javascript">
        $(function () {
            // gudingbiaotounew($("#myGrid"), $(window).height() - 200);

            $("#btn_selectyszj").click(function () {
                var url = "../select/selectyszj.aspx";
                var isxm = '<%=Request["isxm"]%>';
                var xmcode = '<%=Request["xmcode"]%>';
                if (isxm != "" && isxm == "1") {
                    url += "?isxm=1&xmcode=" + xmcode;
                }

                var strcode = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:650px;status:no;scroll:yes');
                if (strcode != "") {
                    $("#hidcodes").val(strcode);
                    $("#btn_xsmx").click();
                }
            });
        });
        function calLj(obj) {
            var currentCode = $(obj).parent().parent().find("td:eq(0)").text();

            var arrIndex = new Array();
            var arrCode = new Array();
            var arrVal = new Array();

            var index = 0;
            $("#myGrid").find("tr").each(function () {
                if (index == 0) {
                    index = index + 1;
                }
                else {
                    arrIndex.push(index);
                    arrCode.push($(this).find("td:eq(1)").html());
                    arrVal.push($(this).find(".rightBox:eq(0)").val());
                    index = index + 1;
                }
            });
            var list = ysgl_yszjAdd.getCalResult(currentCode, arrIndex, arrCode, arrVal).value;

            //循环赋值
            index = 0;
            $("#myGrid").find("tr").each(function () {
                if (index == 0) {
                    index = index + 1;
                }
                else {
                    var val = "";
                    for (var j = 0; j <= list.length - 1; j++) {
                        var arr = list[j].split(',');
                        if (arr[0] == index) {
                            val = arr[1];
                        }
                    }
                    $(this).find(".rightBox:eq(0)").val(val);
                    index = index + 1;
                }
            });
        }


        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: -10px;margin: 0;'></div>");
        }

    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <table cellpadding="3" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <input type="button" id="btn_selectyszj" runat="server" class="baseButton" value="选择追加单" />
                                <asp:Button runat="server" ID="btn_xsmx" CssClass="baseButton  hiddenbill" Text="显示明细" OnClick="btn_xsmx_Click" />
                                <asp:Button ID="btn_hz" runat="server" Text="显示汇总" CssClass="baseButton" OnClick="btn_hz_Click" />
                                <asp:Button ID="Button1" runat="server" Text="保存" CssClass="baseButton" OnClick="Button1_Click" />
                                <asp:Button ID="Button2" runat="server" Text="取 消" CssClass="baseButton" OnClick="Button2_Click" />
                                <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                  <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="【步骤说明：】1.选择要汇总的追加单2.显示汇总后的结果3.保存"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; display: none">说明:
                            <asp:TextBox ID="txt_sm" TextMode="MultiLine" Height="30" runat="server" Width="450px"></asp:TextBox>
                            </td>
                            <td>

                                <div style="border-bottom: 1px dashed #CDCDCD; height: 20px;">
                                    附件：  &nbsp;&nbsp;&nbsp;
                                            <asp:FileUpload ID="upLoadFiles" runat="server" Width="100px" />
                                    <asp:HiddenField ID="hidfilnename" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hiddFileDz" runat="server" />
                                    <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />
                                    <%--<input type="button" id="btn_lookpic" runat="server" value="查看图片附件" class="baseButton" />
                                    <asp:Button ID="btn_lookpic" runat="server" Text="查看图片附件" CssClass="baseButton" OnClick="btn_lookpic_Click" />--%>
                                    <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <div id="divBxdj" runat="server">
                                    </div>
                                </div>
                                <asp:Literal ID="Lafilename" runat="server" Text=""></asp:Literal>
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr_zjmx" runat="server">
                <td>
                    <hr />
                    追加单明细
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound" Width="1100">
                            <Columns>
                                <asp:BoundColumn DataField="kmname" HeaderText="科目编号" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ysje" HeaderText="追加金额" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sm" HeaderText="说明" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xm" HeaderText="项目" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        <asp:HiddenField ID="hidcodes" runat="server" />
                    </div>
                </td>
            </tr>
            <tr id="tr_hzje" runat="server">
                <td>
                    <hr />
                    汇总结果
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:DataGrid ID="myGridhz" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound" Width="1100">
                            <Columns>
                                <asp:BoundColumn DataField="kmname" HeaderText="科目编号" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ysje" HeaderText="追加金额" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sm" HeaderText="说明" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xm" HeaderText="项目" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>

                            </Columns>
                        </asp:DataGrid>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </div>
                </td>
            </tr>
            <tr id="tr_shxx_history" runat="server">
                <td>审核详细：
                    <span id="txt_shxx_history" runat="server"></span>
                </td>
            </tr>

            <tr id="tr_shyj_history" runat="server">
                <td>历史驳回意见：
               
                    <span id="txt_shyj_History" runat="server"></span>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
