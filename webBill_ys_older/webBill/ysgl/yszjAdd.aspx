<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszjAdd.aspx.cs" Inherits="ysgl_yszjAdd" %>

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
            gudingbiaotounew($("#myGrid"), $(window).height() - 100);
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

        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '0.00';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function yzsz(obj)//必须输入数字包括负数
        {
            
            if (/^-?\d+(?:\.\d+)?$/.test(obj.value)) {
                return true;
            }
            else {
                alert("必须用阿拉伯数字表示！");
                return false;
            }
        }
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 50) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: -10px;margin: 0;'></div>");
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
                            <td>部门：<asp:DropDownList runat="server" ID="LaDept" AutoPostBack="True" OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
                            </asp:DropDownList>
                                <div style="display:none">
                                      预算类型：<asp:DropDownList
                                    runat="server" ID="ddlYsType" AutoPostBack="True" OnSelectedIndexChanged="ddlYsType_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                              
                                <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />
                                <asp:Button ID="Button2" runat="server" Text="取 消" CssClass="baseButton" OnClick="Button2_Click" />
                                <asp:Label ID="Label1" runat="server" ForeColor="Red">填写追加金额</asp:Label>
                                <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
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
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound" Width="1200">
                            <Columns>
                                <asp:BoundColumn DataField="yskmCode" HeaderText="科目编号" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmBm" HeaderText="科目代码" ItemStyle-Width="80" HeaderStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmMc" HeaderText="科目名称" ItemStyle-Width="400" HeaderStyle-Width="400">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>

                                <asp:BoundColumn DataField="" HeaderText="剩余预算" ItemStyle-Width="120" HeaderStyle-Width="115">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>


                                <asp:TemplateColumn HeaderText="追加金额" ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Width="90%" CssClass="rightBox" Text="0.00"
                                            onblur="yzsz(this);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="追加说明" ItemStyle-Width="400" HeaderStyle-Width="400">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtShuoming" runat="server" Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="tblx" HeaderText="填报类型" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
