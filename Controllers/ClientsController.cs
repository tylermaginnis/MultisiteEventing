using Microsoft.AspNetCore.Mvc;
using MultisiteEventing.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using StoreModel = MultisiteEventing.Models.Store;
using LocationModel = MultisiteEventing.Models.Location;
using ContactModel = MultisiteEventing.Models.Contact;

namespace MultisiteEventing.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "ClientsCacheKey";

        public ClientsController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            if (!_cache.TryGetValue(CacheKey, out List<Client> _))
            {
                _cache.Set(CacheKey, new List<Client>());
            }
        }

        public IActionResult Index()
        {
            var clients = _cache.Get<List<Client>>(CacheKey);
            ViewData["Assets"] = _cache.Get<List<AssetModel>>("AssetsList");

            return View(clients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Assets"] = _cache.Get<List<AssetModel>>("AssetsList");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                var clients = _cache.Get<List<Client>>(CacheKey);
                client.Id = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1;
                clients.Add(client);
                _cache.Set(CacheKey, clients);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var clients = _cache.Get<List<Client>>(CacheKey);
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            ViewBag.Assets = _cache.Get<List<Asset>>("AssetsList");
            return PartialView("_EditClientPartial", client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                var clients = _cache.Get<List<Client>>(CacheKey);
                var existingClient = clients.FirstOrDefault(c => c.Id == client.Id);
                if (existingClient != null)
                {
                    existingClient.Name = client.Name;
                    existingClient.Description = client.Description;
                    existingClient.Assets = client.Assets;
                    existingClient.Events = client.Events;
                    _cache.Set(CacheKey, clients);
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var clients = _cache.Get<List<Client>>(CacheKey);
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                _cache.Set(CacheKey, clients);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
