using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Extensions;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRolesService rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<ResidencialRoleDTO>> response = await _rolesService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse.IsSuccess)
            {
                _notyfService.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            ResidencialRoleDTO dto = new ResidencialRoleDTO
            {
                Permissions = permissionsResponse.Result
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(ResidencialRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();

                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;

                return View(dto);
            }

            Response<ResidencialRoleDTO> createResponse = await _rolesService.CreateAsync(dto);
            if (createResponse.IsSuccess)
            {
                _notyfService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(createResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            return View(dto);
        }


        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<ResidencialRoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(ResidencialRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();

                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;

                return View(dto);
            }

            Response<ResidencialRoleDTO> updateResponse = await _rolesService.EditAsync(dto);
            if (updateResponse.IsSuccess)
            {
                _notyfService.Success(updateResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(updateResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            return View(dto);
        }

        [HttpGet]
        [CustomAuthorize(permission: "deleteRoles", module: "Roles")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost("Roles/DeleteConfirmed/{id}")]
        [ActionName("DeleteConfirmed")]
        [CustomAuthorize(permission: "deleteRoles", module: "Roles")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Response<object> response = await _rolesService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notyfService.Success("Rol eliminado correctamente");
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(response.Message ?? "No se pudo eliminar el rol");
            return RedirectToAction(nameof(Index));
        }


    }
}


