using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StudentMobileApp.Models
{
    [Table("Assessments")]
    public class Assessment : AcademicItem
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }

        private string _assessmentTitle;
        public new string Title
        {
            get => _assessmentTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Assessment title cannot be empty.");

                _assessmentTitle = value;
                base.Title = value; // keep base Title in sync
            }
        }

        public int CourseId { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Polymorphism: specialized summary
        public override string GetSummary()
        {
            return $"Assessment: {Title} ({Type}) {StartDate:MM/dd/yyyy}–{EndDate:MM/dd/yyyy}";
        }
    }
}
