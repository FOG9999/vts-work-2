<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<script type="text/javascript">
    Get_Menu('left_kiennghi', '<%=Request.Url.AbsolutePath.ToString()%>', '/Home/Ajax_Left_Menu_Kiennghi/');
    
</script>
<div id="left" style="">   
    <div class="subnav">
        <div class="subnav-title">
			<a href="#" class='toggle-subnav'><i class="icon-angle-down"></i><span>Kiến nghị cử tri</span></a>
            <a href="#" class="add_mini_bar"><button><i class="icon-align-justify"></i></button></a>
		</div>
		<ul class="subnav-menu" id="left_kiennghi">                     
        </ul>		
	</div>
</div>