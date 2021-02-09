<%@ Page Language="C#" AutoEventWireup="true" CodeFile="srlr.aspx.cs" Inherits="webBill_xsreport_srlr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预算内容</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="Text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="Text/javascript">
        function sum1()
        {
            var srys=document.getElementById("Lbsrys1").value;
            srys=Number(srys)+Number(document.getElementById("Lbsrys2").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys3").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys4").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys5").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys6").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys7").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys8").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys9").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys10").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys11").value);
            srys=Number(srys)+Number(document.getElementById("Lbsrys12").value);
            document.getElementById("<%=Lbsrys.ClientID %>").innerHTML=srys;
            var srsj=document.getElementById("Lbsrsj1").value;
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj2").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj3").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj4").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj5").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj6").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj7").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj8").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj9").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj10").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj11").value);
            srsj=Number(srsj)+Number(document.getElementById("Lbsrsj12").value);
            document.getElementById("<%=Lbsrsj.ClientID %>").innerHTML=srsj;
            var clys=document.getElementById("Lbclys1").value;
            clys=Number(clys)+Number(document.getElementById("Lbclys2").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys3").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys4").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys5").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys6").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys7").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys8").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys9").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys10").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys11").value);
            clys=Number(clys)+Number(document.getElementById("Lbclys12").value);
            document.getElementById("<%=Lbclys.ClientID %>").innerHTML=clys;
            var clsj=document.getElementById("Lbclsj1").value;
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj2").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj3").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj4").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj5").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj6").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj7").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj8").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj9").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj10").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj11").value);
            clsj=Number(clsj)+Number(document.getElementById("Lbclsj12").value);
            document.getElementById("<%=Lbclsj.ClientID %>").innerHTML=clsj;
        }
        window.onload=sum1;

    </script>
    
    </head>
<body >
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 28px"><asp:Label ID="Label2" runat="server" Text="年度"></asp:Label>
                    <asp:TextBox ID="TBnd" runat="server"></asp:TextBox>
                    <asp:Button ID="Button3" runat="server" Text="查 询" CssClass="baseButton" 
                        onclick="Button3_Click"  />
                    <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" 
                        onclick="Button1_Click"  />
            </tr>
            <tr>
                <td>
                    <table  border="0" cellpadding="0" cellspacing="0" class="myTable" width="90%">
                        <tr>
                            <td class="tableBg" width="20%">
                                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="tableBg" width="20%">预算收入</td>
                            <td class="tableBg" width="20%">实际收入</td>
                            <td class="tableBg" width="20%">预算产量</td>
                            <td class="tableBg" width="20%">实际产量</td>
                        </tr>
                        <tr>
                            <td class="tableBg">1月份</td>
                            <td><asp:TextBox ID="Lbsrys1" runat="server" Text="0" style=" text-align:right;"   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj1" runat="server" Text="0" style= " text-align:right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys1" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj1" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">2月份</td>
                            <td><asp:TextBox ID="Lbsrys2" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj2" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys2" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj2" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">3月份</td>
                            <td><asp:TextBox ID="Lbsrys3" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj3" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys3" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj3" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">4月份</td>
                            <td><asp:TextBox ID="Lbsrys4" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj4" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys4" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj4" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">5月份</td>
                            <td><asp:TextBox ID="Lbsrys5" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj5" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys5" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj5" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">6月份</td>
                            <td><asp:TextBox ID="Lbsrys6" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj6" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys6" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj6" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">7月份</td>
                            <td><asp:TextBox ID="Lbsrys7" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj7" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys7" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj7" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">8月份</td>
                            <td><asp:TextBox ID="Lbsrys8" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj8" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys8" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj8" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">9月份</td>
                            <td><asp:TextBox ID="Lbsrys9" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj9" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys9" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj9" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">10月份</td>
                            <td><asp:TextBox ID="Lbsrys10" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj10" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys10" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj10" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">11月份</td>
                            <td><asp:TextBox ID="Lbsrys11" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj11" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys11" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj11" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">12月份</td>
                            <td><asp:TextBox ID="Lbsrys12" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbsrsj12" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclys12" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                            <td><asp:TextBox ID="Lbclsj12" runat="server" Text="0" style= "TEXT-ALIGN:   right "   Width=95% onblur="sum1();"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">总年度</td>
                            <td class="tableBg">
                                <asp:Label ID="Lbsrys" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="tableBg">
                                <asp:Label ID="Lbsrsj" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="tableBg">
                                <asp:Label ID="Lbclys" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="tableBg">
                                <asp:Label ID="Lbclsj" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
