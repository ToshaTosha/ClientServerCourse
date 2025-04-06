using EntityDataModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework01
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<Grade> Grades { get; set; } = null!;

        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        public ApplicationContext() { Database.EnsureDeleted(); Database.EnsureCreated(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=sqlite20250404.db");
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            //optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);

            // Включение ленивой загрузки
            optionsBuilder.UseLazyLoadingProxies().UseSqlite(config.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging().UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Grade>().HasIndex(u => u.StudentID).IsUnique();

            modelBuilder.Entity<Enrollment>()
               .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

        }
        
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { TeacherId = 1, FirstName = "John", LastName = "Doe" },
            new Teacher { TeacherId = 2, FirstName = "Jane", LastName = "Smith" }
        );

        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = 1, FirstName = "Alice", LastName = "Johnson", Age = 20, Address = "123 Main St" },
            new Student { StudentId = 2, FirstName = "Bob", LastName = "Brown", Age = 22, Address = "456 Elm St" }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { CourseId = 1, CourseName = "C# Basics" },
            new Course { CourseId = 2, CourseName = "Entity Framework Core" }
        );

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { EnrollmentId = 1, CourseId = 1, StudentId = 1 },
            new Enrollment { EnrollmentId = 2, CourseId = 1, StudentId = 2 },
            new Enrollment { EnrollmentId = 3, CourseId = 2, StudentId = 1 }
        );

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Teachers)
            .WithMany(t => t.Courses)
            .UsingEntity(j => j.HasData(
                new { CoursesCourseId = 1, TeachersTeacherId = 1 },
                new { CoursesCourseId = 1, TeachersTeacherId = 2 },
                new { CoursesCourseId = 2, TeachersTeacherId = 1 }
            ));

        public void UpdateStudent(int studentId, string newFirstName, string newLastName, int newAge, string newAddress)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                student.FirstName = newFirstName;
                student.LastName = newLastName;
                student.Age = newAge;
                student.Address = newAddress;
                SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            var student = Students.Find(studentId);
            if (student != null)
            {
                Students.Remove(student);
                SaveChanges();
            }
        }
    }
}