namespace Entities.Models;

public class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public double Price { get; set; }


    public int CategoryId { get; set; }
    public  Category Category { get; set; }
}