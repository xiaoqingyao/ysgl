<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptList.aspx.cs" Inherits="webBill_yskm_deptList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <style type="text/css">
        .cwtb {
            color: Red;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">


        function selectcwkm(url, obj) {
            var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (str != undefined) {
                obj.parentNode.getElementsByTagName('input')[0].value = str;
            }
        }
        function openSelectCwkm(obj) {
            var deptCode = document.getElementById("lblDeptCode").innerHTML;
            var yskm = $(obj).parent().parent().find("td:eq(0)").text();

            var returnValue = window.showModalDialog('selectCwkm.aspx?deptCode=' + deptCode + '&yskmCode=' + yskm + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');

            if (returnValue == undefined || returnValue == "")
            { }
            else
            {
                $(obj).parent().parent().find("td:eq(3)").text(returnValue);
            }
        }
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            gudingbiaotounew($("#myGrid"), $(window).height() - 100);
            initMainTableClass("<%=myGrid.ClientID%>");

            if ('<%=Request["deptCode"]%>' == "") {
                $("#fuzhi").hide();
            }

        });

        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }

        function FuZhiQt() {
            var deptCode = '<%=Request["deptCode"]%>';
            var djlx = $("#ddlBill").val();
            djlx = djlx.substr(0, djlx.length - djlx.indexOf("|") - 1);
            var returnValue = window.showModalDialog('deptList_fuzhi.aspx?deptCode=' + deptCode + '&djlx=' + djlx, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:350px;status:no;scroll:no');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <div runat="server" id="divdjlx" style="float:left">
                        单据类型:
                    <asp:DropDownList runat="server" ID="ddlBill" AutoPostBack="True" OnSelectedIndexChanged="ddlBill_SelectedIndexChanged">
                    </asp:DropDownList>
                    </div>

                    &nbsp;                  
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="保 存" OnClientClick="return confirm('保存预算科目设置,会清除财务科目对照关系,是否继续？');"
                        OnClick="Button1_Click" />
                    <input type="button" id='fuzhi' class="baseButton" value="复制其他部门" onclick="FuZhiQt()" />
                    <asp:Button ID="btn_Export" runat="server" Text="导出Excel" OnClick="btn_Export_Click" CssClass="baseButton" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                    操作说明：选中部门需要启用的预算科目; 选择完毕后,点击保存。
                    <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" PageSize="17" Width="1250" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn HeaderText="启用" HeaderStyle-Width="40" ItemStyle-Width="40">
                                    <ItemTemplate>
                                        &nbsp;&nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="yskmcode" HeaderText="科目编码" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmcode" HeaderText="科目代码" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmname" HeaderText="科目名称" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="借方科目" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_jfkmcode1" runat="server" Text='<%#Bind("jfkmcode1name") %>' CssClass="txtright"></asp:TextBox>
                                        <input type="button" class="baseButton" id="btn_jf1" onclick="selectcwkm('../select/selectcwkmframe.aspx', this);" value="选择" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="贷方会计科目" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_dfkmcode1" runat="server" Text='<%#Bind("dfkmcode1name") %>'
                                            CssClass="txtright"></asp:TextBox>
                                        <input type="button" id="btn_df1" class="baseButton" onclick="selectcwkm('../select/selectcwkmframe.aspx', this);" value="选择" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbeTotalAmount" runat="server" Text="0"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="借方会计科目2">
                                    <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Width="200" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="hiddenbill" Width="200" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_jfkmcode2" runat="server" Text='<%#Bind("jfkmcode2name") %>'
                                            CssClass="txtright"></asp:TextBox><input type="button" class="baseButton" id="btn_jf2" value="选择" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="myGridItem"
                                    HeaderText="贷方科目2">
                                    <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Width="200" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="hiddenbill" Width="200" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_dfkmcode2" runat="server" Text='<%#Bind("dfkmcode2name") %>'
                                            CssClass="txtright"></asp:TextBox>
                                        <input type="button" id="btn_df2" class="baseButton" value="选择" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="tbsm" HeaderText="填报说明" HeaderStyle-Width="300" ItemStyle-Width="300">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tblx" HeaderText="填报类型" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="isend" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"></asp:BoundColumn>
                                <asp:BoundColumn DataField="isCheck" ></asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr style="display: none;">
                <td style="height: 30px">
                    <asp:Label ID="lblDeptCode" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
