namespace PlagaCero.API.Models
{
    public class PlantaTemp
    {
        public int TempId { get; set; } // Identificador único del registro de temperatura
        public int PlantaId { get; set; } // Identificador de la planta asociada
        public double TempRegist { get; set; } // Temperatura registrada
        public DateTime FechaTemp { get; set; } // Fecha del registro de temperatura
        public string Descripcion { get; set; } = string.Empty; // Descripción del registro de temperatura

        // Relación
        public Planta? Planta { get; set; } // Relación con la tabla Plantas
    }
}
