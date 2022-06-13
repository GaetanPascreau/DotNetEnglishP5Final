using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCarHubApp.Data;

namespace TheCarHubApp.Controllers
{
    public class CarPhotoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarPhotoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarPhotoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarPhotos.ToListAsync());
        }

        // GET: CarPhotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPhoto = await _context.CarPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carPhoto == null)
            {
                return NotFound();
            }

            return View(carPhoto);
        }

        // GET: CarPhotoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarDetailId,PhotoTitle,PhotoFilePath")] CarPhoto carPhoto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carPhoto);
        }

        // GET: CarPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPhoto = await _context.CarPhotos.FindAsync(id);
            if (carPhoto == null)
            {
                return NotFound();
            }
            return View(carPhoto);
        }

        // POST: CarPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarDetailId,PhotoTitle,PhotoFilePath")] CarPhoto carPhoto)
        {
            if (id != carPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarPhotoExists(carPhoto.Id))
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
            return View(carPhoto);
        }

        // GET: CarPhotoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPhoto = await _context.CarPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carPhoto == null)
            {
                return NotFound();
            }

            return View(carPhoto);
        }

        // POST: CarPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carPhoto = await _context.CarPhotos.FindAsync(id);
            _context.CarPhotos.Remove(carPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarPhotoExists(int id)
        {
            return _context.CarPhotos.Any(e => e.Id == id);
        }
    }
}
