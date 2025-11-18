namespace crud_c_.Modules.Productos.Domain
{
  /// <summary>
  /// Representa un producto.
  /// </summary>
  public class Producto
  {
    /// <summary>Identificador único del producto.</summary>
    public int Id { get; set; }
    /// <summary>Nombre del producto.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Descripción del producto.</summary>
    public string? Descripcion { get; set; }
    /// <summary>Precio unitario.</summary>
    public decimal Precio { get; set; }
    /// <summary>Unidades disponibles en inventario.</summary>
    public int Stock { get; set; }

    // Propiedad calculada: precio formateado
    public string PrecioFormateado => $"${Precio:F2}";
  }
}