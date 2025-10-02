# ClienteCRUD

## Descripción
ClienteCRUD es un sistema CRUD (Crear, Leer, Actualizar, Eliminar) para gestionar tres entidades principales por consola:
- **Clientes**
- **Lotes**
- **Productos**

El sistema utiliza una base de datos SQLite para almacenar los datos y sigue un diseño modular con repositorios para cada entidad.

---

## Flujo de Peticiones

### 1. **Inicio del Programa**
El programa comienza en el método `Main` dentro de `Program.cs`. Aquí se inicializan las conexiones a la base de datos y los repositorios:
- `ClienteRepository`
- `LoteRepository`
- `ProductoRepository`

El menú principal se muestra al usuario, permitiendo seleccionar entre las opciones de gestión de clientes, lotes o productos.

### 2. **Gestión de Clientes**
Cuando el usuario selecciona la opción de clientes:
- Se llama al método `MostrarMenuClientes`.
- Este método muestra un submenú con opciones como agregar, listar, buscar, actualizar y eliminar clientes.
- Cada opción llama a un método específico como `AgregarCliente`, `ListarClientes`, etc., que interactúa con el repositorio `ClienteRepository`.

### 3. **Gestión de Lotes**
Cuando el usuario selecciona la opción de lotes:
- Se llama al método `MostrarMenuLotes`.
- Este método muestra un submenú con opciones como agregar, listar, buscar, actualizar y eliminar lotes.
- Cada opción llama a un método específico como `AgregarLote`, `ListarLotes`, etc., que interactúa con el repositorio `LoteRepository`.

### 4. **Gestión de Productos**
Cuando el usuario selecciona la opción de productos:
- Se llama al método `MostrarMenuProductos`.
- Este método muestra un submenú con opciones como agregar, listar, buscar, actualizar y eliminar productos.
- Cada opción llama a un método específico como `AgregarProducto`, `ListarProductos`, etc., que interactúa con el repositorio `ProductoRepository`.

---

## Modificaciones

### ¿Cómo modificar el flujo?
1. **Agregar una nueva entidad**:
   - Crear una nueva clase para la entidad (por ejemplo, `Pedido.cs`).
   - Crear una interfaz de repositorio (por ejemplo, `IPedidoRepository.cs`).
   - Implementar el repositorio (por ejemplo, `PedidoRepository.cs`).
   - Agregar métodos CRUD en el repositorio.
   - Crear un submenú en `Program.cs` para gestionar la nueva entidad.

2. **Modificar una entidad existente**:
   - Actualizar la clase de la entidad (por ejemplo, agregar un nuevo campo en `Cliente.cs`).
   - Actualizar las consultas SQL en el repositorio correspondiente (por ejemplo, `ClienteRepository.cs`).
   - Actualizar los métodos en `Program.cs` que interactúan con la entidad.

3. **Cambiar la base de datos**:
   - Modificar la conexión en `SqliteConnectionFactory.cs`.
   - Actualizar las consultas SQL en todos los repositorios.

---

## Archivos Clave

### 1. **Program.cs**
Contiene el flujo principal del programa y los menús para interactuar con las entidades.

### 2. **Repositorios**
- `ClienteRepository.cs`: Métodos CRUD para clientes.
- `LoteRepository.cs`: Métodos CRUD para lotes.
- `ProductoRepository.cs`: Métodos CRUD para productos.

### 3. **Entidades**
- `Cliente.cs`: Representa la entidad Cliente.
- `Lote.cs`: Representa la entidad Lote.
- `Producto.cs`: Representa la entidad Producto.

### 4. **Base de Datos**
- `clientes.db`: Archivo SQLite que almacena los datos.
- `baseShoppe.sql`: Contiene las consultas SQL para inicializar la base de datos.

---

## Ejecución

1. Asegúrate de tener instalado el SDK de .NET.
2. Abre una terminal en la carpeta del proyecto.
4. Ejecuta primero el comando:
   ```bash
   dotnet build
   ```
3. Y despues el comando:
   ```bash
   dotnet run
   ```

---

## Notas
- Si necesitas agregar nuevas dependencias, actualiza el archivo `ClienteCRUD.csproj`.
- Asegúrate de realizar migraciones en la base de datos si cambias el esquema.

---

¡Gracias por usar ClienteCRUD!