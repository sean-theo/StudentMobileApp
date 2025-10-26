using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StudentMobileApp.Models
{
    [Table("Courses")]
    public class Course : AcademicItem
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }

        private string _courseTitle;

        public string CourseTitle
        {
            get => _courseTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Course title is required.");

                _courseTitle = value;
                Title = value; // sync with base class
            }
        }
        public int TermId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; }

        public override string GetSummary()
        {
            return $"Course: {CourseTitle} ({Status}) - Instructor: {InstructorName}";
        }
    }
}
