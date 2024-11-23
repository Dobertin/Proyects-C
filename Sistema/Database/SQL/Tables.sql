CREATE DATABASE `sistema`

CREATE TABLE Rol (
    idRol INT AUTO_INCREMENT PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL UNIQUE, -- Ejemplo: "Administrador", "Supervisor", "Vendedor"
    descripcion TEXT NULL
);

CREATE TABLE Tienda (
    idTienda INT AUTO_INCREMENT PRIMARY KEY,
    nombreTienda VARCHAR(100) NOT NULL UNIQUE,
    direccion VARCHAR(255) NULL,
    telefono VARCHAR(15) NULL
);
CREATE TABLE Usuario (
    idUsuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telefono VARCHAR(15) NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    contrasena VARCHAR(255) NOT NULL, -- Encriptada
    pathFoto VARCHAR(255) NULL, -- Ruta para la foto de perfil
    idRol INT NOT NULL,
    idTienda INT DEFAULT NULL, -- Solo para supervisores y vendedores
    fechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    fechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    usuarioCreacion INT NOT NULL, -- ID del usuario que crea este registro
    usuarioActualizacion INT DEFAULT NULL, -- ID del usuario que actualiza este registro
    CONSTRAINT FK_Rol FOREIGN KEY (idRol) REFERENCES Rol(idRol),
    CONSTRAINT FK_Tienda FOREIGN KEY (idTienda) REFERENCES Tienda(idTienda)
);
CREATE TABLE AuditoriaUsuario (
    idAuditoria INT AUTO_INCREMENT PRIMARY KEY,
    idUsuario INT NOT NULL, -- Usuario afectado
    accion VARCHAR(50) NOT NULL, -- Ejemplo: "Cambio de tienda", "Cambio de rol"
    detalles TEXT NOT NULL, -- Detalles de la acción
    fechaAccion DATETIME DEFAULT CURRENT_TIMESTAMP,
    usuarioResponsable INT NOT NULL, -- Usuario que realizó la acción
    CONSTRAINT FK_Usuario_Auditado FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
    CONSTRAINT FK_Usuario_Responsable FOREIGN KEY (usuarioResponsable) REFERENCES Usuario(idUsuario)
);
