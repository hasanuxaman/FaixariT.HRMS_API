using System;
using System.Collections.Generic;

namespace FaixariT.HRMS.DBModels;

public partial class AppModule
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public virtual ICollection<AppMenu> AppMenu { get; set; } = new List<AppMenu>();
}
