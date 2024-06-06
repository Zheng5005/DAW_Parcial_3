-- Eliminar el procedimiento almacenado existente si existe
IF OBJECT_ID('AddComentarioToTicket', 'P') IS NOT NULL
DROP PROCEDURE AddComentarioToTicket;
GO

-- Crear el nuevo procedimiento almacenado
CREATE PROCEDURE AddComentarioToTicket
    @id_ticket INT,
    @comentario VARCHAR(MAX),
    @estado VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar el comentario en la tabla Comentarios
    INSERT INTO Comentarios (id_ticket, comentario, estado)
    VALUES (@id_ticket, @comentario, @estado);

    -- Actualizar el progreso del ticket
    UPDATE Tickets
    SET progreso = @estado
    WHERE id_ticket = @id_ticket;
END;
GO
