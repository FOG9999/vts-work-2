create or replace PROCEDURE "PRC_LIST_TONGHOP_CHUATRALOI" (res out sys_refcursor, p_USER IN NUMBER, 
                                                                p_ITINHTRANG in NUMBER, /* DEFAULT: -1*/
                                                                p_MAKIENNGHI IN NVARCHAR2,/* DEFAULT: */ 
                                                                p_DTUNGAY IN DATE, /* DEFAULT: date min*/
                                                                p_DDENNGAY IN DATE, /* DEFAULT: date max*/
                                                                p_LISTNGUONKN IN VARCHAR2, /* DEFAULT: '0'*/  
                                                                p_IDONVITIEPNHAN IN NUMBER,
                                                                p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                                p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                                p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                                p_ITHAMQUYENDONVI_PARENT IN NUMBER,
                                                                p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/																																 
                                                                p_LISTKYHOP in NVARCHAR2, /* DEFAULT: '0'*/                                                                 
                                                                p_CNOIDUNG IN NVARCHAR2,/* DEFAULT: */                                                                 
                                                                p_PAGE IN NUMBER,/* DEFAULT: */
                                                                p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                                 ) AS
    BEGIN
      Open res for
      WITH 
      SOKIENNGHI_TONGHOP as (
          SELECT COUNT(*) AS TONGSOKIENNGHI,KN_TONGHOP.ITONGHOP AS ID_TONGHOP
                FROM KN_TONGHOP
                INNER JOIN KN_KIENNGHI ON (KN_TONGHOP.ITONGHOP=KN_KIENNGHI.ITONGHOP OR KN_TONGHOP.ITONGHOP=KN_KIENNGHI.ITONGHOP_BDN)
                WHERE KN_KIENNGHI.ID_GOP<=0 GROUP BY KN_TONGHOP.ITONGHOP
      ),
      SOKIENNGHI_TRALOI as (
          SELECT COUNT(*) AS TONGSOKIENNGHI_TRALOI,KN_TONGHOP.ITONGHOP AS ID_TONGHOP
                FROM KN_TONGHOP 
                INNER JOIN KN_KIENNGHI ON (KN_TONGHOP.ITONGHOP=KN_KIENNGHI.ITONGHOP OR KN_TONGHOP.ITONGHOP=KN_KIENNGHI.ITONGHOP_BDN)
                INNER JOIN KN_KIENNGHI_TRALOI ON KN_KIENNGHI.IKIENNGHI=KN_KIENNGHI_TRALOI.IKIENNGHI 
                WHERE KN_KIENNGHI.ID_GOP<=0 GROUP BY KN_TONGHOP.ITONGHOP
      ),
        TBL3 as (SELECT  KN_TONGHOP.ITONGHOP,KN_TONGHOP.IUSER AS IUSER_CAPNHAT,            
            COQUAN_TONGHOP.CTEN AS TEN_DONVITONGHOP,
            KN_TONGHOP.IDONVITONGHOP AS ID_DONVITONGHOP,
            KN_TONGHOP.ITHAMQUYENDONVI AS ID_THAMQUYEN_DONVI_TONGHOP,
            COQUAN_THAMQUYEN.CTEN AS TEN_THAMQUYEN_DONVI_TONGHOP,
            LINHVUC_COQUAN.CTEN AS TEN_LINHVUC_TONGHOP,KN_TONGHOP.ILINHVUC AS ID_LINHVUC_TONGHOP,            
            COQUAN_THAMQUYEN.IPARENT AS ID_PARENT_THAMQUYEN_DONVI,
            KN_VANBAN.DNGAYDUKIENHOANTHANH AS NGAYDUKIENHOANTHANH,
            KN_VANBAN.DNGAYBANHANH AS NGAYBANHANH_VANBAN,
            KN_TONGHOP.CNOIDUNG AS NOIDUNG_TONGHOP, 
            COQUAN_THAMQUYEN_PARENT.CTEN AS TEN_PARENT_THAMQUYEN_DONVI,
            COALESCE(SOKIENNGHI_TONGHOP.TONGSOKIENNGHI,0) AS SOKIENNGHI_TONGHOP,
            COALESCE(SOKIENNGHI_TRALOI.TONGSOKIENNGHI_TRALOI,0) AS SOKIENNGHI_DATRALOI,
            (COALESCE(SOKIENNGHI_TONGHOP.TONGSOKIENNGHI,0)-COALESCE(SOKIENNGHI_TRALOI.TONGSOKIENNGHI_TRALOI,0)) AS SOKIENNGHI_CHUATRALOI
        FROM KN_TONGHOP Inner JOIN KN_VANBAN ON KN_TONGHOP.ITONGHOP=KN_VANBAN.ITONGHOP 
        Inner JOIN QUOCHOI_COQUAN COQUAN_TONGHOP ON KN_TONGHOP.IDONVITONGHOP=COQUAN_TONGHOP.ICOQUAN 
        Inner JOIN QUOCHOI_COQUAN COQUAN_THAMQUYEN ON KN_TONGHOP.ITHAMQUYENDONVI=COQUAN_THAMQUYEN.ICOQUAN 
        Inner JOIN QUOCHOI_COQUAN COQUAN_THAMQUYEN_PARENT ON COQUAN_THAMQUYEN.IPARENT=COQUAN_THAMQUYEN_PARENT.ICOQUAN 
        LEFT JOIN LINHVUC_COQUAN ON LINHVUC_COQUAN.ILINHVUC=KN_TONGHOP.ILINHVUC
        LEFT JOIN SOKIENNGHI_TONGHOP ON SOKIENNGHI_TONGHOP.ID_TONGHOP=KN_TONGHOP.ITONGHOP
        LEFT JOIN SOKIENNGHI_TRALOI ON SOKIENNGHI_TRALOI.ID_TONGHOP=KN_TONGHOP.ITONGHOP
        WHERE 1=1 
        and ( p_USER = 0 or  KN_TONGHOP.IUSER = p_USER or (KN_TONGHOP.itonghop in ( select itonghop from kn_kiennghi where kn_kiennghi.IUSER= p_USER )))
        --SOKIENNGHI_TONGHOP.TONGSOKIENNGHI>0 AND 
        AND (KN_VANBAN.CLOAI='tonghop_chuyendonvi_xuly' OR KN_VANBAN.CLOAI='tonghop_chuyenxuly')
        --AND (p_IDONVITONGHOP=0 OR KN_TONGHOP.IDONVITONGHOP=p_IDONVITONGHOP) 
        AND (p_ITRUOCKYHOP=-1 OR KN_TONGHOP.ITRUOCKYHOP=p_ITRUOCKYHOP) 
        AND (p_ITHAMQUYENDONVI=0 OR KN_TONGHOP.ITHAMQUYENDONVI=p_ITHAMQUYENDONVI or  KN_TONGHOP.IDONVITONGHOP=p_ITHAMQUYENDONVI)  
        AND KN_TONGHOP.ITHAMQUYENDONVI!=0 
--        AND UPPER(KN_TONGHOP.CNOIDUNG) LIKE '%' || UPPER(p_CNOIDUNG) || '%'
        AND (COQUAN_THAMQUYEN_PARENT.ICOQUAN=p_ITHAMQUYENDONVI_PARENT OR p_ITHAMQUYENDONVI_PARENT=0)
        ORDER BY KN_TONGHOP.ITHAMQUYENDONVI,KN_TONGHOP.ILINHVUC,KN_TONGHOP.ITONGHOP),
        KN_GOP AS (SELECT DISTINCT KN_KIENNGHI.ID_GOP AS ID_KIENNGHI_PARENT_GOP, QUOCHOI_COQUAN.CTEN AS TENCOQUAN FROM 
                KN_KIENNGHI LEFT JOIN QUOCHOI_COQUAN ON QUOCHOI_COQUAN.ICOQUAN =KN_KIENNGHI.IDONVITIEPNHAN
                WHERE KN_KIENNGHI.ID_GOP>0  ORDER BY KN_KIENNGHI.ID_GOP),
      TBL2 as( select  KN_GOP.ID_KIENNGHI_PARENT_GOP AS ID_KIENNGHI_PARENT_GOP,
              LISTAGG( convert(KN_GOP.TENCOQUAN, 'UTF8', 'AL32UTF8'), ', ') WITHIN GROUP 
              (ORDER BY KN_GOP.ID_KIENNGHI_PARENT_GOP) TENDONVITIEPNHAN_GOP FROM KN_GOP GROUP BY KN_GOP.ID_KIENNGHI_PARENT_GOP),
       KIENNGHI AS(
        SELECT KN_KIENNGHI.IKIENNGHI AS IKIENNGHI,KN_KIENNGHI.ID_GOP,KN_KIENNGHI.ITONGHOP_BDN AS KIENNGHI_ID_TONGHOP,KN_KIENNGHI.CNOIDUNG AS NOIDUNG_KIENNGHI , KN_KIENNGHI.ITINHTRANG  , 
                                KN_KIENNGHI_TRALOI.ITRALOI AS ITRALOI,COALESCE(KN_KIENNGHI_TRALOI.ITRALOI,0) AS ID_TRALOI,
        KN_KIENNGHI.ITONGHOP AS KIENNGHI_ID_TONGHOP_DIAPHUONG,COALESCE(KN_KIENNGHI.ILINHVUC,0) AS ID_LINHVUC_KIENNGHI,
        LINHVUC_COQUAN.CTEN AS TEN_LINHVUC_KIENNGHI,QUOCHOI_COQUAN.CTEN AS TEN_DONVITIEPNHAN_KIENNGHI,
        TBL2.TENDONVITIEPNHAN_GOP, KN_KIENNGHI.INGAYQUYDINH, KN_KIENNGHI.ICANHBAO,
        TRALOI_PHANLOAI.CTEN AS TRALOI_PHANLOAI, TRALOI_PHANLOAI_PARENT.CTEN AS PARENT_TRALOI_PHANLOAI,
        KN_KIENNGHI_TRALOI.CSOVANBAN AS TRALOI_SOVANBAN, KN_KIENNGHI_TRALOI.CTRALOI AS TRALOI_NOIDUNG,KN_KIENNGHI.IDONVITIEPNHAN
        FROM KN_KIENNGHI LEFT JOIN LINHVUC_COQUAN ON LINHVUC_COQUAN.ILINHVUC=KN_KIENNGHI.ILINHVUC
        LEFT JOIN QUOCHOI_COQUAN ON KN_KIENNGHI.IDONVITIEPNHAN=QUOCHOI_COQUAN.ICOQUAN  
        LEFT JOIN TBL2 ON TBL2.ID_KIENNGHI_PARENT_GOP=KN_KIENNGHI.IKIENNGHI
        LEFT JOIN KN_KIENNGHI_TRALOI on KN_KIENNGHI_TRALOI.IKIENNGHI = KN_KIENNGHI.IKIENNGHI
        LEFT JOIN KN_TRALOI_PHANLOAI TRALOI_PHANLOAI ON TRALOI_PHANLOAI.IPHANLOAI=KN_KIENNGHI_TRALOI.IPHANLOAI
        LEFT JOIN KN_TRALOI_PHANLOAI TRALOI_PHANLOAI_PARENT ON TRALOI_PHANLOAI_PARENT.IPHANLOAI=TRALOI_PHANLOAI.IPARENT
        INNER JOIN
        (        
            SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ',') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
            FROM KIENNGHI_NGUONDON
            INNER JOIN 
            (
                SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ',') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                FROM KIENNGHI_NGUONDON
                INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                WHERE  (p_LISTNGUONKN = '0' OR p_LISTNGUONKN IS NULL OR  KN_NGUONDON.INGUONDON IN (  Select Regexp_Substr(p_LISTNGUONKN,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(p_LISTNGUONKN,'[^,]+',1,Level) Is Not Null))
                GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
            ) KN_ND_SELECT ON KN_ND_SELECT.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
            INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
            GROUP BY KIENNGHI_NGUONDON.IKIENNGHI       
        ) KN_ND ON KN_ND.IKIENNGHI = KN_KIENNGHI.IKIENNGHI
        WHERE (KN_KIENNGHI.IKYHOP IN (select regexp_substr(p_LISTKYHOP,'[^,]+', 1, level)
					from dual
					connect BY regexp_substr(p_LISTKYHOP, '[^,]+', 1, level)
					is not null) OR p_LISTKYHOP = '0')
        AND UPPER(KN_KIENNGHI.CMAKIENNGHI) LIKE '%' || UPPER(p_MAKIENNGHI) || '%'
        AND (p_IDONVITIEPNHAN = 0 OR KN_KIENNGHI.IDONVITIEPNHAN = p_IDONVITIEPNHAN)
        AND (p_DTUNGAY is null OR KN_KIENNGHI.DDATE >= p_DTUNGAY)
        AND (p_DDENNGAY is null OR KN_KIENNGHI.DDATE <= p_DDENNGAY)
        AND (p_ILINHVUC=-1 OR KN_KIENNGHI.ILINHVUC=p_ILINHVUC) 
--        AND (p_LISTNGUONKN  = '0'  OR  KN_NGUONDON.INGUONDON IN ( Select Regexp_Substr(p_LISTNGUONKN,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(p_LISTNGUONKN,'[^,]+',1,Level) Is Not Null)) 
        --AND KN_KIENNGHI.ITONGHOP_BDN!=0 
        --AND (p_ITHAMQUYENDONVI=0 OR KN_KIENNGHI.ITHAMQUYENDONVI=p_ITHAMQUYENDONVI)
      ),
        TBL4 AS( SELECT TBL3.*,KIENNGHI.* FROM TBL3 LEFT JOIN KIENNGHI ON 
        TBL3.ITONGHOP=KIENNGHI.KIENNGHI_ID_TONGHOP OR TBL3.ITONGHOP=KIENNGHI.KIENNGHI_ID_TONGHOP_DIAPHUONG
        WHERE (KIENNGHI.ITINHTRANG = p_ITINHTRANG or (p_ITINHTRANG = -1 and (KIENNGHI.ITINHTRANG = 4 or KIENNGHI.ITINHTRANG = 5 or KIENNGHI.ITINHTRANG = 6 or KIENNGHI.ITINHTRANG = 8)))
        	AND ( (UPPER(TBL3.NOIDUNG_TONGHOP) LIKE '%' || UPPER(p_CNOIDUNG) || '%') OR (UPPER(KIENNGHI.NOIDUNG_KIENNGHI) LIKE '%' || UPPER(p_CNOIDUNG) || '%'))
        	--AND KIENNGHI.ID_TRALOI > 0
        ORDER BY TBL3.ID_THAMQUYEN_DONVI_TONGHOP,TBL3.ITONGHOP,KIENNGHI.ID_LINHVUC_KIENNGHI
        )
        SELECT * FROM 
        ( SELECT k.*,(SELECT COUNT(*) FROM TBL4) AS TOTAL,  ROWNUM r_ FROM( select * from TBL4 ) k
              WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 ) ) WHERE r_>=    (((p_PAGE-1) * p_PAGE_SIZE) + 1)
        ;

  END PRC_LIST_TONGHOP_CHUATRALOI;