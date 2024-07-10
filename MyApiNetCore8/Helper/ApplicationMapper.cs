using AutoMapper;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.Helper
{
    public class ApplicationMapper : Profile
    {
        //CreateMap<ProductRequest, Product>()
        //    .ForMember(dest => dest.Category, opt => opt.Ignore());
        public ApplicationMapper()
        {

            //User Map
            CreateMap<User, AccountResponse>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
          
            //User Map
        }
    }
}
