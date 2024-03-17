DELIMITER  //
create procedure tmp202306131()  BEGIN
	if (select ConfigValue from app_config where ConfigCode='Site') = 'ahvia' then 
		insert into app_emailtemplate (EmailTemplateId, TemplateName, Subject, Content)
		values 
        (1, 
        'notifyNewMember',
		'Se ha registrado un nuevo miembro',
		'<html>
		<head>
			<style>
				body, * {
					font-family: ''Lucida Sans Unicode'',''Lucida Sans'',''Sans Serif'',sans-serif
				}

				.table {
					display: table;
				}

				.row {
					display: row;
				}

				.cell {
					display: cell;
				}
			</style>
		</head>
		<body>
			<p>Hola {user.Login},</p>
			<p>Se ha registrado un nuevo postulante en el Sistema de Miembros de la AHVIA.</p>
			<table>
				<tr>
					<td>Nombre:</td>
					<td>{member.FullName}</td>
				</tr>        
				<tr>
					<td>Ciudad:</td>
					<td>{member.City}</td>
				</tr>
			</table>
			<p><a href=''{url}''>{url}</a></p>    
		</body>
		</html>'),
		(2,
        'welcome',
		'Bienvenido a AHVIA',
		'<html>
		<head>
			<style>
				body, * {
					font-family: ''Lucida Sans Unicode'',''Lucida Sans'',''Sans Serif'',sans-serif
				}
			</style>
		</head>
		<body>
			<p>Hola {member.GivenName},</p>
			<p>
				Recibimos tu solicitud para unirte a la Asociavión Hispano Venezolana de Ingeniería y Arquitectura. 
			</p>
			<p>
				Si no has completado las preguntas del perfil académico, puedes hacerlo ahora en este enlace:
				<a href=''{url}''>{url}</a>
			</p>
			<p>¡Saludos!</p>
		</body>
		</html>'
		),
        (3,
        'passwordChange',
        'Cambio de Password',
        '<html>
		<body>
			<p>Hola {user.Login},</p>
			<p>Recibiste este correo porque tú o alguien en tu nombre solicitó restablecer tu contraseña en el sistema de Registro de Miembros del MLV</p>
			<p>Usa el siguiente enlace dentro de los próximos 5 minutos para cambiar tu contraseña: <a href=''{url}''>RESTABLECER</a></p>
			<p>¡Por la libertad!</p>
		</body>
		</html>') as n 
        on duplicate key update
			TemplateName = n.TemplateName,
            Subject = n.Subject,
            Content = n.Content;
    
    elseif (select ConfigValue from app_config where ConfigCode='Site') = 'mlv' then
		insert into app_emailtemplate (TemplateName, Subject, Content)
		values 
        (1,
        'notifyNewMember',
		'Se ha registrado un nuevo miembro',
		'<html>
		<head>
			<style>
				body, * {
					font-family: ''Lucida Sans Unicode'',''Lucida Sans'',''Sans Serif'',sans-serif
				}

				.table {
					display: table;
				}

				.row {
					display: row;
				}

				.cell {
					display: cell;
				}
			</style>
		</head>
		<body>
			<p>Hola {user.Login},</p>
			<p>Se ha registrado un nuevo postulante en el Sistema de Miembros del MLV.</p>
			<table>
				<tr>
					<td>Nombre:</td>
					<td>{member.FullName}</td>
				</tr>        
                <tr>
					<td>País:</td>
					<td>{country.CountryName}</td>
				</tr>
				<tr>
					<td>Ciudad:</td>
					<td>{member.City}</td>
				</tr>
			</table>
			<p><a href=''{url}''>{url}</a></p>    
		</body>
		</html>'),
		(2,
        'welcome',
        'Bienvenido al MLV',
		'<html>
		<head>
			<style>
				body, * {
					font-family: ''Lucida Sans Unicode'',''Lucida Sans'',''Sans Serif'',sans-serif
				}
			</style>
		</head>
		<body>
			<p>Hola {member.GivenName},</p>
			<p>
				Recibimos tu solicitud para formar parte del Movimiento Libertario de Venezuela. Nos contenta mucho poder
				contar contigo. Pronto se pondrá en contacto contigo alguno de los coordinadores para conocerte e iniciar
				el proceso de aceptación como miembro simpatizante e incorporarte en nuestras actividades.
			</p>
			<p>
				Si no has completado las preguntas del perfil político de Nolan, puedes hacerlo ahora en este enlace:
				<a href=''{url}''>{url}</a>
			</p>
			<p>¡Por la libertad!</p>
		</body>
		</html>'			
		) ,
        (3,
        'passwordChange',
        'Cambio de Password',
        '<html>
		<body>
			<p>Hola {user.Login},</p>
			<p>Recibiste este correo porque tú o alguien en tu nombre solicitó restablecer tu contraseña en el sistema de Registro de Miembros del MLV</p>
			<p>Usa el siguiente enlace dentro de los próximos 5 minutos para cambiar tu contraseña: <a href=''{url}''>RESTABLECER</a></p>
			<p>¡Por la libertad!</p>
		</body>
		</html>')   as new(EmailTemplateId, TemplateName, Subject, Content)
        on duplicate key update
			TemplateName = new.TemplateName,
            Subject = new.Subject,
            Content = new.Content;
        
    end if;
END //
DELIMITER ;
call tmp202306131;
drop procedure tmp202306131;
