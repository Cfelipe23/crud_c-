namespace ClienteCRUD.Modules.Productos.Domain
{
  public class Producto
  {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    // Propiedad calculada: precio formateado
    public string PrecioFormateado => $"${Precio:F2}";
  }
}