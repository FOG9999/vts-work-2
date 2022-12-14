--------------------------------------------------------
--  File created - Friday-October-07-2022   
--------------------------------------------------------
DROP INDEX "ACTION_PK";
DROP INDEX "DANTOC_PK";
DROP INDEX "DONVI_LINHVUC_PK";
DROP INDEX "FILE_UPLOAD_PK";
DROP INDEX "KNTC_DON_LICHSU_PK";
DROP INDEX "KNTC_DON_PK";
DROP INDEX "KNTC_GIAMSAT_PK";
DROP INDEX "KNTC_LOAIDON_PK";
DROP INDEX "KNTC_NOIDUNGDON_PK";
DROP INDEX "KNTC_TINHCHAT_PK";
DROP INDEX "KNTC_VANBAN_PK";
DROP INDEX "KN_CHUONGTRINH_CHITIET_PK";
DROP INDEX "KN_CHUONGTRINH_DAIBIEU_PK";
DROP INDEX "KN_CHUONGTRINH_DIAPHUONG_PK";
DROP INDEX "KN_CHUYENXULY_PK";
DROP INDEX "KN_DOANGIAMSAT_KIENNGHI_PK";
DROP INDEX "KN_DOANGIAMSAT_PK";
DROP INDEX "KN_DOANGIAMSAT_YKIEN_PK";
DROP INDEX "KN_GIAMSAT_DANHGIA_PK";
DROP INDEX "KN_GIAMSAT_PHANLOAI_PK";
DROP INDEX "KN_GIAMSAT_PK";
DROP INDEX "KN_IMPORT_PK";
DROP INDEX "KN_KIENNGHI_IMPORT_PK";
DROP INDEX "KN_KIENNGHI_PK";
DROP INDEX "KN_KIENNGHI_TRALOI_PK";
DROP INDEX "KN_TONGHOP_PK";
DROP INDEX "KN_TRALOI_PHANLOAI_PK";
DROP INDEX "KN_TUKHOA_PK";
DROP INDEX "KN_VANBAN_PK";
DROP INDEX "LOGIN_FAIL_PK";
DROP INDEX "NGHENGHIEP_PK";
DROP INDEX "QUOCTICH_PK";
DROP INDEX "SYS_C0023943";
DROP INDEX "SYS_IL0000120482C00003$$";
DROP INDEX "SYS_IL0000120486C00005$$";
DROP INDEX "SYS_IL0000120489C00006$$";
DROP INDEX "SYS_IL0000120492C00005$$";
DROP INDEX "SYS_IL0000120496C00004$$";
DROP INDEX "SYS_IL0000120500C00007$$";
DROP INDEX "SYS_IL0000120505C00003$$";
DROP INDEX "TD_VUVIEC_PK";
DROP INDEX "TD_VUVIEC_XULY_PK";
DROP INDEX "TIEPDAN_DINHKY_LOAIVUVIEC_PK";
DROP INDEX "TIEPDAN_DINHKY_PK";
DROP INDEX "TIEPDAN_DINHKY_VUVIEC_PK";
DROP INDEX "TIEPDAN_THUONGXUYEN_KETQUA_PK";
DROP INDEX "TIEPDAN_THUONGXUYEN_PK";
DROP INDEX "TOKEN_PK";
DROP INDEX "TRACKING_PK";
DROP INDEX "VB_DONVI_VANBAN_PK";
DROP INDEX "VB_FILE_VANBAN_PK";
DROP INDEX "VB_LOAI_PK";
DROP INDEX "VB_VANBAN_PK";
--------------------------------------------------------
--  DDL for Index ACTION_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ACTION_PK" ON "ACTION" ("IACTION") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index DANTOC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "DANTOC_PK" ON "DANTOC" ("IDANTOC") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index DONVI_LINHVUC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "DONVI_LINHVUC_PK" ON "DONVI_LINHVUC" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 MAXSIZE UNLIMITED
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index FILE_UPLOAD_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "FILE_UPLOAD_PK" ON "FILE_UPLOAD" ("ID_FILE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_DON_LICHSU_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_DON_LICHSU_PK" ON "KNTC_DON_LICHSU" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_DON_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_DON_PK" ON "KNTC_DON" ("IDON") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_GIAMSAT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_GIAMSAT_PK" ON "KNTC_GIAMSAT" ("IGIAMSAT") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_LOAIDON_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_LOAIDON_PK" ON "KNTC_LOAIDON" ("ILOAIDON") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_NOIDUNGDON_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_NOIDUNGDON_PK" ON "KNTC_NOIDUNGDON" ("INOIDUNG") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_TINHCHAT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_TINHCHAT_PK" ON "KNTC_TINHCHAT" ("ITINHCHAT") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KNTC_VANBAN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KNTC_VANBAN_PK" ON "KNTC_VANBAN" ("IVANBAN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_CHUONGTRINH_CHITIET_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_CHUONGTRINH_CHITIET_PK" ON "KN_CHUONGTRINH_CHITIET" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_CHUONGTRINH_DAIBIEU_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_CHUONGTRINH_DAIBIEU_PK" ON "KN_CHUONGTRINH_DAIBIEU" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_CHUONGTRINH_DIAPHUONG_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_CHUONGTRINH_DIAPHUONG_PK" ON "KN_CHUONGTRINH_DIAPHUONG" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_CHUYENXULY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_CHUYENXULY_PK" ON "KN_CHUYENXULY" ("IKN_CHUYENXULY") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_DOANGIAMSAT_KIENNGHI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_DOANGIAMSAT_KIENNGHI_PK" ON "KN_DOANGIAMSAT_KIENNGHI" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index KN_DOANGIAMSAT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_DOANGIAMSAT_PK" ON "KN_DOANGIAMSAT" ("IDOAN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index KN_DOANGIAMSAT_YKIEN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_DOANGIAMSAT_YKIEN_PK" ON "KN_DOANGIAMSAT_YKIEN" ("IYKIEN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index KN_GIAMSAT_DANHGIA_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_GIAMSAT_DANHGIA_PK" ON "KN_GIAMSAT_DANHGIA" ("IDANHGIA") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_GIAMSAT_PHANLOAI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_GIAMSAT_PHANLOAI_PK" ON "KN_GIAMSAT_PHANLOAI" ("IPHANLOAI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_GIAMSAT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_GIAMSAT_PK" ON "KN_GIAMSAT" ("IGIAMSAT") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_IMPORT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_IMPORT_PK" ON "KN_IMPORT" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_KIENNGHI_IMPORT_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_KIENNGHI_IMPORT_PK" ON "KN_KIENNGHI_IMPORT" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_KIENNGHI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_KIENNGHI_PK" ON "KN_KIENNGHI" ("IKIENNGHI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_KIENNGHI_TRALOI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_KIENNGHI_TRALOI_PK" ON "KN_KIENNGHI_TRALOI" ("ITRALOI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_TONGHOP_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_TONGHOP_PK" ON "KN_TONGHOP" ("ITONGHOP") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_TRALOI_PHANLOAI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_TRALOI_PHANLOAI_PK" ON "KN_TRALOI_PHANLOAI" ("IPHANLOAI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_TUKHOA_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_TUKHOA_PK" ON "KN_TUKHOA" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index KN_VANBAN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "KN_VANBAN_PK" ON "KN_VANBAN" ("IVANBAN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index LOGIN_FAIL_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "LOGIN_FAIL_PK" ON "LOGIN_FAIL" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index NGHENGHIEP_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "NGHENGHIEP_PK" ON "NGHENGHIEP" ("INGHENGHIEP") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index QUOCTICH_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "QUOCTICH_PK" ON "QUOCTICH" ("IQUOCTICH") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index SYS_C0023943
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_C0023943" ON "KN_NGUONDON" ("INGUONDON") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120482C00003$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120482C00003$$" ON "KN_KIENNGHI_TRALOI" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120486C00005$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120486C00005$$" ON "KN_VANBAN" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120489C00006$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120489C00006$$" ON "KNTC_GIAMSAT" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120492C00005$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120492C00005$$" ON "KNTC_VANBAN" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120496C00004$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120496C00004$$" ON "KN_KIENNGHI_IMPORT" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120500C00007$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120500C00007$$" ON "KN_CHUONGTRINH" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index SYS_IL0000120505C00003$$
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_IL0000120505C00003$$" ON "USER_GROUP" (
  PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" 
  PARALLEL (DEGREE 0 INSTANCES 0) ;
--------------------------------------------------------
--  DDL for Index TD_VUVIEC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TD_VUVIEC_PK" ON "TD_VUVIEC" ("IVUVIEC") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TD_VUVIEC_XULY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TD_VUVIEC_XULY_PK" ON "TD_VUVIEC_XULY" ("IXULY") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TIEPDAN_DINHKY_LOAIVUVIEC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TIEPDAN_DINHKY_LOAIVUVIEC_PK" ON "TIEPDAN_DINHKY_LOAIVUVIEC" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TIEPDAN_DINHKY_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TIEPDAN_DINHKY_PK" ON "TIEPDAN_DINHKY" ("IDINHKY") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TIEPDAN_DINHKY_VUVIEC_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TIEPDAN_DINHKY_VUVIEC_PK" ON "TIEPDAN_DINHKY_VUVIEC" ("IVUVIEC") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TIEPDAN_THUONGXUYEN_KETQUA_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TIEPDAN_THUONGXUYEN_KETQUA_PK" ON "TIEPDAN_THUONGXUYEN_KETQUA" ("IKETQUA") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TIEPDAN_THUONGXUYEN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TIEPDAN_THUONGXUYEN_PK" ON "TIEPDAN_THUONGXUYEN" ("ITHUONGXUYEN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TOKEN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TOKEN_PK" ON "TOKEN" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index TRACKING_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "TRACKING_PK" ON "TRACKING" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index VB_DONVI_VANBAN_PK
--------------------------------------------------------

  CREATE INDEX "VB_DONVI_VANBAN_PK" ON "VB_DONVI_VANBAN" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index VB_FILE_VANBAN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "VB_FILE_VANBAN_PK" ON "VB_FILE_VANBAN" ("IFILE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index VB_LOAI_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "VB_LOAI_PK" ON "VB_LOAI" ("ILOAI") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
--------------------------------------------------------
--  DDL for Index VB_VANBAN_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "VB_VANBAN_PK" ON "VB_VANBAN" ("IVANBAN") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "KIENNGHI_THA" ;
