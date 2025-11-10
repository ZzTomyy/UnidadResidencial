using UnidadResidencial.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Data.Entities
{
    public class Residencial : IId  
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64)]
        public required string Name { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string Content { get; set; } = null!;

        public required Guid SectionId { get; set; }

        public Section? Section { get; set; }
    }
}
