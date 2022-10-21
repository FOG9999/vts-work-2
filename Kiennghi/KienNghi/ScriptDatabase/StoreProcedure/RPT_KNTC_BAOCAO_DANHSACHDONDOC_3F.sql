create or replace PROCEDURE RPT_KNTC_BAOCAO_DANHSACHDONDOC_3F 
(
    iLoaiBaoCao IN NUMBER,
    iTuNgay IN DATE,
    iDenNgay IN DATE,
    p_IUSER IN NUMBER,
    res  OUT sys_refcursor
 )
IS
BEGIN
  OPEN res FOR
  SELECT KNTC_DON.*,to_char( ROW_NUMBER() OVER (ORDER BY kntc_vanban.ivanban)) AS TT,
      CONCAT(CONCAT(trim(kntc_don.cnguoigui_ten), ', '),CONCAT(kntc_don.cnguoigui_diachi, CONCAT(', ',CONCAT(diaphuong2.cten,CONCAT(', ',CONCAT(diaphuong1.cten, CONCAT(', ', diaphuong0.cten))))))) AS HOTEN_DIACHI,
        Case when KNTC_DON.ILOAIDON = 1 then 'X'
        Else '' End AS LOAIDON_KN,
        Case when KNTC_DON.ILOAIDON = 2 then 'X'
        Else '' End AS LOAIDON_TC,
        Case when KNTC_DON.ILOAIDON = 3 then 'X'
        Else '' End AS LOAIDON_PAKN,
        Case when KNTC_DON.ILOAIDON = 4 then 'X'
        Else '' End AS LOAIDON_DCNNDN,
        Case when (KNTC_DON.ILINHVUC = 1 OR KNTC_DON.ILINHVUC = 6 OR KNTC_DON.ILINHVUC = 11 OR KNTC_DON.ILINHVUC = 16) then 'X'
        Else '' End AS LINHVUC_HC,
        Case when (KNTC_DON.ILINHVUC = 2 OR KNTC_DON.ILINHVUC = 7 OR KNTC_DON.ILINHVUC = 12 OR KNTC_DON.ILINHVUC = 17) then 'X'
        Else '' End AS LINHVUC_TP,
        Case when (KNTC_DON.ILINHVUC = 3 OR KNTC_DON.ILINHVUC = 8 OR KNTC_DON.ILINHVUC = 13 OR KNTC_DON.ILINHVUC = 18) then 'X'
        Else '' End AS LINHVUC_DTT,
        Case when (KNTC_DON.ILINHVUC = 4 OR KNTC_DON.ILINHVUC = 9 OR KNTC_DON.ILINHVUC = 14 OR KNTC_DON.ILINHVUC = 19) then 'X'
        Else '' End AS LINHVUC_TN,
        Case when (KNTC_DON.ILINHVUC = 5 OR KNTC_DON.ILINHVUC = 10 OR KNTC_DON.ILINHVUC = 15 OR KNTC_DON.ILINHVUC = 20) then 'X'
        Else '' End AS LINHVUC_KHAC,
        kntc_vanban.CSOVANBAN AS SOVANBAN, kntc_vanban.DNGAYBANHANH AS NGAYBANHANHTRALOI, kntc_vanban.THOIGIANBC AS NGAYBAOCAO, '' AS NGAYBANHANHTRALOISTRING
  FROM KNTC_DON
    LEFT JOIN DIAPHUONG diaphuong0 ON diaphuong0.idiaphuong = kntc_don.idiaphuong_0
    LEFT JOIN DIAPHUONG diaphuong1 ON diaphuong1.idiaphuong = kntc_don.idiaphuong_1
    LEFT JOIN DIAPHUONG diaphuong2 ON diaphuong2.idiaphuong = kntc_don.idiaphuong_2
    INNER JOIN KNTC_VANBAN ON kntc_vanban.idon = kntc_don.idon
    where kntc_vanban.cloai = 'vanbandondocthuchien' 
        AND (iTuNgay is null OR kntc_vanban.dngaybanhanh >= iTuNgay)
        AND (iDenNgay is null OR kntc_vanban.dngaybanhanh <= iDenNgay)
        AND (iLoaiBaoCao = -1 OR kntc_don.idoituonggui = iLoaiBaoCao )
        AND (p_IUSER = 0 OR KNTC_DON.IUSER = p_IUSER)
        AND KNTC_DON.IDELETE = 0
    ORDER BY KNTC_DON.DDATE DESC
  ;
END RPT_KNTC_BAOCAO_DANHSACHDONDOC_3F;