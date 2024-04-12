create table org_memberfeebatch_status
(
	MemberFeeBatchId int not null,
    StatusId int not null,
    constraint pk_org_memberfeebatch_status primary key (MemberFeeBatchId, StatusId),
    constraint fk_org_memberfeebatch_status_MemberFeeBatchId foreign key (MemberFeeBatchId) references org_memberfeebatch(MemberFeeBatchId),
    constraint fk_org_memberfeebatch_status_StatusId foreign key (StatusId) references org_status(StatusId)
)    