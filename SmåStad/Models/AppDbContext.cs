using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmåStad.Models {
  public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Errand> Errands { get; set; }
    public DbSet<ErrandStatus> ErrandStatuses { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Sample> Samples { get; set; }
    public DbSet<Sequence> Sequences { get; set; }
  }
}
