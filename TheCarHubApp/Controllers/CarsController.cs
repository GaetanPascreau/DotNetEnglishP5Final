using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using javax.jws;
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

            // Get all the photos associated with the car of the given id into a list
            var photos = await _context.CarPhotos.Where(p => p.CarId == id).ToListAsync();
            ViewBag.Photoes = photos;

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

            ViewBag.YearList = GetYearRange();

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
                // Get MakeName and ModelName using the foreign keys in Cars table
                model.MakeName = _context.CarMakes.Where(x => x.Id == model.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                model.ModelName = _context.CarModels.Where(x => x.Id == model.CarModelId).Select(y => y.ModelName).FirstOrDefault();

                // Create a new instance of Car to save into the Cars table in the database
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
                };

                _context.Cars.Add(newcar);
                await _context.SaveChangesAsync();

                // Save the (list of) photo file name(s) inside the CarPhotos table in the database
                string uniqueFileName = null;
                List<CarPhoto> photos = new List<CarPhoto>();
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    int counter = 1;
                    foreach (IFormFile photo in model.Photos)
                    {
                        CarPhoto carPhoto = new CarPhoto();
                        // Add a path to the upload folder (= Images folder)
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                        // Add a unique identifier at the begining of the file Name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        //Create the path for the file 
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Upload the file into the Images Folder
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            photo.CopyTo(fileStream);
                        }

                        carPhoto.PhotoName = uniqueFileName;
                        carPhoto.CarId = newcar.Id;
                        carPhoto.PhotoTitle = model.MakeName + "_" + model.ModelName + "_" + model.VIN + "-" + counter;
                        photos.Add(carPhoto);
                        counter++;
                    }
                }

                _context.CarPhotos.AddRange(photos);
                await _context.SaveChangesAsync();

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

            // Add the lists to the ViewBag
            ViewBag.ListofCarMake = carMakeList;
            ViewBag.ListofCarModel = carModelList;
            ViewBag.YearList = GetYearRange();

            // Get all the photoes associated with the car of the given id into a list
            var photoes = await _context.CarPhotos.Where(p => p.CarId == id).ToListAsync();
            ViewBag.Photoes = photoes;

            return View(car);
        }


        [Authorize(Roles = "Administrator")]
        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarMakeId,CarModelId,VIN,Year,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SellingPrice,SaleDate,Description,Milleage,Color,MakeName,ModelName,PhotoPath,CarPhotoes")] Car car)
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

                    // Link newly uploaded photoes to the car and add them to the Images folder and to the database
                    string uniqueFileName = null;
                    List<CarPhoto> photos = new List<CarPhoto>();

                    if (car.CarPhotoes != null && car.CarPhotoes.Count > 0)
                    {
                        // Get the number of the last photo associated with this car
                        IEnumerable<CarPhoto> query = _context.CarPhotos.OrderBy(photo => photo.Id);
                        var lastPhotoTitle = query.Last().PhotoTitle;
                        var LastphotoNumber = lastPhotoTitle.Substring(lastPhotoTitle.IndexOf("-") + 1);
                        // Increment that number to name the newly uploaded photo
                        var nextphotoNumber = Convert.ToInt32(LastphotoNumber) + 1;

                        foreach (IFormFile photo in car.CarPhotoes)
                        {
                            CarPhoto carPhoto = new CarPhoto();
                            // Add a path to the upload folder (= Images folder)
                            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                            // Add a unique identifier at the begining of the file Name
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                            //Create the path for the file 
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            // Upload the file into the Images Folder
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                photo.CopyTo(fileStream);
                            }

                            carPhoto.PhotoName = uniqueFileName;
                            carPhoto.CarId = car.Id;
                            carPhoto.PhotoTitle = car.MakeName + "_" + car.ModelName + "_" + car.VIN + "-" + nextphotoNumber;
                            photos.Add(carPhoto);
                            nextphotoNumber++;
                            Console.WriteLine(nextphotoNumber);
                        }
                    }

                    _context.CarPhotos.AddRange(photos);
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

            // Remove Photos associated to the car from the CarPhotos table
            var carAssociatedPhotos = _context.CarPhotos.Where(x => x.CarId == car.Id);
            _context.CarPhotos.RemoveRange(carAssociatedPhotos);

            // Remove photos associated to the car from the Images folder
            foreach (var photo in carAssociatedPhotos)
            {
                // Add a path to the upload folder (= Images folder)
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                //Create the path for the file 
                string filePath = Path.Combine(uploadsFolder, photo.PhotoName);
                FileInfo file = new FileInfo(filePath);

                if (file.Exists)
                {
                    file.Delete();
                }
            }

            // Remove the car from the Cars table
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Method to delete a single car photo from the Edit Page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeletePhoto")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            // Get the photo object associated with the Id from the parameters
            var Photo = _context.CarPhotos.Find(id);

            int carID = Photo.CarId;

            // Delete the selected photo from the Images folder
            // Create the path for the file
            string PhotoFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", Photo.PhotoName);
            FileInfo file = new FileInfo(PhotoFilePath);
            if (file.Exists)
            {
                file.Delete();
            }

            // Delete the selected photo from the CarPhotos table in the database
            _context.CarPhotos.Remove(Photo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { carID });

        }


        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }


        /// <summary>
        /// Method to get the list of Car Model associated with a selected Make
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult FetchCarModel(int Id)
        {
            var data = _context.CarModels
                .Where(c => c.CarMakeId == Id).ToList()
                ;
            return Json(new { ModelList = data });
        }


        /// <summary>
        /// Create a limited list of years (from 1990 to the current year)
        /// to choose from in the Create and Edit pages
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetYearRange()
        {
            int CurrentYear = int.Parse(DateTime.Now.Year.ToString());
            int FirstYear = 1990;
            var YearList = new List<SelectListItem>();
            YearList.Insert(0, new SelectListItem { Text = "-- Select a Year --" });

            for (var i = FirstYear; i <= CurrentYear; i++)
            {
                YearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            return YearList;
        }
    }
}