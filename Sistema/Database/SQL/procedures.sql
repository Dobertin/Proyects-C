CREATE PROCEDURE CambiarTiendaUsuario(
    IN p_idUsuario INT,
    IN p_idTienda INT,
    IN p_idAdmin INT
)
BEGIN
    DECLARE rolAdmin INT;

    -- Verificar si el usuario que realiza la acción es Administrador
    SELECT idRol INTO rolAdmin FROM Usuario WHERE idUsuario = p_idAdmin;
    IF rolAdmin != 1 THEN -- Suponiendo que el ID del rol Administrador es 1
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Solo un administrador puede cambiar la tienda de un usuario.';
    END IF;

    -- Actualizar la tienda del usuario
    UPDATE Usuario
    SET idTienda = p_idTienda, usuarioActualizacion = p_idAdmin
    WHERE idUsuario = p_idUsuario;

    -- Registrar en auditoría
    INSERT INTO AuditoriaUsuario (idUsuario, accion, detalles, usuarioResponsable)
    VALUES (p_idUsuario, 'Cambio de tienda', CONCAT('Nueva tienda ID: ', p_idTienda), p_idAdmin);
END


CREATE PROCEDURE CambiarRolUsuario(
    IN p_idUsuario INT,
    IN p_idRol INT,
    IN p_idResponsable INT
)
BEGIN
    DECLARE rolResponsable INT;

    -- Verificar el rol del usuario que realiza la acción
    SELECT idRol INTO rolResponsable FROM Usuario WHERE idUsuario = p_idResponsable;

    IF rolResponsable NOT IN (1, 2) THEN -- Solo Administradores y Supervisores pueden cambiar roles
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Solo un administrador o supervisor puede cambiar roles.';
    END IF;

    -- Restricción adicional: Supervisores no pueden asignar roles de Administrador o Supervisor
    IF rolResponsable = 2 AND p_idRol IN (1, 2) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Un supervisor no puede asignar roles de Administrador o Supervisor.';
    END IF;

    -- Actualizar el rol del usuario
    UPDATE Usuario
    SET idRol = p_idRol, usuarioActualizacion = p_idResponsable
    WHERE idUsuario = p_idUsuario;

    -- Registrar en auditoría
    INSERT INTO AuditoriaUsuario (idUsuario, accion, detalles, usuarioResponsable)
    VALUES (p_idUsuario, 'Cambio de rol', CONCAT('Nuevo rol ID: ', p_idRol), p_idResponsable);
END
