using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MultisiteEventing.Models;
using System.Collections.Generic;

namespace MultisiteEventing.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IMemoryCache _cache;
        private const string ClientsCacheKey = "ClientsCacheKey";
        private const string EventsCacheKey = "EventsCacheKey";

        public CalendarController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            if (!_cache.TryGetValue(ClientsCacheKey, out List<Client> _))
            {
                _cache.Set(ClientsCacheKey, new List<Client>());
            }
            if (!_cache.TryGetValue(EventsCacheKey, out List<Event> _))
            {
                _cache.Set(EventsCacheKey, new List<Event>());
            }
        }

        public IActionResult Index()
        {
            ViewData["Clients"] = _cache.Get<List<Client>>(ClientsCacheKey);
            ViewData["Events"] = _cache.Get<List<Event>>(EventsCacheKey);
            return View();
        }
    }
}
