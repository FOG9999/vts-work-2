<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<script type="text/javascript">
    <%if (Request.Url.AbsolutePath.IndexOf("/Home/Error") != -1)
       {
           Response.Write(""); 
       }
       else
       {
           Response.Write("Get_Menu('menutop_', '', '/Home/Ajax_Load_MenuTop');"); 
       }
         %>

  
    
</script>
<div id="navigation" class="">
    <div class="container-fluid container">
        
        <ul class='main-nav' id="menutop_">

        </ul>
        <%--<div class="user">
            <div class="dropdown">
                <a href="#" class='dropdown-toggle menu0' data-toggle="dropdown" id="user_name"></a>
                <ul class="dropdown-menu pull-right">
                    <li>
                        <a href="/Home/Taikhoan">Tài khoản & Mật khẩu</a>
                    </li>
                    <li>
                        <a href="/Home/Logout/">Thoát</a>
                    </li>
                </ul>
            </div>
        </div>--%>
    </div>
</div>
<style>
    #navigation .main-nav li:first-child ul.dropdown-menu li a {
        padding-left: 20px;
    }
</style>
