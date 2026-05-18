using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
