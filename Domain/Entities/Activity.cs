using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Activity : BaseLogs
    {
        public int id { get; set; }
        public int property_id { get; set; }
        public DateTime schedule { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public Property Property { get; set; }
        public Survey Survey { get; set; }
    }
}
