alter table app_emailtemplate 
modify column TemplateName varchar(256) not null;

alter table app_emailtemplate 
add constraint uq_app_emailtemplate_TemplateName unique(TemplateName);

insert into app_emailtemplate (TemplateName, Subject, Content)
values ('memberLogin',
'Enlace para entrar al área de Miembros',
'<html>
	<body>
		<p>Usa este enlace para entrar al área de miembros:</p>
		<p><a href=''{url}''>{url}</a></p>		
	</body>
</html>') as s
ON DUPLICATE KEY UPDATE
    Subject = s.Subject,
    Content = s.Content;

/*
select * from app_emailtemplate;
delete from app_emailtemplate where EmailTemplateId=5;
update app_emailtemplate set TemplateName='memberLogin' where EmailTemplateId = 4;
*/