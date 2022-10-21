CREATE OR REPLACE
PACKAGE "PKG_TD_VUVIEC" AS 

  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
PROCEDURE PRC_PHANTRANG_TIEPDANVUVIEC(res out sys_refcursor);

PROCEDURE PRC_TIEPDAN_VUVIEC_TRACUU(res out sys_refcursor, NGAYBATDAU in date,
                                                    NGAYKETHUC IN date,
                                                    IDOANDONGNGUOITRACUU in INT,
                                                    ICOQUANTIEP in INT,
                                                    CTENCONGDAN in NVARCHAR2,
                                                    CDIACHI IN  NVARCHAR2,
                                                    CTOMTAT IN NVARCHAR2,
                                                    ILOAIDONTRACUU IN INT,
                                                    ILINHVUCTRACUU IN INT,
                                                    INHOMNOIDUNGTRACUU IN INT,
                                                    ITINHCHATVUVIEC IN INT,
                                                    ITINHTRANGXULYTRACUU IN INT,
                                                    IKIEMTRUNG IN INT,ILOAIVUVIEC IN INT,TIEPDOTXUAT IN INT,NGUOIDUNG IN INT);

PROCEDURE PRC_TRACUUVUVIEC(res out sys_refcursor , CTUKHOA in NVARCHAR2);
PROCEDURE PRC_TIEPDANLIST( p_PAGE IN INT, p_PAGE_SIZE IN INT,DINHKY IN INT,TIEPDOTXUAT IN INT,DONVI IN INT ,UERS IN INT ,res out sys_refcursor);
PROCEDURE PRC_TRACUUVUVIECNHANH(res out sys_refcursor , CTUKHOA in NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT,DINHKY IN INT,TIEPDOTXUAT IN INT,DONVI IN INT ,UERS IN INT);   

PROCEDURE PRC_TIEPDAN_VUVIECDANHSACH(res out sys_refcursor, NGAYBATDAU in date,
                                                    NGAYKETHUC IN date,
                                                    IDOANDONGNGUOITRACUU in INT,
                                                    ICOQUANTIEP in INT,
                                                    CTENCONGDAN in NVARCHAR2,
                                                    CDIACHI IN  NVARCHAR2,
                                                    CTOMTAT IN NVARCHAR2,
                                                    ILOAIDONTRACUU IN INT,
                                                    ILINHVUCTRACUU IN INT,
                                                    INHOMNOIDUNGTRACUU IN INT,
                                                    ITINHCHATVUVIEC IN INT,
                                                    ITINHTRANGXULYTRACUU IN INT,
                                                    IKIEMTRUNG IN INT,ILOAIVUVIEC IN INT,TIEPDOTXUAT IN INT,
                                                    CTUKHOA in NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT, MADINHKY IN INT,NGUOIDUNG IN INT);


PROCEDURE PRC_LICHTIEPDINHKY(res out sys_refcursor , CTUKHOA in NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT,MADONVI IN INT,NGUOIDUNG IN INT);
PROCEDURE PRC_TIEPDAN_VUVIECTAMTHOI(res out sys_refcursor, NGAYBATDAU in date,
                                                    NGAYKETHUC IN date,
                                                    IDOANDONGNGUOITRACUU in INT,
                                                    ICOQUANTIEP in INT,
                                                    CTENCONGDAN in NVARCHAR2,
                                                    CDIACHI IN  NVARCHAR2,
                                                    CTOMTAT IN NVARCHAR2,
                                                    ILOAIDONTRACUU IN INT,
                                                    ILINHVUCTRACUU IN INT,
                                                    INHOMNOIDUNGTRACUU IN INT,
                                                    ITINHCHATVUVIEC IN INT,
                                                    ITINHTRANGXULYTRACUU IN INT,
                                                    IKIEMTRUNG IN INT,ILOAIVUVIEC IN INT,TIEPDOTXUAT IN INT,
                                                    CTUKHOA in NVARCHAR2,p_PAGE IN INT, p_PAGE_SIZE IN INT, MADINHKY IN INT,NGUOIDUNG IN INT);


PROCEDURE PRC_TIEPDAN_TRACUUVUVIEC(res out sys_refcursor, NGAYBATDAU in date,
                                                    NGAYKETHUC IN date,
                                                    IDOANDONGNGUOITRACUU in INT,
                                                    ICOQUANTIEP in INT,
                                                    CTENCONGDAN in NVARCHAR2,
                                                    CDIACHI IN  NVARCHAR2,
                                                    CTOMTAT IN NVARCHAR2,
                                                    ILOAIDONTRACUU IN INT,
                                                    ILINHVUCTRACUU IN INT,
                                                    INHOMNOIDUNGTRACUU IN INT,
                                                    ITINHCHATVUVIEC IN INT,
                                                    ITINHTRANGXULYTRACUU IN INT,
                                                    IKIEMTRUNG IN INT,ILOAIVUVIEC IN INT,TIEPDOTXUAT IN INT,p_PAGE IN INT, p_PAGE_SIZE IN INT,IDUSER IN INT);




END PKG_TD_VUVIEC;

