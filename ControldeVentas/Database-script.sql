create database VentasComerciales;
go

use ventasComerciales;
go

create table productos
(
	id_producto int identity(1,1) primary key,
	nombre_com varchar(70),
	tipo char(7),
	puntos decimal(7,4)
)
create table cliente
(
	id_cliente int identity(1,1)  primary key,
	nombres varchar(30),
	apellidos varchar(30),
	tipo_doc char(4),
	nro_doc char(10),
	telefono char(10),
	celular char(10)
)

create table asesores
(
	id_asesor int identity(1,1) primary key,
	usuario varchar(20),
	nombres varchar(30),
	apellido varchar(30),
	tipo_doc char(4),
	nro_doc char(10),
	cant_ventas int,
	meta_propuesta int
)
create table ventas
(
	id_venta int identity(1,1) primary key,
	id_cliente int,
	id_asesor int,
	id_producto int,
	periodo char(6),
	puntos_obtenidos int,
	fecha_venta datetime,
	monto_desembolsado decimal(10,3),
	estado_registro int,
	FOREIGN KEY (id_producto) REFERENCES productos(id_producto),
	FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
	FOREIGN KEY (id_asesor) REFERENCES asesores(id_asesor)
)
go

insert into asesores values ('rhuacca','roberto','huacca','dni','71532795',0,null);
insert into asesores values ('jhuamani','juana','huamani','dni','30584634',0,null);
go
insert into productos values ('Tarjeta de Crédito "Clasica"','T',10);
insert into productos values ('Tarjeta de Crédito "Oro"','T',20);
insert into productos values ('Tarjeta de Crédito "Platino"','T',40);
insert into productos values ('Crédito Hipotecario','C',0.0050);
insert into productos values ('Crédito Efectivo','C',0.0030);
go
insert into cliente values ('juan','contreras','dni','715232796','985006407','985006407');
insert into cliente values ('juana','torres','dni','30145278','985005407','985005407');
go