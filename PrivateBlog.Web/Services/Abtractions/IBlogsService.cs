using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.DTOs;

namespace UnidadResidencial.Web.Services.Abtractions
{
    public interface IBlogsService
    {
        public Task<Response<ResidencialDTO>> CreateAsync(ResidencialDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<ResidencialDTO>> EditAsync(ResidencialDTO dto);
        public Task<Response<ResidencialDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<ResidencialDTO>>> GetPaginatedListAsync(PaginationRequest request);
    }
}
