using Microsoft.AspNetCore.Mvc;

namespace Indigo.Areas.IndigoAdmin.Controllers
{
    [Area("IndigoAdmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
