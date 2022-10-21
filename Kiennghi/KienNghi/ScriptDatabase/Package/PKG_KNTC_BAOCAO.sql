CREATE OR REPLACE
PACKAGE "PKG_KNTC_BAOCAO" AS
  PROCEDURE PRO_BAOCAO_TK_LOAIKHIEUTO(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_NOIGUIDON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_TQGQ(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT ,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_NGUOINHAPDON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_NGUOIXULY(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_COQUANCHUYENDON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_TRUNGDON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_TONGSODON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_CHIITETDON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_DIABAN1(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_BAOCAO_TK_DIABAN2(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
  PROCEDURE PRO_LIST_NGUONDON(res out sys_refcursor);
  PROCEDURE PRO_LIST_NHOMLINHVUC(res out sys_refcursor);
  PROCEDURE PRO_LIST_ALL_DON(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT , pram06 in INT, pram07 in INT,pram08 in INT,pram09 in INT);
	PROCEDURE PRO_BAOCAO_CVCHUYENDON(res out sys_refcursor, pram01 in DATE, pram02 in DATE, pram03 in INT, pram04 in INT);
	PROCEDURE PRO_BAOCAO_THANG(res out sys_refcursor,p_tuNgay in DATE, p_denNgay in DATE);
	PROCEDURE PRO_BAOCAO_THEODOIGQD(res out sys_refcursor, pram01 in DATE, pram02 in DATE, pram03 in INT, pram04 in INT);
	PROCEDURE PRO_BAOCAO_DONTHUHANGTUAN(res out sys_refcursor, pram01 in DATE, pram02 in DATE, pram03 in INT);
END PKG_KNTC_BAOCAO;

