using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCarHubApp.Data;

namespace TheCarHubApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CarPhotoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CarPhotoesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,CarDetailId,PhotoTitle,PhotoFile")] CarPhoto carPhoto)
        {
            if (ModelState.IsValid)
            {
                // Save photo to wwwroot/Image folder
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(carPhoto.PhotoFile.FileName);
                string extension = Path.GetExtension(carPhoto.PhotoFile.FileName);
                carPhoto.PhotoFilePath = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new  FileStream(path, FileMode.Create))
                {
                    await carPhoto.PhotoFile.CopyToAsync(fileStream);
                }

                // Insert record into database
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
