using Estudo.Model;
using Microsoft.EntityFrameworkCore;

namespace Calculadora.DTO
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {
        }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
    }
}