using CoachOS.Domain.Common;

namespace CoachOS.Domain.Tenancy
{
    public class Module : BaseEntity
    {
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? RoutePath { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? ParentModuleId { get; set; }
        public bool IsMenuItem { get; set; }
        public bool IsDefaultEnabled { get; set; }
        public bool IsActive { get; set; } = true;

        public Module? ParentModule { get; set; }
        public ICollection<Module> SubModules { get; set; } = new List<Module>();
    }
}
