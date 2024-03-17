use lvs_fateor;
select * from app_config;
update app_config set ConfigValue='mlv' where ConfigCode='Site';
update app_config set ConfigValue='Sistema de Miembros del MLV' where ConfigCode='Site_Name';

select * from mysql.user;