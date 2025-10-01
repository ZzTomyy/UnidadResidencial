using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.DTOs;

namespace UnidadResidencial.Web.Services.Abstractions
{
    public interface ISectionsService
    {
        Task<Response<SectionDTO>> CreateAsync(SectionDTO dto);
        Task<Response<object>> DeleteAsync(Guid id);
        Task<Response<SectionDTO>> EditAsync(SectionDTO dto);
        Task<Response<List<SectionDTO>>> GetListAsync();
        Task<Response<SectionDTO>> GetOneAsync(Guid id);
        Task<Response<PaginationResponse<SectionDTO>>> GetPaginatedListAsync(PaginationRequest request);
        Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto);
    }
}
