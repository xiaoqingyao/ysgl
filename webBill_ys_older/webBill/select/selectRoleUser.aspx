<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectRoleUser.aspx.cs" Inherits="webBill_select_selectRoleUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择角色【人员】</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript">
    function enterstr()
    {
        var str= document.getElementById("txt_se").value;
        if(str=="")
        {
            var ss= document.getElementById("txt_se").value;
            if(ss=="")
            {
                alert("没有选择数据！");
            }
            else
            {
                window.returnValue=ss;
                window.close();
            }
        }
        else
        {
            window.returnValue=str;
            window.close();
        } 
    } 
    
    function setsel(str)
    { 
        document.getElementById("txt_se").value=str;
    }
    
    function selected(obj)
     {
        window.returnValue=obj;
        window.close();
     }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 23px">
                &nbsp; 审核模式：<asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="一般审核">一般审核[管理单位单据]</asp:ListItem>
                    <asp:ListItem Value="业务主管审核">业务主管审核[主管单位单据]</asp:ListItem>
                    <asp:ListItem Value="分管领导审核">分管领导审核[分管单位单据]</asp:ListItem>
                </asp:DropDownList>选择角色：<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                </asp:DropDownList>&nbsp; &nbsp;&nbsp;
                <asp:Button ID="btn_select" runat="server" Text="确定选择" CssClass="baseButton" OnClick="btn_select_Click" />
                &nbsp; 
                <asp:Button ID="btn_cancel" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_cancel_Click" />
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                <span style="color: #ff0000" id="showInfo" runat="server">指定角色和人员：审核管理单位单据！仅指定人员：审核所有单位单据！</span></td>
        </tr>
        <tr>
            <td align="left">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="myGrid" Width="610px" PageSize="8">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" Width="35px" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />  
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="usercode" HeaderText="人员编号">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="username" HeaderText="人员姓名">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="usergroup" HeaderText="角色">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userstatus" HeaderText="人员状态">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userdept" HeaderText="所在部门">
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
