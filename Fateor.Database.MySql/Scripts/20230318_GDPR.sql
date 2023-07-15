create table app_gdpr
(GdprId int not null auto_increment,
UserId int not null,
Timestamp datetime,
Export bit not null,
constraint pk_app_gdpr primary key(GdprId),
constraint fk_app_gdpr_UserId foreign key(UserId) references app_user(UserId));

create table app_gdpr_member
(GdprId int not null ,
MemberId int not null,
constraint pk_app_gdpr_member primary key (GdprId, MemberId),
constraint fk_app_gdpr_member_GdprId foreign key(GdprId ) references app_gdpr(GdprId),
constraint fk_app_gdpr_member_MemberId foreign key (MemberId) references org_member(MemberId));

