use master;
go;

CREATE DATABASE Jocsan;
go;

use Jocsan;
go;

CREATE TABLE Jocsan.dbo.Vuelto (
	idVuelto int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	idCliente int NOT NULL,
	monto decimal(18,2) NULL,
	comentario varchar(500) COLLATE Modern_Spanish_CI_AS NULL,
	fechaVuelto datetime NULL,
	UsuarioCreacion varchar(20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	FechaCreacion datetime NOT NULL,
	UsuarioModifica varchar(20) COLLATE Modern_Spanish_CI_AS NULL,
	FechaModifica datetime NULL,
	estado int DEFAULT 1 NULL,
	tipoVuelto smallint NULL	
);

CREATE TABLE [dbo].[Abono] (
  [idAbono] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [descripcion] varchar(200),
  [idCliente] int NOT NULL,
  [fechaAbono] datetime,
  [valorAbono] [decimal](18, 2) NOT NULL,
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime] 
);

CREATE TABLE [dbo].[MovimientosCredito] (
  [idMovimiento] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [idCredito] int NOT NULL,
  [idAbono] int,
  [cantidad] [decimal](18, 2) NOT NULL,
  [fechaMovimiento] datetime,
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime]
);

CREATE TABLE [dbo].[Creditos] (
  [idCredito] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [descripcion] varchar(200),
  [idCliente] int,
  [fechaCredito] datetime,
  [valorCredito] [decimal](18, 2),
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime],
  [estado] [int] DEFAULT 1 NULL
);

CREATE TABLE [dbo].[Cliente] (
  [idCliente] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [nombre] varchar(50),
  [capitan] varchar(50),
  [porcentaje] [decimal](18, 2) ,
  [gasolina] [decimal](18, 2),
  [nuevaEmbarcacion] bit,
  [peon1] varchar(50),
  [peon2] varchar(50),
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime],
  [estado] [int] DEFAULT 1 NULL
);

CREATE TABLE [dbo].[Factura] (
  [idFactura] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [idCliente] int,
  [fechaVenta] datetime,
  [porcentaje] [decimal](18, 2) ,
  [galones] int,
  [hielo] int,
  [subTotalProd] [decimal](18, 2) ,
  [g-h] [decimal](18, 2) ,
  [subTotalGH] [decimal](18, 2) ,
  [terceros] [decimal](18, 2) ,
  [peladores] [decimal](18, 2) ,
  [subTotal] [decimal](18, 2) ,
  [25] [decimal](18, 2) ,
  [13] [decimal](18, 2) ,
  [Abono] [decimal](18, 2) ,
  [totalVenta] [decimal](18, 2) ,
  [estado] [int] DEFAULT 1 NULL,
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime]
);

CREATE TABLE [dbo].[DetalleFactura] (
  [idDetalleFactura][int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [idProducto] int,
  [idFactura] int,
  [precioUnitario] [decimal](18, 2) ,
  [cantidad] [decimal](18, 2),
  [subTotalParcial] [decimal](18, 2) ,
  [totalParcial] [decimal](18, 2) ,
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime] 
);

CREATE TABLE [dbo].[Producto] (
  [idProducto] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [codigo] char(10),
  [nombreLocal] varchar(100),
  [precio] [decimal](18, 2) ,
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime] 
);

CREATE TABLE [dbo].[Gasolina] (
	[idGasolina] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[idCliente] [int] NOT NULL,
	[precioGalonPagado] [decimal](18,2) NULL,
	[cantGalonPagado] [int] NULL,
	[precioGalonCargado] [decimal](18,2) NULL,
	[cantGalonCargado] [int] NULL,
	[totalGalonCargado] [decimal](18,2) NULL,
	[totalGalonPagado] [decimal](18,2) NULL,
	[fechaOperacion] [datetime] NOT NULL,
	[comentario] [varchar](500) NULL,
	[UsuarioCreacion] [varchar](20) NOT NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioModifica] [varchar](20) NULL,
	[FechaModifica] [datetime] NULL
);

CREATE TABLE [dbo].[Parametro] (
  [idparametro] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
  [tipParametro] [int],
  [descripcion] [varchar](200),
  [valorN] [decimal](18,2),
  [valorT] [varchar](200),
  [UsuarioCreacion] [varchar](20)NOT NULL,
  [FechaCreacion] [datetime] NOT NULL,
  [UsuarioModifica] [varchar](20),
  [FechaModifica] [datetime] 
);

GO;

ALTER TABLE [dbo].[MovimientosCredito]  WITH CHECK ADD  CONSTRAINT [FK_AbonoidAbono] FOREIGN KEY([idAbono]) REFERENCES [dbo].[Abono]([idAbono]);
ALTER TABLE [dbo].[MovimientosCredito]  WITH CHECK ADD  CONSTRAINT [FK_CreditosidCredito] FOREIGN KEY([idCredito]) REFERENCES [dbo].[Creditos]([idCredito]);
ALTER TABLE [dbo].[Abono]  WITH CHECK ADD  CONSTRAINT [FK_AbonoidCliente] FOREIGN KEY([idCliente]) REFERENCES [dbo].[Cliente]([idCliente]);
ALTER TABLE [dbo].[Creditos]  WITH CHECK ADD  CONSTRAINT [FK_CreditosidCliente] FOREIGN KEY([idCliente]) REFERENCES [dbo].[Cliente]([idCliente]);
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_FacturaidCliente] FOREIGN KEY([idCliente]) REFERENCES [dbo].[Cliente]([idCliente]);
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetalleFacturaidFactura] FOREIGN KEY([idFactura]) REFERENCES [dbo].[Factura]([idFactura]);
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetalleFacturaidProducto] FOREIGN KEY([idProducto]) REFERENCES [dbo].[Producto]([idProducto]);
ALTER TABLE [dbo].[Gasolina]  WITH CHECK ADD  CONSTRAINT [Gasolina_Cliente_FK] FOREIGN KEY([idCliente]) REFERENCES [dbo].[Cliente]([idCliente]);
ALTER TABLE [dbo].[Vuelto] WITH CHECK ADD  CONSTRAINT [Cuentas_Cliente_FK] FOREIGN KEY (idCliente) REFERENCES [dbo].[Cliente]([idCliente]);
GO;

INSERT INTO dbo.Cliente VALUES('CUNGA','CUNGA','0','3200','1','JEOVANY','carro','system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('30jimmy','JIMMY','0','3200','1','PEYTON',null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('30jimmy','PELON','0','3200','1','JOSIAS','casa','system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('30jimmy','TOÑO','0','3200','1','PEPE','NOSE','system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('1LEO','LEO','0.03','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2022ISMAEL','ISMAEL','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('1RONNY','RONNY','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('32JOEL','JOEL','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2023alejandro','ALEJANDRO','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2022EZEQUIEL','EZEQUIEL','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('MANGO','MANGO','0.33','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2022CHEVECO','CHEVECO','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ALBERT','ALBERT','0.33','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2024pepin','PEPINalan','0.33','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2024pepin','PEPINalan','0.33','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ROSBYN','ROSBYN','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ROBERTO','ROBERTO','0.25','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ANDREY','ANDREY','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('CACHO','CACHO','0.5','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('IGLESIA','IGLESIA','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('TIO OLDE','TIO OLDE','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ARIOC','ARIOC','0.5','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('JULI','JULI','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('DILAN','DILAN','0.33','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('30jimmy','TINGO','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('SERGIO','SERGIO','0.33','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('JORDI','JORDI','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('JORDI','JORDI','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('RAMON','RAMON','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('prueba','prueba','0.25','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('ALBERT','40ALBERT','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('30jimmy','TINGO','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('MARIO','MARIO','0.05','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('GENGO','GENGO','0.25','3600','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2024pepin','2024pepin','0.25','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('2sergio','2sergio','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('lote Jimmy','lote jimmy','0','3200','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('joel','joel','0','0','0',null,null,'system',getdate(), null, null,1);
INSERT INTO dbo.Cliente VALUES('chira','chira','0','3600','0',null,null,'system',getdate(), null, null,1);

INSERT INTO dbo.Producto VALUES('PP','PP',1700,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('CL','CLASE',1100,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('PG','PG',2500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('CH','CHATARRA',300,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('A','CHATARRAA',300,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('BL','JUBENIL',5000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('BU','CARABALI',2500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('TI','TITI',1600,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('ba','Bagre',700,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('PG2','PG',4000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('RN','CASARONNY',1,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('co','cola',1000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('Y','JUMBO',7000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('afu','afuera',1,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('be','berrogate',600,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('CL2','CLASE',2000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('P','PARGO',1700,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('V','Volador',700,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('can','candado',600,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('ca','CABEZA',8000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('bur','burroespecial',3000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('G','GASDEUDA',1,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('J2','JUVENIL',6500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('LE','LENGUADO',1100,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('1ti','titi',2600,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('PUN','PUNTILLA',3400,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('CAA','CABEZA',7500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('te','teblina',600,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('MA','MACARELA',1200,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('CAA','CABEZA',7500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('ja','jaiva',1000,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('tii','titi',2300,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('pan','pampano',500,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('pp3','pepe',1800,'system',getdate(),null,null);
INSERT INTO dbo.Producto VALUES('RO','ROBALO',2000,'system',getdate(),null,null);

INSERT INTO dbo.Factura VALUES(1, '2024-10-06', 0.15, 1, 1, 50580150, 4900, 50575250, 200, 200, 50574850, 12643712.5, 6574730.5, 7586227.5, 25287025, 1,'system', getdate(), '', '');
INSERT INTO dbo.Factura VALUES(3, '2024-10-03', 0, 0, 0, 2033, 0, 2033, 0, 0, 2033, 508.25, 264.29, 0, 2033,1, 'system', getdate(), '', '');

INSERT INTO dbo.DetalleFactura VALUES(4, 1, 300, 3700, 1110000, 1110000, 'system', getdate(), '', '');
INSERT INTO dbo.DetalleFactura VALUES(5, 1, 300, 900, 270000, 270000, 'system', getdate(), '', '');
INSERT INTO dbo.DetalleFactura VALUES(3, 1, 4000, 300, 1200000, 1200000, 'system', getdate(), '', '');
INSERT INTO dbo.DetalleFactura VALUES(14, 1, 1, 150, 150, 150, 'system', getdate(), '', '');
INSERT INTO dbo.DetalleFactura VALUES(20, 1, 8000, 6000, 48000000, 48000000, 'system', getdate(), '', '');

INSERT INTO dbo.DetalleFactura VALUES(1, 2, 1700, 0.98, 1615, 1615, 'system', getdate(), '', '');
INSERT INTO dbo.DetalleFactura VALUES(2, 2, 1100, 0.38, 418, 418, 'system', getdate(), '', '');

INSERT INTO dbo.MovimientosCredito VALUES( 1, 1, 582.00, '2024-10-18', 'system', getdate(), null, null);

INSERT INTO dbo.Parametro VALUES( 1, '0 %', 0,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '15 %', 0.15,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '20 %', 0.2,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '25 %', 0.25,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '30 %', 0.3,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '33 %', 0.33,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '50 %', 0.5,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 1, '100 %', 1,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 2, 'precio Hielo', 1700,null, 'system', getdate(), null, null);
INSERT INTO dbo.Parametro VALUES( 3, 'precio Gasolina', 3200,null, 'system', getdate(), null, null);

GO;