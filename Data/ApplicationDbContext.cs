using Microsoft.EntityFrameworkCore;
using TechCoreSolutions.Models;

namespace TechCoreSolutions.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeID);

            modelBuilder.Entity<Employee>()
                .Property(e => e.DailyRate)
                .HasColumnType("decimal(18,2)");

            // Configure Payroll entity
            modelBuilder.Entity<Payroll>()
                .HasKey(p => p.PayrollID);

            modelBuilder.Entity<Payroll>()
                .Property(p => p.Deduction)
                .HasColumnType("decimal(18,2)");

            // Configure relationship between Employee and Payroll
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
