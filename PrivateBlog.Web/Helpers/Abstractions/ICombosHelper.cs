using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnidadResidencial.Web.Helpers.Abstractions
{
    public interface ICombosHelper
    {
        public Task<List<SelectListItem>> GetComboSections();
    }
}
