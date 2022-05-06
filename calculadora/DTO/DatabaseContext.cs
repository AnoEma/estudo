using Calculadora.Model;
using Estudo.Model;
using Microsoft.EntityFrameworkCore;

namespace Calculadora.DTO
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}