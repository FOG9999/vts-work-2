<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div>
<div id="popup" class="popup halp">
    <div id="main">
        <div class="container-fluid">
            <div class="row-fluid">
        		<div class="span12">
                    <div class="box box-color">
						<div class="box-title">
							<h3 >
								<i class="icon-reorder"> Tìm kiếm cơ quan</i> 
							</h3>
                            <ul class="tabs">
								<li class="active">
									<a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
								</li>
                            </ul>
                        </div>
                        <div class="box-content popup_info" style="padding:0px !important">
                            <form id="form_" method="get" action="/Thietlap/Coquan/">     
                                <table class="table table-bordered">
                                    <tr>
                                        <td width="20%">Chọn cơ quan</td>
                                        <td><p>
                                                <select class="form-control" name="iCoQuan" id="iCoQuan">
                                                <%=ViewData["OptionCoquan"] %>
                                                </select>
                                            </p>
                                           
                                        </td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td nowrap>Tên cơ quan</td>
                                        <td>
                                           <input />
                                        </td>
                                    </tr>
                                    
                                </table>                            
                                <p class="tcenter">
                                    <button type="submit" class="btn btn-success"> Tra cứu</button>                                      
                                    <a href="#" onclick="HidePopup();" class="btn btn-warning">Quay lại</a>
                                </p>
                            </form>
                            
                        </div>                            
                    </div>
                </div>
           </div>
       </div>
   </div>
</div>