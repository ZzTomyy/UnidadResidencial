using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.Data;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Services.Implementations
{
    public class RolesService : CustomQueryableOperationsService, IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ResidencialRoleDTO>> CreateAsync(ResidencialRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid newRoleId = Guid.NewGuid();

                    // ResidencialRole
                    ResidencialRole role = _mapper.Map<ResidencialRole>(dto);

                    await _context.ResidencialRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    // Permissions
                    List<Guid> permissionIds = new();

                    if (!string.IsNullOrEmpty(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                    }

                    foreach (Guid permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            ResidencialRoleId = newRoleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Response<ResidencialRoleDTO>.Success(dto, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Response<ResidencialRoleDTO>.Failure(ex);
                }
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<ResidencialRole>(id);
        }

        public async Task<Response<ResidencialRoleDTO>> EditAsync(ResidencialRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return Response<ResidencialRoleDTO>.Failure($"El rol '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                // ResidencialRole
                ResidencialRole role = _mapper.Map<ResidencialRole>(dto);
                _context.ResidencialRoles.Update(role);

                // Permissions
                List<Guid> permissionIds = new();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                }

                // Delete old
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.ResidencialRoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Create new ones
                foreach (Guid permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        ResidencialRoleId = role.Id,
                        PermissionId = permissionId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                await _context.SaveChangesAsync();

                return Response<ResidencialRoleDTO>.Success(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<ResidencialRoleDTO>.Failure(ex);
            }
        }

        public async Task<Response<ResidencialRoleDTO>> GetOneAsync(Guid id)
        {
            Response<ResidencialRoleDTO> response = await GetOneAsync<ResidencialRole, ResidencialRoleDTO>(id);

            if (!response.IsSuccess)
            {
                return response;
            }

            ResidencialRoleDTO dto = response.Result;

            List<PermissionsForRoleDTO> permissions = await _context.Permissions.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.ResidencialRoleId == dto.Id)
            }).ToListAsync();

            dto.Permissions = permissions;

            return Response<ResidencialRoleDTO>.Success(dto, "Rol obtenido con éxito");
        }

        public async Task<Response<PaginationResponse<ResidencialRoleDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<ResidencialRole> query = _context.ResidencialRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(r => r.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<ResidencialRole, ResidencialRoleDTO>(request, query);
        }

        public async Task<Response<List<PermissionsForRoleDTO>>> GetPermissionsAsync()
        {
            Response<List<PermissionDTO>> permissionsResponse = await GetCompleteListAsync<Permission, PermissionDTO>();

            if (!permissionsResponse.IsSuccess)
            {
                return Response<List<PermissionsForRoleDTO>>.Failure(permissionsResponse.Message);
            }

            List<PermissionsForRoleDTO> dto = permissionsResponse.Result.Select(p => new PermissionsForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = false
            }).ToList();

            return Response<List<PermissionsForRoleDTO>>.Success(dto);
        }
    }
}
