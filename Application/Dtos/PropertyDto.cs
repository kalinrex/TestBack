﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class PropertyDto
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? disabled_at { get; set; }
        public string status { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }
}
