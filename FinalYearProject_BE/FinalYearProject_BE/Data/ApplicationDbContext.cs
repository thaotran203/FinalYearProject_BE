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
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<GradeModel> Grades { get; set; }
        public DbSet<LessonModel> Lessons { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<TestModel> Tests { get; set; }
        public DbSet<TestQuestionModel> TestQuestions { get; set; }
        public DbSet<UserTokenModel> UserTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Sender relationship
            modelBuilder.Entity<MessageModel>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Receiver relationship
            modelBuilder.Entity<MessageModel>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between TestQuestion and Test
            modelBuilder.Entity<TestQuestionModel>()
                .HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestions)
                .HasForeignKey(tq => tq.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between TestQuestion and Question
            modelBuilder.Entity<TestQuestionModel>()
                .HasOne(tq => tq.Question)
                .WithMany(q => q.TestQuestions)
                .HasForeignKey(tq => tq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
