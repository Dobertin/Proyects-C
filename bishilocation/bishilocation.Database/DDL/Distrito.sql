CREATE TABLE [dbo].[Distrito]
(
	[IdDistrito] varchar(6) NOT NULL PRIMARY KEY,
	[Nombre] varchar(45) DEFAULT NULL,
	[IdProvincia] varchar(4) DEFAULT NULL,
	[IdDepartamento] varchar(2) DEFAULT NULL, 
    CONSTRAINT [FK_Distrito_ToTable] FOREIGN KEY ([IdProvincia]) REFERENCES [Provincia]([IdProvincia]), 
    CONSTRAINT [FK_Distrito_ToTable_1] FOREIGN KEY ([IdDepartamento]) REFERENCES [Departamento]([IdDepartamento])
)

 