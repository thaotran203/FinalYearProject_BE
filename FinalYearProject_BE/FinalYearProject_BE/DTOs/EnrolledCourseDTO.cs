namespace FinalYearProject_BE.DTOs
{
    public class EnrolledCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string ImageLink { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int InstructorId { get; set; }
        public string TeacherName { get; set; }
    }
}
