using System;

namespace ClienteCRUD.Modules.Lotes.Domain
{
    public class Lote
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
        public int Cantidad { get; set; }
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
