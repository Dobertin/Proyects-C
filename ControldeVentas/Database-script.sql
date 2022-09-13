Create database VentasComerciales;

use ventasComerciales;
go

create table productos
(
	id_producto int identity(1,1) primary key,
	tipo char(7),
	puntos decimal(2,2)
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
	monto_desembolsado decimal(7,3),
	FOREIGN KEY (id_producto) REFERENCES productos(id_producto),
	FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
	FOREIGN KEY (id_asesor) REFERENCES asesores(id_asesor)
)
go
