using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class Helpers
    {
        private readonly ActivityDbContext _context;

        public Helpers(ActivityDbContext context)
        {
            _context = context;
        }
        public int todayValidate(DateTime dateCompare)
        {
            var today = DateTime.Now;
            var compareDate = DateTime.Compare(today, dateCompare);
            return compareDate;
        }
        public async Task<bool> sameDateSchedule(int idProperty, DateTime schedule)
        {
            var query = await _context.Activity.Where(x => x.property_id == idProperty).ToListAsync();
            if (query.Any())
            {
                foreach (var activity in query)
                {
                    DateTime dateStart = activity.schedule;
                    DateTime dateEnd = activity.schedule.AddHours(1);

                    if (schedule >= dateStart && schedule < dateEnd)
                        return true;
                }
            }
            return false;
        }
        public async Task<bool> sameDateSchedule(int idProperty, DateTime schedule, int activity_id)
        {
            var query = await _context.Activity.Where(x => x.property_id == idProperty).ToListAsync();
            if (query.Any())
            {
                foreach (var activity in query)
                {
                    DateTime dateStart = activity.schedule;
                    DateTime dateEnd = activity.schedule.AddHours(1);

                    if (schedule >= dateStart && schedule < dateEnd && activity.id != activity_id)
                        return true;
                }
            }
            return false;
        }
    }
}
