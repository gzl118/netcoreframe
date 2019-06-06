using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.BLL
{
    public class SysRoleVO
    {
        public SysRoleDto roleDto { get; set; }
        public List<SysMenuDto> objCheckMenus { get; set; }
    }
}
