using Microsoft.AspNetCore.Mvc;

namespace MultisiteEventing.Controllers
{
    public class StoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
