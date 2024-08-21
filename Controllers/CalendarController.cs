using Microsoft.AspNetCore.Mvc;

namespace MultisiteEventing.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
