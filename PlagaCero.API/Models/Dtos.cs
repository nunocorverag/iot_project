namespace Fition.Server.Models;

public class CreateAuthorDto
{
    public required string Name { get; set; }
}

public class UpdateAuthorDto
{
    public required string Name { get; set; }
}

public class AuthorDto
{
    public required int AuthorId { get; set; }
    public required string Name { get; set; }
}

public class AuthorWithBooksDto
{
    public required int AuthorId { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<BookDto> Books { get; set; }
}

public class CreateBookDto
{
    public required string Title { get; set; }
    public required int AuthorId { get; set; }
}

public class UpdateBookDto
{
    public required string Title { get; set; }
    public required int AuthorId { get; set; }
}

public class BookDto
{
    public required int BookId { get; set; }
    public required string Title { get; set; }
    public required int AuthorId { get; set; }
}

public class BookWithAuthorDto
{
    public required int BookId { get; set; }
    public required string Title { get; set; }
    public required AuthorDto Author { get; set; }
}
public class CreateCourseDto
{
    public required string Name { get; set; }
}

public class UpdateCourseDto
{
    public required string Name { get; set; }
}

public class CourseDto
{
    public required int CourseId { get; set; }
    public required string Name { get; set; }
}

public class CourseWithStudentsDto
{
    public required int CourseId { get; set; }
    public required string Name { get; set; }
    public required List<StudentDto> Students { get; set; }
}
public class CreateStudentDto
{
    public required string Name { get; set; }
}

public class UpdateStudentDto
{
    public required string Name { get; set; }
}

public class StudentDto
{
    public required int StudentId { get; set; }
    public required string Name { get; set; }
}

public class StudentWithCoursesDto
{
    public required int StudentId { get; set; }
    public required string Name { get; set; }
    public required List<CourseDto> Courses { get; set; }
}