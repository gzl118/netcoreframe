using DInjectionProvider;
using Microsoft.AspNetCore.Mvc;
using SANS.BLL.Interface;
using SANS.DbEntity.Models;
using SANS.WebApp.Data;
using SANS.WebApp.Filters;

namespace SANS.WebApp.Components
{
    [ViewComponent(Name = "Navigation")]
    [VerificationLogin]
    public class NavigationViewComponent : ViewComponent
    {
        private readonly WholeInjection injection;
        public NavigationViewComponent(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IViewComponentResult Invoke()
        {
            var user = injection.GetT<UserAccount>().GetUserInfo();
            var MenuDto = injection.GetT<ISysMenuBLL>().GetMenusBy(user, SysEnum.Enum_AuthorityType.Type_Menu).Data;
            return View(MenuDto);
        }
    }
}
