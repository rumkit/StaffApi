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

        public DbSet<StaffApi.Models.Employee> Employees { get; set; }
    }
}
