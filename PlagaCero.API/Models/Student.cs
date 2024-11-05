namespace PlagaCero.API.Models;

using System;
using System.Collections.Generic;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; } = null!;

    // Relación de muchos a muchos con Courses
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
