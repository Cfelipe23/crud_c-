-- Cliente
CREATE TABLE Cliente (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre TEXT NOT NULL,
    apellido TEXT NOT NULL,
    direccion TEXT,
    telefono TEXT,
    email TEXT UNIQUE
);

-- Producto
CREATE TABLE Producto (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre TEXT NOT NULL,
    descripcion TEXT,
    precio REAL NOT NULL,
    stock INTEGER NOT NULL
);

-- Lote
CREATE TABLE Lote (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    codigo TEXT NOT NULL,
    fecha_ingreso DATE,
    cantidad INTEGER NOT NULL,
    producto_id INTEGER NOT NULL,
    FOREIGN KEY (producto_id) REFERENCES Producto(id)
);

-- Factura 
CREATE TABLE Factura (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    fecha DATE NOT NULL,
    total REAL NOT NULL,
    cliente_id INTEGER NOT NULL,
    FOREIGN KEY (cliente_id) REFERENCES Cliente(id)
);

-- Detalle de la Factura
CREATE TABLE Detalle_factura (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    factura_id INTEGER NOT NULL,
    producto_id INTEGER NOT NULL,
    cantidad INTEGER NOT NULL,
    precio_unitario REAL NOT NULL,
    subtotal REAL NOT NULL,
    FOREIGN KEY (factura_id) REFERENCES Factura(id),
    FOREIGN KEY (producto_id) REFERENCES Producto(id)
);

-- Separacion
CREATE TABLE Separacion (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    cliente_id INTEGER NOT NULL,
    producto_id INTEGER NOT NULL,
    fecha DATE NOT NULL,
    monto REAL NOT NULL,
    FOREIGN KEY (cliente_id) REFERENCES Cliente(id),
    FOREIGN KEY (producto_id) REFERENCES Producto(id)
);

-- Abonos sobre separaciones
CREATE TABLE Abonos (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    separacion_id INTEGER NOT NULL,
    fecha DATE NOT NULL,
    monto REAL NOT NULL,
    FOREIGN KEY (separacion_id) REFERENCES Separacion(id)
);
-- Inserciones para Clientes
INSERT INTO Cliente (nombre, apellido, direccion, telefono, email) VALUES
('Maria', 'Gonzalez', 'Av. Principal 123', '3001234567', 'maria.gonzalez@email.com'),
('Carlos', 'Rodriguez', 'Calle 45 #67-89', '3102345678', 'carlos.rodriguez@email.com');

-- Inserciones para Productos (Zapatillas)
INSERT INTO Producto (nombre, descripcion, precio, stock) VALUES
('Nike Air Max 270', 'Zapatillas deportivas con tecnologia Air Max', 350000, 50),
('Adidas Ultraboost 22', 'Zapatillas running con amortiguacion Boost', 420000, 30);

-- Inserciones para Lotes
INSERT INTO Lote (codigo, fecha_ingreso, cantidad, producto_id) VALUES
('lote-nike-001', '2024-01-15', 25, 1),
('lote-nike-002', '2024-01-20', 25, 1),
('lote-adidas-001', '2024-01-16', 15, 2);
