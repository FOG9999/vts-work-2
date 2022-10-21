create or replace procedure rpt_kn_baocao_tonghopykien_1b1
(
    iUserType in number,
    lstKyHop in varchar2,
    lstNguonKN in varchar2,
    lstLinhVuc in varchar2,
    iDoiTuong in varchar2,
    res out sys_refcursor
)
as 
begin
    open res for
    with
    kn_nd as (
        select kn.ILINHVUC as ILINHVUC, knnd.INGUONDON as INGUONDON, count(*) as TONGSOKIENNGHI
        from KN_KIENNGHI kn
        inner join KIENNGHI_NGUONDON knnd on kn.IKIENNGHI = knnd.IKIENNGHI
        left join LINHVUC_COQUAN lvcq on kn.ILINHVUC = lvcq.ILINHVUC
        left join QUOCHOI_KYHOP qhkh on kn.IKYHOP = qhkh.IKYHOP
        where kn.IDELETE = 0 and kn.ITINHTRANG = 0
          and (iUserType is null or iUserType = 0 or kn.IUSER = iUserType)
          and (iDoiTuong is null or iDoiTuong = -1 or kn.IDOITUONGGUI = iDoiTuong)
          and (lstKyHop is null or lstKyHop = '' or qhkh.IKYHOP in (Select Regexp_Substr(lstKyHop,'[^,]+',1,Level) IKYHOP From  Dual Connect By Regexp_Substr(lstKyHop,'[^,]+',1,Level) Is Not Null))
          and (lstLinhVuc is null or lstLinhVuc = ''
                or lvcq.ILINHVUC in (Select Regexp_Substr(lstLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(lstLinhVuc,'[^,]+',1,Level) Is Not Null)
--                or lvcq.IPARENT in (Select Regexp_Substr(lstLinhVuc,'[^,]+',1,Level) ILINHVUC From  Dual Connect By Regexp_Substr(lstLinhVuc,'[^,]+',1,Level) Is Not Null)
                )
        group by (kn.ILINHVUC, knnd.INGUONDON)
    )
    select to_char(row_number() over (order by nd.INGUONDON)) as TT,
        nd.CTEN as DIAPHUONG, kn_nd.TONGSOKIENNGHI, kn_nd.ILINHVUC
    from KN_NGUONDON nd
    inner join kn_nd on nd.INGUONDON = kn_nd.INGUONDON
    where nd.IDELETE = 0 and nd.IHIENTHI = 1
      and (lstNguonKN is null or lstNguonKN = '' or nd.INGUONDON in (Select Regexp_Substr(lstNguonKN,'[^,]+',1,Level) INGUONDON From Dual Connect By Regexp_Substr(lstNguonKN,'[^,]+',1,Level) Is Not Null))
    order by nd.INGUONDON
    ;
end rpt_kn_baocao_tonghopykien_1b1;