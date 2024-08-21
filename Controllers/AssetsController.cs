using Microsoft.AspNetCore.Mvc;
using MultisiteEventing.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace MultisiteEventing.Controllers
{
    public class AssetsController : Controller
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "AssetsList";

        public AssetsController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            if (!_cache.TryGetValue(CacheKey, out List<AssetModel> _))
            {
                _cache.Set(CacheKey, new List<AssetModel>());
            }
        }

        public IActionResult Index()
        {
            var assets = _cache.Get<List<AssetModel>>(CacheKey);
            return View(assets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                var assets = _cache.Get<List<AssetModel>>(CacheKey);
                asset.Id = assets.Count > 0 ? assets.Max(a => a.Id) + 1 : 1;
                asset.CreatedDate = DateTime.Now;
                asset.UpdatedDate = DateTime.Now;
                
                assets.Add(asset);
                _cache.Set(CacheKey, assets);

                return RedirectToAction("Index");
            }
            return View(asset);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var assets = _cache.Get<List<AssetModel>>(CacheKey);
            var asset = assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                return NotFound();
            }
            return View(asset);
        }

        [HttpPost]
        public IActionResult Edit(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                var assets = _cache.Get<List<AssetModel>>(CacheKey);
                var existingAsset = assets.FirstOrDefault(a => a.Id == asset.Id);
                if (existingAsset != null)
                {
                    existingAsset.Name = asset.Name;
                    existingAsset.Description = asset.Description;
                    existingAsset.Category = asset.Category;
                    existingAsset.UnitOfMeasure = asset.UnitOfMeasure;
                    existingAsset.UpdatedDate = DateTime.Now;

                    _cache.Set(CacheKey, assets);
                }
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var assets = _cache.Get<List<AssetModel>>(CacheKey);
            var asset = assets.FirstOrDefault(a => a.Id == id);
            if (asset != null)
            {
                assets.Remove(asset);
                _cache.Set(CacheKey, assets);
            }
            return RedirectToAction("Index");
        }
    }
}