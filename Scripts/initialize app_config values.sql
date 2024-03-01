insert into app_config 
SELECT * FROM (
          SELECT 'Site' ConfigCode1, 'ahvia' ConfigValue1
UNION ALL SELECT 'Site_Name', 'Sistema de Miembros de la AHVIA'          
UNION ALL SELECT 'Url', 'https://localhost/'
UNION ALL SELECT 'Default_StatusId','1'
UNION ALL SELECT 'Default_UserId','1'
UNION ALL SELECT 'Default_InitialPoll','Test de Nolan'
UNION ALL SELECT 'Credentials_Enabled','0'
UNION ALL SELECT 'Credentials_Generator_Assembly','Fateor.Application'
UNION ALL SELECT 'Credentials_Generator_Type',''
UNION ALL SELECT 'Email_DisabledAll','1'
UNION ALL SELECT 'Email_Service_Provider','mailgun'
UNION ALL SELECT 'Email_Service_Domain','mailgun.ahvia.es'
UNION ALL SELECT 'Email_Service_Key',''
UNION ALL SELECT 'Email_Service_Server',''
UNION ALL SELECT 'Email_Service_Port',''
UNION ALL SELECT 'Email_Service_User',''
UNION ALL SELECT 'Email_Service_Password',''
UNION ALL SELECT 'Email_Sender_Name','AHVIA'
UNION ALL SELECT 'Email_Sender_Address','info@ahvia.es'
UNION ALL SELECT 'Default_UnitId','1'
UNION ALL SELECT 'Unit_Selector_Assembly',''
UNION ALL SELECT 'Unit_Selector_Type',''
UNION ALL SELECT 'ForumSystem_CreateUser','0' ) X
ON DUPLICATE KEY UPDATE ConfigValue=x.ConfigValue1