<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qtbxSetDept.aspx.cs" Inherits="webBill_bxgl_qtbxSetDept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>其他报销费用科目单位</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:480px;status:no;scroll:no');
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("btn_sele").click();
            }
        }
        
        function openSelectZg(deptCode)
        {
            var returnValue=window.showModalDialog('selctZgUser.aspx?deptCode='+deptCode+'', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
        }
        
        function openSelectLd(deptCode)
        {
            var returnValue=window.showModalDialog('selctLdUser.aspx?deptCode='+deptCode+'', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
        }
    </script>

    <script language="javascript" type="text/javascript"> 
    function selectdeptAbc(mxGuid)
    {
        window.showModalDialog('qtdeptFrame.aspx?mxGuid='+mxGuid, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
       
       document.getElementById("btnRefresh2").click();
       
       
       window.location.reload();
        
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr style="display:none;">
                <td style="height: 27px" id="resultButton" runat="server">
                    &nbsp;<asp:Button ID="btn_add" runat="server" Text="增 加" CssClass="baseButton" OnClick="btn_add_Click1" />
                    &nbsp;<asp:Button ID="btn_dele" runat="server" Text="删 除" CssClass="baseButton" OnClick="btn_dele_Click" />
                    &nbsp;<asp:Button ID="Button2" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button2_Click" />&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="刷新数据" CssClass="baseButton" OnClick="Button1_Click" />
                </td>
            </tr>
            <tr style="display:none;">
                <td id="resultList" runat="server">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Width="100%" PageSize="17">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="mxGuid" HeaderText="mxGuid" Visible="False">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptname" HeaderText="部门">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn> <asp:TemplateColumn HeaderText="金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("je") %>' Width="131px"
                                        CssClass="rightBox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </td>
            </tr>
            <tr style="display:none;">
                <td style="height: 30px">
                    &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton><asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                    第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged"></asp:DropDownList>页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                    <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                        runat="server"></asp:Label>条<asp:Button ID="btnRefresh2" runat="server" Text="刷新数据" CssClass="baseButton" OnClick="btnRefresh2_Click" /></td>
            </tr>
            <tr>
                <td id="selectTree" runat="server">
    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True">
    </asp:TreeView>
                </td>
            </tr>
            <tr><td id="selectButton" runat="server" style="text-align:center;">
                <asp:Button ID="Button3" runat="server" CssClass="baseButton" Text="确 定" OnClick="Button1_Click1" />&nbsp;<asp:Button
                    ID="Button4" runat="server" CssClass="baseButton" Text="取 消" OnClick="Button3_Click" /></td></tr>
        </table>
    </form>
</body>
</html>
