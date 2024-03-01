select * from bul_bulletin;

drop table bul_BulletinStatus;

create table bul_BulletinStatus
(BulletinStatusId tinyint not null,
BulletinStatusName varchar(255) not null,
constraint pk_bul_BulletinStatus primary key (BulletinStatusId)
)

insert into bul_BulletinStatus
values(0, 'No enviado'),(1,'Procesando'), (2,'Enviado')

select * from bul_bulletinstatusfilter