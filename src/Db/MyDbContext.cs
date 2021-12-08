using _8Hours.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Hours.Db
{
    internal class MyDbContext : DbContext
    {
        public DbSet<TimeRecord> TimeRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Application.db;Cache=Shared");
        }
    }
}
