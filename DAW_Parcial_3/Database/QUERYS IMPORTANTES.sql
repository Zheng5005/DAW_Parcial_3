USE SistemaTickets;

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
JOIN Empleados E ON E.id_empleado = T.id_empleado
JOIN Comentarios C ON C.id_comentario = T.id_comentario WHERE C.estado = 'Iniciado';

SELECT T.id_ticket, E.nombre, E.apellido, C.comentario, T.progreso, T.prioridad FROM Tickets T
LEFT JOIN Empleados E ON E.id_empleado = T.id_empleado
LEFT JOIN Comentarios C ON C.id_comentario = T.id_comentario;

-- Estados
-- No Asignado (Solo lo ve el admin)
-- Iniciado (Este es cuando se le asigna a un trabajador)
-- Procesando (El trabajador se pone a trabajar en ello)
-- Finalizado (El trabajador a finalizado con ese trabajo)
SELECT T.id_ticket, T.id_user, T.progreso, C.estado, T.prioridad, C.id_comentario, C.comentario, T.fecha_inicio, T.fecha_fin FROM Tickets T
JOIN Comentarios C ON C.id_ticket = T.id_ticket;

-- Para el manejo hacer datos pequeños para agrupar
SELECT progreso, count(progreso) FROM Tickets
WHERE id_empleado = 2 group by progreso;


-- Empleado ve tickets
SELECT T.id_ticket, U.nombre, U.apellido FROM Tickets T
JOIN Usuarios U on U.id_user = T.id_user;

-- Estimado de tiempo en finalizar un ticket
SELECT
DATEDIFF(day, fecha_inicio, fecha_fin) AS Dias,
DATEDIFF(HOUR, fecha_inicio, fecha_fin) AS Horas,
DATEDIFF(MINUTE, fecha_inicio, fecha_fin) AS Minutos,
DATEDIFF(SECOND, fecha_inicio, fecha_fin) AS Segundos
FROM Tickets WHERE id_user = 1;

SELECT A.nombre, COUNT(A.nombre) FROM Tickets T
JOIN Areas A ON A.id_area = T.id_area GROUP BY A.nombre;