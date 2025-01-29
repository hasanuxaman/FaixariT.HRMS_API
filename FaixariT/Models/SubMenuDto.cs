using FaixariT.Models;

namespace FaixariT.HRMS.Models
{
    public class SubMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MenuId { get; set; }
        public MenuDto Menu { get; set; }
    }
}
