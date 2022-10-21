create or replace PROCEDURE "RPT_KN_QH_BAOCAO_TONGHOP_HUYEN" 
(
    listKyHop in NVARCHAR2,
    listLinhVuc in NVARCHAR2,
    iDonViThamQuyen in number,
    listHuyen_Xa_ThanhPho in NVARCHAR2,
    keydata in number,
    p_IUSER in number,
    iTenBaoCao in number,
    res  OUT sys_refcursor
)
IS
BEGIN
OPEN res FOR
SELECT KN_PA.*,  ROW_NUMBER() OVER (ORDER BY KN_PA.IDOITUONGGUI) AS DEPTH
from (
         SELECT kn.IKIENNGHI,kn.IKYHOP, kn.IDIAPHUONG0, kn_nd.DIAPHUONG as TENNGUONKIENNGHI, kn_nd.DIAPHUONG as CTENDIAPHUONG, kn.CMAKIENNGHI,
             kn.CNOIDUNG,kn.IDOITUONGGUI , kn.ILINHVUC, lvcq.CTEN as CTENLINHVUC, kn.CGHICHU,kn.IDONVITIEPNHAN,kn.ITHAMQUYENDONVI,kn.DDATE,kn.INGUONKIENNGHI,lvcq.IPARENT
              ,Case when lvcq.ILINHVUC = 1 then lvcq.CTEN
                    when lvcq.IPARENT = 1 then lvcq.CTEN
                    Else '' End as ICOCHE
              ,Case when lvcq.ILINHVUC = 4 then lvcq.CTEN
                    when lvcq.IPARENT = 4 then lvcq.CTEN
                    Else '' End as ITUPHAP
              ,Case when lvcq.ILINHVUC = 5 then lvcq.CTEN
                    when lvcq.IPARENT = 5 then lvcq.CTEN
                    Else '' End as ANQP
              ,Case when lvcq.ILINHVUC = 2 then lvcq.CTEN
                    when lvcq.IPARENT = 2 then lvcq.CTEN
                    Else '' End as IKINHTE
              ,Case when lvcq.ILINHVUC = 3 then lvcq.CTEN
                    when lvcq.IPARENT = 3 then lvcq.CTEN
                    Else '' End as IVANHOA
         FROM KN_KIENNGHI kn
                  LEFT JOIN LINHVUC_COQUAN lvcq on lvcq.ILINHVUC = kn.ILINHVUC
                  LEFT JOIN QUOCHOI_KYHOP qh_kh ON kn.IKYHOP = qh_kh.IKYHOP
                  INNER JOIN
              (
                  SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                  FROM KIENNGHI_NGUONDON
                      INNER JOIN
                      (
                      SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                      FROM KIENNGHI_NGUONDON
                      INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                      WHERE  (listHuyen_Xa_ThanhPho = '' OR listHuyen_Xa_ThanhPho IS NULL OR  KN_NGUONDON.INGUONDON IN (Select Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(listHuyen_Xa_ThanhPho,'[^,]+',1,Level) Is Not Null))
                      GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                      ) KN_ND_SELECT ON KN_ND_SELECT.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
                      INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                  GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
              ) kn_nd ON kn_nd.IKIENNGHI = kn.IKIENNGHI
                  INNER JOIN QUOCHOI_COQUAN qhcq on qhcq.ICOQUAN = kn.ITHAMQUYENDONVI AND qhcq.CTYPE='HUYEN'
         WHERE (kn.IDOITUONGGUI=keydata or keydata = 2)
           AND kn.ITINHTRANG = 1
           AND (listKyHop = '' OR listKyHop IS NULL OR qh_kh.IKYHOP IN (  Select Regexp_Substr(listKyHop,'[^,]+',1,Level) IKYHOP From  Dual Connect By Regexp_Substr(listKyHop,'[^,]+',1,Level) Is Not Null))
    AND (listLinhVuc = '' OR listLinhVuc IS NULL OR  lvcq.ILINHVUC IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null)
                       OR lvcq.IPARENT IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null) )
                AND( iThamQuyenDonVi IS NULL OR iThamQuyenDonVi = kn.ITHAMQUYENDONVI)
                AND (p_IUSER = 0 OR kn.IUSER = p_IUSER)
                AND kn.IDELETE = 0
                AND kn.ITONGHOP != 0
                )KN_PA ORDER BY KN_PA.IDOITUONGGUI

;
END RPT_KN_QH_BAOCAO_TONGHOP_HUYEN;