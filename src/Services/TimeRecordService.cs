using _8Hours.Db;
using _8Hours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Hours.Services
{
    internal class TimeRecordService
    {
        public async Task JobStart(TimeRecord item)
        {
            await using var db = new MyDbContext();
            db.TimeRecords.Add(item);
            await db.SaveChangesAsync();
        }

        public async Task JobStop()
        {
            await using var db = new MyDbContext();
            var item = db.TimeRecords.FirstOrDefault(x => x.EndTime == null);
            if (item == null)
            {
                return;
            }
            item.EndTime = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}
