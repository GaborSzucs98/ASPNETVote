using Microsoft.AspNetCore.Mvc;

namespace MVCapp.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
