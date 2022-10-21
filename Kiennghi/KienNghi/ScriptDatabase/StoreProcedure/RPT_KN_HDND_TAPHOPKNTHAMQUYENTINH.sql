create or replace PROCEDURE "RPT_KN_HDND_TAPHOPKNTHAMQUYENTINH" 
(
    listLinhVuc in NVARCHAR2,
    iTenBaoCao in number,
    listKyHop in NVARCHAR2,
    listHuyen_Xa_ThanhPho in NVARCHAR2,
    p_IUSER in number,
    res OUT sys_refcursor
)
IS
BEGIN
OPEN res FOR
SELECT KN_PA.*, ROW_NUMBER() OVER (ORDER BY KN_PA.IDOITUONGGUI DESC) AS STT FROM (
                                                                                     SELECT kn.IKIENNGHI, kn.IDOITUONGGUI, kn_nd.DIAPHUONG as CTENDIAPHUONG, kn.CMAKIENNGHI AS CSOKIENNGHI, kn.CNOIDUNG, kn.ILINHVUC, kn.CGHICHU
                                                                                          ,Case when lvcq.ILINHVUC = 1 then lvcq.CTEN
                                                                                                when lvcq.IPARENT = 1 then lvcq.CTEN
                                                                                                Else '' End as CCOCHECHINHSACH
                                                                                          ,Case when lvcq.ILINHVUC = 2 then lvcq.CTEN
                                                                                                when lvcq.IPARENT = 2 then lvcq.CTEN
                                                                                                Else '' End as CKINHTENGANSACH
                                                                                          ,Case when lvcq.ILINHVUC = 3 then lvcq.CTEN
                                                                                                when lvcq.IPARENT = 3 then lvcq.CTEN
                                                                                                Else '' End as CVANHOAXAHOI
                                                                                          ,Case when lvcq.ILINHVUC = 4 then lvcq.CTEN
                                                                                                when lvcq.IPARENT = 4 then lvcq.CTEN
                                                                                                Else '' End as CTUPHAP
                                                                                          ,Case when lvcq.ILINHVUC = 5 then lvcq.CTEN
                                                                                                when lvcq.IPARENT = 5 then lvcq.CTEN
                                                                                                Else '' End as CANQP
                                                                                          , '' AS ROWTITLE
                                                                                     FROM KN_KIENNGHI kn
                                                                                              LEFT JOIN LINHVUC_COQUAN lvcq on lvcq.ILINHVUC = kn.ILINHVUC
                                                                                              LEFT JOIN QUOCHOI_KYHOP qh_kh ON kn.IKYHOP = qh_kh.IKYHOP
                                                                                              INNER JOIN QUOCHOI_COQUAN cq on cq.ICOQUAN = kn.iTHAMQUYENDONVI
                                                                                              INNER JOIN KN_TONGHOP kn_th on kn.ITONGHOP=kn_th.ITONGHOP
                                                                                              INNER JOIN
                                                                                          (
                                                                                              SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                                                                                              FROM KIENNGHI_NGUONDON
                                                                                                  INNER JOIN
                                                                                                  (
                                                                                                  SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                                                                                                  FROM KIENNGHI_NGUONDON
                                                                                                  INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                                                                                                  WHERE  (listHuyen_Xa_ThanhPho = '' OR listHuyen_Xa_ThanhPho IS NULL OR  KN_NGUONDON.INGUONDON IN (  Select Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) Is Not Null))
                                                                                                  GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                                                                                                  ) KN_ND_SELECT ON KN_ND_SELECT.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
                                                                                                  INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                                                                                              GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                                                                                          ) kn_nd ON kn_nd.IKIENNGHI = kn.IKIENNGHI
                                                                                     WHERE cq.CTYPE = 'TINH'
                                                                                       AND kn.ITINHTRANG = 1
                                                                                       AND (p_IUSER = 0 OR kn.IUSER = p_IUSER)
                                                                                       AND (listKyHop = '' OR listKyHop IS NULL OR qh_kh.IKYHOP IN (  Select Regexp_Substr(listKyHop,'[^,]+',1,Level) IKYHOP From  Dual Connect By Regexp_Substr(listKyHop,'[^,]+',1,Level) Is Not Null))
--                AND (listHuyen_Xa_ThanhPho = '' OR listHuyen_Xa_ThanhPho IS NULL 
--                                                OR (kn.IDIAPHUONG0 IN (Select Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) IDIAPHUONG0 From Dual Connect By Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) Is Not Null)))
    AND (listLinhVuc = '' OR listLinhVuc IS NULL OR  lvcq.ILINHVUC IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null)
                       OR lvcq.IPARENT IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null) )
                AND (iTenBaoCao = 2 OR (iTenBaoCao = 0 AND kn.IDOITUONGGUI = 1 ) OR (iTenBaoCao = 1 and kn.IDOITUONGGUI = 0))
                AND (p_IUSER = 0 OR kn.IUSER = p_IUSER)
                AND kn.IDELETE = 0
                AND kn.ITONGHOP != 0
ORDER BY kn.IDOITUONGGUI DESC
    ) KN_PA;
END RPT_KN_HDND_TAPHOPKNTHAMQUYENTINH;