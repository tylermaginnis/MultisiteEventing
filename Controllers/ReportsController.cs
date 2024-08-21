using Microsoft.AspNetCore.Mvc;

namespace MultisiteEventing.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
