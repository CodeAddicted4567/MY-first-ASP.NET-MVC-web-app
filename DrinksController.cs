using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrinksStoreManage.Data;
using DrinksStoreManage.Models;

namespace DrinksStoreManage.Controllers
{
    public class DrinksController : Controller
    {
        private readonly AppsDbContext _context;

        public DrinksController(AppsDbContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index(string searchStr,string findType,string sorts)//parameters will be used on viewModel properties and also used on URL
        {
            if(_context==null)
            {
                return Problem("The existing context is empty");
            }

            var drnk = from m in _context.Drinks
                       select m;
            //Create list of types for DropDown items
            IQueryable<string> q = from m in _context.Drinks
                                   orderby m.Type
                                   select m.Type;
            //example of searching
            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                drnk = drnk.Where(s => s.Name!.Replace(" ", "").ToLower().Contains(searchStr.Replace(" ", "").ToLower()));
            }
            //example of filtering
            if(!string.IsNullOrWhiteSpace(findType))
            {
                drnk = drnk.Where(m => m.Type == findType);
            }
            //example of sorting
            if(!string.IsNullOrWhiteSpace(sorts))
            { 
                drnk = drnk.OrderByDescending(m => m.Name); 
            }

            var drnkVM = new DrinksViewModel
            {
                Drinks = await drnk.ToListAsync(),//list show by search
                TypeList = new SelectList(await q.Distinct().ToListAsync()),//list show on dropdown 'Distinct' use for avoid duplocations
                sorts = await Task.FromResult(sorts)
            };
            

            return View(drnkVM);
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Qty,Type")] Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drinks);
                await _context.SaveChangesAsync();
                TempData["Created"] = "Item created Successfully!";
                return RedirectToAction("Index");
                //return RedirectToAction(nameof(Index));
            }
            return View(drinks);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks == null)
            {
                return NotFound();
            }
            return View(drinks);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Qty,Type")] Drinks drinks)
        {
            if (id != drinks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drinks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinksExists(drinks.Id))
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
            return View(drinks);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks != null)
            {
                _context.Drinks.Remove(drinks);
            }

            await _context.SaveChangesAsync();
            TempData["Deleted"] = "Item was Deleted!";
            return RedirectToAction("Index");
            //return RedirectToAction(nameof(Index));
        }

        private bool DrinksExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
