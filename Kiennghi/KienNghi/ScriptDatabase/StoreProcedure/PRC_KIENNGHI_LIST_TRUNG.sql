create or replace PROCEDURE "PRC_KIENNGHI_LIST_TRUNG" (res out sys_refcursor,      
                                                               p_LISTLINHVUC IN NVARCHAR2, 
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_DTUNGAY IN DATE,
                                                               p_DDENNGAY IN DATE,
                                                               p_CNOIDUNG IN NVARCHAR2,
                                                               p_LISTKYHOP IN NVARCHAR2,
                                                               p_CMAKIENNGHI IN NVARCHAR2,
                                                               p_INGUONKIENNGHI IN NVARCHAR2,
                                                               p_IUSER_CAPNHAT IN NUMBER,--0
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               ) AS
BEGIN
Open res for
    WITH TBL1 AS
    ( SELECT  KN_KIENNGHI.IKIENNGHI AS ID_KIENNGHI, KN_KIENNGHI.ID_GOP, KN_KIENNGHI.IUSER AS ID_USER_CAPNHAT,
              KN_KIENNGHI.IKIENNGHI_TRUNG AS ID_KIENNGHI_TRUNG, KN_KIENNGHI.ITONGHOP AS ID_KIENNGHI_TONGHOP,
              KN_KIENNGHI.ITONGHOP_BDN AS ID_KIENNGHI_TONGHOP_BDN, KN_KIENNGHI.IDONVITIEPNHAN AS ID_DONVITIEPNHAN,
              --(SELECT COUNT(*) FROM KN_KIENNGHI WHERE ID_GOP=IKIENNGHI) AS SOLUONG_KIENNGHI_GOP,
              DONVI_TIEPNHAN.CTEN AS TEN_DONVITIEPNHAN,
              KN_KIENNGHI.ITHAMQUYENDONVI AS ID_THAMQUYEN_DONVI,KN_KIENNGHI.ILINHVUC AS ID_LINHVUC,ITINHTRANG,
              DONVI_THAMQUYEN.IPARENT AS ID_PARENT_THAMQUYEN_DONVI,
              USERS.IDONVI AS ID_DONVI_CAPNHAT,DONVI_THAMQUYEN.CTEN AS TEN_THAMQUYEN_DONVI,              
              LINHVUC_COQUAN.CTEN AS TEN_LINHVUC, CNOIDUNG AS NOIDUNG_KIENNGHI
      FROM KN_KIENNGHI 
      LEFT JOIN QUOCHOI_COQUAN DONVI_TIEPNHAN ON KN_KIENNGHI.ITHAMQUYENDONVI=DONVI_TIEPNHAN.ICOQUAN
      LEFT JOIN QUOCHOI_COQUAN DONVI_THAMQUYEN ON KN_KIENNGHI.ITHAMQUYENDONVI=DONVI_THAMQUYEN.ICOQUAN
      LEFT JOIN USERS ON USERS.IUSER=KN_KIENNGHI.IUSER
      LEFT JOIN LINHVUC_COQUAN ON LINHVUC_COQUAN.ILINHVUC=KN_KIENNGHI.ILINHVUC
      INNER JOIN KIENNGHI_NGUONDON on KN_KIENNGHI.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
      INNER JOIN KN_NGUONDON on KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
      WHERE  1=1 AND
      (p_INGUONKIENNGHI IS NULL OR p_INGUONKIENNGHI = '' OR  KN_NGUONDON.INGUONDON IN ( Select Regexp_Substr(p_INGUONKIENNGHI,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(p_INGUONKIENNGHI,'[^,]+',1,Level) Is Not Null)) AND 
      UPPER(KN_KIENNGHI.CMAKIENNGHI) LIKE '%' || UPPER(TRIM(p_CMAKIENNGHI)) || '%' 
      AND (p_IUSER_CAPNHAT =0 or KN_KIENNGHI.IUSER =  p_IUSER_CAPNHAT)
      AND (KN_KIENNGHI.IKYHOP IN (select regexp_substr(p_LISTKYHOP,'[^,]+', 1, level)
                from dual
                connect BY regexp_substr(p_LISTKYHOP, '[^,]+', 1, level)
                is not null) OR p_LISTKYHOP = '0')  
      AND (p_LISTLINHVUC IS NULL OR LINHVUC_COQUAN.ILINHVUC IN (select regexp_substr(p_LISTLINHVUC,'[^,]+', 1, level) from dual connect BY regexp_substr(p_LISTLINHVUC, '[^,]+', 1, level) is not null)
            OR LINHVUC_COQUAN.IPARENT IN (select regexp_substr(p_LISTLINHVUC,'[^,]+', 1, level) from dual connect BY regexp_substr(p_LISTLINHVUC, '[^,]+', 1, level) is not null))
      AND (p_IDONVITIEPNHAN = 0 OR KN_KIENNGHI.IDONVITIEPNHAN = p_IDONVITIEPNHAN)
      AND (p_ITHAMQUYENDONVI=0 OR KN_KIENNGHI.ITHAMQUYENDONVI = p_ITHAMQUYENDONVI)
      AND UPPER(KN_KIENNGHI.CNOIDUNG) LIKE '%' || UPPER(TRIM(p_CNOIDUNG)) || '%'
      AND (p_DTUNGAY is null OR KN_KIENNGHI.DDATE >= p_DTUNGAY)
      AND (p_DDENNGAY is null OR KN_KIENNGHI.DDATE <= p_DDENNGAY) 
      AND KN_KIENNGHI.IKIENNGHI_TRUNG!=0 AND KN_KIENNGHI.IKIEMTRATRUNG=1 
      AND KN_KIENNGHI.ITINHTRANG = 7
        )
SELECT * FROM
    (
        SELECT k.*,(SELECT COUNT(*) FROM TBL1) AS TOTAL,
               ROWNUM r_ FROM( select * from TBL1 ) k
        WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
    )
WHERE r_>=    (((p_PAGE-1) * p_PAGE_SIZE) + 1);
END PRC_KIENNGHI_LIST_TRUNG;