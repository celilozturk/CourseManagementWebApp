using System.ComponentModel.DataAnnotations;

namespace CourseManagementWebApp.Models
{
    public class Candidate
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName?.ToUpper()}";
        public int Age { get; set; }
        public List<Course> SelectedCourses { get; set; } 
        public DateTime ApplyAt { get; set; }
        public Candidate()
        {
            ApplyAt=DateTime.Now;
        }
    }
}
