using Microsoft.Azure.Cosmos.Table;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementWebApp.Models
{
    
    public class Course:TableEntity
    {
        [DisplayName("Course Name")]
        public string Name { get; set; }
        [DisplayName("Course Category")]
        public ECourseCategory CourseCategory { get; set; }
        [DisplayName("Total Participants")]
        public int TotalParticipant { get; set; }
        [DisplayName("Create Date")]
        public DateTime createdAt { get; set; } = DateTime.Now;

    }
}
