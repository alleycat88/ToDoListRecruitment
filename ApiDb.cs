using System;
using Microsoft.EntityFrameworkCore;
using ToDoListRecruitment.Models;

namespace ToDoListRecruitment
{
    public class ApiDb : DbContext, IDisposable
    {
        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(@"Server="+(string)Program.settings.database.host+";Port="+(string)Program.settings.database.port+";database="+(string)Program.settings.database.name+";uid="+(string)Program.settings.database.uid+";pwd="+(string)Program.settings.database.pwd+";connection timeout=60");
    }
}