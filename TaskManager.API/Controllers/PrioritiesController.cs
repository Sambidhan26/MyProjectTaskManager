using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers
{
    public class PrioritiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
