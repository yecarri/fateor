use lvs_fateor;

update app_config
set ConfigValue = 1 where configCode='Credentials_Enabled';

update app_config
set ConfigValue = 'Fateor.Application' where configCode='Credentials_Generator_Assembly';

update app_config
set ConfigValue = 'Fateor.Application.Credentials.Services.MLVCredentialService' where configCode='Credentials_Generator_Assembly';

update app_config
set ConfigValue = 0 where configCode='Email_DisabledAll';

update app_config
set ConfigValue = 'miembros@movimientolibertario.org' where configCode='Email_Sender_Address';

update app_config
set ConfigValue = 'Sistema de Miembros del MLV' where configCode='Email_Sender_Name';

update app_config
set ConfigValue = 'mailgun.movimientolibertario.org' where configCode='Email_Service_Domain';

update app_config
set ConfigValue = 'https://api.mailgun.net/v3/' where configCode='Email_Service_Server';

update app_config
set ConfigValue = 'REDACTED' where configCode='Email_Service_Key';

update app_config
set ConfigValue = 'mailgun' where configCode='Email_Service_Provider';

update app_config
set ConfigValue = 'Fateor.Application' where configCode='Unit_Selector_Assembly';

update app_config
set ConfigValue = 'Fateor.Application.Organization.Service.MLVUnitSelector' where configCode='Unit_Selector_Type';


select * from app_config;

