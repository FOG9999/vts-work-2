create or replace PROCEDURE "RPT_KN_QH_BAOCAO_KNCT_TAPHOP_DACOTRALOI" 
(
    p_USER in number,
    iTruocKyHop in number,
    iLoaiBaoCao in number,
    listLinhVuc in NVARCHAR2,
    listKyHop in NVARCHAR2,
    listHuyen_Xa_ThanhPho in NVARCHAR2,
    res  OUT sys_refcursor
)
IS 
BEGIN
OPEN res FOR  
select KN_PA.*, to_char(ROW_NUMBER() OVER (ORDER BY KN_PA.IDOITUONGGUI ,KN_PA.CTENDIAPHUONGCUTRI )) AS depth from (
  SELECT kn.IKIENNGHI,'' as TITLE, kn.IDIAPHUONG1,kn.IDOITUONGGUI , KN_ND.DIAPHUONG as CTENDIAPHUONG, kn.CMAKIENNGHI, kn.CNOIDUNG, lvcq.CCODE AS CCODELIUNHVUC, lvcq.ILINHVUC, lvcq.IPARENT as iLinhVucPARENT, lvcq.CTEN as CTENLINHVUC, kn.CGHICHU
                ,KN_ND.DIAPHUONG as CTENDIAPHUONGCUTRI ,cqtl.CTEN as CCOQUANTRALOI , tl.CTRALOI as CTRALOI , tl.CSOVANBAN as CSOVANBAN
                ,Case when lvcq.CCODE LIKE '1%' then lvcq.CTEN
                Else '' End as LVCoChe_ChinhSach
                ,Case when lvcq.CCODE LIKE '4%' then lvcq.CTEN
                Else '' End as LVTuPhap
                ,Case when lvcq.CCODE LIKE '5%' then lvcq.CTEN
                Else '' End as LVANQP
                ,Case when lvcq.CCODE LIKE '2%' then lvcq.CTEN
                Else '' End as LVKinhTe_NganSach
                ,Case when lvcq.CCODE LIKE '3%' then lvcq.CTEN
                Else '' End as LVVHXH
                FROM KN_KIENNGHI kn 
                LEFT JOIN DIAPHUONG dp1 on dp1.IDIAPHUONG = kn.IDIAPHUONG1
                LEFT JOIN LINHVUC_COQUAN lvcq on lvcq.ILINHVUC = kn.ILINHVUC
                LEFT JOIN QUOCHOI_COQUAN cq on cq.ICOQUAN = kn.iTHAMQUYENDONVI
                LEFT JOIN KN_KIENNGHI_TRALOI tl on tl.IKIENNGHI = kn.IKIENNGHI
                LEFT JOIN QUOCHOI_COQUAN cqtl on tl.ICOQUANTRALOI = cqtl.ICOQUAN
                INNER JOIN
                (        
                    SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ',') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                    FROM KIENNGHI_NGUONDON
                    INNER JOIN 
                    (
                        SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ',') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                        FROM KIENNGHI_NGUONDON
                        INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                        WHERE  (listHuyen_Xa_ThanhPho = '' OR listHuyen_Xa_ThanhPho IS NULL OR  KN_NGUONDON.INGUONDON IN (  Select Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) Is Not Null))
                        GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                    ) KN_ND_SELECT ON KN_ND_SELECT.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
                    INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                    GROUP BY KIENNGHI_NGUONDON.IKIENNGHI       
                ) KN_ND ON KN_ND.IKIENNGHI = kn.IKIENNGHI
                WHERE (kn.ITRUOCKYHOP = iTruocKyHop or iTruocKyHop is null or iTruocKyHop = 2)
                AND KN.IDELETE = 0
                AND (kn.IKYHOP IN (select regexp_substr(listKyHop,'[^,]+', 1, level)
					from dual
					connect BY regexp_substr(listKyHop, '[^,]+', 1, level)
					is not null) OR listKyHop = '' OR listKyHop is null)  
--                AND (nd.INGUONDON IN (select regexp_substr(listHuyen_Xa_ThanhPho,'[^,]+', 1, level)
--					from dual
--					connect BY regexp_substr(listHuyen_Xa_ThanhPho, '[^,]+', 1, level)
--					is not null) OR listHuyen_Xa_ThanhPho = '' OR listHuyen_Xa_ThanhPho is null)  
                AND (iLoaiBaoCao = -1 or iLoaiBaoCao = kn.IDOITUONGGUI )
                AND (kn.ILINHVUC IN (select regexp_substr(listLinhVuc,'[^,]+', 1, level)
					from dual
					connect BY regexp_substr(listLinhVuc, '[^,]+', 1, level)
					is not null)OR (kn.IPARENT IN (select regexp_substr(listLinhVuc,'[^,]+', 1, level)
					from dual
					connect BY regexp_substr(listLinhVuc, '[^,]+', 1, level)
					is not null) OR listLinhVuc = '' OR listLinhVuc is null))
                AND (kn.ITINHTRANG = 4 or kn.ITINHTRANG = 5 or kn.ITINHTRANG = 6 or kn.ITINHTRANG = 8)
                AND (p_USER = 0 OR  p_USER = kn.IUSER)
                )KN_PA 
                
                Order by KN_PA.IDOITUONGGUI , KN_PA.CTENDIAPHUONGCUTRI
                ;
END RPT_KN_QH_BAOCAO_KNCT_TAPHOP_DACOTRALOI;