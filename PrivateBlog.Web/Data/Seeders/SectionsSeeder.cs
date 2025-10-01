using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Data.Seeders
{
    public class SectionsSeeder
    {
        private readonly DataContext _context;

        public SectionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Section> sections = new List<Section>()
            {
                new Section { Id = Guid.NewGuid(), Name = "General", Description = "Basic company information"},
                new Section { Id = Guid.NewGuid(), Name = "IT"},
                new Section { Id = Guid.NewGuid(), Name = "Pentesting"},
                new Section { Id = Guid.NewGuid(), Name = "Classes", IsHidden = true}
            };

            foreach (Section section in sections)
            {
                bool exists = await _context.Sections.AnyAsync(s => s.Name == section.Name);

                if (!exists)
                {
                    await _context.Sections.AddAsync(section);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}