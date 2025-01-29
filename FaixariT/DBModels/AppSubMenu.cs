using System;
using System.Collections.Generic;

namespace FaixariT.HRMS.DBModels;

public partial class AppSubMenu
{
    public int SubMenuId { get; set; }

    public int MenuId { get; set; }

    public string SubMenuName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual AppMenu Menu { get; set; } = null!;
}
