using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TicketData
{
    public class TicketSystemDbContext : DbContext
    {
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatClaim> SeatClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var userPath = Environment.GetEnvironmentVariable(
              RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
              "LOCALAPPDATA" : "Home");

            var pathDb = userPath + "\\TicketSystem.db";

            optionsBuilder.UseSqlite("Data Source=" + pathDb);
        }

        public void DatabaseMigrate()
        {
            this.Database.Migrate();
        }
    }
}
