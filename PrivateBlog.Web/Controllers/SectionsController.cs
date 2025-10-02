using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using UnidadResidencial.Web.Core.Pagination;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Services.Abstractions;

namespace UnidadResidencial.Web.Controllers
{
    [Route("Sections")]
    public class SeccionesController : Controller
    {
        private readonly ISectionsService _sectionsService;
        private readonly INotyfService _notyfService;

        public SeccionesController(ISectionsService sectionsService, INotyfService notyfService)
        {
            _sectionsService = sectionsService;
            _notyfService = notyfService;
        }

        //  LISTADO
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            var response = await _sectionsService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        //  CREAR 
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _sectionsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        //  EDITAR 
        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            var response = await _sectionsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }
            return View(response.Result);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromForm] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            var response = await _sectionsService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        //  DETALLES 
        [HttpGet("details/{id:guid}")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var response = await _sectionsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        //  ELIMINAR 
        [HttpPost("delete/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _sectionsService.DeleteAsync(id);

            if (!response.IsSuccess)
                _notyfService.Error(response.Message);
            else
                _notyfService.Success(response.Message);

            return RedirectToAction(nameof(Index));
        }

        //  TOGGLE 
        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromForm] ToggleSectionStatusDTO dto)
        {
            var response = await _sectionsService.ToggleAsync(dto);

            if (!response.IsSuccess)
                _notyfService.Error(response.Message);
            else
                _notyfService.Success(response.Message);

            return RedirectToAction(nameof(Index));
        }
    }
}
