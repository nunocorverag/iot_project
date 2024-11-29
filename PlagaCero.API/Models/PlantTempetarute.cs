namespace PlagaCero.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PlantTemperature
    {
        public int PlantTemperatureId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public float Temperatura { get; set; }

        // Clave foránea para la relación con Plant
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
    }
}
