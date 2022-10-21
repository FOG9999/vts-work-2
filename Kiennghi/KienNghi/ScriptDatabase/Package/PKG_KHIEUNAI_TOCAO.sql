CREATE OR REPLACE
PACKAGE "PKG_KHIEUNAI_TOCAO" AS 
    PROCEDURE PRO_TRACUUDON(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON in INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH in INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT
                                        , P_ITINHTRANGDON IN INT, P_IDONVITHULY IN INT,P_IDONVI IN INT,P_INGUOINHAN IN INT
																				, P_IUSER IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);

    PROCEDURE PRO_LISTDON(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT ,P_INGUOINHAN IN INT);

    PROCEDURE PRO_LISTLICHSUDON(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT ,P_INGUOINHAN IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT, P_IDONVI IN INT, P_ICHUYENXULY IN INT);

    PROCEDURE PRO_LISTDONDAXULY(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT,P_INGUOINHAN IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT, P_IDONVIXULY IN INT, P_ITRANGTHAI IN INT);

    PROCEDURE PRO_DONMOICAPNHAT(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT
                                        , P_IDONVITIEPNHAN IN INT, P_ITINHTRANGXULY IN INT,P_IUSER IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
     PROCEDURE PRO_DONCHOXULY(res out sys_refcursor, P_USER in INT
										, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_IDONVITIEPNHAN IN INT, P_ITINHTRANGXULY IN INT,P_IUSER_DUOCGIAOXULY IN INT
                                        , P_IDIEUKIENXULY IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
    PROCEDURE PRO_DONDAPHANLOAI(res out sys_refcursor, P_USER in INT
																				, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_IDONVITIEPNHAN IN INT, P_ITINHTRANGXULY IN INT,P_IUSER_DUOCGIAOXULY IN INT
                                        , P_ITHAMQUYEN IN INT,P_IDIEUKIEN IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
PROCEDURE PRO_LISTDONDANHANXULY(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                         , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_IDONVITHULY IN INT, P_ITINHTRANGXULY IN INT

                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
    PROCEDURE PRO_LISTDALUANCHUYEN(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT,P_INGUOINHAN IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT, P_IDONVI IN INT, P_ICHUYENXULY IN INT, P_ITINHTRANGXULY IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
   PROCEDURE PRO_LISTDACHUYENXULY(res out sys_refcursor, P_USER in INT
																				, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT,P_INGUOINHAN IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT, P_IDONVI IN INT, P_ICHUYENXULY IN INT
																				, P_ITINHTRANGXULY IN INT, P_IUSER_GIAOXULY IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT); 
   PROCEDURE PRO_LISTDACHUYENXULYCOTRALOI(res out sys_refcursor, P_USER in INT
                                        , P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT,P_INGUOINHAN IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT, P_IDONVI IN INT, P_ICHUYENXULY IN INT, P_ITINHTRANGXULY IN INT, P_IUSER_GIAOXULY IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT); 
   PROCEDURE PRO_LISTDONDAXULYGIAIQUYET(res out sys_refcursor, P_USER in INT
																				, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_IDONVITHULY IN INT, P_ITINHTRANGXULY IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT) ; 
   PROCEDURE PRO_LISTDONTRUNG(res out sys_refcursor, P_CKEY in NVARCHAR2, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON IN INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH IN INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_ITINHTRANGDON IN INT, P_IDONVITHULY IN INT,P_IDONVI IN INT );

  PROCEDURE PRO_LISTDONTAMXOA(res out sys_refcursor, P_USER in INT
																				, P_CKEY in NVARCHAR2, P_IKHOA IN INT, P_IDOANDONGNGUOI IN INT
                                        , P_DTUNGAY IN DATE , P_DDENNGAY IN DATE
                                        , P_INGUONDON in INT ,P_ITINHTHANH in INT
                                        , P_IQUOCTICH in INT, P_IDANTOC IN INT
                                        , P_ILOAIDON IN INT,P_ILINHVUC IN INT
                                        , P_INOIDUNG IN INT, P_ITINHCHAT IN INT,P_INGUOINHAN IN INT
                                        , P_ITINHTRANGDON IN INT, P_IDONVITHULY IN INT,P_IDONVI IN INT
                                        , P_PAGE IN INT , P_PAGE_SIZE IN INT);
	PROCEDURE PRO_DONLUUKHONGXULY(res out sys_refcursor, P_USER in INT, P_CKEY in NVARCHAR2, P_IKHOA IN INT , P_IUSER_GIAOXULY IN INT, P_PAGE IN INT,   P_PAGE_SIZE IN INT); 
END PKG_KHIEUNAI_TOCAO;

