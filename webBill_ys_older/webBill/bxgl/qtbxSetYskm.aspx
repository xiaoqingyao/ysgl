<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qtbxSetYskm.aspx.cs" Inherits="webBill_bxgl_qtbxSetYskm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>一般报销费用科目项目设置</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    function selectdept(mxGuid)
    {
        var str=window.showModalDialog('deptFrame.aspx?mxGuid='+mxGuid, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
       
        document.getElementById("Button1").click();
        
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    &nbsp;<asp:Button ID="btn_dele" runat="server" Text="清 除" CssClass="baseButton" OnClick="btn_dele_Click" Visible="False" />
                    &nbsp;<asp:Button ID="Button2" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button2_Click" />&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="刷新数据" CssClass="baseButton" OnClick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Width="100%" PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
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
                            <asp:BoundColumn DataField="xmCode" HeaderText="mxGuid" Visible="False">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="xmName" HeaderText="核算项目">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn> <asp:TemplateColumn HeaderText="金额" Visible="false">
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
           
            <tr><td></td></tr>
        </table>
    </form>
</body>
</html>
