using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
