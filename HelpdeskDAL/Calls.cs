using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Calls :HelpdeskEntity
    {
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }

        public virtual Employees Employee { get; set; }
        public virtual Problems Problem { get; set; }
        public virtual Employees Tech { get; set; }
    }
}
