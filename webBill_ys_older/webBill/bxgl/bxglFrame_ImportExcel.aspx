<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxglFrame_ImportExcel.aspx.cs"
    Inherits="webBill_bxgl_bxglFrame_ImportExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>一般报销excel导入</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <style type="text/css">
        .style1 {
            width: 74px;
            text-align: right;
        }

        .style2 {
            text-align: right;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#tabData tr:eq(0)").addClass("myGridHeader");
            $("#txt_billDate").datepicker();
        });

        function exportExcel2() {
            var deptcode = $("#ddl_dept").val();

            var dydj = '<%=Request["dydj"]%>';

            var url = "ExportYskmExcel.aspx?dept=" + deptcode + "&dydj=" + dydj;
            // showModalDialog(url, "newwindow", "center:yes;dialogHeight:240px;dialogWidth:860px;status:no;scroll:yes");
            window.open(url, "newwindow2", 'height=100,width=400,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <label id="msg" style="color: Red">
                【友情提示】：请导入标准Excel文件且文档的列必须与模板完全一致，否则将导入不成功！</label>
            <table class="myTable" width="98%">
                <tr>
                    <td class="style1">部门：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_dept" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">日期：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_billDate" runat="server"></asp:TextBox>
                    </td>
                    <td class="style1">导出模板：
                    </td>
                    <td>
                        <input class="baseButton" value="导出Excel模板" type="button" onclick="exportExcel2();" />
                        <%-- <a href="bxglImportExcel.xls" class="baseButton" target="_blank" style="display: block; text-align: center; width: 45px">导出</a>--%>
                    </td>

                    <%-- <td class="style1">
                    预算过程：
                </td>
                <td>
                    <asp:DropDownList ID="ddlNd" runat="server">
                    </asp:DropDownList>
                </td>--%>
                </tr>
                <tr>
                    <td class="style1">导入：
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="baseButton" Width="85%" />
                        <asp:Button ID="btnImport" runat="server" Text="导 入" CssClass="baseButton" OnClick="btnImport_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 400px; width: 100%; overflow: auto;">
                <table class="myTable" width="100%" id="tabData">
                    <tr>
                        <th>部门
                        </th>
                        <th>预算科目
                        </th>
                        <th>决算金额
                        </th>
                        <th>核算部门
                        </th>
                        <th>核算金额
                        </th>
                    </tr>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>[<%#Eval("deptCode") %>]<%#Eval("deptName") %>
                                    <asp:HiddenField ID="hfDeptCode" runat="server" Value='<%#Eval("deptCode") %>' />
                                </td>
                                <td>[<%#Eval("fykmCode") %>]<%#Eval("fykmName")%>
                                    <asp:HiddenField ID="hfFykmCode" runat="server" Value='<%#Eval("fykmCode") %>' />
                                </td>
                                <td class="myGridItemRight">
                                    <%#Eval("bxje", "{0:c}")%>
                                    <asp:HiddenField ID="hfBxje" runat="server" Value='<%#Eval("bxje") %>' />
                                </td>
                                <td>[<%#Eval("hsDeptCode") %>]<%#Eval("hsDeptName")%>
                                    <asp:HiddenField ID="hfHsDeptCode" runat="server" Value='<%#Eval("hsDeptCode") %>' />
                                </td>
                                <td class="myGridItemRight">
                                    <%#Eval("hsje", "{0:c}")%>
                                    <asp:HiddenField ID="hfHsje" runat="server" Value='<%#Eval("hsje") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btn_zd"  CssClass="baseButton" runat="server" Text="制 单" OnClick="btn_zd_Click" />
            </div>
        </div>
    </form>
</body>
</html>
