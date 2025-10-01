using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.Data;
using UnidadResidencial.Web.Models;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Services.Abstractions;

namespace UnidadResidencial.Web.Services.Implementations
{
    public class SectionsService : CustomQueryableOperationsService, ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<SectionDTO>> CreateAsync(SectionDTO dto)
        {
            return await CreateAsync<Section, SectionDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Section>(id);
        }

        public async Task<Response<SectionDTO>> EditAsync(SectionDTO dto)
        {
            return await EditAsync<Section, SectionDTO>(dto, dto.Id);
        }

        public async Task<Response<List<SectionDTO>>> GetListAsync()
        {
            return await GetCompleteListAsync<Section, SectionDTO>();
        }

        public async Task<Response<SectionDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<Section, SectionDTO>(id);
        }

        public async Task<Response<PaginationResponse<SectionDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Section> query = _context.Sections.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s =>
                    s.Name.ToLower().Contains(request.Filter.ToLower()) ||
                    (s.Description != null && s.Description.ToLower().Contains(request.Filter.ToLower()))
                );
            }

            return await GetPaginationAsync<Section, SectionDTO>(request, query);
        }

        public async Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto)
        {
            try
            {
                Section? section = await _context.Sections.AsNoTracking()
                                                          .FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (section is null)
                {
                    return Response<object>.Failure($"No section found with id: {dto.SectionId}");
                }

                section.IsHidden = dto.Hide;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return Response<object>.Success("Section updated successfully");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
    }
}