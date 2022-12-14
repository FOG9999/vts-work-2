CREATE OR REPLACE
PACKAGE BODY "PKG_TCD_BAOCAO" 
AS
----Bao cao 1
  PROCEDURE  PRO_BAOCAO_TK_PHULUC(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT , pram05 in INT) AS
  BEGIN
    OPEN res FOR
        --    
   SELECT 
   DISTINCT
 EXTRACT(YEAR FROM pram02)  AS THOIGIAN ,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC
  WHERE TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02
  AND TD_VUVIEC.IDONVI = pram05 ) AS TONGSO,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC
  WHERE ITINHTRANGXULY != 2 
  AND TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02 
  AND TD_VUVIEC.IDONVI = pram05  )
  AS TIEPCONGDANTHEOLINHVUC ,
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC
 WHERE IDOANDONGNGUOI = 0 
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02
 AND TD_VUVIEC.IDONVI = pram05)
 AS TIEPCONGDANCUACANHANDBQH,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC 
  WHERE TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02
   AND TD_VUVIEC.IDONVI = pram05 
  ) 
  AS SOVUVIEC,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC,KNTC_LOAIDON
  WHERE TD_VUVIEC.ILOAIDON = KNTC_LOAIDON.ILOAIDON
  AND KNTC_LOAIDON.IMALOAIDON = 1 
  AND TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02 
  AND TD_VUVIEC.IDONVI = pram05 ) 
  AS KHIEUNAI,
  (SELECT COUNT(IVUVIEC) 
  FROM TD_VUVIEC,KNTC_LOAIDON
  WHERE TD_VUVIEC.ILOAIDON = KNTC_LOAIDON.ILOAIDON
  AND KNTC_LOAIDON.IMALOAIDON = 2
  AND TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02
  AND TD_VUVIEC.IDONVI = pram05 )
  AS TOCAO, 
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC,KNTC_LOAIDON
  WHERE TD_VUVIEC.ILOAIDON = KNTC_LOAIDON.ILOAIDON 
  AND KNTC_LOAIDON.IMALOAIDON = 3
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02 
  AND TD_VUVIEC.IDONVI = pram05 )
  AS KHIENNGHIPHANANH, 
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC,LINHVUC
  WHERE LINHVUC.ILINHVUC = TD_VUVIEC.ILINHVUC 
  AND LINHVUC.INHOM = 1
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02
  AND TD_VUVIEC.IDONVI = pram05 ) 
  AS HANHCHINH , 
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC,LINHVUC 
  WHERE LINHVUC.ILINHVUC = TD_VUVIEC.ILINHVUC
  AND LINHVUC.INHOM = 2
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02
  AND TD_VUVIEC.IDONVI = pram05 ) 
  AS TUPHAP,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC WHERE IDOANDONGNGUOI = 1
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02
 AND TD_VUVIEC.IDONVI = pram05 ) 
 AS DOANDONGNGUOI,
   (SELECT COUNT(IVUVIEC) 
   FROM TD_VUVIEC
   WHERE ITINHTRANGXULY = 1
   AND TD_VUVIEC.DNGAYNHAN >= pram01 
   AND TD_VUVIEC.DNGAYNHAN <= pram02
   AND TD_VUVIEC.IDONVI = pram05 ) 
   AS HUONGDANBANGVANBAN,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC
  WHERE ITINHTRANGXULY = 0
  AND TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02
  AND TD_VUVIEC.IDONVI = pram05 ) 
  AS HUONGDANGIAITHICHTRUCTIEP, 
   (SELECT COUNT(IVUVIEC)
   FROM TD_VUVIEC
   WHERE ITINHTRANGXULY = 3
   AND TD_VUVIEC.DNGAYNHAN >= pram01
   AND TD_VUVIEC.DNGAYNHAN <= pram02
   AND TD_VUVIEC.IDONVI = pram05 ) AS CHUYENDENCOQUANCOTHAMQUYEN


   FROM TD_VUVIEC WHERE( pram03 = 0 OR ILOAIDON = pram03) AND (ILINHVUC = pram04 OR pram04 = 0 ) AND (IDONVI = pram05)  ; 

  END PRO_BAOCAO_TK_PHULUC;
----Bao cao 2
  PROCEDURE PRO_BAOCAO_TK_SOLIEUTINH(res out sys_refcursor, pram01 in DATE, pram02 IN DATE , pram03 in INT , pram04 in INT,pram05 in INT)AS 
  BEGIN
    OPEN res FOR
 SELECT
 CTEN AS DIAPHUONG, 
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0 
 AND DIAPHUONG.IDELETE =0  AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02 
 AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)

 AND (TD_VUVIEC.IDONVI = pram05) ) 
 AS SOBUOITCD ,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02 
 AND(TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)

 AND (TD_VUVIEC.IDONVI = pram05))
 AS LUOTNGUOI,
  (SELECT COUNT(IVUVIEC) 
  FROM TD_VUVIEC 
  WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
  AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02
 AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)

  AND (TD_VUVIEC.IDONVI = pram05) )
  AS SOVUVIEC,
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC 
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.IDOANDONGNGUOI = 0 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05))
 AS DOANDONGNGUOI,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC
  WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
  AND  DIAPHUONG.IPARENT = 0
  AND DIAPHUONG.IDELETE =0
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02
 AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
  AND (TD_VUVIEC.IDONVI = pram05))
  AS TONGDONNHAN ,
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC WHERE
  TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
  AND  DIAPHUONG.IPARENT = 0 
  AND DIAPHUONG.IDELETE =0 
  AND TD_VUVIEC.DNGAYNHAN >= pram01
  AND TD_VUVIEC.DNGAYNHAN <= pram02 
  AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
  AND (TD_VUVIEC.IDONVI = pram05) )
  AS KHIEUNAI,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC WHERE 
 TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
 AND  DIAPHUONG.IPARENT = 0 
 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05) )
 AS TOCAO , 
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0 
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05))
AS KIENNGHIPHANANH,
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC 
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
 AND  DIAPHUONG.IPARENT = 0
 AND DIAPHUONG.IDELETE =0
 AND TD_VUVIEC.IVUVIECTRUNG != 0
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)

 AND (TD_VUVIEC.IDONVI = pram05) ) AS DONTRUNG, 
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC,KNTC_NOIDUNGDON
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0 
AND TD_VUVIEC.INOIDUNG = KNTC_NOIDUNGDON.IMANOIDUNG 
AND KNTC_NOIDUNGDON.IMANOIDUNG  =1
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05) ) 
AS DATDAI , 
 (SELECT
 COUNT(IVUVIEC)
 FROM TD_VUVIEC,KNTC_NOIDUNGDON
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
 AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0  
 AND TD_VUVIEC.INOIDUNG = KNTC_NOIDUNGDON.IMANOIDUNG 
 AND KNTC_NOIDUNGDON.IMANOIDUNG  = 2
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05) )
 AS CHINHSACHXH,
(SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC,KNTC_NOIDUNGDON
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0  
 AND TD_VUVIEC.INOIDUNG = KNTC_NOIDUNGDON.IMANOIDUNG 
 AND KNTC_NOIDUNGDON.IMANOIDUNG  = 3
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05))
 AS VIPHAMPLTHAMNHUNG,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC,KNTC_NOIDUNGDON
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
 AND  DIAPHUONG.IPARENT = 0
 AND DIAPHUONG.IDELETE =0   
 AND TD_VUVIEC.INOIDUNG = KNTC_NOIDUNGDON.IMANOIDUNG  
 AND KNTC_NOIDUNGDON.IMANOIDUNG  = 4 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05)) AS QUANLIKINHTEXH,
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC,KNTC_NOIDUNGDON
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0 
AND DIAPHUONG.IDELETE =0  
AND KNTC_NOIDUNGDON.IMANOIDUNG  > 4 
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05  ))  AS KHAC ,
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC,LINHVUC
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
AND  DIAPHUONG.IPARENT = 0 
AND DIAPHUONG.IDELETE =0
AND TD_VUVIEC.ILINHVUC = LINHVUC.ILINHVUC 
AND INHOM = 2
AND TD_VUVIEC.DNGAYNHAN >= pram01 
AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05)) AS TUPHAP, 
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC,LINHVUC
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0
AND DIAPHUONG.IDELETE =0
AND TD_VUVIEC.ILINHVUC = LINHVUC.ILINHVUC
AND INHOM = 1
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND( TD_VUVIEC.IDONVI = pram05  )) AS HANHCHINH , 
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC,LINHVUC
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
AND  DIAPHUONG.IPARENT = 0 
AND DIAPHUONG.IDELETE =0
AND TD_VUVIEC.ILINHVUC = LINHVUC.ILINHVUC 
AND INHOM = 3 
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05 )) AS LINHVUCKHAC,  
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0
 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.ITINHTRANGXULY = 0 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
 AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05)) AS DANGNGHIENCUU,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
 AND  DIAPHUONG.IPARENT = 0 
 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.ITINHTRANGXULY = 3 
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05)) AS SODONLUUTHEODOI ,
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC 
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0
 AND DIAPHUONG.IDELETE =0
 AND TD_VUVIEC.ITINHTRANGXULY = 2
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05)) AS SOVUVIECDACHUYEN, 
(SELECT COUNT(IXULY)
FROM TD_VUVIEC,TD_VUVIEC_XULY 
WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0
AND TD_VUVIEC.ITINHTRANGXULY = 3 
AND  TD_VUVIEC.IVUVIEC = TD_VUVIEC_XULY.IVUVIEC 
AND TD_VUVIEC_XULY.CLOAI='traloichuyenxuly' 
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05 )) AS SOVUVUDATRALOI, 
  (SELECT COUNT(IVUVIEC)
  FROM TD_VUVIEC WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG
  AND  DIAPHUONG.IPARENT = 0 
  AND DIAPHUONG.IDELETE =0 
  AND TD_VUVIEC.ITINHTRANGXULY = 1 
  AND TD_VUVIEC.DNGAYNHAN >= pram01 
  AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
  AND (TD_VUVIEC.IDONVI = pram05 ))
  AS HUONGDANTRALOI, 
 (SELECT COUNT(IVUVIEC) 
 FROM TD_VUVIEC 
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0 
 AND (TD_VUVIEC.ITINHTRANGXULY = 1 OR TD_VUVIEC.ITINHTRANGXULY = 2 OR TD_VUVIEC.ITINHTRANGXULY = 3) 
 AND TD_VUVIEC.DNGAYNHAN >= pram01 
 AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05  )) AS TYLE, 
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0
AND DIAPHUONG.IDELETE =0 
AND TD_VUVIEC.IDONDOC = 2
AND TD_VUVIEC.DNGAYNHAN >= pram01
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND TD_VUVIEC.ILINHVUC = pram04
AND (TD_VUVIEC.IDONVI = pram05) ) AS DONDOCVUVIECCUTHE,
 (SELECT COUNT(IVUVIEC)
 FROM TD_VUVIEC
 WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
 AND  DIAPHUONG.IPARENT = 0
 AND DIAPHUONG.IDELETE =0 
 AND TD_VUVIEC.IGIAMSAT = 1
 AND TD_VUVIEC.DNGAYNHAN >= pram01
 AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
 AND (TD_VUVIEC.IDONVI = pram05 )) AS CHUYENDE,
(SELECT COUNT(IVUVIEC) 
FROM TD_VUVIEC WHERE TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0 
AND DIAPHUONG.IDELETE =0 
AND TD_VUVIEC.IGIAMSAT = 2 
AND TD_VUVIEC.DNGAYNHAN >= pram01 
AND TD_VUVIEC.DNGAYNHAN <= pram02  
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05)) AS LONGGHEP,
(SELECT COUNT(IVUVIEC)
FROM TD_VUVIEC WHERE
TD_VUVIEC.IDIAPHUONG_0 = DIAPHUONG.IDIAPHUONG 
AND  DIAPHUONG.IPARENT = 0
AND DIAPHUONG.IDELETE =0 
AND TD_VUVIEC.IGIAMSAT = 3 
AND TD_VUVIEC.DNGAYNHAN >= pram01 
AND TD_VUVIEC.DNGAYNHAN <= pram02 
AND (TD_VUVIEC.ILOAIDON = pram03 OR pram03 = 0)
AND (TD_VUVIEC.IDONVI = pram05 )) AS VUVIECCUTHE ,
DIAPHUONG.IDIAPHUONG AS IDDIAPHUONG
FROM DIAPHUONG
  WHERE DIAPHUONG.IPARENT = 0 AND DIAPHUONG.IDELETE =0;

  END PRO_BAOCAO_TK_SOLIEUTINH;
  ----Bao cao 3
END PKG_TCD_BAOCAO;

