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
        internal async Task JobStart(TimeRecord item)
        {
            await using var db = new MyDbContext();
            db.TimeRecords.Add(item);
            await db.SaveChangesAsync();
        }

        internal async Task JobStop()
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

        internal async Task<List<TimeRecord>> GetJobDetail(DateTime beginTime, DateTime endTime)
        {
            //这里不考虑数据跨天，所以直接以结束时间作为条件来过滤
            await using var db = new MyDbContext();
            var list = db.TimeRecords
                .Where(x => x.EndTime >= beginTime && x.EndTime <= endTime && x.EndTime != null)
                .ToList();
            return list;
        }
    }
}
