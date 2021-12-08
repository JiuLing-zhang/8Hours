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
        public void Add(TimeRecord item)
        {
            using (var db = new MyDbContext())
            {
                db.TimeRecords.Add(item);
                db.SaveChanges();
            }
        }
    }
}
