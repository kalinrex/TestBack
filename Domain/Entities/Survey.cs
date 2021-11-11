using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Survey
    {
        public int id { get; set; }
        [Column(TypeName = "jsonb")]
        public string answers { get; set; }
        public DateTime created_at { get; set; }
        public int activity_id { get; set; }
        public Activity Activity { get; set; }
    }
}
