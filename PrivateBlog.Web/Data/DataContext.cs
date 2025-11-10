using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;    
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Residencial> Blogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ResidencialRole> ResidencialRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureIndexes(builder);

            ConfigureKeys(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            // ResidencialRole Permissions
            builder.Entity<RolePermission>().HasKey(rp => new { rp.PermissionId, rp.ResidencialRoleId });

            builder.Entity<RolePermission>().HasOne(rp => rp.Role)
                                            .WithMany(r => r.RolePermissions)
                                            .HasForeignKey(rp => rp.ResidencialRoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<ResidencialRole>().HasIndex(r => r.Name)
                                             .IsUnique();

            // Sections
            builder.Entity<Section>().HasIndex(s => s.Name)
                                     .IsUnique();

            // User
            builder.Entity<User>().HasIndex(u => u.Document)
                                  .IsUnique();

            // Permission
            builder.Entity<Permission>().HasIndex(p => p.Name)
                                        .IsUnique();
        }
    }
}
