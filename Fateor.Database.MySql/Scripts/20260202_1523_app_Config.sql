CREATE TABLE `app_configtype`
(	ConfigTypeId int not null primary key,
	ConfigTypeName varchar(80)    
);
  
CREATE TABLE `app_configoption`
(	ConfigOptionId int not null primary key,
	`Value` varchar(8000)    
);  
    
ALTER TABLE app_Config 
ADD COLUMN Description VARCHAR(8000) AFTER ConfigValue,
ADD COLUMN ConfigTypeId int,
ADD CONSTRAINT fk_app_Config_ConfigTypeId foreign key (ConfigTypeId) references `app_configtype`(ConfigTypeId);

/*
ALTER TABLE app_Config
DROP COLUMN Description;

ALTER TABLE app_Config
DROP CONSTRAINT fk_app_Config_ConfigTypeId;

ALTER TABLE app_Config
DROP COLUMN ConfigTypeId;

DROP TABLE `app_configtype`;

DROP TABLE `app_configoption`
*/

insert into app_configtype
values 
	(1,'Text'),
    (2,'Integer'),
    (3,'Real'),
    (4,'Boolean'),
    (5,'Options');

INSERT INTO `app_config` (ConfigCode, Description, ConfigTypeId)
VALUES
('Credentials_Enabled',               'Enable or disable credentials system',          4),
('Credentials_Generator_Assembly',    'Assembly containing the credentials generator', 1),
('Credentials_Generator_Type',        'Type name of the credentials generator',         1),
('Default_InitialPoll',               'Default initial questionnaure',                 1),
('Default_StatusId',                  'Default status identifier',                     2),
('Default_UnitId',                    'Default unit identifier',                       2),
('Default_UserId',                    'Default user identifier',                       2),
('Email_DisabledAll',                 'Disable all outgoing email notifications',      4),
('Email_Sender_Address',              'Sender email address',                          1),
('Email_Sender_Name',                 'Sender display name for emails',                1),
('Email_Service_Domain',              'Email service domain',                          1),
('Email_Service_Key',                 'API key for email service',                     1),
('Email_Service_Password',            'Password for email service authentication',     1),
('Email_Service_Port',                'SMTP port for email service',                   2),
('Email_Service_Provider',            'Email service provider name',                   1),
('Email_Service_Server',              'SMTP server hostname',                          1),
('Email_Service_User',                'Username for email service authentication',     1),
('ForumSystem_CreateUser',            'Allow forum system to create users automatically',4),
('ID_Validator_Type',                 'Identifier validator implementation type',      1),
('Site',                              'Site identifier',                               1),
('Site_Name',                         'Site Name, title displayed in web pages',        1),
('Unit_Selector_Assembly',            'Assembly containing the unit selector',          1),
('Unit_Selector_Type',                'Type name of the unit selector',                 1),
('Url',                               'Base site URL',                                 1) as S
ON DUPLICATE KEY UPDATE
    Description  = s.Description,
    ConfigTypeId = s.ConfigTypeId;
