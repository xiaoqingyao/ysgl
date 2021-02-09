<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bill_bmsrysList.aspx.cs"
    Inherits="webBill_tjbb_dz_bill_bmsrysList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="Text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script language="javascript" type="Text/javascript">
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

        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '0.00';
                alert("必须用阿拉伯数字表示！");
            };
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
                                <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />

                                <input type="button" value="关 闭" id="btnSr" runat="server" class="baseButton" onclick="self.close()" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="deptCode" HeaderText="部门编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptname" HeaderText="部门名称">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptMc" HeaderText="部门名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="收入预算金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Width="131px" CssClass="rightBox" Text="0.00"
                                        onkeyup="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="收入决算金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtjsje" runat="server" Width="131px" CssClass="rightBox" Text="0.00"
                                        onkeyup="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />

                    <input type="button" value="关 闭" id="Button3" runat="server" class="baseButton" onclick="self.close()" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
