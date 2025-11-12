using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private const string ADMINISTRADOR = "Administrador";
        private const string PROPIETARIO = "Residente";

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
            await BasicRoleAsync();
            await ContentManagerRoleAsync();
        }

        private async Task CheckUsersAsync()
        {
            // Admin
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "manuel@yopmail.com");

            if (user is null)
            {
                ResidencialRole adminRole = await _context.ResidencialRoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User 
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Domínguez",
                    PhoneNumber = "3000000000",
                    UserName = "manuel@yopmail.com",
                    Document = "1111",
                    ResidencialRoleId = adminRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }

            // Content manager
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "anad@yopmail.com");

            if (user is null)
            {
                ResidencialRole contentManagerRole = await _context.ResidencialRoles.FirstOrDefaultAsync(r => r.Name == ADMINISTRADOR);

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "3100000000",
                    UserName = "anad@yopmail.com",
                    Document = "222",
                    ResidencialRoleId = contentManagerRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token =  (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }

            // Basic
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "jhond@yopmail.com");

            if (user is null)
            {
                ResidencialRole basicRole = await _context.ResidencialRoles.FirstOrDefaultAsync(r => r.Name == PROPIETARIO);

                user = new User
                {
                    Email = "jhond@yopmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "3200000000",
                    UserName = "jhond@yopmail.com",
                    Document = "333",
                    ResidencialRoleId = basicRole!.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.ResidencialRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                ResidencialRole role = new ResidencialRole { Id = Guid.NewGuid(), Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.ResidencialRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task BasicRoleAsync()
        {
            bool exists = await _context.ResidencialRoles.AnyAsync(r => r.Name == PROPIETARIO);

            if (!exists)
            {
                ResidencialRole role = new ResidencialRole { Id = Guid.NewGuid(), Name = PROPIETARIO };
                await _context.ResidencialRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.ResidencialRoles.AnyAsync(r => r.Name == ADMINISTRADOR);

            if (!exists)
            {
                ResidencialRole role = new ResidencialRole { Id = Guid.NewGuid(), Name = ADMINISTRADOR };
                await _context.ResidencialRoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones" || p.Module == "Residencials")
                                                                         .ToListAsync();
                foreach(Permission permission in permissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission { PermissionId = permission.Id, ResidencialRoleId = role.Id });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
