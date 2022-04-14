using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TamKafadan.Filters;
using TamKafadan.Models;

namespace TamKafadan.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Admin]
    public class KonuController : Controller
    {
        private readonly AppDbContext _context;

        public KonuController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Konu
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konular.ToListAsync());
        }

        // GET: Admin/Konu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konular
                .FirstOrDefaultAsync(m => m.KonuId == id);
            if (konu == null)
            {
                return NotFound();
            }

            return View(konu);
        }

        // GET: Admin/Konu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Konu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KonuId,KonuAdi")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(konu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konu);
        }

        // GET: Admin/Konu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konular.FindAsync(id);
            if (konu == null)
            {
                return NotFound();
            }
            return View(konu);
        }

        // POST: Admin/Konu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KonuId,KonuAdi")] Konu konu)
        {
            if (id != konu.KonuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(konu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonuExists(konu.KonuId))
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
            return View(konu);
        }

        // GET: Admin/Konu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konular
                .FirstOrDefaultAsync(m => m.KonuId == id);
            if (konu == null)
            {
                return NotFound();
            }

            return View(konu);
        }

        // POST: Admin/Konu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var konu = await _context.Konular.FindAsync(id);
            _context.Konular.Remove(konu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KonuExists(int id)
        {
            return _context.Konular.Any(e => e.KonuId == id);
        }
    }
}
