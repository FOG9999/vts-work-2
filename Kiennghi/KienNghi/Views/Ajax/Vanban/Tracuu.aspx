<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="span12">
			    <div class="box box-color box-bordered">
				    <div class="box-title">
					    <h3><i class="icon-tags"></i>Danh sách văn bản tìm kiếm </h3>
                        
				    </div>
				    <div class="box-content nopadding">
					    <table class="table table-bordered table-striped">
                            <thead>
                                <tr>                          
                                     <th nowrap width="3%">STT</th>   
                                    <th nowrap width="67%" >Thông tin văn bản</th>   
                                         
                                    <th class="tcenter"  width="5%" nowrap>File</th>                                                    
                                    <th nowrap class="tcenter" width="15%">Chức năng</th>
                                </tr>
                            </thead>
                            <tbody >                          
                              <%=ViewData["data"] %>
                            </tbody>
                        </table>
				    </div>
			    </div>
		    </div>

