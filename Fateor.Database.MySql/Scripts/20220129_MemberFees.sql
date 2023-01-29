create table if not exists org_MemberFeeBatch
(
	MemberFeeBatchId int not null auto_increment,
    BatchTitle varchar(200),
	DateFrom date not null,
    DateTo date not null,
    IsDeleted tinyint default(0),
	LastModificationUserId int,
    LastModificationDate timestamp,
    constraint pk_org_MemberFeeBatch primary key (MemberFeeBatchId)
);
create table if not exists org_MemberFee 
(
	MemberFeeId int not null auto_increment,
    DateFrom date not null,
    DateTo date not null,
    MemberId int not null,
    MemberFeeBatchId int null,
    IsDeleted tinyint default(0),
	LastModificationUserId int,
    LastModificationDate timestamp,
    constraint pk_org_MemberFee primary key (MemberFeeId),
    constraint fk_org_MemberFee_MemberId foreign key (MemberId) references org_Member(MemberId),
    constraint fk_org_MemberFeeBatch foreign key (MemberFeeBatchId) references org_MemberFeeBatch(MemberFeeBatchId)    
);
