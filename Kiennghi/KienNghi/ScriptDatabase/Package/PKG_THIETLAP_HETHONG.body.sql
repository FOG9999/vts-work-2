CREATE OR REPLACE
PACKAGE BODY "PKG_THIETLAP_HETHONG" AS

  PROCEDURE PRC_NGUOIDUNG(res out sys_refcursor, p_TUKHOA IN NVARCHAR2                                              
                                                               ) AS
  BEGIN
    -- TODO: Implementation required for PROCEDURE PKG_THIETLAP_HETHONG.PRC_NGUOIDUNG
     Open res for
       WITH TBL1 AS(
        SELECT USERS.IUSER AS ID_USER, USERS.CUSERNAME AS USERNAME,USERS.CTEN AS USER_TEN,
              USERS.CEMAIL AS USER_EMAIL,USERS.CSDT AS USER_SDT, USERS.ISTATUS AS USER_STATUS,
              QUOCHOI_COQUAN.CTEN AS USER_TENDONVI,
              COQUAN_PARENT.IVITRI AS VITRI_DONVI,
              USERS.IDONVI AS USER_ID_DONVI,
              USER_PHONGBAN.CTEN AS USER_TENPHONGBAN,
              USERS.IPHONGBAN AS USER_ID_PHONGBAN,
              USER_CHUCVU.CTEN AS USER_TENCHUCVU,
              USERS.CARRGROUP AS USER_NHOM
        FROM USERS INNER JOIN QUOCHOI_COQUAN ON QUOCHOI_COQUAN.ICOQUAN=USERS.IDONVI
                    INNER JOIN QUOCHOI_COQUAN COQUAN_PARENT ON QUOCHOI_COQUAN.IPARENT=COQUAN_PARENT.ICOQUAN
                    LEFT JOIN USER_PHONGBAN ON USER_PHONGBAN.IPHONGBAN=USERS.IPHONGBAN
                    LEFT JOIN USER_CHUCVU ON USER_CHUCVU.ICHUCVU=USERS.ICHUCVU
        WHERE USERS.IUSER!=1 
        AND( UPPER(USERS.CUSERNAME) LIKE '%' || UPPER(p_TUKHOA) || '%'  OR UPPER(USERS.CTEN) LIKE '%' || UPPER(p_TUKHOA) || '%')
        ),
        TBL2 AS (SELECT IGROUP AS IDNHOMQUYEN,CTEN AS TENNHOMQUYEN FROM USER_GROUP)
        SELECT TBL1.*,TBL2.* FROM TBL1, TBL2 ORDER BY TBL1.USER_ID_DONVI,TBL1.USER_ID_PHONGBAN,TBL1.ID_USER;



  END PRC_NGUOIDUNG;

  PROCEDURE PRC_NGUOIDUNG_OPT(res out sys_refcursor, p_TUKHOA IN NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT ) AS
  BEGIN
    -- TODO: Implementation required for PROCEDURE PKG_THIETLAP_HETHONG.PRC_NGUOIDUNG
     Open res for

      WITH TBL1 AS(
        SELECT USERS.IUSER AS ID_USER, USERS.CUSERNAME AS USERNAME,USERS.CTEN AS USER_TEN,
              USERS.CEMAIL AS USER_EMAIL,USERS.CSDT AS USER_SDT, USERS.ISTATUS AS USER_STATUS,
              QUOCHOI_COQUAN.CTEN AS USER_TENDONVI,
              COQUAN_PARENT.IVITRI AS VITRI_DONVI,
              USERS.IDONVI AS USER_ID_DONVI,
              USER_PHONGBAN.CTEN AS USER_TENPHONGBAN,
              USERS.IPHONGBAN AS USER_ID_PHONGBAN,
              USER_CHUCVU.CTEN AS USER_TENCHUCVU,
              USERS.CARRGROUP AS USER_NHOM
        FROM USERS INNER JOIN QUOCHOI_COQUAN ON QUOCHOI_COQUAN.ICOQUAN=USERS.IDONVI
                    INNER JOIN QUOCHOI_COQUAN COQUAN_PARENT ON QUOCHOI_COQUAN.IPARENT=COQUAN_PARENT.ICOQUAN
                    LEFT JOIN USER_PHONGBAN ON USER_PHONGBAN.IPHONGBAN=USERS.IPHONGBAN
                    LEFT JOIN USER_CHUCVU ON USER_CHUCVU.ICHUCVU=USERS.ICHUCVU
        WHERE 
        USERS.IUSER!=1 AND
        ( UPPER(USERS.CUSERNAME) LIKE '%' || UPPER(p_TUKHOA) || '%'  OR UPPER(USERS.CTEN) LIKE '%' || UPPER(p_TUKHOA) || '%')
        ),
       TD_PHANTRANG AS(
        SELECT TBL1.* FROM TBL1
        ORDER BY TBL1.USER_ID_DONVI,TBL1.USER_ID_PHONGBAN,TBL1.ID_USER)
SELECT * FROM 
(
   SELECT k.*,(SELECT COUNT(*) FROM TD_PHANTRANG) AS TOTAL, 
   ROWNUM r_ FROM( select * from TD_PHANTRANG ) k
   WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
) 
 WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1); 

  END PRC_NGUOIDUNG_OPT;
   PROCEDURE PKG_COQUANGOC(res out sys_refcursor  ) AS
  BEGIN
   Open res for
   SELECT
    QUOCHOI_COQUAN.ICOQUAN AS IDCOQUAN,
 QUOCHOI_COQUAN.CTEN AS CTENCOQUAN,
 QUOCHOI_COQUAN.IPARENT AS IDPARENT,
 QUOCHOI_COQUAN.CCODE AS CMACOQUAN,
 QUOCHOI_COQUAN.IGROUP AS IDGROUP,
 QUOCHOI_COQUAN.IVITRI AS  IDVITRI,
 QUOCHOI_COQUAN.IUSE AS IDUSER,
 QUOCHOI_COQUAN.IDELETE AS IDDELETE,
 TINH.CTEN AS CTENDIAPHUONG

    FROM QUOCHOI_COQUAN 
    LEFT JOIN DIAPHUONG TINH ON TINH.IDIAPHUONG = QUOCHOI_COQUAN.IDIAPHUONG
    WHERE QUOCHOI_COQUAN.IPARENT = 0 AND QUOCHOI_COQUAN.IDELETE = 0   ORDER BY QUOCHOI_COQUAN.IVITRI;

  END PKG_COQUANGOC;
   PROCEDURE PKG_COQUAN_CAPKHAC(res out sys_refcursor , IDPARENT IN INT ) AS
  BEGIN
   Open res for
  SELECT  
    QUOCHOI_COQUAN.ICOQUAN AS IDCOQUAN,
 QUOCHOI_COQUAN.CTEN AS CTENCOQUAN,
 QUOCHOI_COQUAN.IPARENT AS IDPARENT,
 QUOCHOI_COQUAN.CCODE AS CMACOQUAN,
 QUOCHOI_COQUAN.IGROUP AS IDGROUP,
 QUOCHOI_COQUAN.IVITRI AS  IDVITRI,
 QUOCHOI_COQUAN.IUSE AS IDUSER,
 QUOCHOI_COQUAN.IDELETE AS IDDELETE,
 TINH.CTEN AS CTENDIAPHUONG

    FROM QUOCHOI_COQUAN 
     LEFT JOIN DIAPHUONG TINH ON TINH.IDIAPHUONG = QUOCHOI_COQUAN.IDIAPHUONG
    WHERE QUOCHOI_COQUAN.IPARENT = IDPARENT
    AND QUOCHOI_COQUAN.IDELETE = 0  
    ORDER BY QUOCHOI_COQUAN.IVITRI;

  END PKG_COQUAN_CAPKHAC;


  PROCEDURE PKG_TIMKIEMLICHSU(res out sys_refcursor ,NGAYBATDAU IN DATE , NGAYKETHUC IN DATE  ,CTUKHOA in NVARCHAR2 ) AS
  BEGIN
   Open res for

   SELECT * FROM TRACKING 
WHERE DDATE >= TO_DATE(NGAYBATDAU) AND DDATE <= TO_DATE(NGAYKETHUC) AND   UPPER(CACTION) LIKE '%' || UPPER(CTUKHOA) || '%';
  END PKG_TIMKIEMLICHSU;
  PROCEDURE PKG_TIMKIEMTHEODONVI(res out sys_refcursor , IMADONVI IN INT  ,CTUKHOA in NVARCHAR2 ) AS
  BEGIN
   Open res for

   SELECT * 
   FROM USERS 
WHERE (IDONVI = IMADONVI OR IMADONVI = -1) AND (  UPPER(CTEN) LIKE '%' || UPPER(CTUKHOA) || '%' OR  UPPER(CUSERNAME) LIKE '%' || UPPER(CTUKHOA) || '%');
  END PKG_TIMKIEMTHEODONVI;


PROCEDURE PKG_COQUAN_PHANTRANG(res out sys_refcursor , p_TUKHOA IN NVARCHAR2, p_PAGE IN INT, p_PAGE_SIZE IN INT ) AS
  BEGIN
   Open res for

WITH TD_PHANTRANG AS (  select 
quochoi_coquan.ICOQUAN AS ICOQUAN,
quochoi_coquan.CTEN AS CTEN,
quochoi_coquan.IPARENT AS IPARENT,
quochoi_coquan.CCODE AS CCODE,
quochoi_coquan.IMACDINH AS IMACDINH,
quochoi_coquan.IDIAPHUONG AS IDIAPHUONG,
quochoi_coquan.IGROUP AS IGROUP,
quochoi_coquan.IVITRI AS IVITRI,
quochoi_coquan.IUSE AS IUSE,
quochoi_coquan.IHIENTHI AS IHIENTHI,
quochoi_coquan.IDELETE AS IDELETE
 from quochoi_coquan
 where IDELETE = 0 and IHIENTHI = 1 AND ( UPPER(quochoi_coquan.CTEN) LIKE '%' || UPPER(p_TUKHOA) || '%'  )
start with iparent=0
connect by prior icoquan=iparent
order siblings by ivitri  
)

   SELECT * FROM 
(
   SELECT k.*,(SELECT COUNT(*) FROM TD_PHANTRANG) AS TOTAL, 
   ROWNUM r_ FROM( select * from TD_PHANTRANG ) k
   WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
) 
 WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1); 
  END PKG_COQUAN_PHANTRANG;  

PROCEDURE PRC_DAIBIEU_PHANTRANG(res out sys_refcursor, p_DAIBIEU IN INT, p_KHOA IN INT, p_LOAIDAIBIEU IN INT ,p_PAGE IN INT, p_PAGE_SIZE IN INT) AS
 BEGIN 
         OPEN res FOR
         WITH TD_PHANTRANG AS (  select *
         from DAIBIEU d 
				 left join DAIBIEU_KYHOP a
				 on d.IDAIBIEU = a.ID_DAIBIEU
         WHERE d.IDELETE = 0 AND (d.IDAIBIEU = p_DAIBIEU OR p_DAIBIEU = 0)
				 AND (a.ID_KYHOP = p_KHOA OR p_KHOA = 0)
				 AND (d.ILOAIDAIBIEU = p_LOAIDAIBIEU OR p_LOAIDAIBIEU = -1)
				 ORDER BY d.IDIAPHUONG
         )
         SELECT * FROM 
         (
            SELECT k.*,(SELECT COUNT(*) FROM TD_PHANTRANG) AS TOTAL, 
            ROWNUM r_ FROM( select * from TD_PHANTRANG ) k
            WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
         ) 
         WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1); 
 END PRC_DAIBIEU_PHANTRANG; 

PROCEDURE PKG_DIAPHUONG_PHANTRANG(res out sys_refcursor , p_TUKHOA in NVARCHAR2, p_PAGE IN INT, p_PAGE_SIZE IN INT ) AS
  BEGIN
   Open res for
WITH DIAPHUONG0 AS(SELECT IDIAPHUONG AS IDIAPHUONG_PARENT,
CTEN AS CTENDIAPHUONG_PARENT,
IPARENT AS IDPARENT_PARENT,
CCODE AS CMADIAPHUONG_PARENT,
CTYPE AS TYPE_PARENT,
IHIENTHI AS IDHIENTHI_PARENT,
IDELETE AS IDDELETE_PARENT
FROM DIAPHUONG WHERE IPARENT=0 AND IDELETE=0 ORDER BY CTENDIAPHUONG_PARENT),
TD_PHANTRANG AS ( 
SELECT
IDIAPHUONG AS IDIAPHUONG,
CTEN AS CTENDIAPHUONG,
IPARENT AS IDPARENT,
CCODE AS CMADIAPHUONG,
CTYPE AS TYPE_,
IHIENTHI AS IDHIENTHI,
IDELETE AS IDDELETE,
DIAPHUONG0.IDIAPHUONG_PARENT,
DIAPHUONG0.CTENDIAPHUONG_PARENT,
DIAPHUONG0.IDPARENT_PARENT,
DIAPHUONG0.CMADIAPHUONG_PARENT,
DIAPHUONG0.TYPE_PARENT,
DIAPHUONG0.IDHIENTHI_PARENT,
DIAPHUONG0.IDDELETE_PARENT
  FROM DIAPHUONG 
  INNER JOIN DIAPHUONG0 ON DIAPHUONG0.IDIAPHUONG_PARENT=DIAPHUONG.IPARENT 
  WHERE IDELETE = 0
	AND ( UPPER(CTEN) LIKE '%' || UPPER(p_TUKHOA) || '%' )
		ORDER BY DIAPHUONG0.CTENDIAPHUONG_PARENT)

   SELECT * FROM 
(
   SELECT k.*,(SELECT COUNT(*) FROM TD_PHANTRANG) AS TOTAL, 
   ROWNUM r_ FROM( select * from TD_PHANTRANG ) k
   WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
) 
 WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1); 
  END PKG_DIAPHUONG_PHANTRANG;  



 PROCEDURE PKG_DIAPHUONG(res out sys_refcursor ) AS
  BEGIN
   Open res for
WITH DIAPHUONG0
AS
(SELECT IDIAPHUONG AS IDIAPHUONG_PARENT,
CTEN AS CTENDIAPHUONG_PARENT,
IPARENT AS IDPARENT_PARENT,
CCODE AS CMADIAPHUONG_PARENT,
CTYPE AS TYPE_PARENT,
IHIENTHI AS IDHIENTHI_PARENT,
IDELETE AS IDDELETE_PARENT
FROM DIAPHUONG WHERE IPARENT=0 AND IDELETE=0 ORDER BY CTENDIAPHUONG_PARENT)
SELECT
IDIAPHUONG AS IDIAPHUONG,
CTEN AS CTENDIAPHUONG,
IPARENT AS IDPARENT,
CCODE AS CMADIAPHUONG,
CTYPE AS TYPE_,
IHIENTHI AS IDHIENTHI,
IDELETE AS IDDELETE,
DIAPHUONG0.IDIAPHUONG_PARENT,
DIAPHUONG0.CTENDIAPHUONG_PARENT,
DIAPHUONG0.IDPARENT_PARENT,
DIAPHUONG0.CMADIAPHUONG_PARENT,
DIAPHUONG0.TYPE_PARENT,
DIAPHUONG0.IDHIENTHI_PARENT,
DIAPHUONG0.IDDELETE_PARENT
  FROM DIAPHUONG 
  INNER JOIN DIAPHUONG0 ON DIAPHUONG0.IDIAPHUONG_PARENT=DIAPHUONG.IPARENT 
  WHERE IDELETE = 0  ORDER BY DIAPHUONG0.CTENDIAPHUONG_PARENT;

  END PKG_DIAPHUONG; 






  PROCEDURE PKG_LICHSU(res out sys_refcursor , NGAYBATDAU IN DATE , NGAYKETHUC IN DATE ,CTUKHOANOIDUNG IN NVARCHAR2,IMADONVI IN INT ,CTUKHOANGUOIDUNG IN NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT ) AS
  BEGIN
   Open res for
   WITH PHANTRANG AS(
    SELECT 
  TRACKING.DDATE AS THOIGIAN,
  USERS.CUSERNAME AS USERNAME,
   USERS.CTEN AS TEN,
   TRACKING.CACTION AS NOIDUNG
  FROM TRACKING 
    INNER JOIN USERS   ON  TRACKING.IUSER = USERS.IUSER AND (USERS.IDONVI = IMADONVI OR IMADONVI = -1)AND (  UPPER(USERS.CTEN) LIKE '%' || UPPER(CTUKHOANGUOIDUNG) || '%' OR  UPPER(USERS.CUSERNAME) LIKE '%' || UPPER(CTUKHOANGUOIDUNG) || '%')
WHERE TO_DATE(DDATE) >= NGAYBATDAU AND TO_DATE(DDATE) <= NGAYKETHUC AND   UPPER(CACTION) LIKE '%' || UPPER(CTUKHOANOIDUNG) || '%'
order by TRACKING.IUSER)
 SELECT * FROM 
(
   SELECT k.*,(SELECT COUNT(*) FROM PHANTRANG) AS TOTAL, 
   ROWNUM r_ FROM( select * from PHANTRANG ) k
   WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
) 
 WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1);

  END PKG_LICHSU;  
  PROCEDURE PRC_PHANTRANG_VANBAN(res out sys_refcursor,NGAYBATDAU IN DATE,NGAYKETTHUC IN DATE,CTUKHOA IN NVARCHAR2,IMADONVI IN INT ,LOAIVANBAN IN INT,LINHVUC IN INT, TRANGTHAI IN INT , KYHOP IN INT,p_PAGE IN INT, p_PAGE_SIZE IN INT) AS
  BEGIN
     OPEN res FOR
 WITH PHANTRANG AS(
    SELECT 
VB_VANBAN.IVANBAN AS IDVANBAN,
VB_VANBAN.CTIEUDE AS TIEUDE,
VB_VANBAN.CTRICHYEU AS TRICHYEU,
VB_VANBAN.IHIENTHI AS HIENTHI,
VB_VANBAN.DDATECREATE AS DATECREATE,
LINHVUC.CTEN AS TENLINHVUC,
VB_LOAI.CTEN AS TENLOAIVB,
QUOCHOI_KYHOP.CTEN AS TENKYHOP
FROM VB_VANBAN 
LEFT JOIN VB_DONVI_VANBAN ON VB_DONVI_VANBAN.IVANBAN = VB_VANBAN.IVANBAN
LEFT JOIN QUOCHOI_KYHOP ON QUOCHOI_KYHOP.IKYHOP = VB_VANBAN.IKYHOP   AND QUOCHOI_KYHOP.IDELETE = 0
LEFT JOIN LINHVUC ON VB_VANBAN.ILINHVUC = LINHVUC.ILINHVUC AND LINHVUC.IDELETE = 0
LEFT JOIN VB_LOAI ON VB_LOAI.ILOAI = VB_VANBAN.ILOAI AND VB_LOAI.IDELETE = 0
WHERE (VB_VANBAN.ILOAI = LOAIVANBAN OR LOAIVANBAN= -1) 
AND (VB_VANBAN.ILINHVUC = LINHVUC OR LINHVUC = -1)
AND(VB_DONVI_VANBAN.IDONVI  = IMADONVI OR IMADONVI= -1)
AND (VB_VANBAN.IKYHOP = KYHOP OR  KYHOP = -1)
AND (VB_VANBAN.IHIENTHI = TRANGTHAI OR TRANGTHAI= -1)
AND DDATECREATE >= TO_DATE(NGAYBATDAU) AND DDATECREATE <= TO_DATE(NGAYKETTHUC)
AND  ( UPPER(CTRICHYEU) LIKE '%' || UPPER(CTUKHOA) || '%' OR  UPPER(CTIEUDE) LIKE '%' || UPPER(CTUKHOA) || '%' )
)
SELECT * FROM 
(
   SELECT k.*,(SELECT COUNT(*) FROM PHANTRANG) AS TOTAL, 
   ROWNUM r_ FROM( select * from PHANTRANG ) k
   WHERE ROWNUM < ((p_PAGE * p_PAGE_SIZE) + 1 )
) 
 WHERE r_>= (((p_PAGE-1) * p_PAGE_SIZE) + 1);

  END PRC_PHANTRANG_VANBAN;
END PKG_THIETLAP_HETHONG;
