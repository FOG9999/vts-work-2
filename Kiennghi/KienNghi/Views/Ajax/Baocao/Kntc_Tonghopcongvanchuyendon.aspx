<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<div class="row-fluid">
        <div class="span12">
            <div class="box box-color box-bordered">
                <div class="box-title">
                    <h3>
                        <i class="icon-reorder"></i>Tổng hợp đơn (Công văn chuyển đơn)
                    </h3>
                
                </div>

                <div class=" box-content popup_info nopadding">
                    
                        <div>
                            <%=ViewData["data"] %>
                        </div>
                </div>
            </div>
        </div>
    </div>
