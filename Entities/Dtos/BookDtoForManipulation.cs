using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public abstract record BookDtoForManipulation
{
    [Required(ErrorMessage = "Title is a required field.")]
    [MinLength(2,ErrorMessage = "Title must consist at least 2 characters")]
    [MaxLength(75,ErrorMessage = "Title must consist at maximum 75 characters")]
    public string Title { get; init; }
    
    [Required(ErrorMessage = "Price is a required field.")]
    [Range(10,1000,ErrorMessage = "")]
    public double Price { get; init; }
    
}