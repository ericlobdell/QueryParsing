using Microsoft.EntityFrameworkCore;
using QueryParser.Web.Models;

namespace QueryParser.Web.Data
{
    public class TestContext : DbContext
    {
        public TestContext( DbContextOptions<TestContext> options )
            : base( options ) { }

        public DbSet<Person> Person { get; set; }
        public DbSet<Foo> Foo { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            if ( !optionsBuilder.IsConfigured )
            {
                optionsBuilder.UseSqlServer( @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0" );
            }
        }
    }
}
