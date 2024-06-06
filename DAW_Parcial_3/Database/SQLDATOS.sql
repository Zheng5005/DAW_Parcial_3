USE SistemaTickets;

GO
INSERT INTO Empleados 
(nombre, apellido, correo, telefono, contrasena, rol, foto)
VALUES 
('Francisco', 'Cisneros', 'jorgefranciscocz@gmail.com', 75566457, '852456', 'Admin', NULL),
('Brandon', 'Heat', 'gungraveheat123@gmail.com', 74169587, '852456', 'Empleado', NULL),
('Francisco', 'Chacon', 'franciscoeduaroc@gmail.com', 75962189, '156268', 'Empleado', NULL),
('Jose', 'Manzanares', 'josecarlosm@gmail.com', 78313548, '564443', 'Empleado', NULL),
('Esmeralda', 'Garcia', 'esmeraldanoemig@gmail.com', 79213056, '126345', 'Empleada', NULL),
('Fernando', 'Gomez', 'fernandouliseg@gmail.com', 74619813, '897164', 'Empleado', NULL),
('Juan', 'Perez', 'juancarlosp@gmail.com', 71319831, '489168', 'Empleado', NULL);

INSERT INTO Usuarios
(nombre, apellido, correo, telefono, contrasena, nombre_empresa, nombre_contacto, foto)
VALUES
('Jorge', 'Zometa', 'jorge.cisneros1@catolica.edu.sv', 75459565, '852456', NULL, NULL, NULL),
('Jorge', 'Zometa', 'HarryMC@gmail.com', 75459565, '852456', NULL, NULL, NULL);

INSERT INTO Areas
(nombre)
VALUES
('Base de datos'),('Correo'),('Instalacion'),('Cobro');