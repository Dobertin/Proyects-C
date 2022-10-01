CREATE TABLE [dbo].[Provincia]
(
	[IdProvincia] VARCHAR(4) NOT NULL PRIMARY KEY, 
    [Nombre] VARCHAR(45) NULL, 
    [IdDepartamento] VARCHAR(2) NULL, 
    CONSTRAINT [FK_Provincia_ToTable] FOREIGN KEY ([IdDepartamento]) REFERENCES [Departamento]([IdDepartamento]) 
)
