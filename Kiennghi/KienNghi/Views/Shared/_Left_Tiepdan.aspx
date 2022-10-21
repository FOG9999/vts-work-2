<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<script type="text/javascript">
    Get_Menu('left_tiepdan', '<%=Request.Url.AbsolutePath.ToString()%>', '/Home/Ajax_Left_Menu_Tiepdan/');
    
</script>
<div id="left" style="">
    <div class="subnav">
		<div class="subnav-title">
			<a href="#" class='toggle-subnav'><i class="icon-angle-down"></i><span>Tiếp công dân</span></a>
            <a href="#" class="add_mini_bar"><button><i class="icon-align-justify"></i></button></a>
		</div>
		<ul class="subnav-menu" id="left_tiepdan">                     
        </ul>
	</div>
    
</div>