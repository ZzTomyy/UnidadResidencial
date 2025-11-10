namespace UnidadResidencial.Web.Data.Entities
{
    public class RolePermission
    {
        public required Guid ResidencialRoleId { get; set; }
        public required Guid PermissionId { get; set; }
        public ResidencialRole Role { get; set; }
        public Permission Permission { get; set; }

    }
}
