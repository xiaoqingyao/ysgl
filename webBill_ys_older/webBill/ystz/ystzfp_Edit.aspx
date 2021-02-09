<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystzfp_Edit.aspx.cs" Inherits="webBill_ystz_ystzfp_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


</head>
<body>
    <form id="form1" runat="server">
    <div>
           <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <asp:Button ID="Button1" runat="server" Text="确 定" CssClass="baseButton" 
                        onclick="Button1_Click" />&nbsp;&nbsp;
                    <input type="button" value ="取 消" class="baseButton" onclick="javascript:window.close();" />
                    &nbsp;&nbsp;
                    </td>
            </tr>
            <tr>
                <td>
                    
                    <table class="myTable">

                        <tr>
                            <td class="tableBg2">
                                一月:</td>
                            <td>
                            <asp:TextBox ID="tb_01" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                二月:</td>
                            <td>
                            <asp:TextBox ID="tb_02" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                三月:</td>
                            <td>
                            <asp:TextBox ID="tb_03" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                四月:</td>
                            <td>
                            <asp:TextBox ID="tb_04" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                五月:</td>
                            <td>
                            <asp:TextBox ID="tb_05" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                六月:</td>
                            <td>
                            <asp:TextBox ID="tb_06" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                七月:</td>
                            <td>
                            <asp:TextBox ID="tb_07" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                八月:</td>
                            <td>
                            <asp:TextBox ID="tb_08" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                九月:</td>
                            <td>
                            <asp:TextBox ID="tb_09" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十月:</td>
                            <td>
                            <asp:TextBox ID="tb_10" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十一月:</td>
                            <td>
                            <asp:TextBox ID="tb_11" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十二月:</td>
                            <td>
                                <asp:TextBox ID="tb_12" CssClass="baseText" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>

            </table>
    </div>
    </form>
</body>
</html>
