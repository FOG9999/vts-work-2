create or replace PROCEDURE "PRC_GET_THKIENNGHI" 
(
    p_ITINHTRANG in number,
    p_IDONVITONGHOP in number,
    p_IKYHOP in number,
    p_IUSER in number,
    p_LOAICQ in NVARCHAR2,
    p_ITHAMQUYENDONVI in number,
    res  OUT sys_refcursor
)
IS
BEGIN
OPEN res FOR
SELECT KN_TONGHOP.* FROM KN_TONGHOP
                             INNER JOIN QUOCHOI_COQUAN ON KN_TONGHOP.ITHAMQUYENDONVI = QUOCHOI_COQUAN.ICOQUAN
WHERE KN_TONGHOP.ITINHTRANG = p_ITINHTRANG
  AND (p_IDONVITONGHOP = 0 OR KN_TONGHOP.IDONVITONGHOP = p_IDONVITONGHOP)
  AND (p_IKYHOP = 0 OR KN_TONGHOP.IKYHOP = p_IKYHOP)
  AND (KN_TONGHOP.ITINHTRANG = p_ITINHTRANG)
  AND (p_ITHAMQUYENDONVI = 0 OR p_ITHAMQUYENDONVI is null OR KN_TONGHOP.ITHAMQUYENDONVI = p_ITHAMQUYENDONVI)
  AND (p_IUSER = 0 OR p_IUSER is null OR KN_TONGHOP.IUSER = p_IUSER)
  AND QUOCHOI_COQUAN.CTYPE = p_LOAICQ
;
END PRC_GET_THKIENNGHI;