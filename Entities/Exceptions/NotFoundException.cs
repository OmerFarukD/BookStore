namespace Entities.Exceptions;

public abstract class NotFoundException :Exception
{
    protected NotFoundException(string message ): base(message)
    {
        
    }
}

public sealed class BookNotFound : NotFoundException
{
    public BookNotFound(int id) : base($"The book with id : {id} not found.")
    {
    }
}