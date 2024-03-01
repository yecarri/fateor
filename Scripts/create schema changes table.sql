/* drop table app_SchemaChanges; */
create table app_SchemaChanges
(
	ScriptName varchar(255) not null,
	ExecutionTime datetime not null,
	constraint pk_app_SchemaChanges primary key(ScriptName)
)

select * from mysql.user;
