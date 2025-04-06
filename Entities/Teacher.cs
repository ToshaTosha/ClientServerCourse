using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityDataModel.Entities
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
