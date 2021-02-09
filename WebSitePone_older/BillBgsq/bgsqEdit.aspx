<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bgsqEdit.aspx.cs" Inherits="BillBgsq_bgsqEdit" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>报告申请单编辑</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
    $(function(){
      $("#txt_content").css({"minHeight":"200px"});
      $("#txt_sm").css({"minHeight":"100px"});
    });
    
    function Check()
    {
    var cgbh=$("#txt_cgbh").val().length;
    if(cgbh==0)
    {
    alert("采购单号不能为空！");
    return false;
    }
    else
    {
       return true;
    }
    }
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
            <a data-icon="home" data-ajax="false" onclick="ConfirmReturn('../Index.aspx','单据未保存，确定要返回吗')">
                返回</a>
            <h1>
               报告申请单编辑</h1>
            <a href="bgsqList.aspx" data-role="button" class="ui-btn-right" data-icon="grid"
                data-ajax="false">单据列表</a>
        </div>
        <div data-role="content">
            <table>
                <tr>
                    <td style="width: 70px" class="tdEnenRight">
                        采购单号:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_cgbh" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdEnenRight">
                        申请日期:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_sj" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdEnenRight">
                        承办人:
                    </td>
                    <td>
                       
                       <asp:TextBox ID="txt_cbr" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdEnenRight">
                        预计费用:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_yjfy" runat="server" onkeyup='replaceNaNNum(this)'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdEnenRight">
                        部门:
                    </td>
                    <td>
                     <asp:TextBox ID="txt_dept" runat="server"  ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdEnenRight">
                        申请类别:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        内容:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txt_content" runat="server" TextMode="MultiLine" CssClass="mutiText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        说明:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txt_sm" runat="server" TextMode="MultiLine" CssClass="mutiText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="保存" data-inline="true" OnClick="btnSave_Click"
                            OnClientClick="return Check()" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hffj" runat="server" />
            <asp:HiddenField ID="hftzr" runat="server" />
        </div>
        <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>
</body>
</html>
