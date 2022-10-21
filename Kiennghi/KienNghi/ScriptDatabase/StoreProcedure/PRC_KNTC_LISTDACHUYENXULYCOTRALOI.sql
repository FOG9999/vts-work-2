create or replace PROCEDURE PRC_KNTC_LISTDACHUYENXULYCOTRALOI 
    (res out sys_refcursor, 
    P_USER in INT, 
    P_CKEY in NVARCHAR2, 
    P_IKHOA IN INT, 
    P_IDOANDONGNGUOI IN INT, 
    P_DTUNGAY IN DATE , 
    P_DDENNGAY IN DATE, 
    P_INGUONDON IN INT ,
    P_IQUANHUYEN in INT, 
    P_IXAPHUONG in INT, 
    P_IQUOCTICH IN INT, 
    P_IDANTOC IN INT, 
    P_ILOAIDON IN INT,
    P_ILINHVUC IN INT,
    P_INGUOINHAN IN INT, 
    P_INOIDUNG IN INT, 
    P_ITINHCHAT IN INT, 
    P_IDONVI IN INT, 
    P_ICHUYENXULY IN INT, 
    P_ITINHTRANGXULY IN INT, 
    P_IUSER_GIAOXULY IN INT, 
    P_PAGE IN INT , P_PAGE_SIZE IN INT)
    IS
    BEGIN
    OPEN res FOR 
    WITH LIST_DACHUYENCOTRALOI AS(
    SELECT distinct  A.IDON,A.CNOIDUNG,A.CNGUOIGUI_DIACHI,TINH.CTEN AS CTENTINH,HUYEN.CTEN AS CTENHUYEN,A.DNGAYNHAN,A.IDONVITHULY,DVTHULY.CTEN,A.ITHAMQUYEN,A.IDUDIEUKIEN,A.ITINHTRANGXULY
        ,A.IUSER,A.IDONTRUNG,A.CNGUOIGUI_TEN,A.IUSER_DUOCGIAOXULY,A.IDUDIEUKIEN_KETQUA,A.ILUUTHEODOI,A.INGAYQUYDINH,A.ICANHBAO,DVTIEPNHAN.CTEN AS DONVITIEPNHAN,A.IDONVITIEPNHAN,A.IDANHGIA,A.CGHICHUDANHGIA
    FROM (SELECT KNTC_DON.* FROM KNTC_DON_LICHSU INNER JOIN KNTC_DON ON KNTC_DON_LICHSU.IDON = KNTC_DON.IDON WHERE KNTC_DON_LICHSU.IDONVIGUI=P_IDONVI AND KNTC_DON_LICHSU.ICHUYENXULY = P_ICHUYENXULY AND KNTC_DON.IDELETE = 0) A
    LEFT JOIN QUOCHOI_COQUAN DVTHULY ON DVTHULY.ICOQUAN = A.IDONVITHULY
    LEFT JOIN DIAPHUONG TINH ON TINH.IDIAPHUONG = A.IDIAPHUONG_0
    LEFT JOIN DIAPHUONG HUYEN ON HUYEN.IDIAPHUONG =A.IDIAPHUONG_1
    LEFT JOIN QUOCHOI_COQUAN DVTIEPNHAN ON A.IDONVITIEPNHAN = DVTIEPNHAN.ICOQUAN
    WHERE 1=1 
        AND ( ( P_USER =0 or   A.IUSER = P_USER) OR (P_IUSER_GIAOXULY = 0 OR A.IUSER_GIAOXULY = P_IUSER_GIAOXULY or A.IUSER_DUOCGIAOXULY = P_USER ))
        AND (P_ITINHCHAT = 0 OR A.ITINHCHAT = P_ITINHCHAT)
        AND (P_INGUOINHAN = 0 OR A.IUSER = P_INGUOINHAN)
        AND (P_INGUONDON = 0 OR A.INGUONDON = P_INGUONDON)
        AND (P_INOIDUNG = 0 OR A.INOIDUNG = P_INOIDUNG)
        AND (P_ILINHVUC = 0 OR A.ILINHVUC = P_ILINHVUC)
        AND (P_ILOAIDON = 0 OR A.ILOAIDON = P_ILOAIDON)
        AND (P_IDANTOC = 0 OR A.INGUOIGUI_DANTOC =P_IDANTOC)
        AND (P_IUSER_GIAOXULY = 0 OR A.IUSER_GIAOXULY =P_IUSER_GIAOXULY)
        AND (P_IQUOCTICH = 0 OR A.INGUOIGUI_QUOCTICH = P_IQUOCTICH)
        AND (P_IQUANHUYEN = 0 OR A.IDIAPHUONG_1 = P_IQUANHUYEN)
        AND (P_IXAPHUONG = 0 OR A.IDIAPHUONG_2 = P_IXAPHUONG)
        AND (P_IDOANDONGNGUOI = 0 OR IDOANDONGNGUOI = P_IDOANDONGNGUOI)
        AND (A.ITINHTRANGXULY = 6 OR A.ITINHTRANGXULY = 7 OR A.ITINHTRANGXULY = 8 OR A.ITINHTRANGXULY = 9)
        AND (P_ITINHTRANGXULY = 0 OR A.ITINHTRANGXULY = P_ITINHTRANGXULY)
        AND (P_IKHOA = 0 OR A.IKHOA = P_IKHOA)
        AND (P_DTUNGAY ='' OR P_DTUNGAY IS NULL OR A.DNGAYNHAN >= P_DTUNGAY )
        AND (P_DDENNGAY ='' OR P_DDENNGAY IS NULL OR A.DNGAYNHAN <= P_DDENNGAY)
        AND (P_CKEY='' OR P_CKEY IS NULL OR (UPPER(A.CNOIDUNG) LIKE '%' || UPPER(P_CKEY) || '%' OR UPPER(A.CNGUOIGUI_TEN) LIKE '%' || UPPER(P_CKEY) || '%'  OR UPPER(A.CNGUOIGUI_DIACHI) LIKE '%' || UPPER(P_CKEY) || '%'))
        ORDER BY A.IDON DESC)
    SELECT * FROM
    (
        SELECT k.*,(SELECT COUNT(*) FROM LIST_DACHUYENCOTRALOI) AS TOTAL,
        ROWNUM STT FROM( select * from LIST_DACHUYENCOTRALOI ) k
        WHERE ROWNUM < ((P_PAGE * P_PAGE_SIZE) + 1 )
    )
    WHERE STT >=    (((P_PAGE-1) * P_PAGE_SIZE) + 1) ORDER BY STT;
END PRC_KNTC_LISTDACHUYENXULYCOTRALOI;