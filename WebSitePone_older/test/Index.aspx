<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <style>
        .div_mid
        {
            margin: 0px;
            width: 138px;
            height: 138px;
            display: block;
            float: left;
            padding: 3px;
            position: relative;
        }
        .div_small
        {
            margin: 0px;
            display: block;
            float: left;
            padding: 3px;
            position: relative;
        }
        .div_content
        {
            margin-top: 0px;
        }
        .div_bubble
        {
            display: block;
            position: absolute;
            right: 0px;
            top: 0px;
        }
        .div_bubble a
        {
            position: absolute;
            right: 0px;
            top: -2px;
            text-decoration: none;
        }
        .div_bubble a:hover
        {
            color: Red;
        }
        .div_bubble span
        {
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
        .div_bubble a:hover span
        {
            text-decoration: none;
        }
    </style>

    <script>
 function objfocus(obj) {
        obj.focus();
    }
    
    $(function(){
        $(".div_bubble a span").each(function()
        {
          //  alert($(this).text());
            $(this).click(function(event){
             event.stopPropagation();
            });
        });
            
         $("#div_mid").click(function(){
            
            
         });
        //$("a").attr("data-ajax",true);
    });
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false" data-position="fixed">
    <%--data-fullscreen="true"--%>
    <div>
        <div  data-role="header" data-position="fixed" >
         <a href="Login.aspx"  data-role="button" data-icon="back" data-ajax="false">退出</a>
            <h1>
                我的主页</h1>
        </div>
        <div data-role="content">
            <div class="div_content">
                <div class="div_mid">
                 <a href="bxd/ybbxEditMain.aspx" data-ajax="false"><img src="images/metro/ID-New.png" /></a>
                </div>
                <div class="div_mid">
                    <div>
                        <div class="div_bubble" style="display: block;" id="bub_ybbx_unCheck" runat="server">
                            <a href="workflow/ybbxAuditList.aspx">
                                <img src="images/metro/Shape2.png" />
                                <span id="span_ybbxd_unCheck" runat="server">2</span></a>
                        </div>
                        <a  href="workflow/ybbxAuditList.aspx" data-ajax="false">
                            <img src="images/metro/Mail-New.png" /></a>
                    </div>
                </div>
                <div class="div_mid">
                    <div>
                        <div class="div_bubble" style="display: block;" id="bub_ybbx_unSubmit" runat="server">
                            <a href="bxd/ybbxList.aspx">
                                <img src="images/metro/Shape2.png" />
                                <span id="span_ybbxd_unSubmit" runat="server">2</span></a>
                        </div>
                        <a href="bxd/ybbxList.aspx"  data-ajax="false" ><img src="images/metro/Forward.png" /></a>
                    </div>
                </div>
                
                
                <div class="div_mid">
                    <div class="div_small">
                        <div class="div_bubble" style="display: block;" id="Div1" runat="server">
                            <a href="http://www.jnhxsoft.com/">
                                <img src="images/metro/Shape2.png" />
                                <span>3</span></a>
                        </div>
                        <img src="images/metro/Message-Mail.png" />
                    </div>
                    <div class="div_small">
                        <div class="div_bubble" style="display: block;" id="Div2" runat="server">
                            <a href="#">
                                <img src="images/metro/Shape2.png" />
                                <span>200</span></a>
                        </div>
                        <img src="images/metro/Messages-Information-01.png" />
                    </div>
                    <div class="div_small">
                        <div class="div_bubble" style="display: block;" id="Div3" runat="server">
                            <a href="#">
                                <img src="images/metro/Shape2.png" />
                                <span>100</span></a>
                        </div>
                        <img src="images/metro/Messages-Information-02.png" />
                    </div>
                    <div class="div_small">
                        <div class="div_bubble" style="display: block;" id="Div4" runat="server">
                            <a href="#">
                                <img src="images/metro/Shape2.png" />
                                <span>2</span></a>
                        </div>
                        <img src="images/metro/Messages-Information-03.png" />
                    </div>
                </div>
               
                <div class="div_mid">
                    <img src="images/metro/Message-Edit.png" />
                </div>
                <div class="div_mid">
                    <img src="images/metro/Messages-Information.png" />
                </div>
                <div class="div_mid">
                    <div class="div_small">
                        <img src="images/metro/Message-Mail.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-01.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-02.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-03.png" />
                    </div>
                </div>
                <div class="div_mid">
                    <div class="div_small">
                        <img src="images/metro/Message-Mail.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-01.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-02.png" />
                    </div>
                    <div class="div_small">
                        <img src="images/metro/Messages-Information-03.png" />
                    </div>
                </div>
                <div class="div_mid">
                    <img src="images/metro/Bill.png" />
                </div>
            </div>
        </div>
        <div data-role="footer" data-position="fixed" style="position: fixed; width: 100%;
            left: 0px; bottom: 0px;">
              <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    <div id="divMenu"  style="right:0px;bottom:20px;position: fixed;" ><a href="#" > 
                                                                    <img src="images/toTop.jpg" border=0/></a></div>
                                                                    <script src="js/backtop.js" type="text/javascript"></script>
    </form>
</body>
</html>
