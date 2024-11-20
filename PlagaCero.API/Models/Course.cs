namespace PlagaCero.API.Models;

using System;
using System.Collections.Generic;

public class Course
{
    public int CourseId { get; set; }
    public string Name { get; set; } = null!;

    // Relación de muchos a muchos con Students
    public ICollection<Student> Students { get; set; } = new List<Student>();
}