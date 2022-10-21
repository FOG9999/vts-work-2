create or replace PROCEDURE "RPT_KN_QH_BAOCAO_KNCT_THUOC_TRUNGUONG" 
(
    iDonViXuLy in number,
    listLinhVuc in NVARCHAR2,
    iTenBaoCao in number,
    listKyHop in NVARCHAR2,
    iNguonKienNghi in NVARCHAR2,
    listHuyen_Xa_ThanhPho in NVARCHAR2,
    p_IUSER in number,
    res  OUT sys_refcursor
)
IS
BEGIN
OPEN res FOR
select KN_PA.*, to_char(ROW_NUMBER() OVER (ORDER BY KN_PA.IDOITUONGGUI)) AS depth from (
                                                                                           SELECT kn.IKIENNGHI,'' as TITLE, kn.IDIAPHUONG0,kn.IDOITUONGGUI, kn.INGUONKIENNGHI , kn_nd.DIAPHUONG as CTENDIAPHUONG, kn.CMAKIENNGHI, kn.CNOIDUNG, kn.ILINHVUC,lvcq.IPARENT as iLinhVucPARENT, lvcq.CTEN as CTENLINHVUC, kn.CGHICHU
                                                                                                ,Case when lvcq.ILINHVUC = 1 then lvcq.CTEN
                                                                                                      when lvcq.IPARENT = 1 then lvcq.CTEN
                                                                                                      Else '' End as LVCoChe_ChinhSach
                                                                                                ,Case when lvcq.ILINHVUC = 4 then lvcq.CTEN
                                                                                                      when lvcq.IPARENT = 4 then lvcq.CTEN
                                                                                                      Else '' End as LVTuPhap
                                                                                                ,Case when lvcq.ILINHVUC = 5 then lvcq.CTEN
                                                                                                      when lvcq.IPARENT = 5 then lvcq.CTEN
                                                                                                      Else '' End as LVANQP
                                                                                                ,Case when lvcq.ILINHVUC = 2 then lvcq.CTEN
                                                                                                      when lvcq.IPARENT = 2 then lvcq.CTEN
                                                                                                      Else '' End as LVKinhTe_NganSach
                                                                                                ,Case when lvcq.ILINHVUC = 3 then lvcq.CTEN
                                                                                                      when lvcq.IPARENT = 3 then lvcq.CTEN
                                                                                                      Else '' End as LVVHXH
                                                                                           FROM KN_KIENNGHI kn
                                                                                                    LEFT JOIN LINHVUC_COQUAN lvcq on lvcq.ILINHVUC = kn.ILINHVUC
                                                                                                    LEFT JOIN QUOCHOI_KYHOP qh_kh ON kn.IKYHOP = qh_kh.IKYHOP
                                                                                                    INNER JOIN KN_TONGHOP kn_th on kn.ITONGHOP=kn_th.ITONGHOP
                                                                                                    INNER JOIN QUOCHOI_COQUAN cq on cq.ICOQUAN = kn.iTHAMQUYENDONVI
                                                                                                    INNER JOIN
                                                                                                (
                                                                                                    SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                                                                                                    FROM KIENNGHI_NGUONDON
                                                                                                        INNER JOIN
                                                                                                        (
                                                                                                        SELECT KIENNGHI_NGUONDON.IKIENNGHI AS IKIENNGHI , LISTAGG(KN_NGUONDON.CTEN, ', ') WITHIN GROUP (ORDER BY  KIENNGHI_NGUONDON.IKIENNGHI )AS  DIAPHUONG
                                                                                                        FROM KIENNGHI_NGUONDON
                                                                                                        INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                                                                                                        WHERE  (iNguonKienNghi = '' OR iNguonKienNghi IS NULL OR  KN_NGUONDON.INGUONDON IN (  Select Regexp_Substr(iNguonKienNghi,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(iNguonKienNghi,'[^,]+',1,Level) Is Not Null))
                                                                                                        GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                                                                                                        ) KN_ND_SELECT ON KN_ND_SELECT.IKIENNGHI = KIENNGHI_NGUONDON.IKIENNGHI
                                                                                                        INNER JOIN KN_NGUONDON ON KN_NGUONDON.INGUONDON = KIENNGHI_NGUONDON.INGUONDON
                                                                                                    GROUP BY KIENNGHI_NGUONDON.IKIENNGHI
                                                                                                ) kn_nd ON kn_nd.IKIENNGHI = kn.IKIENNGHI
                                                                                           WHERE cq.CTYPE = 'TW' and (kn.ithamquyendonvi = iDonViXuLy or iDonViXuLy is null or iDonViXuLy = 0)
                                                                                             AND kn.ITINHTRANG = 1
                                                                                             AND (listKyHop = '' OR listKyHop IS NULL OR qh_kh.IKYHOP IN (  Select Regexp_Substr(listKyHop,'[^,]+',1,Level) IKYHOP From  Dual Connect By Regexp_Substr(listKyHop,'[^,]+',1,Level) Is Not Null))
    AND (iTenBaoCao = 2 or (iTenBaoCao = 3 and  kn.IDOITUONGGUI = 1 ) or (iTenBaoCao = 4 and  kn.IDOITUONGGUI = 0 ))
                AND (listLinhVuc = '' OR listLinhVuc IS NULL OR  lvcq.ILINHVUC IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null)
                       OR lvcq.IPARENT IN (  Select Regexp_Substr(listLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(listLinhVuc,'[^,]+',1,Level) Is Not Null) )
                AND (p_IUSER = 0 OR kn.IUSER = p_IUSER)
                AND kn.IDELETE = 0
                AND kn.ITONGHOP != 0
                )KN_PA

Order by KN_PA.IDOITUONGGUI
;
END RPT_KN_QH_BAOCAO_KNCT_THUOC_TRUNGUONG;