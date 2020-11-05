using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Planner.Model
{
    class PlannerDbContext : DbContext
    {
        public DbSet<Status> Status { get; set; }
        public DbSet<Task> Task { get; set; }
    }
}
