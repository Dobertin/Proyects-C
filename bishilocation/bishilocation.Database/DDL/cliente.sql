CREATE TABLE [dbo].[cliente]
(
	[IdCliente] INT identity NOT NULL PRIMARY KEY, 
    [Nombre] NCHAR(50) NULL, 
    [ApellidoMaterno] NCHAR(30) NULL, 
    [ApellidoPaterno] NCHAR(30) NULL, 
    [TipoDoc] CHAR NULL, 
    [NroDoc] NCHAR(15) NULL, 
    [Direccion] NCHAR(80) NULL, 
    [IdDistrito] VARCHAR(6) NOT NULL, 
    CONSTRAINT [FK_cliente_ToTable] FOREIGN KEY ([IdDistrito]) REFERENCES [Distrito]([IdDistrito])
)
