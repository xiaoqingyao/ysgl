<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoreUserList.aspx.cs" Inherits="webBill_select_MoreUserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
      function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                    Text="包含下级" />
                &nbsp; &nbsp;  编号：<asp:TextBox ID="txt_code" runat="server"></asp:TextBox>
                姓名：<asp:TextBox ID="txt_name" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="查  询"  CssClass="baseButton" 
                    onclick="Button1_Click" />   &nbsp; &nbsp;   &nbsp; &nbsp;
                <asp:Button ID="btn_select" runat="server" Text="确  定" CssClass="baseButton" OnClick="btn_select_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_cancel" runat="server" Text="取  消" CssClass="baseButton" OnClick="btn_cancel_Click" />
            </td>
        </tr>
        <tr>
            <td align="left">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="myGrid" Width="610px" AllowPaging="True" PageSize="8">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                            <HeaderTemplate>
                              <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                            </HeaderTemplate>
                                <HeaderStyle  CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" Width="45px" />
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
        <tr style="display:none">
        <td>
            <asp:TextBox ID="txt_se" runat="server"></asp:TextBox>
        </td>
        </tr>
    </table>
    </form>
</body>
</html>