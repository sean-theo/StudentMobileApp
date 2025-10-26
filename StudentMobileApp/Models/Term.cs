using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StudentMobileApp.Models
{
    [Table("Terms")]
    public class Term : AcademicItem
    {
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }

        private string _termTitle { get; set; }
        public string TermTitle
        {
            get => _termTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Term title cannot be empty");

                _termTitle = value;
                Title = value;
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Polymorphic summary method
        public override string GetSummary()
        {
            return $"Term: {TermTitle} ({StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy})";
        }
    }
}