using System.Runtime.CompilerServices;

namespace Entities.Dtos;

public record BookDto
{
    public int  Id { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
}