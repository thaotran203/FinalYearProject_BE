using AutoMapper;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
            CreateMap<CourseResponseDTO, CourseModel>().ReverseMap();
            CreateMap<RegisterUserDTO, UserModel>().ReverseMap();
            CreateMap<UpdateUserDTO, UserModel>().ReverseMap();
            CreateMap<UserDTO, UserModel>().ReverseMap();
            CreateMap<LessonDTO, LessonModel>().ReverseMap();
            CreateMap<VideoResponseDTO, LessonVideoModel>().ReverseMap();
        }
    }
}
