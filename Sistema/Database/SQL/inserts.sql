-- Roles
INSERT INTO Rol (nombreRol, descripcion) VALUES
('Administrador', 'Usuario con acceso completo al sistema.'),
('Supervisor', 'Usuario encargado de gestionar tiendas y equipos de ventas.'),
('Vendedor', 'Usuario que realiza ventas en una tienda.');

-- Tiendas
INSERT INTO Tienda (nombreTienda, direccion, telefono) VALUES
('Tienda Central', 'Av. Principal 123', '555-1234'),
('Sucursal Norte', 'Calle Norte 456', '555-5678');

-- Usuarios
INSERT INTO Usuario (nombre, apellidos, email, telefono, usuario, contrasena, idRol, idTienda, usuarioCreacion)
VALUES
('Juan', 'Perez', 'admin@example.com', '555-9999', 'admin', SHA2('password123', 256), 1, NULL, 1),
('Carlos', 'Lopez', 'supervisor@example.com', '555-8888', 'supervisor', SHA2('password123', 256), 2, 1, 1),
('Ana', 'Martinez', 'vendedor@example.com', '555-7777', 'vendedor', SHA2('password123', 256), 3, 1, 1);
