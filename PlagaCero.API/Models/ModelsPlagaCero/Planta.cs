namespace PlagaCero.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Maiz
    {
        [Key]
        public int Id { get; set; }

        public double TemperaturaMin { get; set; }

        public double TemperaturaMax { get; set; }

        public double HumedadMin { get; set; }

        public double HumedadMax { get; set; }

        public string ColorNormal { get; set; }
    }
}
