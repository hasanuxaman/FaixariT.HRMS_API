using System;
using System.Collections.Generic;

namespace FaixariT.HRMS.DBModels;

public partial class AppMenu
{
    public int MenuId { get; set; }

    public int ModuleId { get; set; }

    public string MenuName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual ICollection<AppSubMenu> AppSubMenu { get; set; } = new List<AppSubMenu>();

    public virtual AppModule Module { get; set; } = null!;
}
