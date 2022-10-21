create or replace PROCEDURE RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4D
(
	iUserType IN NUMBER,
	iLoaiBaoCao IN NUMBER,
	iTuNgay IN DATE,
	iDenNgay IN DATE,
	iTuNgayKyTruoc IN DATE,
	iDenNgayKyTruoc IN DATE,
	res OUT sys_refcursor
)
IS 
BEGIN
  OPEN res FOR
	WITH
	DONNHIEUNGUOIDUNGTENKYTRUOC AS (
		SELECT COUNT(*) AS COUNTDONNHIEUNGUOIDUNGTENKYTRUOC
		FROM KNTC_DON kntcdon
		WHERE kntcdon.IPLSONGUOI = 1
            AND kntcdon.ILOAIDON = 2
            AND (kntcdon.ITINHTRANGXULY = 1 OR kntcdon.ITINHTRANGXULY = 2)
            AND (iTuNgayKyTruoc IS NULL OR iTuNgayKyTruoc = ''  OR kntcdon.DNGAYNHAN >= iDenNgayKyTruoc)
            AND (iDenNgayKyTruoc IS NULL OR iDenNgayKyTruoc = '' OR kntcdon.DNGAYNHAN <= iDenNgayKyTruoc)
            AND (iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
            AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONMOTNGUOIDUNGTENKYTRUOC AS (
		SELECT COUNT(*) AS COUNTDONMOTNGUOIDUNGTENKYTRUOC
		FROM KNTC_DON kntcdon
		WHERE kntcdon.IPLSONGUOI = 0
            AND kntcdon.ILOAIDON = 2
            AND (kntcdon.ITINHTRANGXULY = 1 OR kntcdon.ITINHTRANGXULY = 2)
            AND (iTuNgayKyTruoc IS NULL OR iTuNgayKyTruoc = ''  OR kntcdon.DNGAYNHAN >= iDenNgayKyTruoc)
            AND (iDenNgayKyTruoc IS NULL OR iDenNgayKyTruoc = '' OR kntcdon.DNGAYNHAN <= iDenNgayKyTruoc)
            AND (iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
            AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONNHIEUNGUOIDUNGTENTRONGKY AS (
		SELECT COUNT(*) AS COUNTDONNHIEUNGUOIDUNGTENTRONGKY
		FROM KNTC_DON kntcdon
		WHERE kntcdon.IPLSONGUOI = 1
            AND kntcdon.ILOAIDON = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
            AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
            AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONMOTNGUOIDUNGTENTRONGKY AS (
		SELECT COUNT(*) AS COUNTDONMOTNGUOIDUNGTENTRONGKY
		FROM KNTC_DON kntcdon
		WHERE kntcdon.IPLSONGUOI = 0
            AND kntcdon.ILOAIDON = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
            AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
            AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
    SODONDAXULYTRUOCKY AS (
		SELECT COUNT(*) AS COUNTSODONDAXULYTRUOCKY
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
    SODONDAXULYTRONGKY AS (
		SELECT COUNT(*) AS COUNTSODONDAXULYTRONGKY
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	
	CHEDOCHINHSACH AS (
		SELECT COUNT(*) AS COUNTCHEDOCHINHSACH
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
        AND (kntcdon.INOIDUNG = 1 OR kntcdon.INOIDUNG = 5 OR kntcdon.INOIDUNG = 9 OR kntcdon.INOIDUNG = 13)
            AND kntcdon.IDELETE = 0
	),
    DATDAINHACUA AS (
		SELECT COUNT(*) AS COUNTDATDAINHACUA
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
        AND (kntcdon.INOIDUNG = 2 OR kntcdon.INOIDUNG = 6 OR kntcdon.INOIDUNG = 10 OR kntcdon.INOIDUNG = 14)
            AND kntcdon.IDELETE = 0
	),
    CONGCHUCCONGVU AS (
		SELECT COUNT(*) AS COUNTCONGCHUCCONGVU
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
        AND (kntcdon.INOIDUNG = 3 OR kntcdon.INOIDUNG = 7 OR kntcdon.INOIDUNG = 11 OR kntcdon.INOIDUNG = 15)
            AND kntcdon.IDELETE = 0
	),
    KHAC AS (
		SELECT COUNT(*) AS COUNTKHAC
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
        AND (kntcdon.INOIDUNG = 4 OR kntcdon.INOIDUNG = 8 OR kntcdon.INOIDUNG = 12 OR kntcdon.INOIDUNG = 16)
            AND kntcdon.IDELETE = 0
	),
    LINHVUCTUPHAP AS (
		SELECT COUNT(*) AS COUNTLINHVUCTUPHAP
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 2 OR kntcdon.ILINHVUC = 7 OR kntcdon.ILINHVUC = 12 OR kntcdon.ILINHVUC = 17)
            AND kntcdon.IDELETE = 0
	),
    LINHVUCDANGDOANTHE AS (
		SELECT COUNT(*) AS COUNTLINHVUCDANGDOANTHE
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 3 OR kntcdon.ILINHVUC = 8 OR kntcdon.ILINHVUC = 13 OR kntcdon.ILINHVUC = 18)
            AND kntcdon.IDELETE = 0
	),
    THAMNHUNG AS (
		SELECT COUNT(*) AS COUNTTHAMNHUNG
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 4 OR kntcdon.ILINHVUC = 9 OR kntcdon.ILINHVUC = 14 OR kntcdon.ILINHVUC = 19)
            AND kntcdon.IDELETE = 0
	),
    LINHVUCKHAC AS (
		SELECT COUNT(*) AS COUNTLINHVUCKHAC
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND kntcdon.ILOAIDON = 2
            AND kntcdon.IDUDIEUKIEN = 1 
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
        AND (kntcdon.ILINHVUC = 5 OR kntcdon.ILINHVUC = 10 OR kntcdon.ILINHVUC = 15 OR kntcdon.ILINHVUC = 20)
            AND kntcdon.IDELETE = 0
	)
	SELECT
		CASE WHEN iLoaiBaoCao = 1 THEN  'Đoàn ĐB Quốc hội tỉnh' ELSE 'Hội đồng nhân dân tỉnh' END  STENDONVI,
		DONNHIEUNGUOIDUNGTENKYTRUOC.COUNTDONNHIEUNGUOIDUNGTENKYTRUOC,
		DONMOTNGUOIDUNGTENKYTRUOC.COUNTDONMOTNGUOIDUNGTENKYTRUOC,
		DONNHIEUNGUOIDUNGTENTRONGKY.COUNTDONNHIEUNGUOIDUNGTENTRONGKY,
		DONMOTNGUOIDUNGTENTRONGKY.COUNTDONMOTNGUOIDUNGTENTRONGKY,
		SODONDAXULYTRUOCKY.COUNTSODONDAXULYTRUOCKY,
		SODONDAXULYTRONGKY.COUNTSODONDAXULYTRONGKY,
        CHEDOCHINHSACH.COUNTCHEDOCHINHSACH,
        DATDAINHACUA.COUNTDATDAINHACUA,
        CONGCHUCCONGVU.COUNTCONGCHUCCONGVU,
        KHAC.COUNTKHAC,
        LINHVUCTUPHAP.COUNTLINHVUCTUPHAP,
        LINHVUCDANGDOANTHE.COUNTLINHVUCDANGDOANTHE,
        THAMNHUNG.COUNTTHAMNHUNG,
        LINHVUCKHAC.COUNTLINHVUCKHAC
	FROM DONNHIEUNGUOIDUNGTENKYTRUOC,DONMOTNGUOIDUNGTENKYTRUOC,
			DONNHIEUNGUOIDUNGTENTRONGKY,DONMOTNGUOIDUNGTENTRONGKY,
			SODONDAXULYTRUOCKY,SODONDAXULYTRONGKY,CHEDOCHINHSACH,DATDAINHACUA,
            CONGCHUCCONGVU,KHAC,LINHVUCTUPHAP,LINHVUCDANGDOANTHE,THAMNHUNG,LINHVUCKHAC;
END RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4D;