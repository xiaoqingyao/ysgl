<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sp_cgzjList.aspx.cs" Inherits="webBill_cgzj_sp_cgzjList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    
        <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("Button4").click();
            }
        }
        function openLookSpStep(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        $(function() {
            $('#TextBox5').datepicker({  
               dateFormat: 'yy-mm-dd',  //日期格式，自己设置
               clearText:'清除',//下面的就不用详细写注释了吧，呵呵，都是些文本设置
               closeText:'关闭',
               currentText:' ',
               monthNames:['1月','2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月']
           });  
            $('#dialog').dialog({
                autoOpen: false,
                width: 400,
                buttons: {
                    "确定": function() {
                        $(this).dialog("close");
                        $("#btn_summit").click();
                    },
                    "取消": function() {
                        $(this).dialog("close");
                    }
                }
            });
            $("#btn_find").click(function() {
                $("#dialog").parent().appendTo($("form:first")); 
                $('#dialog').dialog('open');
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="dialog" title="查询" style="display:none">
        <table>
        <tr>
        <td>
            所属部门:
        </td>
        <td>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </td>
        <td>
            报销人:
        </td>
        <td>
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        </td>
        </tr>
        <tr>
        <td>
            金额:
        </td>
        <td>
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        </td>
        <td>
            日期:
        </td>
        <td>
            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        </td>
        </tr>
        </table>
    </div>
            <asp:Button ID="btn_summit" runat="server" Text="Button"  
            onclick="btn_summit_Click" style="display:none"/>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <asp:Button ID="Button1" runat="server" Text="审核通过" CssClass="baseButton" OnClick="Button1_Click" />&nbsp;<asp:Button
                        ID="Button2" runat="server" Text="审核退回" CssClass="baseButton" OnClick="Button2_Click" />
                    <asp:Button ID="Button3" runat="server" Text="详细信息" CssClass="baseButton" OnClick="Button3_Click1" />
                    <asp:Button ID="Button5" runat="server" CssClass="baseButton" OnClick="Button5_Click"
                        Text="审核过程" />&nbsp;

                    <input type="button" value="查 询" class="baseButton" id="btn_find"/>
                    <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                        Text="打印预览" />
                    <asp:Button ID="Button7" runat="server" CssClass="baseButton" OnClick="Button3_Click"
                        Text="历史单据" /></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                           <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" HeaderText="报销单号" Visible='False'>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="审核意见">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="216px"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="报销人">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:F2}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID" HeaderText="状态">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" HeaderText="status" Visible="False"></asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="sfgf" HeaderText="是否给付">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>--%>
                            <asp:BoundColumn DataField="bxzy" HeaderText="摘要">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td style="height: 30px">
                    &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                    第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                    </asp:DropDownList>页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                    <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                        runat="server"></asp:Label>条</td>
            </tr>
            <tr style="display:none;">
                <td style="height: 30px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="height: 30px">
                    审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>