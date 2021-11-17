using Application.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public DateTime Schedule { get; set; }
        public string Title { get; set; }
        public DateTime Created_at { get; set; }
        public string  Status { get; set; }
        public string Condition { 
            get 
            {
                DateTime today = DateTime.Now;
                string condition = "";
                if (Status == StatusConst.Active && Schedule > today)
                    condition = "Pendiente a realizar";
                if (Status == StatusConst.Active && Schedule < today)
                    condition = "Atrazada";
                if (Status == StatusConst.Done)
                    condition = "Finalizada";
                if (Status == StatusConst.Cancel)
                    condition = "Cancelada";
                return condition;
            } 
        }
        public PropertyResponseDto Property { get; set; }
        public SurveyResponseDto Survey { get; set; }

    }
    public class PropertyResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
    }
    public class SurveyResponseDto
    {
        public int Id { get; set; }
        public string Answers { get; set; }
        public int Activity_id { get; set; }
    }
}
