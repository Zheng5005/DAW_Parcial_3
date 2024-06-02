CREATE DATABASE SistemaTickets;

GO

USE SistemaTickets;
CREATE TABLE Usuarios (
    id_user INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    correo VARCHAR(250) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    contrasena VARCHAR(10) NOT NULL,
    nombre_empresa VARCHAR(10),
    nombre_contacto VARCHAR(10),
    foto VARCHAR(MAX)
);

CREATE TABLE Empleados (
    id_empleado INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    correo VARCHAR(250) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    contrasena VARCHAR(10) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    foto VARCHAR(MAX)
);

CREATE TABLE Areas (
    id_area INT NOT NULL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL
);

CREATE TABLE Comentarios (
    id_comentario INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    id_ticket INT NOT NULL,
    comentario VARCHAR(MAX) NOT NULL,
    estado VARCHAR(50),
    
);

CREATE TABLE Tickets (
    id_ticket INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    serv_app VARCHAR(100) NOT NULL,
    descripcion VARCHAR(MAX) NOT NULL,
    archivos VARCHAR(MAX),
    prioridad VARCHAR(20) NOT NULL,
    id_user INT NOT NULL,
    id_empleado INT,
    id_area INT NOT NULL,
    progreso VARCHAR(50),
    fecha_inicio DATETIME NOT NULL,
    fecha_asig DATETIME,
    fecha_fin DATETIME,
    id_comentario INT,
    FOREIGN KEY (id_user) REFERENCES Usuarios(id_user),
    FOREIGN KEY (id_empleado) REFERENCES Empleados(id_empleado),
    FOREIGN KEY (id_area) REFERENCES Areas(id_area),
    FOREIGN KEY (id_comentario) REFERENCES Comentarios(id_comentario)
);