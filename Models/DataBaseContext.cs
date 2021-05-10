using Microsoft.EntityFrameworkCore;//for sql connection
 
namespace FunWithFiles.Models
{
    public class DataBaseContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public DataBaseContext(DbContextOptions options) : base(options) { }
        public DbSet<CsvFileDataModel> CsvFiles {get;set;}

    }
}