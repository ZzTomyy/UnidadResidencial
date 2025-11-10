using System.ComponentModel.DataAnnotations;
using UnidadResidencial.Web.Data.Abstractions;
using UnidadResidencial.Web.Data.Entities;

namespace UnidadResidencial.Web.Models
{
    public class Section : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public required string Name { get; set; }

        [MaxLength(64)]
        public string? Description { get; set; }

        public bool IsHidden { get; set; } = false;

        public List<Residencial>? Residencials { get; set; }

    }
}
