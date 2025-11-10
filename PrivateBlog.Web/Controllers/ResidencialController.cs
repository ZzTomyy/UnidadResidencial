using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnidadResidencial.Web.Core;
using UnidadResidencial.Web.Core.Extensions;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Helpers.Abstractions;
using UnidadResidencial.Web.Services.Abtractions;

namespace UnidadResidencial.Web.Controllers
{
    [Authorize]
    public class ResidencialController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IBlogsService _blogsService;
        private readonly ICombosHelper _combosHelper;

        public ResidencialController(INotyfService notyfService, IBlogsService blogsService, ICombosHelper combosHelper)
        {
            _notyfService = notyfService;
            _blogsService = blogsService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showBlogs", module: "Residencials")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<ResidencialDTO>> response = await _blogsService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createBlogs", module: "Residencials")]
        public async Task<IActionResult> Create()
        {
            ResidencialDTO dto = new ResidencialDTO
            {
                Sections = await _combosHelper.GetComboSections()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createBlogs", module: "Residencials")]
        public async Task<IActionResult> Create(ResidencialDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<ResidencialDTO> response = await _blogsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            dto.Sections = await _combosHelper.GetComboSections();
            return RedirectToAction(nameof(Index));
        }
    }
}
