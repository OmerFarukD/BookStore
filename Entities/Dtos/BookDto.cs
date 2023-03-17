using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Entities.Dtos;

public record BookDto :BookDtoForManipulation
{
    
    [Required(ErrorMessage = "Id field is required")]
    public int  Id { get; init; }

}