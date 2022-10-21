<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<script type="text/javascript">
    Get_Menu('left_vanban', '<%=Request.Url.AbsolutePath.ToString()%>', '/Home/Ajax_Left_Menu_Vanban/');
    
</script>
<div id="left" style="">   
    <div class="subnav">
        <div class="subnav-title">
			<a href="#" class='toggle-subnav'><i class="icon-angle-down"></i><span>Văn bản ban hành</span></a>
            <a href="#" class="add_mini_bar"><button><i class="icon-align-justify"></i></button></a>
		</div>
		<ul class="subnav-menu" id="left_vanban">                     
        </ul>		
	</div>
</div>