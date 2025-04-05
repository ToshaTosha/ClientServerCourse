using System;
using System.Collections.Generic;

namespace ClientServerCourseLabs
{
    internal class Lab5
    {
        static void Main(string[] args)
        {
            List<Grade> grades = new List<Grade>
            {
                new Grade { StudentName = "Вася", Subject = "Математика", Score = 90 },
                new Grade { StudentName = "Вася", Subject = "Физика", Score = 85 },
                new Grade { StudentName = "Петя", Subject = "Математика", Score = 75 },
                new Grade { StudentName = "Петя", Subject = "Физика", Score = 80 },
                new Grade { StudentName = "Коля", Subject = "Математика", Score = 95 },
                new Grade { StudentName = "Коля", Subject = "Физика", Score = 90 }
            };

            foreach (var grade in grades)
            {
                Console.WriteLine($"Студент: {grade.StudentName}, Предмет: {grade.Subject}, Оценка: {grade.Score}");
            }
        }
    }

    public class Grade
    {
        public double Score { get; set; }
        public string StudentName { get; set; }
        public string Subject { get; set; }
    }
}