
select * from org_memberfeebatch;

insert into org_memberfeebatch (batchtitle, datefrom, dateto, isdeleted, lastmodificationuserid, lastmodificationdate)
values ('Test Batch', '2023-01-01', '2023-03-01', 0, 1, SYSDATE());

select * from org_memberfeebatch;

select * from org_memberfee;

insert into org_memberfee(DateFrom, DateTo, MemberId, MemberFeeBatchId, IsDeleted, lastmodificationuserid, lastmodificationdate)
select '2023-01-01', '2023-03-01', MemberId, 1, 0, 1, SYSDATE()
from vw_org_MemberStatus 
where StatusId=4
