create or replace procedure rpt_kntc_baocao_tiepnhanxulygiamsat_4e
(
    iLoaiBaoCao in number,
    iTuNgay in date,
    iDenNgay in date,
    iUserType in number,
    res out sys_refcursor
)
as 
begin
    open res for
    with
    tongdondiaphuong as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as TONGSODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
        group by dp.CTEN
    ),
    donkhieunai as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as ND_KN_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.ILOAIDON = 1
        group by dp.CTEN
    ),
    dontocao as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as ND_TC_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.ILOAIDON = 2
        group by dp.CTEN
    ),
    donkiennghiphananh as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as ND_PA_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.ILOAIDON = 3
        group by dp.CTEN
    ),
    donnoidungkhac as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as ND_KHAC_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.ILOAIDON = 4
        group by dp.CTEN
    ),
    linhvuchanhchinh as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as LV_HC_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and (d.ILINHVUC = 1 or d.ILINHVUC = 6 or d.ILINHVUC = 11 or d.ILINHVUC = 16)
        group by dp.CTEN
    ),
    linhvuctuphap as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as LV_TP_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and (d.ILINHVUC = 2 or d.ILINHVUC = 7 or d.ILINHVUC = 12 or d.ILINHVUC = 17)
        group by dp.CTEN
    ),
    linhvucdoanthe as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as LV_DT_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and (d.ILINHVUC = 3 or d.ILINHVUC = 8 or d.ILINHVUC = 13 or d.ILINHVUC = 18)
        group by dp.CTEN
    ),
    linhvucthamnhung as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as LV_TN_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and (d.ILINHVUC = 4 or d.ILINHVUC = 9 or d.ILINHVUC = 14 or d.ILINHVUC = 19)
        group by dp.CTEN
    ),
    linhvuckhac as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as LV_KHAC_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and (d.ILINHVUC = 5 or d.ILINHVUC = 10 or d.ILINHVUC = 15 or d.ILINHVUC = 20)
        group by dp.CTEN
    ),
    dudieukien as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as DK_D_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1
        group by dp.CTEN
    ),
    tongkhongdu as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as DK_KD_TONG_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN != 1
        group by dp.CTEN
    ),
    trungkhongdu as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as DK_KD_TRUNG_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN != 1 and d.IDONTRUNG != 0 and d.IDONTRUNG != -1
        group by dp.CTEN
    ),
    ketquachuyen as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as CHUYEN_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and d.ITINHTRANGXULY = 3
        group by dp.CTEN
    ),
    vanbandondoc as (
        select d.IDON as ID, count(*) as COUNT_VANBAN
        from KNTC_DON d
        inner join KNTC_VANBAN vb on d.IDON = vb.IDON
        where d.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and vb.CLOAI = 'vanbandondocthuchien'
        group by d.IDON
    ),
    ketquadondoc as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as DDOC_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and d.ITINHTRANGXULY = 3
        group by dp.CTEN
    ),
    ketquatraloi as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as TLOI_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and d.ITINHTRANGXULY = 10
        group by dp.CTEN
    ),
    ketquanghiencuu as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as NCUU_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and d.ITINHTRANGXULY = 2
        group by dp.CTEN
    ),
    ketquatheodoi as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as TDOI_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and d.ITINHTRANGXULY = 5
        group by dp.CTEN
    ),
    ketquaphanhoi as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as REP_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.IDUDIEUKIEN = 1 and (d.ITINHTRANGXULY = 6 or d.ITINHTRANGXULY = 7
                                  or d.ITINHTRANGXULY = 8 or d.ITINHTRANGXULY = 9)
        group by dp.CTEN
    ),
    tongdonluu as (
        select dp.CTEN as DIAPHUONG, coalesce(count(*), 0) as TONGLUU_SODON
        from KNTC_DON d
        inner join DIAPHUONG dp on d.IDIAPHUONG_1 = dp.IDIAPHUONG
        where d.IDELETE = 0 and dp.IHIENTHI = 1 and dp.IDELETE = 0
          and (iLoaiBaoCao is null or iLoaiBaoCao = 0 or d.IDOITUONGGUI = (iLoaiBaoCao - 1))
          and (iTuNgay is null or iTuNgay = '' or d.DNGAYNHAN >= iTuNgay)
          and (iDenNgay is null or iDenNgay = '' or d.DNGAYNHAN <= iDenNgay)
          and (iUserType is null or iUserType = 0 or d.IUSER = iUserType)
          and d.ITINHTRANGXULY = 5
        group by dp.CTEN
    )
    select
        to_char(row_number() over (order by tddp.DIAPHUONG)) as TT,
        tddp.DIAPHUONG, tddp.TONGSODON, dkn.ND_KN_SODON, dtc.ND_TC_SODON,
        dknpa.ND_PA_SODON, dndk.ND_KHAC_SODON, lvhc.LV_HC_SODON,
        lvtp.LV_TP_SODON, lvdt.LV_DT_SODON, lvtn.LV_TN_SODON, lvk.LV_KHAC_SODON,
        ddk.DK_D_SODON, tkd.DK_KD_TONG_SODON, trkd.DK_KD_TRUNG_SODON,
        kqc.CHUYEN_SODON, kqdd.DDOC_SODON, kqtl.TLOI_SODON,
        kqnc.NCUU_SODON, kqtd.TDOI_SODON, kqph.REP_SODON
    from tongdondiaphuong tddp
    left join donkhieunai dkn on tddp.DIAPHUONG = dkn.DIAPHUONG
    left join dontocao dtc on tddp.DIAPHUONG = dtc.DIAPHUONG
    left join donkiennghiphananh dknpa on tddp.DIAPHUONG = dknpa.DIAPHUONG
    left join donnoidungkhac dndk on tddp.DIAPHUONG = dndk.DIAPHUONG
    left join linhvuchanhchinh lvhc on tddp.DIAPHUONG = lvhc.DIAPHUONG
    left join linhvuctuphap lvtp on tddp.DIAPHUONG = lvtp.DIAPHUONG
    left join linhvucdoanthe lvdt on tddp.DIAPHUONG = lvdt.DIAPHUONG
    left join linhvucthamnhung lvtn on tddp.DIAPHUONG = lvtn.DIAPHUONG
    left join linhvuckhac lvk on tddp.DIAPHUONG = lvk.DIAPHUONG
    left join dudieukien ddk on tddp.DIAPHUONG = ddk.DIAPHUONG
    left join tongkhongdu tkd on tddp.DIAPHUONG = tkd.DIAPHUONG
    left join trungkhongdu trkd on tddp.DIAPHUONG = trkd.DIAPHUONG
    left join ketquachuyen kqc on tddp.DIAPHUONG = kqc.DIAPHUONG
    left join ketquadondoc kqdd on tddp.DIAPHUONG = kqdd.DIAPHUONG
    left join ketquatraloi kqtl on tddp.DIAPHUONG = kqtl.DIAPHUONG
    left join ketquanghiencuu kqnc on tddp.DIAPHUONG = kqnc.DIAPHUONG
    left join ketquatheodoi kqtd on tddp.DIAPHUONG = kqtd.DIAPHUONG
    left join ketquaphanhoi kqph on tddp.DIAPHUONG = kqph.DIAPHUONG
    order by tddp.DIAPHUONG
    ;
end rpt_kntc_baocao_tiepnhanxulygiamsat_4e;