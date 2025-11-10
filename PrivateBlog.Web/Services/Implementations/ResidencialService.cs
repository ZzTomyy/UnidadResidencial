using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.Data;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Models;
using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Services.Implementations
{
    public class ResidencialService : CustomQueryableOperationsService, IBlogsService
    {
        private readonly DataContext _context;

        public ResidencialService(DataContext context, IMapper mapper) : base (context, mapper)
        {
            _context = context;   
        }

        public async Task<Response<ResidencialDTO>> CreateAsync(ResidencialDTO dto)
        {
            return await CreateAsync<Residencial, ResidencialDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Residencial>(id);
        }

        public async Task<Response<ResidencialDTO>> EditAsync(ResidencialDTO dto)
        {
            return await EditAsync<Residencial, ResidencialDTO>(dto, dto.Id);
        }

        public async Task<Response<ResidencialDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<Residencial, ResidencialDTO>(id);
        }

        public async Task<Response<PaginationResponse<ResidencialDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Residencial> query = _context.Blogs.Include(b => b.Section)
                                                   .Select(b => new Residencial 
                                                   {
                                                       Id = b.Id,
                                                       Name = b.Name,

                                                       Section = new Section 
                                                       {
                                                           Id = b.Section.Id,
                                                           Name = b.Section.Name
                                                       },

                                                       SectionId = b.SectionId                                                       
                                                   })
                                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Residencial, ResidencialDTO>(request, query);
        }
    }
}
