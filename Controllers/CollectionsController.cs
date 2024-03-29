﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviePro.Data;
using MoviePro.Models.Database;
using MoviePro.Models.Settings;

namespace MoviePro.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public CollectionsController(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        // GET: Collections
        [Authorize(Roles = "Administrator, DemoAdmin")]
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Collection.ToListAsync());

            var defaultCollectionName = _appSettings.MovieProSettings.DefaultCollection.Name;
            var collections = await _context.Collection.Where(c => c.Name != defaultCollectionName).ToListAsync();
            // now the view has access to all of the collections except the default All collection
            return View(collections);
        }


        // POST: Collections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Collection collection)
        { 
            _context.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","MovieCollections", new { id = collection.Id });
        }

        // GET: Collections/Edit/5
        [Authorize(Roles = "Administrator, DemoAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Collection collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // make sure collection is not the All collection
                    if (collection.Name == _appSettings.MovieProSettings.DefaultCollection.Name)
                    {
                        return RedirectToAction("Index","Collections");
                    }

                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        [Authorize(Roles = "Administrator, DemoAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            // Collection title for View
            var collection = _context.Collection.FirstOrDefault(c => c.Id == id).Name;
            ViewData["CollectionTitle"] = collection;

            var movieIdsInCollection = await _context.MovieCollection
                                                   .Where(m => m.CollectionId == id)
                                                   .OrderBy(m => m.Order)
                                                   .Select(m => m.MovieId)
                                                   .ToListAsync();

            var moviesInCollection = new List<Movie>();
            // For each id in collection add to new list
            movieIdsInCollection.ForEach(movieId => moviesInCollection.Add(_context.Movie.Find(movieId)));
            
            return View(moviesInCollection);
        }

        // GET: Collections/Delete/5
        [Authorize(Roles = "Administrator, DemoAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            if (collection.Name == _appSettings.MovieProSettings.DefaultCollection.Name)
            {
                return RedirectToAction("Index", "Collections");
            }

            return View(collection);
        }

        // POST: Collections/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collection = await _context.Collection.FindAsync(id);
            _context.Collection.Remove(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Collections");
        }

        private bool CollectionExists(int id)
        {
            return _context.Collection.Any(e => e.Id == id);
        }
    }
}
