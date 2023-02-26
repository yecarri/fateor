/* drop table org_MemberFeeBatch; */
create table if not exists `org_memberfeebatch`
(
	MemberFeeBatchId int not null auto_increment,
    BatchTitle varchar(200),
	DateFrom date not null,
    DateTo date not null,
    RefferencePattern varchar(200),
    IsDeleted tinyint default(0),
	LastModificationUserId int,
    LastModificationDate timestamp,
    constraint `pk_org_memberfeebatch` primary key (MemberFeeBatchId)
);
/* drop table org_MemberFee; */
create table if not exists `org_memberfee`
(
	MemberFeeId int not null auto_increment,
    `Reference` varchar(30) null,
    DateFrom date not null,
    DateTo date not null,
    MemberId int not null,
    MemberFeeBatchId int null,
    IsDeleted tinyint default(0),
	LastModificationUserId int,
    LastModificationDate timestamp,
    constraint pk_org_memberfee primary key (MemberFeeId),
    constraint fk_org_memberfee_MemberId foreign key (MemberId) references `org_member`(MemberId),
    constraint fk_org_memberfee_MemberFeeBatchId foreign key (MemberFeeBatchId) references `org_memberfeebatch`(MemberFeeBatchId)    
);
