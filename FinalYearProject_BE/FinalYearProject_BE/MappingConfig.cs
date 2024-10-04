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
            CreateMap<CourseDTO, CourseModel>().ReverseMap();
        }
    }
}
