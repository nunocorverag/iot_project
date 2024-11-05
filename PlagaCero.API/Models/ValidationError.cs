namespace PlagaCero.API.Models;

public class ValidationError
{
    public string Field { get; set; } = null!;
    public string Message { get; set; } = null!;

    public ValidationError(string field, string message)
    {
        Field = field;
        Message = message;
    }
}
