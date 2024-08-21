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
        private const string StoresCacheKey = "StoresList";

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
            if (!_cache.TryGetValue(StoresCacheKey, out List<Store> _))
            {
                _cache.Set(StoresCacheKey, new List<Store>());
            }
        }

        public IActionResult Index()
        {
            ViewData["Clients"] = _cache.Get<List<Client>>(ClientsCacheKey);
            ViewData["Events"] = _cache.Get<List<Event>>(EventsCacheKey);
            var stores = _cache.Get<List<Store>>(StoresCacheKey);
            var storeLocations = new List<dynamic>();

            foreach (var store in stores)
            {
                foreach (var location in store.Locations)
                {
                    storeLocations.Add(new
                    {
                        StoreId = store.Id,
                        LocationId = location.Id,
                        LocationName = $"{store.Name} - {location.DisplayId} - {location.Address.Street}, {location.Address.City}, {location.Address.State} {location.Address.ZipCode}"
                    });
                }
            }

            ViewData["StoreLocations"] = storeLocations;
            return View();
        }
    }
}
