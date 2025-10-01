using UnidadResidencial.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace UnidadResidencial.Web.Models
{
    public class Section : IId
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsHidden { get; set; } = false;
    }
}
