using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementWebApp.Models
{
    public class CourseVM
    {
        [DisplayName("Course Name")]
        [Required(ErrorMessage ="This field is required!")]
        public string Name { get; set; }
        [DisplayName("Course Category")]
        public ECourseCategory CourseCategory { get; set; }
    }
}
