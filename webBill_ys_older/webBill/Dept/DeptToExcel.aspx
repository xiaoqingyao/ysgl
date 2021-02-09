<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptToExcel.aspx.cs" Inherits="webBill_Dept_DeptToExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <title></title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="导出EXCEL" CssClass="baseButton" 
            onclick="Button1_Click" /> &nbsp;
        <input id="Button2" type="button" value="关闭" class="baseButton" onclick="javascript:window.close();"  />
    </div>
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            onrowdatabound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="deptcode" HeaderText="部门编号" >
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="deptname" HeaderText="部门名称" >
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="sjdeptcode" HeaderText="上级部门" >
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
