using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.DTOs;

namespace UnidadResidencial.Web.Services.Abtractions
{
    public interface IRolesService
    {
        public Task<Response<ResidencialRoleDTO>> CreateAsync(ResidencialRoleDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<ResidencialRoleDTO>> EditAsync(ResidencialRoleDTO dto);
        public Task<Response<ResidencialRoleDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<ResidencialRoleDTO>>> GetPaginatedListAsync(PaginationRequest request);
        public Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync();
    }
}
