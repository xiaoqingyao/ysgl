<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YksqdDetail.aspx.cs" Inherits="webBill_fysq_YksqdDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <base target="_self" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function clo() {
            window.close();
            // self.close();
        }

        $(function () {
            $("#txt_sqsj").datepicker();
            $("#txt_bmfzr_rq").datepicker();
            $("#txt_yfkzy_rq").datepicker();
            $("#txt_cwfzr_rq").datepicker();
            $("#txt_cwxz_rq").datepicker();
            $("#txt_dsz_rq").datepicker();

        });

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }

        .auto-style2 {
            width: 55px;
        }

        .auto-style3 {
            width: 58px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <table style="border-color: #000000"  width="95%"  border="1">
            <tr>
                <th    style="text-align:center; font-size:x-large; height: 30px;" colspan="7"><strong>用款申请单</strong>
                </th>
            </tr>
            <tr>
                <td  style="text-align:right"  colspan="7">
                  申请时间 <asp:TextBox ID="txt_sqsj" runat="server"></asp:TextBox>
                    <asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label>                </td>
            </tr>
                <tr>
                    <td>
                        申请类型
                    </td>
                    <td colspan="6">
                      
                        <asp:DropDownList ID="drp_sqlx" runat="server">
                            <asp:ListItem Value="预付款/分期付款/应付款/押金">预付款/分期付款/应付款/押金</asp:ListItem>
                            <asp:ListItem Value="付现款类">付现款类</asp:ListItem>
                            <asp:ListItem Value="合作返点类">合作返点类</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td>
                        用款部门
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ykdept" runat="server"></asp:TextBox><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                    <td class="auto-style2">
                        用款日期
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ykrq" runat="server"></asp:TextBox><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                    <td>
                        申请人
                    </td>
                    <td  colspan="2">
                        <asp:TextBox ID="txt_sqr" runat="server"></asp:TextBox><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        款项用途
                    </td>
                    <td  colspan="3" class="auto-style1">
                        <asp:TextBox ID="txt_kxyt" runat="server"></asp:TextBox><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                    <td class="auto-style1">
                        用款方式
                    </td>
                    <td colspan="2" class="auto-style1">
                        <asp:DropDownList ID="ddl_ykfs" runat="server">
                            <asp:ListItem Value="转账" >转账</asp:ListItem>
                            <asp:ListItem Value="支票">支票</asp:ListItem>
                             <asp:ListItem Value="现金" >现金</asp:ListItem>
                            <asp:ListItem Value="电汇">电汇</asp:ListItem>
                        </asp:DropDownList><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                        <%-- <asp:RadioButton   GroupName="rad_ykfs" ID="rad_zz"  Text="转账" runat="server">                           
                        </asp:RadioButton>
                          <asp:RadioButton   GroupName="rad_ykfs" ID="rad_zp"  Text="支票" runat="server">                           
                        </asp:RadioButton>
                         <asp:RadioButton   GroupName="rad_ykfs" ID="rad_xj"  Text="现金" runat="server">  
                                                      
                        </asp:RadioButton>
                         <asp:RadioButton   GroupName="rad_ykfs" ID="rad_dh"  Text="电汇" runat="server">  
                                                      
                        </asp:RadioButton>--%>

                    </td>
                </tr>
                <tr>
                    <td>款项金额(大写)</td>
                    <td  colspan="2">
                        <asp:TextBox ID="txt_kxje_dx" runat="server"></asp:TextBox><asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                    <td>
                        款项金额(小写)
                    </td>
                    <td  colspan="3">
                        ￥<asp:TextBox ID="txt_kxje_xx" runat="server"></asp:TextBox>元<asp:Label Text="*"  runat="server" ForeColor="Red"></asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td>
                        收款单位全称
                    </td>
                    <td  colspan="2">
                        <asp:TextBox ID="txt_skdw" runat="server"></asp:TextBox>
                    </td>
                    <td>开户行</td>
                    <td>
                        <asp:TextBox ID="txt_khh"  runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style3">账户</td>
                    <td>
                       <asp:TextBox runat="server" ID="txt_zh">

                       </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td  style="text-align:center">
                        部门负责人
                    </td>
                    <td  style="text-align:center">
                        应付款专员
                    </td>
                    <td  style="text-align:center" colspan="2">
                        财务部负责人意见
                    </td>
                    <td  style="text-align:center" colspan="2">
                        财务分管校长/区域总校长
                    </td>
                    <td style="text-align:center">
                        董事长
                    </td>
                </tr>
                <tr>
                    <td>
                        意见<asp:TextBox ID="txt_bmfzr_yj" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        意见<asp:TextBox ID="txt_yfkzy_yj" runat="server"></asp:TextBox>
                    </td>
                     <td  colspan="2">
                        意见
                         <asp:DropDownList ID="drp_cwfzr_yj" runat="server">
                             <asp:ListItem Value="预算内">预算内</asp:ListItem>
                                 <asp:ListItem Value="预算外">预算外</asp:ListItem>
                                 <asp:ListItem Value="超预算5%">超预算5%</asp:ListItem>
                         </asp:DropDownList>
                        
                    </td>
                     <td  colspan="2">
                        意见<asp:TextBox ID="txt_cwxz_yj" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        意见<asp:TextBox ID="txt_dsz_yj" runat="server"></asp:TextBox>
                    </td>
                </tr>
                  <tr>
                    <td>
                        签字<asp:TextBox ID="txt_bmfzr_qz" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        签字<asp:TextBox ID="txt_yfkzy_qz" runat="server"></asp:TextBox>
                    </td>
                     <td  colspan="2">
                        签字<asp:TextBox ID="txt_cwfzr_qz" runat="server"></asp:TextBox>
                    </td>
                     <td  colspan="2">
                        签字<asp:TextBox ID="txt_cwxz_qz" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        签字<asp:TextBox ID="txt_dsz_qz" runat="server"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td>
                        日期<asp:TextBox ID="txt_bmfzr_rq" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        日期<asp:TextBox ID="txt_yfkzy_rq" runat="server"></asp:TextBox>
                    </td>
                     <td  colspan="2">
                        日期<asp:TextBox ID="txt_cwfzr_rq" runat="server"></asp:TextBox>
                    </td>
                     <td  colspan="2">
                        日期<asp:TextBox ID="txt_cwxz_rq" runat="server"></asp:TextBox>
                    </td>
                     <td>
                        日期<asp:TextBox ID="txt_dsz_rq" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>附件</td>
                    <td>

                    </td>
                </tr>
               <tr>
                   <td  colspan="7">
                         <asp:Button ID="btn_save" Text="保存" runat="server" OnClick="btn_save_Click" />
                        <asp:Button ID="btn_close" runat="server" OnClientClick="clo()" Text="取消" />
                   </td>
               </tr>
        </table>
        </center>
    </form>
</body>
</html>
