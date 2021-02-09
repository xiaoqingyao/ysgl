<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sp_lscgList.aspx.cs" Inherits="webBill_fysq_sp_lscgList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新 
            
            var returnValue=window.showModalDialog(openUrl , 'newwindow', 'center:yes;dialogHeight:450px;dialogWidth:843px;status:no;scroll:yes');
            
            if(returnValue==undefined||returnValue=="")
            {}
            else
            {
                document.getElementById("btn_cx").click();
            }
        } 
        function openSplc(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        
        function openLookShgc(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:450px;dialogWidth:843px;status:no;scroll:yes');
        }

        $(function() {
           $('#TextBox5').datepicker({  
               dateFormat: 'yy-mm-dd', 
               clearText:'清除',
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
    <asp:Button ID="btn_summit" runat="server" Text="Button"  
            onclick="btn_summit_Click" style="display:none"/>
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
    
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td>
                    &nbsp;<asp:Button ID="btn_edit" runat="server" Text="审核通过" CssClass="baseButton" OnClick="btn_edit_Click" />
                    <asp:Button ID="btn_tj" runat="server" Text="审核退回" CssClass="baseButton" OnClick="btn_tj_Click" />&nbsp;<asp:Button ID="Button1" runat="server" Text="审核过程" CssClass="baseButton" OnClick="Button1_Click" />
                    <asp:Button ID="Button2" runat="server" CssClass="baseButton" OnClick="Button2_Click"
                        Text="详细信息" />
                    <input type="button" value="查 询" class="baseButton" id="btn_find"/>
                    <asp:Button ID="Button4" runat="server" CssClass="baseButton" OnClick="Button4_Click"
                        Text="打印预览" />
                    <asp:Button ID="Button3" runat="server" CssClass="baseButton" OnClick="Button3_Click"
                        Text="历史单据" /></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="17" Width="842px" OnItemDataBound="myGrid_ItemDataBound">
                        <ItemStyle HorizontalAlign="Center" />
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
                            <asp:BoundColumn DataField="billCode" HeaderText="billCode" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="审核意见">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="226px"></asp:TextBox>
                                   
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn><asp:BoundColumn DataField="cgDept" HeaderText="采购单位">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="承办人">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cglb" HeaderText="申请类别">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" HeaderText="stepID_ID" Visible="False">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billje" HeaderText="单据金额">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sm" HeaderText="原因说明">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID" HeaderText="单据状态">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
                    </asp:DataGrid></td>
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
            <tr>
                <td style="height: 30px">
                    审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>
