using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Property : BaseLogs
    {
        public int id { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public DateTime disabled_at { get; set; }
        public string status { get; set; }
        public List<Activity> Activities { get; set; }

    }
}
