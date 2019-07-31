using System.Data.Entity;

namespace WorkflowSampleE6Last.Models
{
    public class MyContext : DbContext
    {

        public MyContext():base("DefaultConnection")
        {
            
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasMany(a => a.Children).WithOptional(a => a.Parent)
                .HasForeignKey(a => a.ParentId);

        }

        public virtual DbSet<Vacation> Vacations { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        
    }
}