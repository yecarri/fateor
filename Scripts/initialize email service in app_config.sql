select * from app_config

select * from app_emailtemplate

update app_config
set ConfigValue=0 where ConfigCode='Email_DisabledAll'

update app_config
set configvalue='REDACTED' 
where configcode='Email_Service_Key'

update app_config
set configvalue='https://api.eu.mailgun.net/v3/'
where configcode='Email_Service_Server'