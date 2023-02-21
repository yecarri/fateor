CREATE OR REPLACE VIEW vw_org_MemberStatus
AS
    select
           m.MemberId               AS MemberId                   ,
           m.GivenName              AS GivenName                  ,
           m.Surname                AS Surname                    ,
           m.BirthDate              AS BirthDate                  ,
           m.Email                  AS Email                      ,
           m.IdentityDocumentTypeId AS IdentityDocumentTypeId     ,
           m.IdentityDocumentNumber AS IdentityDocumentNumber     ,
           m.Address1               AS Address1                   ,
           m.Address2               AS Address2                   ,
           m.City                   AS City                       ,
           m.PostCode               AS PostCode                   ,
           m.RegionId               AS RegionId                   ,
           m.CountryId              AS CountryId                  ,
           m.Phone                  AS Phone                      ,
           m.MemberCode             AS MemberCode                 ,
           m.ExternalUserId         AS ExternalUserId             ,
           m.IsDeleted              AS IsDeleted                  ,
           m.LastModificationUserId AS LastModificationUserId     ,
           m.LastModificationDate   AS LastModificationDate       ,
           cs.StatusId              AS StatusId                   ,
           cs.StatusDescription     AS StatusDescription          ,
           cs.StatusDate            AS StatusDate                 ,
           cs.LastModificationDate  AS LastStatusModificationDate ,
           cs.UnitName              AS UnitName
    from org_member m
	join lateral
	  (
		   select
					ms.MemberId               AS MemberId               ,
					ms.StatusDate             AS StatusDate             ,
					ms.StatusId               AS StatusId               ,
					ms.UnitId                 AS UnitId                 ,
					ms.LastModificationUserId AS LastModificationUserId ,
					ms.LastModificationDate   AS LastModificationDate   ,
					s.StatusDescription       AS StatusDescription      ,
					u.UnitName                AS UnitName
		   from
				org_member_status ms
				join org_status s on ms.StatusId = s.StatusId												  
				join org_unit u on ms.UnitId = u.UnitId
		   where ms.MemberId = m.MemberId								
		   order by ms.StatusDate desc
		   limit    1
	  ) cs;