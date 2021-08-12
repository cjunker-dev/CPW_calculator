using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CostPerWear.Data;
using CostPerWear.Models;

namespace CostPerWear.Controllers
{
    public class ClothingItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClothingItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClothingItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClothingItem.ToListAsync());
        }
        
        // GET: ClothingItems/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: ClothingItems/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.ClothingItem.Where
                (j => j.Name.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: ClothingItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }

        // GET: ClothingItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClothingItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Cost,Wears,CostPerWear")] ClothingItem clothingItem)
        {
            if (ModelState.IsValid)
            {
                if (clothingItem.Wears != 0)
                {
                    clothingItem.CostPerWear = CalculateCPW(clothingItem.Cost, clothingItem.Wears);
                }
                _context.Add(clothingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clothingItem);
        }

        // GET: ClothingItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem.FindAsync(id);
            if (clothingItem == null)
            {
                return NotFound();
            }
            return View(clothingItem);
        }

        // POST: ClothingItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Cost,Wears,CostPerWear")] ClothingItem clothingItem)
        {
            if (id != clothingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (clothingItem.Wears != 0)
                    {
                        clothingItem.CostPerWear = CalculateCPW(clothingItem.Cost, clothingItem.Wears);
                    }
                    _context.Update(clothingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingItemExists(clothingItem.Id))
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
            return View(clothingItem);
        }

        // GET: ClothingItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }

        // POST: ClothingItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clothingItem = await _context.ClothingItem.FindAsync(id);
            _context.ClothingItem.Remove(clothingItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClothingItemExists(int id)
        {
            return _context.ClothingItem.Any(e => e.Id == id);
        }

        private double CalculateCPW(double Cost, int Wears)
        {
            double CostPerWear = Cost / Wears;
            return CostPerWear;
        }
    }
}
