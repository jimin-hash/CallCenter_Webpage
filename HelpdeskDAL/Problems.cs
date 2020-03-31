using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Problems :HelpdeskEntity
    {
        public Problems()
        {
            Calls = new HashSet<Calls>();
        }

        public string Description { get; set; }

        public virtual ICollection<Calls> Calls { get; set; }
    }
}
