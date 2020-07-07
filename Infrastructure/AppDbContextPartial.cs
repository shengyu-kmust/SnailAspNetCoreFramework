using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public partial class AppDbContextPartial : DbContext
    {
        public DbSet<Demo> Demo { get; set; }
    }
}
