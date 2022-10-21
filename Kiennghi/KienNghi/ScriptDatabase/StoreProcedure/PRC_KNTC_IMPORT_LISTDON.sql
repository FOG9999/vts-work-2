create or replace PROCEDURE PRC_KNTC_IMPORT_LISTDON 
(
    p_ID_IMPORT IN NUMBER,
    res OUT sys_refcursor
)
IS
BEGIN
Open res for
    SELECT KNTC_DON.*, diaphuong1.cten AS TEN_DIAPHUONG1, diaphuong2.cten AS TEN_DIAPHUONG2, kntc_loaidon.cten AS TEN_LOAIDON
    FROM KNTC_DON
    LEFT JOIN DIAPHUONG diaphuong1 ON diaphuong1.idiaphuong = kntc_don.idiaphuong_1
    LEFT JOIN DIAPHUONG diaphuong2 ON diaphuong2.idiaphuong = kntc_don.idiaphuong_2
    LEFT JOIN KNTC_LOAIDON ON kntc_loaidon.iloaidon = kntc_don.iloaidon
    WHERE (p_ID_IMPORT = 0 OR KNTC_DON.IIDIMPORT = p_ID_IMPORT)
    AND KNTC_DON.IDELETE = 0
    ORDER BY KNTC_DON.ddate
  ;
END PRC_KNTC_IMPORT_LISTDON;