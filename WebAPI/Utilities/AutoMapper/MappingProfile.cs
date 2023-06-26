using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace WebAPI.Utilities.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookForUpdate, Book>().ReverseMap();
        CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<BookDtoForInsertion, Book>().ReverseMap();
        CreateMap<User, UserForRegistrationDto>().ReverseMap();
    }
}