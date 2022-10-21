CREATE OR REPLACE
PACKAGE "PKG_KIENNGHI_CUTRI" AS 

  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
  /*
PROCEDURE PRC_PHANTRANG_KIENNGHI_TRACUU(res out sys_refcursor, p_ITINHTRANG in NUMBER, -- DEFAULT: -1
                                                               p_ILINHVUC IN NUMBER, -- DEFAULT: -1
                                                               p_IDONVITIEPNHAN in NUMBER, -- DEFAULT: 0
                                                               p_IKYHOP in NUMBER, -- DEFAULT: 0
                                                               p_ITRUOCKYHOP IN NUMBER, -- DEFAULT: -1
                                                               p_ITHAMQUYENDONVI in NUMBER,-- DEFAULT: 0
                                                               p_CNOIDUNG IN NVARCHAR2-- DEFAULT:                                                               
                                                               );
 */                                                              
PROCEDURE PRC_TONGHOP_KIENNGHI(res out sys_refcursor,  p_ITINHTRANG in NUMBER, /* DEFAULT: -1*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/*DEFAULT:0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: -1*/
                                                               p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
                                                               --p_TINHTRANG_FROM IN NUMBER,
                                                               --p_TINHTRANG_TO IN NUMBER,

                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               );
PROCEDURE PRC_TONGHOP_KIENNGHI_BDN(res out sys_refcursor,  p_ITINHTRANG in NUMBER, /* DEFAULT: -1*/
                                                               p_LISTLINHVUC IN NVARCHAR2, /* DEFAULT: -1*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/*DEFAULT:0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: -1*/
                                                               p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
                                                               p_LISTKYHOP IN NVARCHAR2,
                                                               p_LIST_HUYEN_XA_TP IN NVARCHAR2,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER,
                                                               p_IUSER IN NUMBER
                                                               );   
PROCEDURE PRC_TONGHOP_KIENNGHI_HUYEN(res out sys_refcursor,  p_ITINHTRANG in NUMBER, /* DEFAULT: -1*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: ''*/
--                                                               p_MAKIENNGHI IN NVARCHAR2, /* DEFAULT: ''*/
--                                                               p_LIST_HUYEN_TP IN NVARCHAR2, /* DEFAULT: ''*/
--                                                               p_DTUNGAY IN DATE, /* DEFAULT: date min*/
--                                                               p_DDENNGAY IN DATE, /* DEFAULT: date max*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/*DEFAULT:0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: -1*/
                                                               p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
                                                               p_LISTKYHOP IN NVARCHAR2,
--                                                               p_iIDONVITIEPNHAN in NUMBER,/* DEFAULT: -1*/
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER,
                                                               p_IUSER IN NUMBER
                                                               );  					
PROCEDURE PRC_TONGHOP_KIENNGHI_HDNDTinh(res out sys_refcursor,  p_ITINHTRANG in NUMBER, /* DEFAULT: -1*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/*DEFAULT:0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: -1*/
                                                               p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
                                                               p_LISTKYHOP IN NVARCHAR2,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER,
                                                               p_IUSER IN NUMBER
                                                               );   
/*                                                               
PROCEDURE PRC_TONGHOP_CHUYEN_BDN(res out sys_refcursor,  p_ILINHVUC IN NUMBER, -- DEFAULT: -1
                                                               p_IDONVITONGHOP in NUMBER, -- DEFAULT: 0
                                                               p_IKYHOP in NUMBER, -- DEFAULT: 0
                                                               p_ITRUOCKYHOP IN NUMBER, -- DEFAULT: -1
                                                               p_ITHAMQUYENDONVI in NUMBER,-- DEFAULT: -1
                                                               p_CNOIDUNG IN NVARCHAR2 -- DEFAULT:                                                             
                                                               );
 */   
 /*
 PROCEDURE PRC_INSERT_IMPORT_TMP(p_NOIDUNG in NVARCHAR2,p_DIAPHUONG in NVARCHAR2,p_DONVI_THAMQUYEN in NVARCHAR2,
                                                  p_LINHVUC in NVARCHAR2,p_KEHOACH in NVARCHAR2,p_KYHOP in NVARCHAR2,
                                                  p_HINHTHUC in NVARCHAR2,p_CONGVAN in NVARCHAR2,p_SOCONGVAN in NVARCHAR2,
                                                  p_NGAYBANHANH in DATE,p_ID_IMPORT in NUMBER);
   */                                               
 PROCEDURE PRC_KIENNGHI_DELETE(res out sys_refcursor,p_ID_USER in NUMBER);
 PROCEDURE PRC_HUY_KIENNGHI_IMPORT(p_ID_IMPORT in NUMBER);
PROCEDURE PRC_LIST_USER_CAPNHAT(res out sys_refcursor,p_ID_DONVI in NUMBER);
PROCEDURE PRC_TAPHOP_IMPORT(res out sys_refcursor,p_ID_IMPORT in NUMBER);
PROCEDURE PRC_COQUAN_LINHVUC(res out sys_refcursor);
PROCEDURE PRC_KIENNGHI_IMPORT(res out sys_refcursor,p_ID_IMPORT in NUMBER);

PROCEDURE PRC_CHUONGTRINH_TXCT_EXPORT(res out sys_refcursor,p_IKYHOP in NUMBER);

PROCEDURE PRC_DOANGIAMSAT(res out sys_refcursor, p_IUSER IN NUMBER, p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
                                                  p_IDDONVI IN NUMBER,
                                                  p_PAGE IN NUMBER,/* DEFAULT: */
                                                  p_PAGE_SIZE IN NUMBER/* DEFAULT: */);
PROCEDURE PRC_KIENNGHI_TRUNG(res out sys_refcursor,  p_CNOIDUNG1 IN NVARCHAR2, /* DEFAULT: */
                                                     p_CNOIDUNG2 IN NVARCHAR2, /* DEFAULT: */
                                                     p_CNOIDUNG3 IN NVARCHAR2, /* DEFAULT: */
                                                     p_CNOIDUNG4 IN NVARCHAR2, /* DEFAULT: */
                                                     p_CNOIDUNG5 IN NVARCHAR2 /* DEFAULT: */
                                                               );     
PROCEDURE PRC_CHUONGTRINH_CHITIET(res out sys_refcursor, p_ICHUONGTRINH in NUMBER );                                                                  
 PROCEDURE PRC_CHUONGTRINH_TIEPXUC_CUTRI(res out sys_refcursor,  p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                                 p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                                 p_IDOAN_LAPKEHOACH in NUMBER,/* DEFAULT: 0*/   
                                                                 p_CNOIDUNG IN NVARCHAR2, /* DEFAULT: */
																																 p_LISTKYHOP in NVARCHAR2,
																																 p_USER IN NUMBER,
                                                                 p_PAGE IN NUMBER,/* DEFAULT: */
                                                                  p_PAGE_SIZE IN NUMBER/* DEFAULT: */		
                                                               );  
PROCEDURE PRC_LIST_KIENNGHI_BY_TONGHOP(res out sys_refcursor, p_ITONGHOP in NUMBER,p_ITONGHOP_BDN in NUMBER); 

PROCEDURE PRC_KIENNGHI_MOICAPNHAT(res out sys_refcursor,       p_IUSER in NUMBER, /* DEFAULT: 0*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_INGUONKIENNGHI in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_IDIAPHUONG0 in NUMBER,/* DEFAULT: 0*/
                                                               p_IDIAPHUONG1 in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,
																															 p_LISTKYHOP in NVARCHAR2,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               );

 PROCEDURE PRC_KIENNGHI_LIST(res out sys_refcursor,       p_ITINHTRANG IN NUMBER,
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */                                                            
                                                               );     
 PROCEDURE PRC_KIENNGHI_LIST_TRUNG(res out sys_refcursor,      p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,
																															 p_LISTKYHOP IN NVARCHAR2,
                                                               p_IUSER_CAPNHAT IN NUMBER,--0
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */                                                            
                                                               ); 
  PROCEDURE PRC_KIENNGHI_LIST_TRACUU(res out sys_refcursor,       p_ITINHTRANG IN NUMBER,
                                                               p_ILINHVUC IN NUMBER, 
                                                               p_IDONVITIEPNHAN in NUMBER, 
                                                               p_IKYHOP in NUMBER, 
                                                               p_ITRUOCKYHOP IN NUMBER, 
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,
                                                               p_ITHAMQUYENDONVI in NUMBER,
                                                               p_CNOIDUNG IN NVARCHAR2,
                                                               p_IUSER_CAPNHAT IN NUMBER,--0
                                                               p_NGAYNHAN_FROM IN DATE,
                                                               p_NGAYNHAN_TO IN DATE,
                                                               p_NGAYTONGHOP_FROM IN DATE,
                                                               p_NGAYTONGHOP_TO IN DATE,
                                                               p_NGAYTRALOI_FROM IN DATE,
                                                               p_NGAYTRALOI_TO IN DATE,
                                                               p_TINHTRANG_FROM IN NUMBER,
                                                               p_TINHTRANG_TO IN NUMBER,
                                                               p_KETQUA_GIAMSAT IN NUMBER,
                                                               p_DA_TRALOI IN NUMBER,
                                                               p_PAGE IN NUMBER,
                                                               p_PAGE_SIZE IN NUMBER   
                                                               );   

    /*
   PROCEDURE PRC_KIENNGHI_LIST_TRACUU(res out sys_refcursor,       p_ITINHTRANG IN NUMBER,
                                                               p_ILINHVUC IN NUMBER, -- DEFAULT: -1
                                                               p_IDONVITIEPNHAN in NUMBER, -- DEFAULT: 0
                                                               p_IKYHOP in NUMBER, -- DEFAULT: 0
                                                               p_ITRUOCKYHOP IN NUMBER, -- DEFAULT: -1
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,-- DEFAULT: 0
                                                               p_ITHAMQUYENDONVI in NUMBER,-- DEFAULT: 0
                                                               p_CNOIDUNG IN NVARCHAR2,-- DEFAULT:  
                                                               p_NGAYNHAN_FROM IN DATE,p_NGAYNHAN_TO IN DATE,
                                                               p_NGAYTONGHOP_FROM IN DATE,p_NGAYTONGHOP_TO IN DATE,
                                                               p_NGAYTRALOI_FROM IN DATE,p_NGAYTRALOI_TO IN DATE
                                                               );    
    */                                                           
   PROCEDURE PRC_TRACUU_TRUNG(res out sys_refcursor,          p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,/* DEFAULT: */ 
                                                               p_NGAYNHAN_FROM IN DATE,
                                                               p_NGAYNHAN_TO IN DATE,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               ); 
 /*                                                              
 PROCEDURE PRC_KIENNGHI_CHUYENKYSAU(res out sys_refcursor,     p_ILINHVUC IN NUMBER, -- DEFAULT: -1
                                                               p_IDONVITIEPNHAN in NUMBER, -- DEFAULT: 0
                                                               p_IKYHOP in NUMBER, -- DEFAULT: 0
                                                               p_ITRUOCKYHOP IN NUMBER, -- DEFAULT: -1
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,-- DEFAULT: 0
                                                               p_ITHAMQUYENDONVI in NUMBER,-- DEFAULT: 0
                                                               p_CNOIDUNG IN NVARCHAR2-- DEFAULT:                                                                
                                                               );  
 */                                                              
 /*                                                              
 PROCEDURE PRC_KIENNGHI_TRALAI(res out sys_refcursor,     p_ILINHVUC IN NUMBER, -- DEFAULT: -1
                                                               p_IDONVITIEPNHAN in NUMBER, -- DEFAULT: 0
                                                               p_IKYHOP in NUMBER, -- DEFAULT: 0
                                                               p_ITRUOCKYHOP IN NUMBER, -- DEFAULT: -1
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,-- DEFAULT: 0
                                                               p_ITHAMQUYENDONVI in NUMBER,-- DEFAULT: 0
                                                               p_CNOIDUNG IN NVARCHAR2-- DEFAULT:                                                                
                                                               );   
 */                                                              
PROCEDURE PRC_KIENNGHI_MOICAPNHAT_DP(res out sys_refcursor,       p_IUSER in NUMBER, /* DEFAULT: 0*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_INGUONKIENNGHI in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/*0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */                                                            
                                                               ); 

PROCEDURE PRC_LIST_KN_TRALOI_DANHGIA(res out sys_refcursor,    p_IDKIENNGHI in NUMBER,
                                                                p_ITONGHOP_BDN IN NUMBER,     /* DEFAULT: 0*/  
                                                               p_ITONGHOP IN NUMBER,     /* DEFAULT: 0*/  
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,
                                                               p_KETQUA_DANHGIA IN NUMBER,/* DEFAULT: */
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */  
                                                               );                                                               
 PROCEDURE PRC_LIST_TONGHOP_KIENNGHI(res out sys_refcursor,    p_ITINHTRANG IN NUMBER, /* DEFAULT: -1*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/*0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,/* DEFAULT: */ 
                                                               p_TINHTRANG_FROM IN NUMBER,
                                                               p_TINHTRANG_TO IN NUMBER,
                                                               p_CHUA_TRALOI IN NUMBER,
                                                               p_DA_TRALOI IN NUMBER,
                                                               p_TRALOI_KIENNGHI IN NUMBER,
                                                               p_DATRALOI_KIENNGHI IN NUMBER,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               );        
PROCEDURE PRC_LIST_TONGHOP_CHUATRALOI(res out sys_refcursor,    p_ILOAI IN NUMBER, /* DEFAULT: -1*/
																																p_USER IN NUMBER,
                                                                p_ITINHTRANG IN NUMBER, /* DEFAULT: -1*/
                                                               p_ILINHVUC IN NUMBER, /* DEFAULT: -1*/
                                                               p_IDONVITONGHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT IN NUMBER,/*0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_LISTKYHOP IN NVARCHAR2,
                                                               p_CNOIDUNG IN NVARCHAR2,/* DEFAULT: */                                                                
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               );  


  PROCEDURE PRC_KIENNGHI_MOICAPNHATTEST(res out sys_refcursor, p_IUSER in NUMBER, /* DEFAULT: -1*/
                                                               p_LSTLINHVUC IN VARCHAR2, /* DEFAULT: -1*/
                                                               p_IDONVITIEPNHAN in NUMBER, /* DEFAULT: 0*/
                                                               p_LSTNGUONKN in VARCHAR2, /* DEFAULT: 0*/
                                                               p_IKYHOP in NUMBER, /* DEFAULT: 0*/
                                                               p_ITRUOCKYHOP IN NUMBER, /* DEFAULT: -1*/
                                                               p_ITHAMQUYENDONVI_PARENT in NUMBER,/* DEFAULT: 0*/
                                                               p_ITHAMQUYENDONVI in NUMBER,/* DEFAULT: 0*/
                                                               p_CNOIDUNG IN NVARCHAR2,/* DEFAULT: */
                                                               p_LISTKYHOP in NVARCHAR2,
                                                               p_MAKIENNGHI IN NUMBER,
                                                               p_TUNGAY IN DATE,
                                                               p_DENNGAY IN DATE,
                                                               p_PAGE IN NUMBER,/* DEFAULT: */
                                                               p_PAGE_SIZE IN NUMBER/* DEFAULT: */
                                                               );                                                              
END PKG_KIENNGHI_CUTRI;

