<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<tr>
    <td>Ngày tiếp</td>
    <td nowrap><input type="text" placeholder="từ ngày" class="input-medium datepick" name="dTuNgay" />
                <input type="text" placeholder="đến ngày" class="input-medium datepick" name="dDenNgay" />
    </td>                                
    <td>Đoàn đông người</td>
    <td><input type="checkbox" name="iDoan" /></td>
</tr>                                    
<tr>
    <td>Cơ quan tiếp</td>
    <td><select name="iDonVi" class="input-block-level">
        <option value="0">- - - Chọn tất cả</option>
        <%=ViewData["opt-donvi"] %>
        </select></td>
    <td></td>
    <td></td>
</tr>
<tr>
    <td>Tên công dân đến</td>
    <td><input type="text" class="input-block-level" name="cNguoiGui_Ten" /></td>
    <td>Địa chỉ công dân</td>
    <td><input type="text" class="input-block-level" name="cNguoiGui_DiaChi" /></td>
</tr>
<tr>
    <td>Tóm tắt nội dung vụ việc</td>
    <td colspan="3"><input type="text" class="input-block-level" name="cNoiDung" /></td>
</tr>
<tr>
    <td class="">Loại vụ việc</td>
    <td><select name="iLoai" id="iLoai" class="input-block-level"> 
        <option value="-1">- - - Chọn tất cả</option>    
        <option value="0">- - - Chưa xác định</option>                                          
        <%=ViewData["opt-loaidon"] %></select>                                        
    </td>
    <td class="">Lĩnh vực</td>
    <td><select name="iLinhVuc" id="iLinhVuc" class="input-block-level">     
            <option value="-1">- - - Chọn tất cả</option><option value="0">- - - Chưa xác định</option>                                               
            <%=ViewData["opt-linhvuc"] %>
        </select>                                        
    </td>
</tr>
<tr>
    <td>Nhóm nội dung</td>
    <td><select name="iNoiDung" class="input-block-level">
        <option value="-1">- - - Chọn tất cả</option><option value="0">- - - Chưa xác định</option>    
        <%=ViewData["opt-noidung"] %></select>                                        
    </td>
    <td>Tính chất vụ việc</td>
    <td><select name="iTinhChat" class="input-block-level">
        <option value="-1">- - - Chọn tất cả</option><option value="0">- - - Chưa xác định</option>    
            <%=ViewData["opt-tinhchat"] %>
        </select>                                        
    </td>
</tr> 
<tr>
    <td colspan="4" class="tcenter">
                                        
        <button type="submit" class="btn btn-success"> Tra cứu</button>        
    </td>
</tr>