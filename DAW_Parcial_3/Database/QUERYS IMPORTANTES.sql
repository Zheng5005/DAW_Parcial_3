-- Si es usuario
SELECT T.id_ticket, T.fecha_inicio, T.serv_app, U.nombre, U.apellido, T.descripcion, T.prioridad FROM Tickets T
JOIN  Usuarios U on U.id_user = T.id_user WHERE U.id_user = 1;

--Si es empleado
SELECT T.id_ticket, T.fecha_inicio, T.serv_app, E.nombre, E.apellido, T.descripcion, T.prioridad FROM Tickets T
JOIN  Usuarios E on E.id_user = T.id_user WHERE T.id_empleado = 13;

-- si es admin ve todos
SELECT T.id_ticket, T.fecha_inicio, T.serv_app, E.nombre, E.apellido, T.descripcion, T.prioridad, E.nombre, E.apellido, T.fecha_fin FROM Tickets T
JOIN  Usuarios U on U.id_user = T.id_user
LEFT JOIN Empleados E on E.id_empleado = T.id_empleado;

-- Mostrar los datos en la zona de gestion
SELECT T.id_ticket, E.nombre, E.apellido, T.id_comentario, C.comentario FROM Tickets T
LEFT JOIN Empleados E ON E.id_empleado = T.id_empleado
LEFT JOIN Comentarios C ON C.id_comentario = T.id_comentario WHERE C.estado = 'Progreso';

-- Estimado de tiempo en finalizar un ticket
SELECT id_user, fecha_asig, fecha_fin, 
DATEDIFF(day, fecha_asig, fecha_fin) AS Dias,
DATEDIFF(HOUR, fecha_asig, fecha_fin) AS Horas 
FROM Tickets WHERE id_user = 3