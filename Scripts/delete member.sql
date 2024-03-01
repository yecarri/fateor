/*
select * from org_member order by memberid desc limit 5;
*/

SET SQL_SAFE_UPDATES = 0;

SET @id = 602;
delete from app_gdpr_member where memberid=@id;

delete from qst_answer where responseid in (select responseid from qst_response where memberid=@id);

delete from qst_response where memberid=@id;

delete from org_member_status where memberid=@id;

delete from org_member where memberid=@id

