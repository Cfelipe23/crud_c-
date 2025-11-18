using System;

namespace crud_c_.Modules.Lotes.Domain
{
  /// <summary>
  /// Lote de entrada de productos al inventario.
  /// </summary>
  public class Lote
  {
    /// <summary>Identificador único del lote.</summary>
    public int Id { get; set; }
    /// <summary>Código del lote.</summary>
    public string Codigo { get; set; } = string.Empty;
    /// <summary>Fecha de ingreso del lote.</summary>
    public DateTime FechaIngreso { get; set; }
    /// <summary>Cantidad de unidades ingresadas.</summary>
    public int Cantidad { get; set; }
    /// <summary>Id del producto al que pertenece el lote.</summary>
    public int ProductoId { get; set; }

    // Propiedad calculada: formatear fecha
    public string FechaFormateada => FechaIngreso.ToString("dd/MM/yyyy");

    // Propiedad calculada: determinar si es reciente (últimos 30 días)
    public bool EsReciente => DateTime.Today.Subtract(FechaIngreso).TotalDays <= 30;

    // Propiedad calculada: estado del lote por antigüedad
    public string EstadoLote => DateTime.Today.Subtract(FechaIngreso).TotalDays switch
    {
      <= 7 => "Muy Reciente",
      <= 30 => "Reciente",
      <= 90 => "Normal",
      _ => "Antiguo"
    };
  }
}