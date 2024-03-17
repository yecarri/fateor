use ahvia_fateor;
update app_config set configvalue='0' where configcode='ForumSystem_CreateUser';
select * from app_config;

/* insert into app_config values('ID_Validator_Type', 'Fateor.Application.Organization.Service.EspIdentificationNumberValidator'); */