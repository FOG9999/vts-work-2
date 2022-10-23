  declare @DuAnId nvarchar(300)
  set @DuAnId = (select iID_DuAnID FROM VDT_DA_DuToan where iID_DuToanID = cast(@duToanId AS nvarchar(300)))

  select 
  nv.iID_NguonVonID as IIdNguonVonId,
  SUM(nv.fTienPheDuyetQDDT) as GiaTriPheDuyet,
  ns.sTen as TenNguonVon,
  cast(0 as float) as TienDeNghi,
  SUM(cast(ISNULL(nguonvonchitiet.fCapPhatTaiKhoBac,0) as float)) AS fCapPhatTaiKhoBac,
  SUM(cast(isnull(nguonvonchitiet.fCapPhatBangLenhChi,0) as float)) AS fCapPhatBangLenhChi
  from VDT_DA_DuToan_Nguonvon nv
  inner join NS_NguonNganSach ns ON ns.iID_MaNguonNganSach = nv.iID_NguonVonID
  left join VDT_KHV_PhanBoVon_ChiTiet nguonvonchitiet on nguonvonchitiet.iID_DuAnID = @DuAnId
  where iID_DuToanID in (select * FROM f_split(@duToanId))
  group by nv.iID_NguonVonID,ns.sTen