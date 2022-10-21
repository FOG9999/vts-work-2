create or replace PROCEDURE RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4F
(
	iUserID IN NUMBER,
	iDoiTuong IN NUMBER,
	iTuNgay IN DATE,
	iDenNgay IN DATE,
	iTuNgayKyTruoc IN DATE,
	iDenNgayKyTruoc IN DATE,
    listDiaPhuongTimKiem IN NVARCHAR2,
	res OUT sys_refcursor
)
IS
BEGIN
OPEN res FOR
	WITH
	kntcdon AS (
		SELECT KNTC_DON.*, diaphuong_1.CTEN as DIAPHUONG1,  
        CONCAT(KNTC_DON.CNGUOIGUI_DIACHI, CONCAT(', ', CONCAT(diaphuong_2.cten, CONCAT(', ',diaphuong_1.cten)))) AS DIAPHUONG2
		FROM KNTC_DON
        INNER JOIN DIAPHUONG diaphuong_1 ON diaphuong_1.IDIAPHUONG = KNTC_DON.IDIAPHUONG_1
        LEFT JOIN DIAPHUONG diaphuong_2 ON diaphuong_2.IDIAPHUONG = KNTC_DON.IDIAPHUONG_2
		WHERE (iTuNgay IS NULL OR iTuNgay = '' OR KNTC_DON.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR KNTC_DON.DNGAYNHAN <= iDenNgay)
            AND (iDoiTuong IS NULL OR KNTC_DON.IDOITUONGGUI = iDoiTuong)
            AND (iUserID = 0 OR iUserID IS NULL OR iUserID = KNTC_DON.iuser)
            AND (listDiaPhuongTimKiem = '' OR listDiaPhuongTimKiem IS NULL OR (diaphuong_1.IDIAPHUONG IN (Select Regexp_Substr(listDiaPhuongTimKiem,'[^,]+',1,Level) IDIAPHUONG0 From Dual Connect By Regexp_Substr(listDiaPhuongTimKiem,'[^,]+',1,Level) Is Not Null)))
            AND KNTC_DON.IDELETE = 0
--            AND (KNTC_DON.ITINHTRANGXULY = 6 OR KNTC_DON.ITINHTRANGXULY = 7 OR KNTC_DON.ITINHTRANGXULY = 8 OR KNTC_DON.ITINHTRANGXULY = 9)
    ),
    vanbandondocthuchien AS (
		SELECT COUNT(*) AS TOTAL_NUMBER, KNTC_VANBAN.IDON
		FROM KNTC_VANBAN
        WHERE KNTC_VANBAN.CLOAI = 'vanbandondocthuchien'
        GROUP BY KNTC_VANBAN.IDON
	)
SELECT kntcdon.idon, kntcdon.DIAPHUONG1, kntcdon.DIAPHUONG2, kntcdon.CNGUOIGUI_TEN AS CNGUOIGUITEN,
       CASE WHEN kntcdon.ILOAIDON = 1 THEN  'X' ELSE '' END  LOAIDON_KN,
       CASE WHEN kntcdon.ILOAIDON = 2 THEN  'X' ELSE '' END  LOAIDON_TC,
       CASE WHEN kntcdon.ILOAIDON = 3 THEN  'X' ELSE '' END  LOAIDON_KNA,
       CASE WHEN kntcdon.ILOAIDON = 4 THEN  'X' ELSE '' END  LOAIDON_KHAC,
       CASE WHEN kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6  OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16 THEN  'X' ELSE '' END  LINHVUC_HC,
       CASE WHEN kntcdon.ILINHVUC = 2 OR kntcdon.ILINHVUC = 7  OR kntcdon.ILINHVUC = 12 OR kntcdon.ILINHVUC = 17 THEN  'X' ELSE '' END  LINHVUC_TP,
       CASE WHEN kntcdon.ILINHVUC = 3 OR kntcdon.ILINHVUC = 8  OR kntcdon.ILINHVUC = 13 OR kntcdon.ILINHVUC = 18 THEN  'X' ELSE '' END  LINHVUC_DTT,
       CASE WHEN kntcdon.ILINHVUC = 4 OR kntcdon.ILINHVUC = 9  OR kntcdon.ILINHVUC = 14 OR kntcdon.ILINHVUC = 19 THEN  'X' ELSE '' END  LINHVUC_TN,
       CASE WHEN kntcdon.ILINHVUC = 5 OR kntcdon.ILINHVUC = 10  OR kntcdon.ILINHVUC = 15 OR kntcdon.ILINHVUC = 20 THEN  'X' ELSE '' END  LINHVUC_KHAC,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 THEN  'X' ELSE '' END  DUDIEUKIEN,
       CASE WHEN kntcdon.IDUDIEUKIEN = 0 THEN  'X' ELSE '' END  KODUDIEUKIEN,
       CASE WHEN kntcdon.IDUDIEUKIEN = 0 AND (kntcdon.IDONTRUNG != 0 OR kntcdon.IDONTRUNG != -1) THEN  'X' ELSE '' END  DONTRUNG,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 3 THEN 'X' ELSE '' END CHUYEN_GIAIQUYET,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 3 AND vanbandondocthuchien.TOTAL_NUMBER != 0 THEN  'X' ELSE '' END  DON_DOC_CQTQ,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 10 THEN  'X' ELSE '' END  TRA_LOI_HD,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 2 THEN  'X' ELSE '' END  DANG_NGHIEN_CUU,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 5 THEN  'X' ELSE '' END  LUU_THEO_DOI,
       CASE WHEN kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ITINHTRANGXULY = 6 OR kntcdon.ITINHTRANGXULY = 7 OR kntcdon.ITINHTRANGXULY = 8 OR kntcdon.ITINHTRANGXULY = 9 THEN  'X' ELSE '' END  NHAN_TRA_LOI,
       CASE WHEN kntcdon.IDUDIEUKIEN = 0 AND kntcdon.ITINHTRANGXULY = 5 THEN  'X' ELSE '' END  KO_LUU_THEO_DOI,
       vanbandondocthuchien.TOTAL_NUMBER
FROM kntcdon
         LEFT JOIN vanbandondocthuchien ON vanbandondocthuchien.IDON = kntcdon.IDON
order by kntcdon.IDON
;
END RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4F;