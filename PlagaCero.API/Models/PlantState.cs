namespace PlagaCero.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PlantState
    {
        public int PlantStateId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [MaxLength(10)]
        public string State { get; set; }  // "Sano" o "No Sano"

        // Clave foránea para la relación con Plant
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
    }
}
