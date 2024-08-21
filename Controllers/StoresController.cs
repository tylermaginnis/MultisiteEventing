using Microsoft.AspNetCore.Mvc;
using MultisiteEventing.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace MultisiteEventing.Controllers
{
    public class StoresController : Controller
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "StoresList";

        public StoresController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            if (!_cache.TryGetValue(CacheKey, out List<Store> _))
            {
                _cache.Set(CacheKey, new List<Store>());
            }
        }

        public IActionResult Index()
        {
            var stores = _cache.Get<List<Store>>(CacheKey);
            return View(stores);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                var stores = _cache.Get<List<Store>>(CacheKey);
                store.Id = stores.Count > 0 ? stores.Max(s => s.Id) + 1 : 1;

                stores.Add(store);
                _cache.Set(CacheKey, stores);

                return RedirectToAction("Index");
            }
            return View(store);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var stores = _cache.Get<List<Store>>(CacheKey);
            var store = stores.FirstOrDefault(s => s.Id == id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        public IActionResult Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                var stores = _cache.Get<List<Store>>(CacheKey);
                var existingStore = stores.FirstOrDefault(s => s.Id == store.Id);
                if (existingStore != null)
                {
                    existingStore.Name = store.Name;
                    // existingStore.Location = store.Location; // Commented out as 'Location' does not exist in 'Store'
                    existingStore.Contacts = store.Contacts;

                    _cache.Set(CacheKey, stores);
                }
                return RedirectToAction("Index");
            }
            return View(store);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var stores = _cache.Get<List<Store>>(CacheKey);
            var store = stores.FirstOrDefault(s => s.Id == id);
            if (store != null)
            {
                stores.Remove(store);
                _cache.Set(CacheKey, stores);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditLocation(int storeId, Location location)
        {
            if (ModelState.IsValid)
            {
                var stores = _cache.Get<List<Store>>(CacheKey);
                var store = stores.FirstOrDefault(s => s.Id == storeId);
                if (store != null)
                {
                    var existingLocation = store.Locations.FirstOrDefault(l => l.Id == location.Id);
                    if (existingLocation != null)
                    {
                        existingLocation.Address = location.Address;
                    }
                    else
                    {
                        store.Locations.Add(location);
                    }

                    _cache.Set(CacheKey, stores);
                }
                return RedirectToAction("Edit", new { id = storeId });
            }
            return View(location);
        }

        [HttpPost]
        public IActionResult SaveStoreLocationContact([FromBody] SaveStoreLocationContactRequest request)
        {
            if (string.IsNullOrEmpty(request.Contact.Name) || string.IsNullOrEmpty(request.Contact.Phone) || string.IsNullOrEmpty(request.Contact.Email) || string.IsNullOrEmpty(request.Contact.Role))
            {
                return Json(new { success = false, message = "Validation failed" });
            }

            var stores = _cache.Get<List<Store>>(CacheKey);
            var store = stores.FirstOrDefault(s => s.Id == request.StoreId);
            if (store != null)
            {
                var location = store.Locations.FirstOrDefault(l => l.Id == request.LocationId);
                if (location != null)
                {
                    var existingContact = location.Contacts.FirstOrDefault(c => c.Id == request.Contact.Id);
                    if (existingContact != null)
                    {
                        existingContact.Name = request.Contact.Name;
                        existingContact.Phone = request.Contact.Phone;
                        existingContact.Email = request.Contact.Email;
                        existingContact.Role = request.Contact.Role;
                    }
                    else
                    {
                        request.Contact.Id = location.Contacts.Count > 0 ? location.Contacts.Max(c => c.Id) + 1 : 1;
                        location.Contacts.Add(request.Contact);
                    }

                    _cache.Set(CacheKey, stores);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        public class SaveStoreLocationContactRequest
        {
            public int StoreId { get; set; }
            public int LocationId { get; set; }
            public Contact Contact { get; set; }
        }
    }
}
