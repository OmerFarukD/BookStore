namespace Entities.Exceptions;

public class PriceOutOfRangeException : BadRequestException
{
    public PriceOutOfRangeException() : base("Maximum Price shoulde be less than 1000 and greater than 10")
    {
    }
}