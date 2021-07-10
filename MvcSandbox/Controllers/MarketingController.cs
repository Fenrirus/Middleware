using Microsoft.AspNetCore.Mvc;

namespace MvcSandbox.Controllers
{
    public class MarketingController : Controller
    {
        public IActionResult NewSplash()
        {
            return Content("The new splash");
        }

        [MobileRedirectActionFilter(Action = "NewSplash", Controller = "Marketing")]
        public IActionResult Splash()
        {
            return Content("The old splash");
        }
    }
}