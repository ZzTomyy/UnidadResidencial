using System.ComponentModel.DataAnnotations;

namespace UnidadResidencial.Web.DTOs
{
    public class ToggleSectionStatusDTO
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        public Guid SectionId { get; set; }

        public bool Hide { get; set; } = true;
    }
}
