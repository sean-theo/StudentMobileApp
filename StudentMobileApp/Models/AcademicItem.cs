using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMobileApp.Models
{
    public abstract class AcademicItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; private set; } = DateTime.Now;

        // Polymorphic summary method
       public virtual string GetSummary()
       {
        return $"{Title} (Created {DateCreated:MM/dd/yyyy})";
       }
    }
}
