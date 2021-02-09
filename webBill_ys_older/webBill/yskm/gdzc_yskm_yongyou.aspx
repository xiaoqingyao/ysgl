<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gdzc_yskm_yongyou.aspx.cs" Inherits="webBill_yskm_gdzc_yskm_yongyou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>科目对照表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">


        function AddCheck() {   
          var txtDept = $("#txtDept").val();
            var nType = $("#nType").val();
            var txtckCode = $("#ddlCk").val();
            var txtYskm = $("#txtYskm").val();
              
            if (txtDept.length == 0) {
                alert("部门不能为空");
                return false;
            }
           else if (nType.length == 0) {
                alert("请选择对应类型");
                return false;
            }
            else if (txtckCode.length == 0) {
                alert("仓库信息不能为空");
                return false;
            }
            else if (txtYskm.length == 0) {
                alert("请选择对应本系统的预算科目");
                return false;
            }
            else {
                return true;
            }
        }


        function selectyskm(obj) {
            var str = window.showModalDialog("../bxgl/YskmSelectNew.aspx?deptCode=" + $("#hfDept").val() + "&dydj=01&flag=s", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined) {
                var json = $.parseJSON(str);
                obj.parentNode.getElementsByTagName('input')[0].value = json[0].Yscode;
            }
        }

        function selectyskm1() {
            var str = window.showModalDialog("../bxgl/YskmSelectNew.aspx?deptCode=" + $("#hfDept").val() + "&dydj=01&flag=s", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined) {
                var json = $.parseJSON(str);
                $("#txtYskm").val(json[0].Yscode);
            }
        }
        
        
        $(function(){
             //部门选择
            $("#txtDept").autocomplete({
                source: availableTagsDept
            });
        });
 
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfDept" runat="server" />
    <div class="baseDiv">
        <input type="button" class="baseButton" value="返回制单页" onclick="javascript:window.location.href='../makebxd/weicaiyaopin_ckd.aspx';" />类型：
        <asp:DropDownList ID="nType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
            AutoPostBack="true">  
            <asp:ListItem Text="耗材" Value="1">卫生材料、药品</asp:ListItem>
           <%-- <asp:ListItem Text="固定资产" Value="2">固定资产</asp:ListItem>--%>
        </asp:DropDownList>
        <div id="topAdd" style="margin-top: 5px;">
            <asp:Panel ID="Panel1" runat="server" GroupingText="添加">
                <table>
                    <tr>
                    <td>管理部门：</td>
                          <td> 
                              <asp:TextBox ID="txtDept" runat="server"></asp:TextBox></td>
                        <td>
                            仓库：
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCk"></asp:DropDownList>
                        </td>
                        <td>
                            预算科目：
                        </td>
                        <td>
                            <asp:TextBox ID="txtYskm" runat="server" CssClass="baseTextReadOnly" onkeyup="javascript:this.value='';"></asp:TextBox>
                            <input type="button" class="baseButton" onclick="selectyskm1();" value="选" />
                        </td>
                        <td>
                            <span style="width: 30px;"></span>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" CssClass="baseButton" Text="添加" OnClientClick="return  AddCheck();"
                                OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div id="divgrid" style="margin-top: 5px;">
            <asp:Panel ID="Panel2" runat="server" GroupingText="修改">
                <div style="margin: 5px;">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="显示未对应" AutoPostBack="true" 
                        oncheckedchanged="CheckBox1_CheckedChanged"  Checked="true" />
                    <asp:Button ID="btnSaveAll" runat="server" Text="保存修改" OnClick="btnSaveAll_Click"
                        CssClass="baseButton" />
                </div>
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word;" Width="750"
                    ShowFooter="false">
                    <Columns>
                        <asp:BoundColumn DataField="nType" HeaderText="对应类型">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader hiddenbill" />
                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                Wrap="true" CssClass="myGridItem hiddenbill" />
                            <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem hiddenbill" />
                        </asp:BoundColumn>
                          <asp:BoundColumn DataField="typeName" HeaderText="类型" ItemStyle-Width="150" HeaderStyle-Width="150">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                Wrap="true" CssClass="myGridItem " />
                            <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                        </asp:BoundColumn>
                         <asp:BoundColumn DataField="deptCode" HeaderText="部门" ItemStyle-Width="200" HeaderStyle-Width="200">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                Wrap="true" CssClass="myGridItem " />
                            <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ckCode" HeaderText="仓库" ItemStyle-Width="200" HeaderStyle-Width="200">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                Wrap="true" CssClass="myGridItem " />
                            <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-Width="200" HeaderStyle-Width="200">
                            <HeaderTemplate>
                                预算科目</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtYskm" runat="server" Text='<%#Eval("yskmcode") %>' Width="70%"
                                    CssClass="baseTextReadOnly" ></asp:TextBox>
                                <input type="button" class="baseButton" id="btnChange" onclick="selectyskm(this);"
                                    value="选" />
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                Wrap="true" CssClass="myGridItem" />
                            <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
                </asp:DataGrid></asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
