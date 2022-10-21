create or replace PROCEDURE RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4B
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
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
            AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
            AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	SODONDAXULY AS (
		SELECT COUNT(*) AS COUNTSODONDAXULY
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONKHIEUNAI AS (
		SELECT COUNT(*) AS COUNTDONKHIEUNAI
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
				AND kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ILOAIDON = 1
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONTOCAO AS (
		SELECT COUNT(*) AS COUNTDONTOCAO
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
				AND kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ILOAIDON = 2
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONKIENNGHI AS (
		SELECT COUNT(*) AS COUNTDONKIENNGHI
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
				AND kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ILOAIDON = 3
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	),
	DONNHIEUNOIDUNGKHAC AS (
		SELECT COUNT(*) AS COUNTDONNHIEUNOIDUNGKHAC
		FROM KNTC_DON kntcdon
		WHERE kntcdon.ITINHTRANGXULY = 2
				AND kntcdon.IDUDIEUKIEN = 1 AND kntcdon.ILOAIDON = 4
            AND (iTuNgay IS NULL OR iTuNgay = ''  OR kntcdon.DNGAYNHAN >= iTuNgay)
            AND (iDenNgay IS NULL OR iDenNgay = '' OR kntcdon.DNGAYNHAN <= iDenNgay)
        AND ( iLoaiBaoCao IS NULL OR iLoaiBaoCao = 0 OR kntcdon.IDOITUONGGUI = (iLoaiBaoCao - 1))
        AND (iUserType = 0 OR iUserType = kntcdon.iuser)
            AND kntcdon.IDELETE = 0
	)
	SELECT
		CASE WHEN iLoaiBaoCao = 1 THEN  'Đoàn ĐB Quốc hội tỉnh' ELSE 'Hội đồng nhân dân tỉnh' END  STENDONVI,
		DONNHIEUNGUOIDUNGTENKYTRUOC.COUNTDONNHIEUNGUOIDUNGTENKYTRUOC,
		DONMOTNGUOIDUNGTENKYTRUOC.COUNTDONMOTNGUOIDUNGTENKYTRUOC,
		DONNHIEUNGUOIDUNGTENTRONGKY.COUNTDONNHIEUNGUOIDUNGTENTRONGKY,
		DONMOTNGUOIDUNGTENTRONGKY.COUNTDONMOTNGUOIDUNGTENTRONGKY,
		SODONDAXULY.COUNTSODONDAXULY,
		DONKHIEUNAI.COUNTDONKHIEUNAI,
		DONTOCAO.COUNTDONTOCAO,
		DONKIENNGHI.COUNTDONKIENNGHI,
		DONNHIEUNOIDUNGKHAC.COUNTDONNHIEUNOIDUNGKHAC
	FROM DONNHIEUNGUOIDUNGTENKYTRUOC,DONMOTNGUOIDUNGTENKYTRUOC,
			DONNHIEUNGUOIDUNGTENTRONGKY,DONMOTNGUOIDUNGTENTRONGKY,
			SODONDAXULY,DONKHIEUNAI,DONTOCAO,DONKIENNGHI,DONNHIEUNOIDUNGKHAC;
				
END RPT_KNTC_BAOCAO_TONGHOPKETQUAXULYDON_4B;