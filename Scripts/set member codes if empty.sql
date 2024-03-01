select 
	GivenName FirstName, 
    Surname LastName, 
    email, 
    identitydocumenttypeid IdentificationDocumentTypeId, 
    identitydocumentnumber,
    StatusDescription,
    UnitName,
    1 Activo
from vw_org_MemberStatus m
join loc_region r on m.RegionId=r.RegionId
where StatusId=4;

select GivenName, Surname, MemberId, MemberCode, StatusDescription, StatusId, StatusDate,  identitydocumentnumber from vw_org_MemberStatus ;

update org_member t, vw_org_MemberStatus s
set t.MemberCode = trim(concat( cast(year(s.StatusDate) as char(4)) , '-' , cast(s.MemberId as char(4)))) 
where s.MemberId=t.MemberId

SET SQL_SAFE_UPDATES = 0;
