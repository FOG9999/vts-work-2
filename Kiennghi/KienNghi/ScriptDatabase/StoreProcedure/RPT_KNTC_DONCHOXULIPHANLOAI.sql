create or replace PROCEDURE "RPT_KNTC_DONCHOXULIPHANLOAI" 
(
    p_IUSER IN NUMBER,
    iLoaiBaoCao in number,
    dTuNgay in DATE,
    dDenNgay in DATE,
    res OUT sys_refcursor
)
IS
BEGIN
  Open res for
    SELECT KNTC_PA.*, ROW_NUMBER() OVER (ORDER BY KNTC_PA.IDOITUONGGUI DESC) AS STT FROM (
          SELECT kntcdon.IDON, kntcdon.IDOITUONGGUI, kntcdon.CNGUOIGUI_TEN, CONCAT(kntcdon.cnguoigui_diachi, CONCAT(', ',CONCAT(diaphuong2.cten,CONCAT(', ',diaphuong1.cten)))) as CNGUOIGUI_DIACHI,
            Case when kntcdon.ILOAIDON = 1 then 'X'
            Else '' End AS CDONKHIEUNAI,
            Case when kntcdon.ILOAIDON = 2 then 'X'
            Else '' End AS CDONTOCAO,
            Case when kntcdon.ILOAIDON = 3 then 'X'
            Else '' End AS CDONKIENNGHIPHANANH,
            Case when kntcdon.ILOAIDON = 4 then 'X'
            Else '' End AS CDONNOIDUNGKHAC,
            Case when ((kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
                        AND (kntcdon.INOIDUNG = 1 OR kntcdon.INOIDUNG = 5 OR kntcdon.INOIDUNG = 9 OR kntcdon.INOIDUNG = 13)) then 'X'
            Else '' End AS CCHEDOCHINHSACH,
            Case when ((kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
                        AND (kntcdon.INOIDUNG = 2 OR kntcdon.INOIDUNG = 6 OR kntcdon.INOIDUNG = 10 OR kntcdon.INOIDUNG = 14)) then 'X'
            Else '' End AS CDATDAINHACUA,
            Case when ((kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
                        AND (kntcdon.INOIDUNG = 3 OR kntcdon.INOIDUNG = 7 OR kntcdon.INOIDUNG = 11 OR kntcdon.INOIDUNG = 15)) then 'X'
            Else '' End AS CCONGCHUCCONGVU,
            Case when ((kntcdon.ILINHVUC = 1 OR kntcdon.ILINHVUC = 6 OR kntcdon.ILINHVUC = 11 OR kntcdon.ILINHVUC = 16)
                        AND (kntcdon.INOIDUNG = 4 OR kntcdon.INOIDUNG = 8 OR kntcdon.INOIDUNG = 12 OR kntcdon.INOIDUNG = 16)) then 'X'
            Else '' End AS CKHAC,
            Case when (kntcdon.ILINHVUC = 2 OR kntcdon.ILINHVUC = 7 OR kntcdon.ILINHVUC = 12 OR kntcdon.ILINHVUC = 17) then 'X'
            Else '' End AS CTUPHAP,
            Case when (kntcdon.ILINHVUC = 3 OR kntcdon.ILINHVUC = 8 OR kntcdon.ILINHVUC = 13 OR kntcdon.ILINHVUC = 18) then 'X'
            Else '' End AS CDANGDOANTHE,
            Case when (kntcdon.ILINHVUC = 4 OR kntcdon.ILINHVUC = 9 OR kntcdon.ILINHVUC = 14 OR kntcdon.ILINHVUC = 19) then 'X'
            Else '' End AS CTHAMNHUNG,
            Case when (kntcdon.ILINHVUC = 5 OR kntcdon.ILINHVUC = 10 OR kntcdon.ILINHVUC = 15 OR kntcdon.ILINHVUC = 20) then 'X'
            Else '' End AS CLINHVUCKHAC,
            Case when kntcdon.IDUDIEUKIEN = 1 then 'X'
            Else '' End AS CDONDUDIEUKIENXULI,
            Case when ((kntcdon.IDONTRUNG <> 0 AND kntcdon.IDONTRUNG <> -1) AND kntcdon.IDUDIEUKIEN = 0) then 'X'
            Else '' End AS CDONTRUNG,
            Case when kntcdon.IDUDIEUKIEN = 0 then 'X'
            Else '' End AS CDONKHONGDUDIEUKIEN,
            '' AS ROWTITLE
            FROM KNTC_DON kntcdon
            LEFT JOIN DIAPHUONG diaphuong1 ON diaphuong1.idiaphuong = kntcdon.idiaphuong_1
            LEFT JOIN DIAPHUONG diaphuong2 ON diaphuong2.idiaphuong = kntcdon.idiaphuong_2
            LEFT JOIN LINHVUC lv ON lv.ILINHVUC = kntcdon.ILINHVUC
            WHERE (kntcdon.ITINHTRANGXULY = 1 OR kntcdon.ITINHTRANGXULY = 2)
                AND (p_IUSER = 0 OR kntcdon.IUSER = p_IUSER)
                AND (dTuNgay IS NULL OR dTuNgay = ''  OR kntcdon.DNGAYNHAN >= dTuNgay)
                AND (dDenNgay IS NULL OR dDenNgay = '' OR kntcdon.DNGAYNHAN <= dDenNgay)
                AND (iLoaiBaoCao = 0 OR (iLoaiBaoCao = 1 AND kntcdon.IDOITUONGGUI = 1) OR (iLoaiBaoCao = 2 and kntcdon.IDOITUONGGUI = 0))
                AND kntcdon.IDELETE = 0
            ORDER BY kntcdon.IDOITUONGGUI DESC
        ) KNTC_PA;
END RPT_KNTC_DONCHOXULIPHANLOAI;