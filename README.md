"# Mantenedor Canchas API's" 

## Query to create the database:

USE MASTER;
GO

-- Creación de la base de datos
CREATE DATABASE ReservaCanchasDB;
GO

USE ReservaCanchasDB;
GO

-- Creación de la tabla TipoCancha
CREATE TABLE TipoCancha (
    TCanchaId NVARCHAR(50) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL,
    MontoReserva DECIMAL(10, 2) NOT NULL,
    CONSTRAINT CHK_TipoCancha_MontoReserva CHECK (MontoReserva > 0)
);
GO

-- Creación de la tabla Canchas
CREATE TABLE Canchas (
    CanchaId NVARCHAR(50) PRIMARY KEY,
    TCanchaId NVARCHAR(50) NOT NULL,
    Estado CHAR(2) CHECK (Estado IN ('D', 'ND')),
    FOREIGN KEY (TCanchaId) REFERENCES TipoCancha(TCanchaId)
);
GO

-- Creación de la tabla Roles
CREATE TABLE Roles (
    RolId NVARCHAR(50) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL
);
GO

-- Creación de la tabla Usuario
CREATE TABLE Usuario (
    UsuarioId NVARCHAR(50) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI INT NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(20) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    RolId NVARCHAR(50) NOT NULL,
    Password NVARCHAR(256) NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(RolId)
);
GO

-- Creación de la tabla Reserva
CREATE TABLE Reserva (
    ReservaId NVARCHAR(50) PRIMARY KEY,
    UsuarioId NVARCHAR(50) NOT NULL,
    CanchaId NVARCHAR(50) NOT NULL,
    FechaReserva DATE NOT NULL,
    HoraInicioReserva TIME NOT NULL,
    NumeroDeHoras INT NOT NULL,
    HoraFinReserva AS DATEADD(HOUR, NumeroDeHoras, HoraInicioReserva),
    Estado CHAR(4) CHECK (Estado IN ('Conf', 'Can', 'Resv')),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (CanchaId) REFERENCES Canchas(CanchaId)
);
GO

-- Creación de la tabla TipoMetodoPago
CREATE TABLE TipoMetodoPago (
    MetodoPagoId NVARCHAR(50) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL
);
GO

-- Creación de la tabla Transacciones
CREATE TABLE Transacciones (
    TransaccionId NVARCHAR(50) PRIMARY KEY,
    ReservaId NVARCHAR(50) NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    FechaTransaccion DATETIME DEFAULT GETDATE(),
    MetodoPagoId NVARCHAR(50) NOT NULL,
    FOREIGN KEY (ReservaId) REFERENCES Reserva(ReservaId),
    FOREIGN KEY (MetodoPagoId) REFERENCES TipoMetodoPago(MetodoPagoId),
    CONSTRAINT CHK_Transacciones_Monto CHECK (Monto > 0)
);
GO

-- Creación de la tabla Auditoria_Logs
CREATE TABLE Auditoria_Logs (
    LogId NVARCHAR(50) PRIMARY KEY,
    UsuarioId NVARCHAR(50) NOT NULL,
    Accion NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(100) NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId)
);
GO

-- Insertar datos iniciales en Roles
INSERT INTO Roles (RolId, Descripcion) VALUES
('1', 'Administrador'),
('2', 'Empleado'),
('3', 'UsuarioGeneral');
GO

-- Insertar datos iniciales en TipoMetodoPago
INSERT INTO TipoMetodoPago (MetodoPagoId, Descripcion) VALUES
('1', 'Yape'),
('2', 'Plin'),
('3', 'Efectivo'),
('4', 'Transferencia');
GO

-- Insertar datos iniciales en TipoCancha
INSERT INTO TipoCancha (TCanchaId, Descripcion, MontoReserva) VALUES
('F7', 'Fútbol 7', 140),
('F9', 'Fútbol 9', 198),
('F11', 'Fútbol 11', 242),
('V', 'Volley', 132);
GO

-- Creación de índices para optimización
-- Índice en la tabla Usuario para buscar por Email rápidamente
CREATE UNIQUE INDEX IDX_Usuario_Email ON Usuario (Email);
GO

-- Índice en la tabla Reserva para mejorar las búsquedas por UsuarioId y CanchaId
CREATE INDEX IDX_Reserva_UsuarioId ON Reserva (UsuarioId);
CREATE INDEX IDX_Reserva_CanchaId ON Reserva (CanchaId);
GO

-- Índice en la tabla Transacciones para mejorar las búsquedas por ReservaId
CREATE INDEX IDX_Transacciones_ReservaId ON Transacciones (ReservaId);
GO

-- Índice en la tabla Auditoria_Logs para mejorar las búsquedas por UsuarioId
CREATE INDEX IDX_Auditoria_Logs_UsuarioId ON Auditoria_Logs (UsuarioId);
GO

## Query to insert test info:

-- Registro preliminares de datos:

use ReservaCanchasDB;
go

-- Insertar datos iniciales en Roles: Se inserta en la creación de la BD
--INSERT INTO Roles (RolId, Descripcion) VALUES
--('1', 'Administrador'),
--('2', 'Empleado'),
--('3', 'UsuarioGeneral');
--GO

-- Insertar datos iniciales en TipoMetodoPago: Se inserta en la creación de la BD
--INSERT INTO TipoMetodoPago (MetodoPagoId, Descripcion) VALUES
--('1', 'Yape'),
--('2', 'Plin'),
--('3', 'Efectivo'),
--('4', 'Transferencia');
--GO

-- Insertar datos iniciales en TipoCancha: Se inserta en la creación de la BD
--INSERT INTO TipoCancha (TCanchaId, Descripcion, MontoReserva) VALUES
--('F7', 'Fútbol 7', 140),
--('F9', 'Fútbol 9', 198),
--('F11', 'Fútbol 11', 242),
--('V', 'Volley', 132);
--GO

-- Insertar datos en la tabla Canchas
INSERT INTO Canchas (CanchaId, TCanchaId, Estado) VALUES
('C1', 'F11', 'D'), -- 1 de Fútbol 11
('C2', 'F7', 'D'),  -- 1 de Fútbol 7
('C3', 'F7', 'ND'), -- 1 de Fútbol 7
('C4', 'F7', 'D'),  -- 1 de Fútbol 7
('C5', 'F9', 'ND'); -- 1 de Fútbol 9
GO

-- Insertar datos en la tabla Usuario
INSERT INTO Usuario (UsuarioId, Nombre, Apellido, DNI, Email, Telefono, RolId, Password) VALUES
-- Administradores
('U1', 'Ana', 'Gomez', 12345678, 'ana.gomez@gmail.com', '912345678', '1', 'AG12345678'),
('U2', 'Carlos', 'Perez', 23456789, 'carlos.perez@gmail.com', '923456789', '1', 'CP23456789'),
-- Empleados
('U3', 'Luis', 'Ramirez', 34567890, 'luis.ramirez@gmail.com', '934567890', '2', 'LR34567890'),
('U4', 'Maria', 'Torres', 45678901, 'maria.torres@gmail.com', '945678901', '2', 'MT45678901'),
('U5', 'Jose', 'Diaz', 56789012, 'jose.diaz@gmail.com', '956789012', '2', 'JD56789012'),
('U6', 'Laura', 'Lopez', 67890123, 'laura.lopez@gmail.com', '967890123', '2', 'LL67890123'),
('U7', 'Juan', 'Garcia', 78901234, 'juan.garcia@gmail.com', '978901234', '2', 'JG78901234'),
-- Usuarios Generales
('U8', 'Pedro', 'Fernandez', 89012345, 'pedro.fernandez@gmail.com', '989012345', '3', 'PF89012345'),
('U9', 'Marta', 'Sanchez', 90123456, 'marta.sanchez@gmail.com', '990123456', '3', 'MS90123456'),
('U10', 'Rosa', 'Martinez', 10234567, 'rosa.martinez@gmail.com', '910234567', '3', 'RM10234567'),
('U11', 'David', 'Ruiz', 20345678, 'david.ruiz@gmail.com', '920345678', '3', 'DR20345678'),
('U12', 'Laura', 'Herrera', 30456789, 'laura.herrera@outlook.com', '930456789', '3', 'LH30456789'),
('U13', 'Enrique', 'Gonzalez', 40567890, 'enrique.gonzalez@outlook.com', '940567890', '3', 'EG40567890'),
('U14', 'Lucia', 'Morales', 50678901, 'lucia.morales@outlook.com', '950678901', '3', 'LM50678901'),
('U15', 'Pablo', 'Ortega', 60789012, 'pablo.ortega@outlook.com', '960789012', '3', 'PO60789012'),
('U16', 'Raquel', 'Romero', 70890123, 'raquel.romero@outlook.com', '970890123', '3', 'RR70890123'),
('U17', 'Sofia', 'Cruz', 80901234, 'sofia.cruz@outlook.com', '980901234', '3', 'SC80901234'),
('U18', 'Hugo', 'Vargas', 91012345, 'hugo.vargas@outlook.com', '991012345', '3', 'HV91012345'),
('U19', 'Teresa', 'Mendoza', 11234567, 'teresa.mendoza@gmail.com', '911234567', '3', 'TM11234567'),
('U20', 'Alberto', 'Guerrero', 22345678, 'alberto.guerrero@gmail.com', '922345678', '3', 'AG22345678'),
('U21', 'Sandra', 'Reyes', 33456789, 'sandra.reyes@gmail.com', '933456789', '3', 'SR33456789'),
('U22', 'Manuel', 'Castro', 44567890, 'manuel.castro@gmail.com', '944567890', '3', 'MC44567890'),
('U23', 'Nuria', 'Delgado', 55678901, 'nuria.delgado@gmail.com', '955678901', '3', 'ND55678901'),
('U24', 'Vicente', 'Gil', 66789012, 'vicente.gil@gmail.com', '966789012', '3', 'VG66789012'),
('U25', 'Julia', 'Soto', 77890123, 'julia.soto@gmail.com', '977890123', '3', 'JS77890123');
GO

-- Insertar datos en la tabla Reserva

-- Reservas Confirmadas
INSERT INTO Reserva (ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado)
VALUES
('R1', 'U8', 'C1', '2024-05-08', '07:00', 1, 'Conf'),  -- Confirmada, 1 hora
('R2', 'U9', 'C1', '2024-05-08', '08:00', 1, 'Conf'),  -- Confirmada, 1 hora
('R3', 'U10', 'C1', '2024-05-08', '10:00', 2, 'Conf'); -- Confirmada, 2 horas
GO

-- Reservas Reservadas (días aleatorios de la siguiente semana)
INSERT INTO Reserva (ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado)
VALUES
('R4', 'U11', 'C2', '2024-05-13', '09:00', 1, 'Resv'),  -- Reservada, 1 hora
('R5', 'U12', 'C2', '2024-05-14', '11:00', 2, 'Resv'),  -- Reservada, 2 horas
('R6', 'U13', 'C2', '2024-05-15', '13:00', 1, 'Resv'),  -- Reservada, 1 hora
('R7', 'U14', 'C2', '2024-05-16', '15:00', 2, 'Resv'),  -- Reservada, 2 horas
('R8', 'U15', 'C2', '2024-05-17', '17:00', 1, 'Resv');  -- Reservada, 1 hora
GO

-- Reservas Canceladas por cruce de horarios con las confirmadas
INSERT INTO Reserva (ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado)
VALUES
('R9', 'U16', 'C1', '2024-05-08', '07:30', 1, 'Can'),  -- Cancelada, cruza con confirmada
('R10', 'U17', 'C1', '2024-05-08', '09:30', 1, 'Can'); -- Cancelada, cruza con confirmada
GO

-- Insertar datos en la tabla Transacciones para reservas confirmadas
INSERT INTO Transacciones (TransaccionId, ReservaId, Monto, FechaTransaccion, MetodoPagoId) VALUES
-- Transacciones Confirmadas
('T1', 'R1', 242 * 1, GETDATE(), '1'), -- Fútbol 11, 1 hora, método de pago: Yape (aleatorio)
('T2', 'R2', 242 * 1, GETDATE(), '2'), -- Fútbol 11, 1 hora, método de pago: Plin (aleatorio)
('T3', 'R3', 242 * 2, GETDATE(), '3'), -- Fútbol 11, 2 horas, método de pago: Efectivo (aleatorio)
-- Transacciones Reservadas
('T4', 'R4', 140 * 1 * 0.2, GETDATE(), '4'),  -- Fútbol 7, 1 hora, 20% del monto, método de pago: Transferencia (aleatorio)
('T5', 'R5', 140 * 2 * 0.3, GETDATE(), '1'),  -- Fútbol 7, 2 horas, 30% del monto, método de pago: Yape (aleatorio)
('T6', 'R6', 140 * 1 * 0.2, GETDATE(), '2'),  -- Fútbol 7, 1 hora, 20% del monto, método de pago: Plin (aleatorio)
('T7', 'R7', 140 * 2 * 0.3, GETDATE(), '3'),  -- Fútbol 7, 2 horas, 30% del monto, método de pago: Efectivo (aleatorio)
('T8', 'R8', 140 * 1 * 0.2, GETDATE(), '4');  -- Fútbol 7, 1 hora, 20% del monto, método de pago: Transferencia (aleatorio)
GO

-- Insertar datos en la tabla Auditoria_Logs
INSERT INTO Auditoria_Logs (LogId, UsuarioId, Accion, Descripcion, FechaHora) VALUES
-- Acciones relacionadas con la creación de reservas confirmadas
('L1', 'U8', 'Crear Reserva', 'Reserva R1 creada y confirmada por el usuario U8', GETDATE()),
('L2', 'U9', 'Crear Reserva', 'Reserva R2 creada y confirmada por el usuario U9', GETDATE()),
('L3', 'U10', 'Crear Reserva', 'Reserva R3 creada y confirmada por el usuario U10', GETDATE()),
-- Acciones relacionadas con la creación de reservas reservadas
('L4', 'U11', 'Crear Reserva', 'Reserva R4 creada y reservada por el usuario U11', GETDATE()),
('L5', 'U12', 'Crear Reserva', 'Reserva R5 creada y reservada por el usuario U12', GETDATE()),
('L6', 'U13', 'Crear Reserva', 'Reserva R6 creada y reservada por el usuario U13', GETDATE()),
('L7', 'U14', 'Crear Reserva', 'Reserva R7 creada y reservada por el usuario U14', GETDATE()),
('L8', 'U15', 'Crear Reserva', 'Reserva R8 creada y reservada por el usuario U15', GETDATE()),
-- Acciones relacionadas con la cancelación de reservas
('L9', 'U16', 'Cancelar Reserva', 'Reserva R9 cancelada por el usuario U16 debido a cruce de horario', GETDATE()),
('L10', 'U17', 'Cancelar Reserva', 'Reserva R10 cancelada por el usuario U17 debido a cruce de horario', GETDATE()),
-- Acciones relacionadas con la creación de transacciones para reservas confirmadas
('L11', 'U8', 'Crear Transacción', 'Transacción T1 creada para la reserva R1 por el usuario U8', GETDATE()),
('L12', 'U9', 'Crear Transacción', 'Transacción T2 creada para la reserva R2 por el usuario U9', GETDATE()),
('L13', 'U10', 'Crear Transacción', 'Transacción T3 creada para la reserva R3 por el usuario U10', GETDATE()),
-- Acciones relacionadas con la creación de transacciones para reservas reservadas
('L14', 'U11', 'Crear Transacción', 'Transacción T4 creada para la reserva R4 por el usuario U11', GETDATE()),
('L15', 'U12', 'Crear Transacción', 'Transacción T5 creada para la reserva R5 por el usuario U12', GETDATE()),
('L16', 'U13', 'Crear Transacción', 'Transacción T6 creada para la reserva R6 por el usuario U13', GETDATE()),
('L17', 'U14', 'Crear Transacción', 'Transacción T7 creada para la reserva R7 por el usuario U14', GETDATE()),
('L18', 'U15', 'Crear Transacción', 'Transacción T8 creada para la reserva R8 por el usuario U15', GETDATE());
GO

## Query to create some transacctions: SP, Trigger, etc.

-- Procedimientos Almacenados:
--		SET NOCOUNT ON: es una configuración útil para mejorar el rendimiento y reducir la sobrecarga de red
-- en procedimientos almacenados y scripts SQL que realizan muchas operaciones de modificación de datos, 
-- suprimiendo el mensaje de recuento de filas afectadas que SQL Server devuelve de forma predeterminada.


-- Procedimiento Almacenado para Insertar o Actualizar el tipo de Cancha
CREATE PROCEDURE sp_InsertOrUpdateTipoCancha
    @TCanchaId NVARCHAR(50),
    @Descripcion NVARCHAR(100),
    @MontoReserva DECIMAL(10, 2)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM TipoCancha WHERE TCanchaId = @TCanchaId)
    BEGIN
        UPDATE TipoCancha
        SET Descripcion = @Descripcion, MontoReserva = @MontoReserva
        WHERE TCanchaId = @TCanchaId;
    END
    ELSE
    BEGIN
        INSERT INTO TipoCancha (TCanchaId, Descripcion, MontoReserva)
        VALUES (@TCanchaId, @Descripcion, @MontoReserva);
    END
END;
GO


-- Procedimiento Almacenado para Cambiar el Estado de una Cancha
-- Este procedimiento almacenado cambiará el estado de una cancha específica entre 'D' (Disponible) y 'ND' (No Disponible). Utiliza transacción para asegurar la consistencia de los datos.

CREATE PROCEDURE sp_ChangeCanchaEstado
    @CanchaId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentEstado CHAR(2);
    DECLARE @NewEstado CHAR(2);

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @CurrentEstado = Estado
        FROM Canchas
        WHERE CanchaId = @CanchaId;

        IF @CurrentEstado IS NULL
        BEGIN
            RAISERROR('La Cancha con este ID no existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @CurrentEstado = 'D'
        BEGIN
            SET @NewEstado = 'ND';
        END
        ELSE IF @CurrentEstado = 'ND'
        BEGIN
            SET @NewEstado = 'D';
        END
        ELSE
        BEGIN
            RAISERROR('Estado inválido en la Cancha.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE Canchas
        SET Estado = @NewEstado
        WHERE CanchaId = @CanchaId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),
               @ErrorSeverity = ERROR_SEVERITY(),
               @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

-- Trigger para Auditar Cambios en la Tabla Canchas
-- Puedes crear un trigger que registre automáticamente cualquier cambio en el estado de las canchas en la tabla Auditoria_Logs.

CREATE TRIGGER trg_AuditCanchaEstadoChange
ON Canchas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(Estado)
    BEGIN
        DECLARE @CanchaId NVARCHAR(50);
        DECLARE @UsuarioId NVARCHAR(50);
        DECLARE @OldEstado CHAR(2);
        DECLARE @NewEstado CHAR(2);

        SELECT @CanchaId = inserted.CanchaId,
               @OldEstado = deleted.Estado,
               @NewEstado = inserted.Estado
        FROM inserted
        INNER JOIN deleted ON inserted.CanchaId = deleted.CanchaId;

        -- Obtener el usuario responsable 
        SET @UsuarioId = SUSER_SNAME(); -- Esta función devuelve el nombre de usuario de la sesión actual.

        INSERT INTO Auditoria_Logs (LogId, UsuarioId, Accion, Descripcion, FechaHora)
        VALUES (NEWID(), @UsuarioId, 'Actualizar Estado Cancha',
                'Estado de la cancha ' + @CanchaId + ' cambiado de ' + @OldEstado + ' a ' + @NewEstado,
                GETDATE());
    END
END;
GO

-- Trigger para Validar Reservas
-- trigger que evite la inserción de reservas existentes en la misma cancha.

CREATE TRIGGER trg_ValidateReservaSolapada
ON Reserva
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CanchaId NVARCHAR(50);
    DECLARE @FechaReserva DATE;
    DECLARE @HoraInicioReserva TIME;
    DECLARE @NumeroDeHoras INT;
    DECLARE @HoraFinReserva TIME;

    DECLARE @ConflictCount INT;

    -- Iterar los registros ya insertados
    DECLARE insert_cursor CURSOR FOR
    SELECT CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras
    FROM inserted;

    OPEN insert_cursor;
    FETCH NEXT FROM insert_cursor INTO @CanchaId, @FechaReserva, @HoraInicioReserva, @NumeroDeHoras;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular la hora de fin
        SET @HoraFinReserva = DATEADD(HOUR, @NumeroDeHoras, @HoraInicioReserva);

        -- Verificar si existe una reserva
        SELECT @ConflictCount = COUNT(*)
        FROM Reserva r
        WHERE r.CanchaId = @CanchaId
          AND r.FechaReserva = @FechaReserva
          AND (
                (r.HoraInicioReserva < @HoraFinReserva AND r.HoraInicioReserva >= @HoraInicioReserva) OR
                (@HoraInicioReserva < DATEADD(HOUR, r.NumeroDeHoras, r.HoraInicioReserva) AND @HoraInicioReserva >= r.HoraInicioReserva)
              )
          AND r.Estado IN ('Conf', 'Resv');

        -- Si hay conflictos, lanzar un error
        IF @ConflictCount > 0
        BEGIN
            RAISERROR('Existe una reserva para la misma cancha y horario.', 16, 1);
            CLOSE insert_cursor;
            DEALLOCATE insert_cursor;
            RETURN;
        END

        -- Insertar la nueva reserva si esta todo ok
        INSERT INTO Reserva (ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado)
        SELECT ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado
        FROM inserted;

        FETCH NEXT FROM insert_cursor INTO @CanchaId, @FechaReserva, @HoraInicioReserva, @NumeroDeHoras;
    END

    CLOSE insert_cursor;
    DEALLOCATE insert_cursor;
END;
GO


-- Trigger para Actualizar el Estado de las Reservas Automáticamente
-- trigger que actualiza el estado de las reservas automáticamente al pasar la fecha y hora de finalización.

CREATE TRIGGER trg_UpdateReservaEstado
ON Reserva
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Reserva
    SET Estado = 'Can'
    WHERE Estado = 'Resv'
      AND GETDATE() > DATEADD(HOUR, NumeroDeHoras, CAST(FechaReserva AS DATETIME) + CAST(HoraInicioReserva AS DATETIME));
END;
GO

-- Procedimiento para Obtener Disponibilidad de Canchas
-- Este procedimiento obtiene la disponibilidad de las canchas para una fecha específica.

CREATE PROCEDURE sp_GetCanchaDisponibilidad
    @FechaReserva DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.CanchaId, c.TCanchaId, c.Estado,
           r.FechaReserva, r.HoraInicioReserva, r.NumeroDeHoras,
           DATEADD(HOUR, r.NumeroDeHoras, r.HoraInicioReserva) AS HoraFinReserva
    FROM Canchas c
    LEFT JOIN Reserva r ON c.CanchaId = r.CanchaId AND r.FechaReserva = @FechaReserva
    WHERE c.Estado = 'D' OR (c.Estado = 'ND' AND r.Estado IS NULL);
END;
GO

-- Procedimiento para Obtener Historial de Reservas de un Usuario
-- Este procedimiento obtiene todas las reservas pasadas y futuras de un usuario.

CREATE PROCEDURE sp_GetHistorialReservasUsuario
    @UsuarioId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.ReservaId, r.CanchaId, r.FechaReserva, r.HoraInicioReserva, r.NumeroDeHoras,
           r.HoraFinReserva, r.Estado
    FROM Reserva r
    WHERE r.UsuarioId = @UsuarioId
    ORDER BY r.FechaReserva DESC, r.HoraInicioReserva DESC;
END;
GO

-- Procedimiento para Cancelar Reserva
-- Este procedimiento cancela una reserva cambiando su estado a 'Can' (Cancelado)

CREATE PROCEDURE sp_CancelarReserva
    @ReservaId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Reserva
    SET Estado = 'Can'
    WHERE ReservaId = @ReservaId
      AND Estado <> 'Can';
END;
GO

--Procedimiento Almacenado para Ingresar un Nuevo Usuario

CREATE PROCEDURE sp_InsertarUsuario
    @UsuarioId NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI INT,
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @RolId NVARCHAR(50),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Iniciar la transacción
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email OR DNI = @DNI)
        BEGIN
            RAISERROR('El usuario con este Email o DNI ya existe.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        INSERT INTO Usuario (UsuarioId, Nombre, Apellido, DNI, Email, Telefono, FechaRegistro, RolId, Password)
        VALUES (@UsuarioId, @Nombre, @Apellido, @DNI, @Email, @Telefono, GETDATE(), @RolId, @Password);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),
               @ErrorSeverity = ERROR_SEVERITY(),
               @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO



-- Procedimiento para Actualizar Información de Usuario

CREATE PROCEDURE sp_ActualizarUsuario
    @UsuarioId NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI INT,
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuario
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        DNI = @DNI,
        Email = @Email,
        Telefono = @Telefono,
        Password = @Password
    WHERE UsuarioId = @UsuarioId;
END;
GO

-- Procedimiento para Crear Transacciones
-- Este procedimiento crea una transacción para una reserva específica.

CREATE PROCEDURE sp_CrearTransaccion
    @ReservaId NVARCHAR(50),
    @Monto DECIMAL(10, 2),
    @MetodoPagoId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Transacciones (TransaccionId, ReservaId, Monto, FechaTransaccion, MetodoPagoId)
    VALUES (NEWID(), @ReservaId, @Monto, GETDATE(), @MetodoPagoId);
END;
GO

-- Procedimiento Almacenado para Generar una Reserva
-- Este procedimiento almacenado insertará una nueva reserva en la tabla Reserva

CREATE PROCEDURE sp_GenerarReserva
    @ReservaId NVARCHAR(50),
    @UsuarioId NVARCHAR(50),
    @CanchaId NVARCHAR(50),
    @FechaReserva DATE,
    @HoraInicioReserva TIME,
    @NumeroDeHoras INT,
    @Estado CHAR(4),
    @Monto DECIMAL(10, 2),
    @MetodoPagoId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar la nueva reserva
        INSERT INTO Reserva (ReservaId, UsuarioId, CanchaId, FechaReserva, HoraInicioReserva, NumeroDeHoras, Estado)
        VALUES (@ReservaId, @UsuarioId, @CanchaId, @FechaReserva, @HoraInicioReserva, @NumeroDeHoras, @Estado);

        -- Llamar al procedimiento almacenado para crear la transacción asociada
        EXEC sp_CrearTransaccion @ReservaId, @Monto, @MetodoPagoId

        -- Confirmar la transacción
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Manejar errores y revertir la transacción
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Reportar el error
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),
               @ErrorSeverity = ERROR_SEVERITY(),
               @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO


-- Función para Obtener el Estado de una Cancha en una Fecha y Hora Específica

CREATE FUNCTION fn_ObtenerEstadoCancha
(
    @CanchaId NVARCHAR(50),
    @FechaReserva DATE,
    @HoraInicioReserva TIME
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Estado NVARCHAR(50);

    SELECT @Estado = CASE
                        WHEN r.ReservaId IS NOT NULL THEN 'Reservada'
                        ELSE c.Estado
                     END
    FROM Canchas c
    LEFT JOIN Reserva r ON c.CanchaId = r.CanchaId
                       AND r.FechaReserva = @FechaReserva
                       AND r.HoraInicioReserva = @HoraInicioReserva
    WHERE c.CanchaId = @CanchaId;

    RETURN @Estado;
END;
GO

