using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new CourseContext())
        {
            context.Database.EnsureCreated();

            var coursesWithTeachersAndStudents = context.Courses
                .Include(c => c.Teachers)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .ToList();

            Console.WriteLine("Eager Loading:");
            foreach (var course in coursesWithTeachersAndStudents)
            {
                Console.WriteLine($"Course: {course.CourseName}");
                foreach (var teacher in course.Teachers)
                {
                    Console.WriteLine($"  Teacher: {teacher.FirstName} {teacher.LastName}");
                }
                foreach (var enrollment in course.Enrollments)
                {
                    Console.WriteLine($"  Student: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                }
            }

            var courseToLoad = context.Courses.First();
            context.Entry(courseToLoad).Collection(c => c.Teachers).Load();
            context.Entry(courseToLoad).Collection(c => c.Enrollments).Load();
            foreach (var enrollment in courseToLoad.Enrollments)
            {
                context.Entry(enrollment).Reference(e => e.Student).Load();
            }

            Console.WriteLine("\nExplicit Loading:");
            Console.WriteLine($"Course: {courseToLoad.CourseName}");
            foreach (var teacher in courseToLoad.Teachers)
            {
                Console.WriteLine($"  Teacher: {teacher.FirstName} {teacher.LastName}");
            }
            foreach (var enrollment in courseToLoad.Enrollments)
            {
                Console.WriteLine($"  Student: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
            }

            var lazyLoadedCourse = context.Courses.First();
            Console.WriteLine("\nLazy Loading:");
            Console.WriteLine($"Course: {lazyLoadedCourse.CourseName}");
            foreach (var teacher in lazyLoadedCourse.Teachers)
            {
                Console.WriteLine($"  Teacher: {teacher.FirstName} {teacher.LastName}");
            }
            foreach (var enrollment in lazyLoadedCourse.Enrollments)
            {
                Console.WriteLine($"  Student: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
            }
        }
    }
}
