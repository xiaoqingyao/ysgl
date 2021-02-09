<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsList.aspx.cs" Inherits="webBill_DeskMessage_NewsList" %>

<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/GridViewHelp.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function() {
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
        });
        function editClick() {
            var code = GetRowCode();
            if (code == "") {
                return false;
            }
            else {
                $("#hf_code").val(code);
                return true;
            }
        }
    </script>
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:5px; margin-left:5px;">
        <asp:Button ID="btn_insert" runat="server" Text="新 增" CssClass="baseButton" 
            onclick="btn_insert_Click" />&nbsp;
        <asp:Button ID="btn_edit" runat="server" Text="修 改" CssClass="baseButton" 
            OnClientClick="return editClick();" onclick="btn_edit_Click" />&nbsp;
        <input type="button" value="删 除" class="baseButton" id="btn_delete" onclick="DataDelete('message');" />&nbsp;
        <asp:Button ID="btn_find" runat="server" Text="查 询" CssClass="baseButton" />&nbsp;
    </div>
    <div style="margin-top:5px; margin-left:5px;">
        <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" 
            AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="code" HeaderText="信息编号" >
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="标题" >
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField DataField="userCode" HeaderText="发布人" >
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField DataField="messageDate" HeaderText="发布时间" 
                    DataFormatString="{0:D}" >
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField DataField="memo" HeaderText="备注" >
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hf_code" runat="server" />
    </div>
    <div style="margin-top:5px; margin-left:5px;">
        <cc1:PaginationToGV ID="PaginationToGV1" runat="server" 
            ongvbind="PaginationToGV1_GvBind"  />
    </div>
    </form>
</body>
</html>
