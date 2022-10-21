create or replace PROCEDURE RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C 
(
    P_IYEAR IN NUMBER,
    P_IMONTH IN NUMBER,
    P_TUNGAY IN DATE,
    P_DENNGAY IN DATE,
    P_ILOAIDON IN NUMBER,
    P_IUSER IN NUMBER,
    P_IDOITUONG IN NUMBER,
    res  OUT sys_refcursor
 )
IS
BEGIN
OPEN res FOR
SELECT to_char( ROW_NUMBER() OVER (ORDER BY kntc_vanban.ivanban)) AS STT,
       kntc_vanban.CSOVANBAN, TO_DATE(kntc_vanban.DNGAYBANHANH, 'DD/MM/YY') AS DNGAYBANNHANH,
       CONCAT(CONCAT(trim(kntc_don.cnguoigui_ten), ', '),CONCAT(kntc_don.cnguoigui_diachi, CONCAT(', ',CONCAT(diaphuong2.cten,CONCAT(', ',diaphuong1.cten))))) AS CNGUOIGUITEN, kntc_don.CNOIDUNG,
       QUOCHOI_COQUAN.CTEN AS CCOQUANNHAN, kntc_loaidon.CTEN AS CLOAIDON, EXTRACT(MONTH FROM kntc_vanban.dngaybanhanh)
FROM KNTC_DON
         LEFT JOIN DIAPHUONG diaphuong1 ON diaphuong1.idiaphuong = kntc_don.idiaphuong_1
         LEFT JOIN DIAPHUONG diaphuong2 ON diaphuong2.idiaphuong = kntc_don.idiaphuong_2
         INNER JOIN KNTC_VANBAN ON kntc_vanban.idon = kntc_don.idon
         INNER JOIN KNTC_LOAIDON ON kntc_loaidon.iloaidon = kntc_don.iloaidon
         INNER JOIN QUOCHOI_COQUAN ON kntc_vanban.ICOQUANNHAN = quochoi_coquan.ICOQUAN
where (kntc_vanban.cloai = 'vanbandondocthuchien' OR kntc_vanban.cloai = 'chuyenxuly_noibo')
  AND kntc_don.ITINHTRANGXULY = 3
  AND (P_TUNGAY is null OR kntc_vanban.dngaybanhanh >= P_TUNGAY)
  AND (P_DENNGAY is null OR kntc_vanban.dngaybanhanh <= P_DENNGAY)
  AND (P_ILOAIDON = 0 OR P_ILOAIDON IS NULL OR KNTC_DON.ILOAIDON = P_ILOAIDON)
  AND (P_IUSER = 0 OR P_IUSER IS NULL OR KNTC_VANBAN.IUSER = P_IUSER)
  AND (P_IDOITUONG = -1 OR P_IDOITUONG = NULL OR kntc_don.idoituonggui = P_IDOITUONG)
--        AND kntc_vanban.ICOQUANNHAN != 429
  AND kntc_don.IDELETE = 0
;
END RPT_KNTC_BAOCAO_CHUACOTRALOI_3B_3C;