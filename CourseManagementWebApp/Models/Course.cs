using Microsoft.Azure.Cosmos.Table;

namespace CourseManagementWebApp.Models
{
    
    public class Course:TableEntity
    {
        public string Name { get; set; }
        public ECourseCategory CourseCategory { get; set; }
        public string TotalParticipant { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

    }
}
