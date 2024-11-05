namespace PlagaCero.API.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;

    // Clave foránea para la relación con Author
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}
