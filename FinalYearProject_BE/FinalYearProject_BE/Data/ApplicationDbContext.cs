using FinalYearProject_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<LessonVideoModel> LessonVideos { get; set; }
        public DbSet<LessonModel> Lessons { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<UserTokenModel> UserTokens { get; set; }
        public DbSet<LessonProgressModel> LessonProgresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
