<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<div id="screen"></div><div id="popup" class="popup halp alert_confirm"><div id="main">
                       <div class="container-fluid"><div class="row-fluid"><div class="span12">
                        <div class="box box-color">
                            <div class="box-title"><h3><i class="icon-warning-sign"></i> Xác nhận hướng dẫn trực tiếp</h3>

                         <ul class="tabs"><li class="active"><a title="Ẩn" href="javascript:void(0)" onclick="HidePopup();" data-toggle="tab"><i class="icon-remove"></i></a>
						</li></ul></div><div class="box-content popup_info"><form method="post" name="_form" id="_form" action="/Tiepdan/Ajax_HuongDanTrucTiep_insert" class="form-horizontal" >
                            <input type="hidden" name="id" value="<%=ViewData["id"] %>" />      
                         <div class="form-actions nomagin tright"><button type="button"  class="btn btn-primary">Đồng ý</button>
                          <button type="submit" class="btn btn-warning btn-focus" id="btn-submit">Hủy bỏ</button></div></form></div></div></div></div></div>
                           </div></div>
