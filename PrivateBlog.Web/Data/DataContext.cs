using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Section> Sections { get; set; }
    }
}
