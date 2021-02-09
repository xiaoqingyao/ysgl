<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszjAdd_bykm.aspx.cs" Inherits="ysgl_yszjAdd_bykm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算批量追加</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="ajaxfileupload.js"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        var gkbm_cache = {};
        $(function () {
            //gudingbiaotounew($("#DataGrid1"), $(window).height() - 100);
           
            $("#txt_yskm").autocomplete({
                source: availableTagsDeptkm
            });

        });


    

        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '0.00';
                alert("必须用阿拉伯数字表示！");
            };
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

                            <td>选择预算过程
                            <asp:DropDownList runat="server" ID="ddlnian" AutoPostBack="true" OnSelectedIndexChanged="ddlnianselectindexchanged"></asp:DropDownList>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />

                                <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />
                                <asp:Button ID="Button2" runat="server" Text="取 消" CssClass="baseButton" OnClick="Button2_Click" />
                                <asp:Label ID="Label1" runat="server" ForeColor="Red">填写追加金额</asp:Label>
                             
                                <label style="color: Red" id="lblmsg" runat="server">【友情提示】：只能选择状态为“已结束”和“进行中”预算的预算过程；</label>
                                <asp:HiddenField ID="hf_page" runat="server" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; display: none">说明:
                            <asp:TextBox ID="txt_sm" TextMode="MultiLine" Height="30" runat="server" Width="450px"></asp:TextBox>
                            </td>
                            <td>
                                <div style="border-bottom: 1px dashed #CDCDCD; height: 20px;">
                                    部门：<asp:DropDownList runat="server" ID="LaDept">
                                    </asp:DropDownList>
                                    科目：
                                      <input type='text' id='txt_yskm' runat="server" />
                                    预算类型：<asp:DropDownList
                                        runat="server" ID="ddlYsType">
                                    </asp:DropDownList>

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
                                <asp:HiddenField ID="hidbillname" runat="server" />
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
                        <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" CellPadding="3"  Width="1000px"
                            PageSize="99999" CssClass="myGrid"  OnItemDataBound="DataGrid1_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择">
                                    <ItemTemplate>
                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill"
                                        Width="38px" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="gcbh" HeaderText="过程编号">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmmc" HeaderText="过程名称">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="kssj" HeaderText="预算开始日期" DataFormatString="{0:D}">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="jzsj" HeaderText="预算截止日期" DataFormatString="{0:D}">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fqr" HeaderText="过程发起人">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fqsj" HeaderText="过程发起时间">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="status" HeaderText="过程状态" Visible="False">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="statusName" HeaderText="过程状态">
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
                                            onkeyup="replaceNaN(this);"></asp:TextBox>
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
                            </Columns>
                        </asp:DataGrid>
                    
                    </div>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
