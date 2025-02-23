using AutoMapper;
using BookAPI.Models;

namespace BookAPI.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
             .ForMember(dest => dest.PopularityScore, opt => opt.MapFrom(src => (src.ViewsCount * 0.5) + ((DateTime.UtcNow.Year - src.PublicationYear) * 2)));
            CreateMap<UpdateBookDto, Book>();
        }
    }
}
