using CollegeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Data
{
    public class CollegeDbContext : DbContext
    {
        public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<TeacherProfile> TeacherProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация связи многие-ко-многим между TeacherProfile и Subject
            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => new { ts.TeacherProfileId, ts.SubjectId });

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.TeacherProfile)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherProfileId);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId);
        }
    }
}
