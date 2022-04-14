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
    public class MakalesController : Controller
    {
        private readonly AppDbContext _context;

        public MakalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Makales
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Makaleler.Include(m => m.Yazar);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Makales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makale = await _context.Makaleler
                .Include(m => m.Yazar)
                .FirstOrDefaultAsync(m => m.MakaleId == id);
            if (makale == null)
            {
                return NotFound();
            }

            return View(makale);
        }

        // GET: Admin/Makales/Create
        public IActionResult Create()
        {
            ViewData["YazarId"] = new SelectList(_context.Yazarlar, "YazarId", "Email");
            return View();
        }

        // POST: Admin/Makales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakaleId,Baslik,Icerik,OlusuturulmaZamani,GoruntulenmeSayisi,OnayliMi,YazarId")] Makale makale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(makale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["YazarId"] = new SelectList(_context.Yazarlar, "YazarId", "Email", makale.YazarId);
            return View(makale);
        }

        // GET: Admin/Makales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makale = await _context.Makaleler.FindAsync(id);
            if (makale == null)
            {
                return NotFound();
            }
            ViewData["YazarId"] = new SelectList(_context.Yazarlar, "YazarId", "Email", makale.YazarId);
            return View(makale);
        }

        // POST: Admin/Makales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MakaleId,Baslik,Icerik,OlusuturulmaZamani,GoruntulenmeSayisi,OnayliMi,YazarId")] Makale makale)
        {
            if (id != makale.MakaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(makale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakaleExists(makale.MakaleId))
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
            ViewData["YazarId"] = new SelectList(_context.Yazarlar, "YazarId", "Email", makale.YazarId);
            return View(makale);
        }

        // GET: Admin/Makales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makale = await _context.Makaleler
                .Include(m => m.Yazar)
                .FirstOrDefaultAsync(m => m.MakaleId == id);
            if (makale == null)
            {
                return NotFound();
            }

            return View(makale);
        }

        // POST: Admin/Makales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var makale = await _context.Makaleler.FindAsync(id);
            _context.Makaleler.Remove(makale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakaleExists(int id)
        {
            return _context.Makaleler.Any(e => e.MakaleId == id);
        }
    }
}
