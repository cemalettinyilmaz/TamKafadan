using Microsoft.EntityFrameworkCore;
using System;

namespace TamKafadan.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Yazar> Yazarlar { get; set; }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Konu> Konular { get; set; }
        public DbSet<Admin> Admins { get; set; }
        internal Yazar FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
}
