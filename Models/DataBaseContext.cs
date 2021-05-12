using Microsoft.EntityFrameworkCore;
 
namespace FunWithFiles.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options) { }
        public DbSet<FileDataModel> File {get;set;}

    }
}