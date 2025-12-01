using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class QueryBuilderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
