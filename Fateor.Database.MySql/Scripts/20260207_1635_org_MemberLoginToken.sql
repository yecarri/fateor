create table org_memberlogintoken
(
	Guid binary(16) not null,
    MemberId int not null, 
    Expiration datetime not null,
    Used bit(1) not null default (b'0'),
    constraint pk_org_memberlogintoken primary key(Guid)   ,
    constraint fk_org_memberlogintoken_MemberId foreign key (MemberId) references org_Member(MemberId)
);

/* drop table org_memberlogintoken */
/* insert into db_scripts values ('20260207_1635_org_MemberLoginToken.sql',CURRENT_DATE()); */
/* select * from org_memberlogintoken */