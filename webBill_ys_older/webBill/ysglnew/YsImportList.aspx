<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YsImportList.aspx.cs" Inherits="webBill_ysgl_YsImportList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算导入列表页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script type="text/javascript">
        function ImportExcel() {
            openinportexecl("YsImportExcel.aspx");
        }
        function openinportexecl(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:730px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("hdpostback").click();
            }
        }

        $(function() {
            $("#GridView1 tr:gt(0)").find("td:gt(0))").each(function(index, obj) {
                if (index % 17 == 0 || index % 17 == 2) {
                    //alert(index+":"+$(obj).html());
                    var td = $(this);
                    td.bind("click", function() {
                        if (td.find("input").size() > 0)
                            return false;
                        var td_text = td.html(); // 把原来单元格的值赋给一个变量 ;
                        td.html("");
                        var input = $("<input type='text' value='" + td_text + "'/>");
                        input.width(td.width() - 30);
                        input.css({ "border": "none" }).val(td_text).appendTo(td);
                        // 在向单元格中加入文本框后,就获得焦点并选择;
                        input.trigger("focus").trigger("select");
                        // 对input 的单击事件 不再触发单元格的单击事件 ;
                        input.click(function() {
                            return false;
                        })
                        // 响应键盘事件 ;
                        input.keyup(my_keyup = function(e) {
                            var keyCode = e.which
                            if (keyCode == 27) {
                                input.val(td_text);
                                td.html(td_text);
                            } else if (keyCode == 13) {
                                var new_val = input.val();
                                td.html(new_val);
                            }
                        }); // keyup end
                        input.blur(function() {
                            var code = "";
                            if ($.trim($(this).val()) == "") {
                                code = td_text;
                            } else {
                                code = $(this).val();
                            }
                            td.html(code);
                            var idex = td.parent().find("td").index(td);
                            var type = (idex == "1" ? "dept" : "yskm");
                            $.post("GetNameHandler.ashx", { "type": type, "code": code }, function(data, status) {
                                if (status == "success" && data != '-1') {
                                    td.next().html(data);
                                    if (td.parent().hasClass("errorrow")) {//如果之前就是红色的
                                        if (type == "dept" && td.parent().children().eq(4).html().indexOf("失败") == -1) {
                                            td.parent().removeClass("errorrow");
                                        } else if (type == "yskm" && td.parent().children().eq(2).html().indexOf("失败") == -1) {
                                            td.parent().removeClass("errorrow");
                                        } else { }
                                    }
                                } else {
                                    var aname = type == "yskm" ? "根据科目编号获取名称失败" : "根据部门编号获取名称失败";
                                    td.parent().addClass("errorrow");
                                    td.next().html(aname);
                                }
                            });

                        });
                    });
                }
            });
            gudingbiaotounew($("#GridView1"), 380);
            initMainTableClass("<%=GridView1.ClientID%>");
        });
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function check() {
            var flg = true;
            $("#GridView1 tr:gt(0)").each(function() {
                if ($(this).hasClass("errorrow")) {
                    flg = false;
                }
            });
            if (!flg) {
                alert("预算表中背景为红色的行数据不合法，可以直接单击对应的部门编号或科目编号进行编辑。");
            }
            return flg;
        }
    </script>

    <style type="text/css">
        .hidden
        {
            display: none;
        }
        .errorrow
        {
            background-color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="option" style="margin: 5px;">
            <%-- <asp:Button runat="server" ID="Button1" Text="Test" OnClick="hdpostback_Click" />--%>
            <input type="button" id="addAttachment" value="导入Excel" class="baseButton" onclick="ImportExcel();" />
            预算年份：
            <asp:DropDownList runat="server" ID="ddlnd">
            </asp:DropDownList>
            <asp:Button ID="btn_Submit" runat="server" Text="生成预算" CssClass="baseButton" OnClick="btn_Submit_Click"
                OnClientClick="return check();" />
            <asp:Button runat="server" ID="hdpostback" CssClass="hidden" OnClick="hdpostback_Click" />
            <asp:HiddenField ID="hfcode" runat="server" />
        </div>
        <div>
            <label style="color: Red; margin-left: 5px;">
                操作提示：请先单击“导入excel”导入编辑好的excel文档，然后检查数据无误后单击“生成预算”生成预算数；单击“部门编号”和“科目编号”列可以直接编辑。</label></div>
        <div id="data">
            <div id="header" style="overflow: hidden;">
            </div>
            <div style="position: relative; word-warp: break-word; word-break: break-all">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    ShowFooter="true" ShowHeader="true" OnRowDataBound="GridView1_OnRowDataBound"
                    Width="1900">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Container.DataItemIndex +1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="部门编号" DataField="deptcode" ItemStyle-Width="110" HeaderStyle-Width="110">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="部门名称" DataField="deptname" ItemStyle-Width="110" HeaderStyle-Width="110">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="科目编号" DataField="yskmcode" ItemStyle-Width="120" HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="科目名称" DataField="yskmmc" ItemStyle-Width="120" HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="1月" DataField="yi" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="2月" DataField="er" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="3月" DataField="san" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="4月" DataField="si" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="5月" DataField="wu" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="6月" DataField="liu" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="7月" DataField="qi" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="8月" DataField="ba" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="9月" DataField="jiu" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="10月" DataField="shi" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="11月" DataField="shiyi" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="12月" DataField="shier" DataFormatString="{0:N2}" ItemStyle-Width="120"
                            HeaderStyle-Width="120">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="合计" DataField="nian" DataFormatString="{0:N2}" ItemStyle-Width="140"
                            HeaderStyle-Width="140">
                            <HeaderStyle CssClass="myGridHeader" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                HorizontalAlign="Center" Wrap="true" />
                            <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Wrap="true" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
