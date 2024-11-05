namespace PlagaCero.API.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Test : IValidator
{
    public int TestId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public int SampleValue { get; set; }
    public float Measurement { get; set; }

    // Método de validación
    public List<ValidationError>? Validate()
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(Name))
        {
            errors.Add(new ValidationError(nameof(Name), "Name is required"));
        }

        if (SampleValue < 0)
        {
            errors.Add(new ValidationError(nameof(SampleValue), "SampleValue cannot be negative"));
        }

        if (Measurement < 0 || Measurement > 100)
        {
            errors.Add(new ValidationError(nameof(Measurement), "Measurement must be between 0 and 100"));
        }

        return errors.Count > 0 ? errors : null;
    }
}