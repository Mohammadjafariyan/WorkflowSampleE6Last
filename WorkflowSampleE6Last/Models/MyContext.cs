using System.Data.Entity;

namespace WorkflowSampleE6Last.Models
{
    public class MyContext : DbContext
    {

        public MyContext():base("DefaultConnection")
        {
            
        }

        public virtual DbSet<Vacation> Vacations { get; set; }
        
    }
}