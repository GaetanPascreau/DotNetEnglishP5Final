using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCarHubApp.Data;
using TheCarHubApp.ViewModels;

namespace TheCarHubApp.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var models = await _context.Cars.ToListAsync();
            // Get the Make Name and the Model Name for each car on the list by using the foreign keys carMakeId and carModelId
            for (int i = 0; i < models.Count; i++)
            {
                models[i].MakeName = _context.CarMakes.Where(x => x.Id == models[i].CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                models[i].ModelName = _context.CarModels.Where(x => x.Id == models[i].CarModelId).Select(y => y.ModelName).FirstOrDefault();
            }
            return View(models);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);

            car.MakeName = _context.CarMakes.Where(x => x.Id == car.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
            car.ModelName = _context.CarModels.Where(x => x.Id == car.CarModelId).Select(y => y.ModelName).FirstOrDefault();


            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }


        [Authorize(Roles = "Administrator")]
        // GET: Cars/Create  MODIFIED with addition of a dropdown lists for CarMakes and CarModels
        public IActionResult Create()
        {
            // Populate the 1st Select options, for Car Makes
            List<CarMake> carMakeList = new List<CarMake>();
            // Fill the list with all items from the CarMakes table
            carMakeList = (from c in _context.CarMakes select c).ToList();
            // At the top of the list, add the text to display on the select box
            carMakeList.Insert(0, new CarMake { Id = 0, MakeName = "-- Select a car make --" });     
            // Add the list to the ViewBag
            ViewBag.ListofCarMake = carMakeList;
          
            // Create a limited list of year (from 1990 to the current year) to choose from
            int CurrentYear = int.Parse(DateTime.Now.Year.ToString());
            int FirstYear = 1990;
            var YearList = new List<SelectListItem>();
            YearList.Insert(0, new SelectListItem { Text = "-- Select a Year --" });

            for(var i = FirstYear; i<= CurrentYear; i++)
            {
                YearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.YearList = YearList;

            return View();
        }

        [Authorize(Roles = "Administrator")]
        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarMakeId,CarModelId,VIN,Year,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SellingPrice,SaleDate,Description,Milleage,Color,MakeName,ModelName,Photos")] CarCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photos != null && model.Photos.Count >0)
                {
                    foreach(IFormFile photo in model.Photos)
                    {
                        // Add a path to the upload folder (= Images folder)
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                        // Add a unique identifier at the begining of the file Name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        //Create the path for the file 
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Upload the file into the Images Folder
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    
                }

                model.MakeName = _context.CarMakes.Where(x => x.Id == model.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                model.ModelName = _context.CarModels.Where(x => x.Id == model.CarModelId).Select(y => y.ModelName).FirstOrDefault();

                Car newcar = new Car
                {
                    CarMakeId = model.CarMakeId,
                    CarModelId = model.CarModelId,
                    VIN = model.VIN,
                    Year = model.Year,
                    Trim = model.Trim,
                    PurchaseDate = model.PurchaseDate,
                    PurchasePrice = model.PurchasePrice,
                    Repairs = model.Repairs,
                    RepairCost = model.RepairCost,
                    LotDate = model.LotDate,
                    SellingPrice = model.SellingPrice,
                    SaleDate = model.SaleDate,
                    Description = model.Description,
                    Milleage = model.Milleage,
                    Color = model.Color,
                    MakeName = model.MakeName,
                    ModelName = model.ModelName,
                    PhotoPath = uniqueFileName
                };
 
                _context.Add(newcar);

                //CarPhoto newCarPhoto = new CarPhoto
                //{
                //    // create a new CarPhoto and save it into the database
                //    PhotoTitle = uniqueFileName
                //};
                //_context.Add(newCarPhoto);

                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("details", new { id = newcar.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            List<CarMake> carMakeList = new List<CarMake>();
            List<CarModel> carModelList = new List<CarModel>();

            // Fill the lists with all items from the CarMakes and CarModels tables
            carMakeList = (from c in _context.CarMakes select c).ToList();
            carModelList = (from c in _context.CarModels select c).ToList();

            // At the top of the lists, add the text to display on the select box
            carMakeList.Insert(0, new CarMake { Id = 0, MakeName = "-- Select a car make --" });
            carModelList.Insert(0, new CarModel { Id = 0, ModelName = "-- Select a car model --" });

            // Add the list to the ViewBag
            ViewBag.ListofCarMake = carMakeList;
            ViewBag.ListofCarModel = carModelList;

            //CarCreateViewModel carCreateViewModel = new CarCreateViewModel
            //{
            //    CarMakeId = car.CarMakeId,
            //    CarModelId = car.CarModelId,
            //    VIN = car.VIN,
            //    Year = car.Year,
            //    Trim = car.Trim,
            //    PurchaseDate = car.PurchaseDate,
            //    PurchasePrice = car.PurchasePrice,
            //    Repairs = car.Repairs,
            //    RepairCost = car.RepairCost,
            //    LotDate = car.LotDate,
            //    SellingPrice = car.SellingPrice,
            //    SaleDate = car.SaleDate,
            //    Description = car.Description,
            //    Milleage = car.Milleage,
            //    Color = car.Color,
            //    MakeName = car.MakeName,
            //    ModelName = car.ModelName,
            //    //Photo = new FileStream(car.PhotoPath, FileMode.Append)
            //    Photo = _context.CarPhotos.Where(x => x.PhotoFile.FileName == car.PhotoPath).Select(y => y.PhotoFile).FirstOrDefault()
            //};

            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarMakeId,CarModelId,VIN,Year,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SellingPrice,SaleDate,Description,Milleage,Color,MakeName,ModelName,PhotoPath")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    car.MakeName = _context.CarMakes.Where(x => x.Id == car.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                    car.ModelName = _context.CarModels.Where(x => x.Id == car.CarModelId).Select(y => y.ModelName).FirstOrDefault();

                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);

            car.MakeName = _context.CarMakes.Where(x => x.Id == car.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
            car.ModelName = _context.CarModels.Where(x => x.Id == car.CarModelId).Select(y => y.ModelName).FirstOrDefault();

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }                                                                                              

        // Method to delete a car photo from the Edit Page
        [Authorize(Roles = "Administrator")]
        // POST: Cars/Edit/5
        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            car.PhotoPath = null;
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id });
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        // Method to get the list of Car Model associated with a selected Make
        [HttpGet]
        public IActionResult FetchCarModel(int Id)
        {
            var data = _context.CarModels
                .Where(c => c.CarMakeId == Id).ToList()
                ;
            return Json(new {ModelList =  data});
        }


    }
}
