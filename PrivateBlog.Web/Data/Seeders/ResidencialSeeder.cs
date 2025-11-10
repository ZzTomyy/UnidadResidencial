using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Data.Seeders
{
    public class ResidencialSeeder
    {
        private readonly DataContext _context;
        public ResidencialSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            Section section = await _context.Sections.FirstOrDefaultAsync();

            List<Residencial> blogs = new List<Residencial>()
            {
                new Residencial { Id = Guid.NewGuid(), Name = "Residencial 1", Content = "<p> Residencial 1 </p>", SectionId = section.Id },
                new Residencial { Id = Guid.NewGuid(), Name = "Residencial 2", Content = "<p> Residencial 2 </p>", SectionId = section.Id },
                new Residencial { Id = Guid.NewGuid(), Name = "Residencial 3", Content = "<p> Residencial 3 </p>", SectionId = section.Id },
            };

            foreach (Residencial blog in blogs)
            {
                bool exists = await _context.Blogs.AnyAsync(s => s.Name == blog.Name);

                if (!exists)
                {
                    await _context.Blogs.AddAsync(blog);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
