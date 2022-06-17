using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCarHubApp.Data;

namespace TheCarHubApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CarModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarModels
        public async Task<IActionResult> Index()
        {
            var models = await _context.CarModels.ToListAsync();
            // Get the Make Name for each model on the list by using the foreign key carMakeId
            for (int i = 0; i < models.Count; i++)
            {
                models[i].MakeName = _context.CarMakes.Where(x => x.Id == models[i].CarMakeId).Select(y => y.MakeName).FirstOrDefault();
            }
            return View(models);
        }

        // GET: CarModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels
                .FirstOrDefaultAsync(m => m.Id == id);

            carModel.MakeName = _context.CarMakes.Where(x => x.Id == carModel.CarMakeId).Select(y => y.MakeName).FirstOrDefault();

            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        //// GET: CarModels/Create ORIGINAL
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // GET: CarModels/Create MODIFIED with addition of a dropdown list for CarMakes
        public IActionResult Create()
        {
            List<CarMake> carMakeList = new List<CarMake>();
            // Fill the list with all items from the CarMakes table
            carMakeList = (from c in _context.CarMakes select c).ToList();
            // At the top of the list, add the text to display on the select box
            carMakeList.Insert(0, new CarMake { Id = 0, MakeName = "-- Select a car make --" });
            // Add the list to the ViewBag
            ViewBag.ListofCarMake = carMakeList;
            return View();
        }


        // POST: CarModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarMakeId,ModelName,MakeName")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                carModel.MakeName = _context.CarMakes.Where(x => x.Id == carModel.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                
                if(carModel.MakeName == null)
                {
                    ViewBag.SelectedMake = carModel.MakeName;
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    ViewBag.SelectedMake = carModel.MakeName;
                    _context.Add(carModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }      
            }
            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }

            List<CarMake> carMakeList = new List<CarMake>();
            // Fill the list with all items from the CarMakes table
            carMakeList = (from c in _context.CarMakes select c).ToList();
            // At the top of the list, add the text to display on the select box
            carMakeList.Insert(0, new CarMake { Id = 0, MakeName = "-- Select a car make --" });
            // Add the list to the ViewBag
            ViewBag.ListofCarMake = carMakeList;

            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarMakeId,ModelName")] CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carModel.MakeName = _context.CarMakes.Where(x => x.Id == carModel.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                    _context.Update(carModel);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarModelExists(carModel.Id))
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
            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels
                .FirstOrDefaultAsync(m => m.Id == id);

            carModel.MakeName = _context.CarMakes.Where(x => x.Id == carModel.CarMakeId).Select(y => y.MakeName).FirstOrDefault();

            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carModel = await _context.CarModels.FindAsync(id);
            _context.CarModels.Remove(carModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarModelExists(int id)
        {
            return _context.CarModels.Any(e => e.Id == id);
        }
    }
}
