using AutoMapper;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {

            //User Map
            CreateMap<User, AccountResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<TaskModel, TaskResponse>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TaskMembers.Select(tm => tm.User).ToList()))
                .ForMember(dest => dest.ChatGroup, opt => opt.MapFrom(src => src.ChatGroup));

            CreateMap<ChatGroup, ChatGroupResponse>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ChatGroupMembers.Select(cgm => cgm.User).ToList()));

            CreateMap<ChatGroup, ChatGroupDto>()
                .ForMember(dest => dest.TaskModelName, opt => opt.MapFrom(src => src.TaskModel.Name));
            CreateMap<ChatGroupMember, ChatGroupMemberDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
