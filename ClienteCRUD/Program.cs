using System;
using System.Threading.Tasks;
using System.Linq;

namespace ClienteCRUD
{
  class Program
  {
    /// <summary>
    /// Punto de entrada principal de la aplicación. Inicializa los repositorios y muestra el menú principal.
    /// </summary>
    static async Task Main(string[] args)
    {
      const string connString = "DataSource=clientes.db";
      var factory = new SqliteConnectionFactory(connString);

      // Repositorios para las tres entidades
      var clienteRepository = new ClienteRepository(factory);
      var loteRepository = new LoteRepository(factory);
      var productoRepository = new ProductoRepository(factory);

      Console.WriteLine("🛒 Sistema CRUD Completo - Cliente, Lote y Producto");
      Console.WriteLine("==================================================");

      while (true)
      {
        MostrarMenuPrincipal();
        var opcion = Console.ReadLine();

        switch (opcion)
        {
          case "1":
            await MostrarMenuClientes(clienteRepository);
            break;
          case "2":
            await MostrarMenuLotes(loteRepository);
            break;
          case "3":
            await MostrarMenuProductos(productoRepository);
            break;

          case "9":
            Console.WriteLine("¡Hasta luego!");
            return;
          default:
            Console.WriteLine("Opción inválida");
            break;
        }
      }
    }

    /// <summary>
    /// Muestra el menú principal de la aplicación.
    /// </summary>
    static void MostrarMenuPrincipal()
    {
      Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
      Console.WriteLine("1. 👤 Gestión de Clientes");
      Console.WriteLine("2. 📦 Gestión de Lotes");
      Console.WriteLine("3. 🛍️  Gestión de Productos");
      Console.WriteLine("9. Salir");
      Console.Write("Seleccione opción: ");
    }

    #region Menús de Cliente

    /// <summary>
    /// Muestra el menú de gestión de clientes y ejecuta la opción seleccionada.
    /// </summary>
    /// <param name="repository">Repositorio de clientes</param>
    static async Task MostrarMenuClientes(IClienteRepository repository)
    {
      while (true)
      {
        Console.WriteLine("\n=== MENÚ CLIENTES ===");
        Console.WriteLine("1. Agregar cliente");
        Console.WriteLine("2. Listar clientes");
        Console.WriteLine("3. Buscar por ID");
        Console.WriteLine("4. Actualizar cliente");
        Console.WriteLine("5. Eliminar cliente");
        Console.WriteLine("9. Volver al menú principal");
        Console.Write("Seleccione opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
          case "1": await AgregarCliente(repository); break;
          case "2": await ListarClientes(repository); break;
          case "3": await BuscarClientePorId(repository); break;
          case "4": await ActualizarCliente(repository); break;
          case "5": await EliminarCliente(repository); break;
          case "9": return;
          default: Console.WriteLine("Opción inválida"); break;
        }
      }
    }

    #endregion

    #region Menús de Lote

    /// <summary>
    /// Muestra el menú de gestión de lotes y ejecuta la opción seleccionada.
    /// </summary>
    /// <param name="repository">Repositorio de lotes</param>
    static async Task MostrarMenuLotes(ILoteRepository repository)
    {
      while (true)
      {
        Console.WriteLine("\n=== MENÚ LOTES ===");
        Console.WriteLine("1. Agregar lote");
        Console.WriteLine("2. Listar lotes");
        Console.WriteLine("3. Buscar por ID");
        Console.WriteLine("4. Actualizar lote");
        Console.WriteLine("5. Eliminar lote");
        Console.WriteLine("9. Volver al menú principal");
        Console.Write("Seleccione opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
          case "1": await AgregarLote(repository); break;
          case "2": await ListarLotes(repository); break;
          case "3": await BuscarLotePorId(repository); break;
          case "4": await ActualizarLote(repository); break;
          case "5": await EliminarLote(repository); break;
          case "9": return;
          default: Console.WriteLine("Opción inválida"); break;
        }
      }
    }

    #endregion

    #region Menús de Producto

    /// <summary>
    /// Muestra el menú de gestión de productos y ejecuta la opción seleccionada.
    /// </summary>
    /// <param name="repository">Repositorio de productos</param>
    static async Task MostrarMenuProductos(IProductoRepository repository)
    {
      while (true)
      {
        Console.WriteLine("\n=== MENÚ PRODUCTOS ===");
        Console.WriteLine("1. Agregar producto");
        Console.WriteLine("2. Listar productos");
        Console.WriteLine("3. Buscar por ID");
        Console.WriteLine("4. Actualizar producto");
        Console.WriteLine("5. Eliminar producto");
        Console.WriteLine("9. Volver al menú principal");
        Console.Write("Seleccione opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
          case "1": await AgregarProducto(repository); break;
          case "2": await ListarProductos(repository); break;
          case "3": await BuscarProductoPorId(repository); break;
          case "4": await ActualizarProducto(repository); break;
          case "5": await EliminarProducto(repository); break;
          case "9": return;
          default: Console.WriteLine("Opción inválida"); break;
        }
      }
    }

    #endregion



    #region Métodos CRUD - Cliente

    /// <summary>
    /// Agrega un nuevo cliente solicitando los datos por consola.
    /// </summary>
    static async Task AgregarCliente(IClienteRepository repository)
    {
      Console.WriteLine("\n--- AGREGAR CLIENTE ---");
      Console.Write("Nombre: ");
      var nombre = Console.ReadLine();
      Console.Write("Apellido: ");
      var apellido = Console.ReadLine();
      Console.Write("Dirección (opcional): ");
      var direccion = Console.ReadLine();
      Console.Write("Teléfono (opcional): ");
      var telefono = Console.ReadLine();
      Console.Write("Email (opcional): ");
      var email = Console.ReadLine();

      var cliente = new Cliente
      {
        Nombre = nombre ?? "",
        Apellido = apellido ?? "",
        Direccion = string.IsNullOrWhiteSpace(direccion) ? null : direccion,
        Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono,
        Email = string.IsNullOrWhiteSpace(email) ? null : email
      };

      await repository.AddAsync(cliente);
      Console.WriteLine($"✅ Cliente {cliente.NombreCompleto} agregado con ID: {cliente.Id}");
    }

    /// <summary>
    /// Lista todos los clientes registrados.
    /// </summary>
    static async Task ListarClientes(IClienteRepository repository)
    {
      Console.WriteLine("\n--- LISTA DE CLIENTES ---");
      var clientes = await repository.GetAllAsync();

      foreach (var cliente in clientes)
      {
        Console.WriteLine($"ID: {cliente.Id} | {cliente.NombreCompleto} |{cliente.Email ?? "Sin email"}");
      }
    }

    /// <summary>
    /// Busca un cliente por su ID.
    /// </summary>
    static async Task BuscarClientePorId(IClienteRepository repository)
    {
      Console.WriteLine("\n--- BUSCAR POR ID ---");
      Console.Write("ID del cliente: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var cliente = await repository.GetByIdAsync(id);
        if (cliente != null)
        {
          Console.WriteLine($"Encontrado: {cliente.NombreCompleto}");
          Console.WriteLine($"Email: {cliente.Email ?? "No tiene"}");
          Console.WriteLine($"Teléfono: {cliente.Telefono ?? "No tiene"}");
        }
        else
        {
          Console.WriteLine("Cliente no encontrado");
        }
      }
    }



    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    static async Task ActualizarCliente(IClienteRepository repository)
    {
      Console.WriteLine("\n--- ACTUALIZAR CLIENTE ---");
      Console.Write("ID del cliente: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var cliente = await repository.GetByIdAsync(id);
        if (cliente == null)
        {
          Console.WriteLine("Cliente no encontrado");
          return;
        }

        Console.WriteLine($"Cliente actual: {cliente.NombreCompleto}");
        Console.Write($"Nuevo nombre ({cliente.Nombre}): ");
        var nombre = Console.ReadLine();
        Console.Write($"Nuevo apellido ({cliente.Apellido}): ");
        var apellido = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nombre)) cliente.Nombre = nombre;
        if (!string.IsNullOrWhiteSpace(apellido)) cliente.Apellido = apellido;

        await repository.UpdateAsync(cliente);
        Console.WriteLine("✅ Cliente actualizado");
      }
    }

    /// <summary>
    /// Elimina un cliente por su ID.
    /// </summary>
    static async Task EliminarCliente(IClienteRepository repository)
    {
      Console.WriteLine("\n--- ELIMINAR CLIENTE ---");
      Console.Write("ID del cliente: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var cliente = await repository.GetByIdAsync(id);
        if (cliente != null)
        {
          Console.WriteLine($"Cliente: {cliente.NombreCompleto}");
          Console.Write("¿Confirma eliminación? (s/n): ");
          if (Console.ReadLine()?.ToLower() == "s")
          {
            await repository.DeleteAsync(id);
            Console.WriteLine("✅ Cliente eliminado");
          }
        }
        else
        {
          Console.WriteLine("Cliente no encontrado");
        }
      }
    }

    #endregion

    #region Métodos CRUD - Lote

    /// <summary>
    /// Agrega un nuevo lote solicitando los datos por consola.
    /// </summary>
    static async Task AgregarLote(ILoteRepository repository)
    {
      Console.WriteLine("\n--- AGREGAR LOTE ---");
      Console.Write("Código del lote: ");
      var codigo = Console.ReadLine();
      Console.Write("Fecha de ingreso (dd/MM/yyyy): ");
      var fechaStr = Console.ReadLine();
      Console.Write("Cantidad: ");
      var cantidadStr = Console.ReadLine();
      Console.Write("ID del producto: ");
      var productoIdStr = Console.ReadLine();

      if (DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha) &&
          int.TryParse(cantidadStr, out int cantidad) &&
          int.TryParse(productoIdStr, out int productoId))
      {
        var lote = new Lote
        {
          Codigo = codigo ?? "",
          FechaIngreso = fecha,
          Cantidad = cantidad,
          ProductoId = productoId
        };

        await repository.AddAsync(lote);
        Console.WriteLine($"✅ Lote {lote.Codigo} agregado con ID: {lote.Id}");
      }
      else
      {
        Console.WriteLine("❌ Error en los datos ingresados");
      }
    }

    /// <summary>
    /// Lista todos los lotes registrados.
    /// </summary>
    static async Task ListarLotes(ILoteRepository repository)
    {
      Console.WriteLine("\n--- LISTA DE LOTES ---");
      var lotes = await repository.GetAllAsync();

      Console.WriteLine($"{"ID",-5} | {"Código",-12} | {"Fecha",-12} | {"Cantidad",-10} | {"Producto ID",-12}");
      Console.WriteLine(new string('-', 60));

      foreach (var lote in lotes)
      {
        Console.WriteLine($"{lote.Id,-5} | {lote.Codigo,-12} | {lote.FechaFormateada,-12} | " +
                        $"{lote.Cantidad,-10} | {lote.ProductoId,-12}");
      }
    }

    /// <summary>
    /// Busca un lote por su ID.
    /// </summary>
    static async Task BuscarLotePorId(ILoteRepository repository)
    {
      Console.WriteLine("\n--- BUSCAR LOTE POR ID ---");
      Console.Write("ID del lote: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var lote = await repository.GetByIdAsync(id);
        if (lote != null)
        {
          Console.WriteLine($"Encontrado: {lote.Codigo}");
          Console.WriteLine($"Fecha: {lote.FechaFormateada}");
          Console.WriteLine($"Cantidad: {lote.Cantidad}");
          Console.WriteLine($"Estado: {lote.EstadoLote}");
          Console.WriteLine($"Es reciente: {(lote.EsReciente ? "Sí" : "No")}");
        }
        else
        {
          Console.WriteLine("Lote no encontrado");
        }
      }
    }



    /// <summary>
    /// Actualiza los datos de un lote existente.
    /// </summary>
    static async Task ActualizarLote(ILoteRepository repository)
    {
      Console.WriteLine("\n--- ACTUALIZAR LOTE ---");
      Console.Write("ID del lote: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var lote = await repository.GetByIdAsync(id);
        if (lote == null)
        {
          Console.WriteLine("Lote no encontrado");
          return;
        }

        Console.WriteLine($"Lote actual: {lote.Codigo}");
        Console.Write($"Nuevo código ({lote.Codigo}): ");
        var codigo = Console.ReadLine();
        Console.Write($"Nueva cantidad ({lote.Cantidad}): ");
        var cantidadStr = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(codigo)) lote.Codigo = codigo;
        if (int.TryParse(cantidadStr, out int cantidad)) lote.Cantidad = cantidad;

        await repository.UpdateAsync(lote);
        Console.WriteLine("✅ Lote actualizado");
      }
    }

    /// <summary>
    /// Elimina un lote por su ID.
    /// </summary>
    static async Task EliminarLote(ILoteRepository repository)
    {
      Console.WriteLine("\n--- ELIMINAR LOTE ---");
      Console.Write("ID del lote: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var lote = await repository.GetByIdAsync(id);
        if (lote != null)
        {
          Console.WriteLine($"Lote: {lote.Codigo} - {lote.Cantidad} unidades");
          Console.Write("¿Confirma eliminación? (s/n): ");
          if (Console.ReadLine()?.ToLower() == "s")
          {
            await repository.DeleteAsync(id);
            Console.WriteLine("✅ Lote eliminado");
          }
        }
        else
        {
          Console.WriteLine("Lote no encontrado");
        }
      }
    }



    #endregion

    #region Métodos CRUD - Producto

    /// <summary>
    /// Agrega un nuevo producto solicitando los datos por consola.
    /// </summary>
    static async Task AgregarProducto(IProductoRepository repository)
    {
      Console.WriteLine("\n--- AGREGAR PRODUCTO ---");
      Console.Write("Nombre del producto: ");
      var nombre = Console.ReadLine();
      Console.Write("Descripción (opcional): ");
      var descripcion = Console.ReadLine();
      Console.Write("Precio: ");
      var precioStr = Console.ReadLine();
      Console.Write("Stock inicial: ");
      var stockStr = Console.ReadLine();

      if (decimal.TryParse(precioStr, out decimal precio) && precio > 0 &&
          int.TryParse(stockStr, out int stock) && stock >= 0)
      {
        var producto = new Producto
        {
          Nombre = nombre ?? "",
          Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion,
          Precio = precio,
          Stock = stock
        };

        await repository.AddAsync(producto);
        Console.WriteLine($"✅ Producto {producto.Nombre} agregado con ID: {producto.Id}");
        Console.WriteLine($"   Precio: {producto.PrecioFormateado}");
      }
      else
      {
        Console.WriteLine("❌ Error en los datos ingresados");
      }
    }

    /// <summary>
    /// Lista todos los productos registrados.
    /// </summary>
    static async Task ListarProductos(IProductoRepository repository)
    {
      Console.WriteLine("\n--- LISTA DE PRODUCTOS ---");
      var productos = await repository.GetAllAsync();

      if (!productos.Any())
      {
        Console.WriteLine("No hay productos registrados.");
        return;
      }

      Console.WriteLine($"{"ID",-5} | {"Nombre",-25} | {"Precio",-10} | {"Stock",-8} | {"Estado",-12}");
      Console.WriteLine(new string('-', 65));

      foreach (var producto in productos)
      {
        Console.WriteLine($"{producto.Id,-5} | {producto.Nombre,-25} | {producto.PrecioFormateado,-10} | " +
                        $"{producto.Stock,-8}");
      }

      Console.WriteLine($"\n📊 Total productos: {productos.Count()} ");
    }

    /// <summary>
    /// Busca un producto por su ID.
    /// </summary>
    static async Task BuscarProductoPorId(IProductoRepository repository)
    {
      Console.WriteLine("\n--- BUSCAR PRODUCTO POR ID ---");
      Console.Write("ID del producto: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var producto = await repository.GetByIdAsync(id);
        if (producto != null)
        {
          Console.WriteLine($"Encontrado: {producto.Nombre}");
          Console.WriteLine($"Descripción: {producto.Descripcion ?? "Sin descripción"}");
          Console.WriteLine($"Precio: {producto.PrecioFormateado}");
          Console.WriteLine($"Stock: {producto.Stock}");

        }
        else
        {
          Console.WriteLine("Producto no encontrado");
        }
      }
    }



    /// <summary>
    /// Actualiza los datos de un producto existente.
    /// </summary>
    static async Task ActualizarProducto(IProductoRepository repository)
    {
      Console.WriteLine("\n--- ACTUALIZAR PRODUCTO ---");
      Console.Write("ID del producto: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var producto = await repository.GetByIdAsync(id);
        if (producto == null)
        {
          Console.WriteLine("Producto no encontrado");
          return;
        }

        Console.WriteLine($"Producto actual: {producto.Nombre} - {producto.PrecioFormateado}");
        Console.Write($"Nuevo nombre ({producto.Nombre}): ");
        var nombre = Console.ReadLine();
        Console.Write($"Nuevo precio ({producto.Precio}): ");
        var precioStr = Console.ReadLine();
        Console.Write($"Nuevo stock ({producto.Stock}): ");
        var stockStr = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nombre)) producto.Nombre = nombre;
        if (decimal.TryParse(precioStr, out decimal precio) && precio > 0) producto.Precio = precio;
        if (int.TryParse(stockStr, out int stock) && stock >= 0) producto.Stock = stock;

        await repository.UpdateAsync(producto);
        Console.WriteLine("✅ Producto actualizado");
      }
    }

    /// <summary>
    /// Elimina un producto por su ID.
    /// </summary>
    static async Task EliminarProducto(IProductoRepository repository)
    {
      Console.WriteLine("\n--- ELIMINAR PRODUCTO ---");
      Console.Write("ID del producto: ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var producto = await repository.GetByIdAsync(id);
        if (producto != null)
        {
          Console.WriteLine($"Producto: {producto.Nombre} - {producto.PrecioFormateado}");
          Console.Write("¿Confirma eliminación? (s/n): ");
          if (Console.ReadLine()?.ToLower() == "s")
          {
            await repository.DeleteAsync(id);
            Console.WriteLine("✅ Producto eliminado");
          }
        }
        else
        {
          Console.WriteLine("Producto no encontrado");
        }
      }
    }


    #endregion
  }
}
