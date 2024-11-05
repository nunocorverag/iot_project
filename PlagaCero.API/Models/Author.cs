namespace PlagaCero.API.Models;

using System;
using System.Collections.Generic;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = null!;

    // Relación de uno a muchos con Books
    public ICollection<Book> Books { get; set; } = new List<Book>();
}