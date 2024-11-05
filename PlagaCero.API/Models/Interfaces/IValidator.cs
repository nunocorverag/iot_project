namespace PlagaCero.API.Models;

public interface IValidator
{
    public List<ValidationError>? Validate();
}
