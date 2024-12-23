﻿using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.DTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CourseContent { get; set; }
        public string ImageLink { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
    }
}
