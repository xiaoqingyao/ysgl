<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>行信全面预算</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link rel="apple-touch-icon" href="ic_launcher.png">
    <link href="js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <style>
        .menu {
            width: 100%;
            padding: 0px;
            margin: 0px 3% 0px;
        }

            .menu ul {
                list-style: none;
                padding: 0px;
                margin: 0px;
                width: 100%;
            }

                .menu ul li {
                    list-style: none;
                    margin: 5px;
                    padding: 5.5% 0;
                    width: 45.5%;
                    height: auto;
                    float: left;
                    display: block;
                    text-align: center;
                }

                    .menu ul li a {
                        color: #FFFFFF;
                        text-decoration: none;
                        text-align: center;
                    }

                    .menu ul li img {
                        display: inline-block;
                        max-width: 50%;
                    }

                    .menu ul li a span {
                        display: block;
                        text-align: center;
                        font-size: 14px;
                        line-height: 25px;
                        color: White;
                        font-weight: normal;
                        font-family: 微软雅黑;
                        text-shadow: none;
                    }

        .div_bubble {
            display: block;
            position: absolute;
            right: 0px;
            top: 0px;
            z-index: 999;
        }

            .div_bubble a {
                position: absolute;
                right: 0px;
                top: -2px;
                text-decoration: none;
            }

                .div_bubble a:hover {
                    color: Red;
                }

            .div_bubble span {
                right: 0px;
                top: -2px;
                color: #fff;
                display: block;
                position: absolute;
                filter: alpha(opacity=50);
                -moz-opacity: 0.6;
                opacity: 0.6;
                font: bold 14px/30px Verdana, Arial;
                cursor: hand;
                width: 32px;
                height: 32px;
                line-height: 32px;
                text-align: center;
            }

            .div_bubble a:hover span {
                text-decoration: none;
            }
    </style>

    <script>
        function objfocus(obj) {
            obj.focus();
        }

        $(function () {
            $("ul li").click(function () {
                var url = $(this).find("a").eq(0).attr("href");
                window.location.href = url;
            });

            $(".div_bubble a span").each(function () {
                if ($(this).text() == "0")
                    $(this).parentsUntil("div").hide();
                else {
                    $(this).click(function (event) {
                        event.stopPropagation();
                    });
                }
            });
        });
    </script>

</head>
<body>
   
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false"
        data-position="fixed">
        <%--data-fullscreen="true"--%>
        <div>
            <div data-role="header" data-position="fixed">
                <a href="Login.aspx?clear=1" data-role="button" data-icon="back" data-ajax="false">退出</a>
                <h1>我的主页</h1>
            </div>
            <div data-role="content">
                <div class="menu">
                    <ul>
                        <asp:Repeater ID="rptMenu" runat="server" OnItemDataBound="rptMenu_ItemDataBound">
                            <ItemTemplate>
                                <li style='<%#Eval("note0") %>'><a href='<%#Eval("menuUrl")%>' data-ajax="false">
                                    <img src='<%#Eval("menuIcon")%>' /><span><%#Eval("showName") %></span></a><asp:HiddenField ID="hfId" runat="server"
                                        Value='<%#Eval("menuid") %>' />
                                    <div class="div_bubble" style="display: block;" id="div_bub" runat="server">
                                        <a href='<%#Eval("menuUrl")%>'>
                                            <img src="images/metro/Shape2.png" class="opcity" />
                                            <span id="span_num" runat="server">0</span>
                                        </a>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <label runat="server" id="msg" style="color: Red; font-size: 13px;">
                </label>
            </div>
            <div data-role="footer" data-position="fixed" style="position: fixed; width: 100%; left: 0px; bottom: 0px;">
                <footer data-role="footer" id="footer">
                    <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
                </footer>
            </div>
        </div>
        <div id="divMenu" style="right: 0px; bottom: 50px; position: fixed;">
            <a href="#">
                <img src="images/toTop.jpg" border="0" /></a>
        </div>

        <script src="js/backtop.js" type="text/javascript"></script>

    </form>
</body>
</html>
