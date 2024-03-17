use ahvia_fateor;
select * from app_config;

update app_config set ConfigValue='ahvia' where ConfigCode='Site';
update app_config set ConfigValue='Sistema de Miembros de AHVIA' where ConfigCode='Site_Name';
update app_config set ConfigValue='http://miembros.ahvia.es/' where ConfigCode='Url';