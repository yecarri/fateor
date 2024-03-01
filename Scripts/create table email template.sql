create table app_emailtemplate
(
	EmailTemplateId int not null auto_increment,
    TemplateName varchar(256) null,
    Subject varchar(256) not null,
    Content varchar(8000) not null,
    constraint pk_app_emailtemplate primary key (EmailTemplateId)
);

/* drop table app_emailaddress */
create table app_emailaddress
(
	EmailAddressId int not null auto_increment,
    Address varchar(256),
    Name varchar(256),
    constraint pk_app_emailadress primary key(EmailAddressId), 
    constraint uq_app_emailaddress_address unique(Address)
);

/* drop table app_emailrecipient */
create table app_emailrecipient
(
	EmailTemplateId int not null,
	EmailAddressId int not null,
    constraint pk_app_emailrecipient primary key (EmailTemplateId, EmailAddressId),
    constraint fk_app_emailrecipient_EmailTemplateId foreign key (EmailTemplateId) references app_emailtemplate(EmailTemplateId),
    constraint fk_app_emailrecipient_EmailAddressId  foreign key (EmailAddressId ) references app_emailaddress(EmailAddressId)    
)    