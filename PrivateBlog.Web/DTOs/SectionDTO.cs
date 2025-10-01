using System.ComponentModel.DataAnnotations;

namespace UnidadResidencial.Web.DTOs
{
    public class SectionDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Section")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Hidden?")]
        public bool IsHidden { get; set; } = false;
    }
}