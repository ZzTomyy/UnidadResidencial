using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _userservice;

        public SeedDb(DataContext context, IUsersService userservice)
        {
            _context = context;
            _userservice = userservice;
        }

        public async Task SeedAsync()
        {
            await new SectionsSeeder(_context).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();
            await new ResidencialSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _userservice).SeedAsync();
        }
    }
}
