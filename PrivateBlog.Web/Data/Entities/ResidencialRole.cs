using UnidadResidencial.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace UnidadResidencial.Web.Data.Entities
{
    public class ResidencialRole : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Name { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
