using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class SqlJoinDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
