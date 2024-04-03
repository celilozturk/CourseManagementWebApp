using Microsoft.Azure.Cosmos.Table;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementWebApp.Models
{
    public class Candidate:TableEntity
    {
        [DisplayName("Email Address")]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName?.ToUpper()}";
        public int Age { get; set; }
        public string SelectedCourse { get; set; } 
        public DateTime ApplyAt { get; set; }
        public Candidate()
        {
            ApplyAt=DateTime.Now;
        }
    }
}
