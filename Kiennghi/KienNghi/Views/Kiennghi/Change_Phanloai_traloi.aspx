<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="row-fluid">    
    <div class="control-group">
	    <label for="textfield" class="control-label ">Kết quả xử lý <i class="f-red">*</i></label>
	    <div class="controls">
            <textarea class="input-block-level" rows="4" id="cNoiDung" name="cNoiDung"></textarea>                                        
	    </div>
    </div>    
</div>
<% if(ViewData["id"].ToString()=="7") { %>
<div class="row-fluid">    
    <div class="control-group">
		<label for="textfield" class="control-label">Ngày dự kiến hoàn thành </label>
		<div class="controls">
            <input type="text" name="DNGAY_DUKIEN" id="DNGAY_DUKIEN" class="datepick input-medium" value="" />
		</div>
	</div>   
</div>
<% } %>