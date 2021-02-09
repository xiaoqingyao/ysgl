<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupList.aspx.cs" Inherits="workGroup_groupList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");

           });
     function openDetail(openUrl) {
         var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:440px;status:no;scroll:no');
         if (returnValue == undefined || returnValue == "") {
             return false;
         }
         else {
             document.getElementById("btn_sele").click();
         }
     }


     function Edit() {
         var Rows = $(".highlight");
         if (Rows.html() == undefined) {
             alert("请先选中要操作的条目！");
         }
         else {
             var checkrow = $(".highlight");
             if (checkrow.val() == undefined) {
                 alert("请先选择行");
                 return;
             }
             var strGroupID = checkrow.find("td")[1].innerHTML;
             openDetail("WorkGroupDetails.aspx?type=edit&groupID=" + strGroupID);

         }
     }

     function Delete() {
         var Rows = $(".highlight");
         if (Rows.html() == undefined) {
             alert("请先选中要操作的条目！");
             return false;
         }
         else if (confirm('使用中的角色禁止删除,是否继续？')) {
             var checkrow = $(".highlight");
             if (checkrow.val() == undefined) {
                 alert("请先选择行");
                 return false;;
             }
             else {
                 return true;
             }

         }
         else {
             return false;
         }
     }
     function gudingbiaotounew(obj, height) {
         var gvn = obj.clone(true).removeAttr("id");
         $(gvn).find("tr:not(:first)").remove();
         $(gvn).css("margin-bottom", "0px");
         obj.css("margin", "0 0 0 0");
         obj.before(gvn);
         obj.find("tr:first").remove();
         obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
     }

     function initWindowHW() {
         //给隐藏域设置窗口的高度
         $("#hdwindowheight").val($(window).height());
         //给gridview表格外部的div设置宽度  宽度为页面宽度
         $("#divgrid").css("width", ($(window).width() - 5));
     }
     $(function () {
         initWindowHW();
     });


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <input type="button" value="增 加" class="baseButton" onclick="openDetail('WorkGroupDetails.aspx?type=add');" />
                    &nbsp;
                <input type="button" id="btn_edit" value="修 改" class="baseButton" onclick="Edit()" />
                    &nbsp;<asp:Button ID="btn_del" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return Delete();"
                        OnClick="btn_del_Click" />
                    &nbsp;<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="700px">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择" ItemStyle-Width="38">
                                    <ItemTemplate>
                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="38px" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="groupID" HeaderText="角色序号" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="groupName" HeaderText="角色名称" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="gTypeName" HeaderText="角色类型" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="gType" HeaderText="角色类型" Visible="false" HeaderStyle-Width="200px" ItemStyle-Width="200px"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 30px">
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
