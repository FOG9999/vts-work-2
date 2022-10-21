create or replace procedure prc_diaphuong(
    res out sys_refcursor
) as 
begin
    open res for
    with
    DIAPHUONG0 as (
        select
            IDIAPHUONG as IDIAPHUONG_PARENT,
            CTEN as CTENDIAPHUONG_PARENT,
            IPARENT as IDPARENT_PARENT,
            CCODE as CMADIAPHUONG_PARENT,
            CTYPE as TYPE_PARENT,
            IHIENTHI as IDHIENTHI_PARENT,
            IDELETE as IDDELETE_PARENT
        from DIAPHUONG
        where IPARENT = 0 AND IDELETE = 0 AND IHIENTHI = 1
    ),
    DIAPHUONG1 as (
        select
            IDIAPHUONG as IDIAPHUONG,
            CTEN as CTENDIAPHUONG,
            IPARENT as IDPARENT,
            CCODE as CMADIAPHUONG,
            CTYPE as TYPE_,
            IHIENTHI as IDHIENTHI,
            IDELETE as IDDELETE,
            dp0.IDIAPHUONG_PARENT,
            dp0.CTENDIAPHUONG_PARENT,
            dp0.IDPARENT_PARENT,
            dp0.CMADIAPHUONG_PARENT,
            dp0.TYPE_PARENT,
            dp0.IDHIENTHI_PARENT,
            dp0.IDDELETE_PARENT
        from DIAPHUONG dp
        inner join DIAPHUONG0 dp0 on dp0.IDIAPHUONG_PARENT = dp.IPARENT
        where IDELETE = 0 AND IHIENTHI = 1
        order by case CTYPE
            when 'Thành phố' then 1
            when 'Thị xã' then 2
            when 'Quận' then 3
            when 'Huyện' then 3
        end
    )
    select * from DIAPHUONG1
    ;
    
end prc_diaphuong;