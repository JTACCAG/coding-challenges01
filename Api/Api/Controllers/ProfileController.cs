using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
