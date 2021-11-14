using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ActivityDto
    {
        public int? id { get; set; }
        public int property_id { get; set; }
        public DateTime schedule { get; set; }
        public string title { get; set; }
        public string MyProperty { get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public string status { get; set; }
        public PropertyDto Property { get; set; }
    }
}
