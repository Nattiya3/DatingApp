using Microsoft.EntityFrameworkCore;
using DatingApp.API.vscode.Models;
namespace DatingApp.API.vscode.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Value> Values {get; set;}

        public DbSet<User> Users { get; set; }
        
    }
}