create or replace PROCEDURE RPT_KNTC_BAOCAO_DANHSACHCACDONVIDACOTRALOI_4A 
(
        p_IUSER IN NUMBER,
        iKhoa IN INT,
        iLoaiBaoCao IN int,
        iCoQuanTraLoi IN int,
        iTuNgay IN DATE,
        iDenNgay IN DATE,
        res  OUT sys_refcursor
)
IS 
BEGIN
  OPEN res FOR  
  SELECT
      KNTC_DON.*, to_char( ROW_NUMBER() OVER (ORDER BY kntc_vanban.ivanban)) AS TT,
      CONCAT(CONCAT(kntc_don.cnguoigui_ten, ', '),CONCAT(kntc_don.cnguoigui_diachi, CONCAT(', ',CONCAT(diaphuong2.cten,CONCAT(', ',CONCAT(diaphuong1.cten, CONCAT(', ', diaphuong0.cten))))))) AS HOTEN_DIACHI,
      kntc_vanban.csovanban as SOCONGVANCHUYEN, kntc_vanban.csovanban as SOCONGVANTRALOI, kntc_vanban.dngaybanhanh AS NGAYBANHANHTRALOI, cqtraloi.cten AS COQUANTRALOI, cqtraloi.cten AS COQUANCHUYENDEN,
      kntc_vanban.cnoidung AS NOIDUNGTRALOI, kntc_vanban.ghichu_xuly AS VANBANGHICHU, kntc_vanban.icoquanbanhanh as ICOQUANBANHANH, kntc_vanban.cloai AS LOAIVANBAN, kntc_vanban.ddate AS DATEVANBAN
  FROM KNTC_DON
    LEFT JOIN DIAPHUONG diaphuong0 ON diaphuong0.idiaphuong = kntc_don.idiaphuong_0
    LEFT JOIN DIAPHUONG diaphuong1 ON diaphuong1.idiaphuong = kntc_don.idiaphuong_1
    LEFT JOIN DIAPHUONG diaphuong2 ON diaphuong2.idiaphuong = kntc_don.idiaphuong_2
    INNER JOIN KNTC_VANBAN ON kntc_vanban.idon = kntc_don.idon
    --LEFT JOIN QUOCHOI_COQUAN cqthuly ON cqthuly.icoquan = kntc_don.idonvithuly 
    LEFT JOIN QUOCHOI_COQUAN cqtraloi ON cqtraloi.icoquan = kntc_vanban.icoquanbanhanh 
    where (iKhoa = 0 OR iKhoa = kntc_don.ikhoa)
        AND (kntc_vanban.cloai = 'chuyenxulylaidon' OR kntc_vanban.cloai = 'chuyenxuly_noibo'  OR kntc_vanban.cloai = 'chuagiaiquyet' OR kntc_vanban.cloai = 'ketqua' OR kntc_vanban.cloai = 'hoanthanh' OR kntc_vanban.cloai = 'dahuongdan')
        AND (iTuNgay IS NULL OR kntc_vanban.dngaybanhanh >= iTuNgay OR kntc_vanban.cloai = 'chuyenxulylaidon' OR kntc_vanban.cloai = 'chuyenxuly_noibo')
        AND (iDenNgay IS NULL OR kntc_vanban.dngaybanhanh <= iDenNgay OR kntc_vanban.cloai = 'chuyenxulylaidon' OR kntc_vanban.cloai = 'chuyenxuly_noibo')
        AND (iCoQuanTraLoi = 0 OR kntc_vanban.icoquanbanhanh = iCoQuanTraLoi)
        AND (iLoaiBaoCao = 0 OR kntc_don.idoituonggui = (iLoaiBaoCao - 1))
        AND (p_IUSER = 0 OR KNTC_DON.IUSER = p_IUSER)
        AND KNTC_DON.IDELETE = 0
    order by kntc_don.idon DESC, kntc_vanban.ddate DESC
    ;
END RPT_KNTC_BAOCAO_DANHSACHCACDONVIDACOTRALOI_4A;