CREATE OR REPLACE
PACKAGE "PKG_TCD_BAOCAO" AS
  PROCEDURE PRO_BAOCAO_TK_PHULUC(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 IN INT, pram04 in INT, pram05 in INT );
  PROCEDURE PRO_BAOCAO_TK_SOLIEUTINH(res out sys_refcursor, pram01 in DATE, pram02 IN DATE, pram03 in INT , pram04 in INT, pram05 in INT);


END PKG_TCD_BAOCAO;

