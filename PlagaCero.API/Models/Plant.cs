namespace PlagaCero.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Plant
    {
        public int PlantId { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        // Navigation properties for related entities
        public List<PlantTemperature> PlantTemperatures { get; set; } = new List<PlantTemperature>();

        public List<PlantState> PlantStates { get; set; } = new List<PlantState>();
    }
}
