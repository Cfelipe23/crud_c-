namespace crud_c_.Modules.Clientes.Domain
{
  /// <summary>
  /// Representa un cliente del sistema.
  /// </summary>
  public class Cliente
  {
    /// <summary>Identificador único del cliente.</summary>
    public int Id { get; set; }
    /// <summary>Nombre del cliente.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Apellido del cliente.</summary>
    public string Apellido { get; set; } = string.Empty;
    /// <summary>Dirección del cliente.</summary>
    public string? Direccion { get; set; }
    /// <summary>Teléfono de contacto.</summary>
    public string? Telefono { get; set; }
    /// <summary>Email de contacto.</summary>
    public string? Email { get; set; }

    // Propiedad calculada
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
  }
}