using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StaffApi.Models;

namespace StaffApi.Data
{
    public class StaffApiContext : DbContext
    {
        public StaffApiContext (DbContextOptions<StaffApiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeePosition>()
                .HasKey(x => new { x.EmployeeId, x.PositionId });
            builder.Entity<EmployeePosition>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.EmployeePositions)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EmployeePosition>()
                .HasOne(x => x.Position)
                .WithMany(x => x.EmployeePositions)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
