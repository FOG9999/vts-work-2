<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Utilities" %>
<html>
<head>
    <title>:. Đăng nhập hệ thống :.</title>
	<meta charset="utf8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
	<meta name="apple-mobile-web-app-capable" content="yes" />
	<meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
	<link rel="stylesheet" href="<%=ResolveUrl("~") %>css/bootstrap.min.css">
	<link rel="stylesheet" href="<%=ResolveUrl("~") %>css/bootstrap-responsive.min.css">
	<link rel="stylesheet" href="<%=ResolveUrl("~") %>css/style.css">
	<link rel="stylesheet" href="<%=ResolveUrl("~") %>css/themes.css">
    <link rel="stylesheet" href="<%=ResolveUrl("~") %>css/qldt.css">
    <link rel="stylesheet" href="<%=ResolveUrl("~") %>css/new.css">
    <link rel="stylesheet" href="<%=ResolveUrl("~") %>css/capcha.css" type="text/css" />
	<!-- jQuery -->
	<script src="<%=ResolveUrl("~") %>js/jquery.min.js"></script>
    <script src="<%=ResolveUrl("~") %>js/functions.js"></script>
	<!-- Bootstrap -->
	<script src="<%=ResolveUrl("~") %>js/bootstrap.min.js"></script>
	<script src="<%=ResolveUrl("~") %>js/eakroko.js"></script>
	<!-- Favicon -->
	<link rel="shortcut icon" href="<%=ResolveUrl("~") %>images/document-icon.png" />
    <script type="text/javascript">
        //CreateNew_Token();
        //<add key="key_encript" value="kntc" />
    </script>
</head>
<body class='login'>  
    <header>
        <div class="container">
            <a href="#" class="logo"><img src="/Images/quochuy.png" alt=""></a>
                <h1>VĂN PHÒNG ĐOÀN ĐẠI BIỂU QUỐC HỘI VÀ HỘI ĐỒNG NHÂN DÂN TỈNH <%=AppConfig.TEN_DIA_PHUONG.ToUpper()%>
                    <%--<span>BAN DÂN NGUYỆN</span>--%>
                </h1>
                
        </div>
    </header>

          
    <div id="main" class="nomargin">
        <div class="left_login">
        <img src="/Images/img_login.png">
    </div>
    <div class="LoginSection">
                    <div class="LoginFormSection">
                        <div class="LoginFormTopS">
                            <div class="LoginFormBtmS">
                                <div class="LoginFormLoopS">
                                    <div class="LoginFormPenuS"> 
                            <form id="form" method="post" class="nomagin" onsubmit="return CheckForm()">
                                
                                <fieldset>
                                    <legend>Đăng nhập</legend>
                                    <label class="LabelStyle">Tên đăng nhập</label>
                                    <p class="BgInputTextStyle Sprite1">
                                        <span class="UserTempStyle Sprite1">
                                            <input type="text" name='cUser' id="cUser" value="<%=ViewData["user"] %>" autocomplete="off"  class='input-block-level'>                                                    
                                        </span>
                                    </p>
                                    <label class="LabelStyle">Mật khẩu</label>
                                    <p class="BgInputTextStyle Sprite1">
                                        <span class="PwdTempStyle Sprite1">
                                            <input type="password" value="<%=ViewData["pass"] %>" name="cPass" id="cPass" class='input-block-level'>
                                        </span>
                                    </p>    
                                    <% if (ViewData["login_fail"].ToString() == "fail")
                                        { %>
                                    <p class="BgInputTextStyle Sprite1">
                                        <span class="PwdTempStyle Sprite1 captcha">
                                            <img id="captchaImage" class="captchaImage" src="<%=ViewData["captchaImage"] %>">
                                            <input type="hidden" name="codecapcha_hidden" id="codecapcha_hidden" value="<%=ViewData["captcha_hidden"] %>" />   
                                            <input type="text" autocomplete="off" name="codecapcha" id="codecapcha" placeholder="Mã bảo mật" />   
                                            <a class="b-green" href="javascript:void(0)" onclick="RefreshCapcha();" title="Chọn mã khác"><i class="icon-refresh"></i></a>  
                                        </span>                   
                                    </p>
                                    <% } %>
                                    <% //if (ViewData["login_fail"].ToString()=="fail"){ %>
                                    
                                      <%--<% MvcCaptcha exampleCaptcha = new MvcCaptcha("ExampleCaptcha");
                                             exampleCaptcha.UserInputID = "CaptchaCode"; %>
                                      <%: Html.Captcha(exampleCaptcha) %>
                                    <p class="BgInputTextStyle Sprite1" style="margin-bottom:0px">
                                        <span class="PwdTempStyle Sprite1">
                                      <input type="text" placeholder="mã bảo mật" required autocomplete="off" name="CaptchaCode" id="CaptchaCode" style="text-transform:uppercase" class="input-block-level" />
                                        </span></p>--%>
                                    <% // } %>            
                                    <div class="ButtonLogin FixFloat">                                                    
                                        <input class="InputLoginStyle Sprite1" name="submit" accesskey="l" value="Đăng nhập" tabindex="4" type="submit">
                                    </div>
                                    
                                     <%=ViewData["err"] %>      
                                        
                                </fieldset>
			                    
                               	
                            </form>
                       </div>
                                </div>            
                            </div>
                        </div>
                    </div>
                </div>
    </div>
    

    
    <div id="footer">
    <p class="Text1Style">© Copyright Tập đoàn Công nghiệp - Viễn thông Quân đội</p>
</div>    
    
    <script type="text/javascript">
        function RefreshCapcha() {
            $.post("/Home/Ajax_Change_captcha", "", function (data) {
                if (data) {
                    $("#codecapcha_hidden").val(data.captcha_encrypt);
                    $("#captchaImage")[0].src = data.captchaImage;
                }
            });
        }
        //$("[name=__RequestVerificationToken]").val("i4sQEtJtGVTUV85XK4DP69lOUjckIK4mJhlEUv-9nRH8mjN0m9_AWeKx8_h7PGmF5UfDtRIFCD7aPUk21nuj2k8Or1xmp793oid0Oc-R4kuPvjGc6krs3JOX5To4q04kOBBeeKdbPaXVt_xzGSGA9q3LSC-dU7s4HBEsa5RkAaA1");
        function CheckForm() {
            if ($("#cUser").val() == "") { alert("Vui lòng điền tên đăng nhập!"); $("#cUser").focus(); return false; }
            if ($("#cPass").val() == "") { alert("Vui lòng điền mật khẩu!"); $("#cPass").focus(); return false; }
            <% if (ViewData["login_fail"].ToString() == "fail"){ %>
                if ($("#codecapcha").val() == "") { alert("Vui lòng điền mã bảo mật!"); $("#codecapcha").focus(); return false; }
            <% }%>
        }


    </script>
</body>
</html>