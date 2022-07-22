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
    [Authorize(Roles = "Administrator")]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CarsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// GET: Cars => Display the list of cars (from the cars table in the database) on the Index page.
        /// </summary>
        /// <returns>
        /// The Cars/Index view with the list of cars.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            var models = await _context.Cars.ToListAsync();

            // Get the Make Name (from CarMake table) and the Model Name (from CarModel table) for each car on the list
            for (int i = 0; i < models.Count; i++)
            {
                models[i].MakeName = _context.CarMakes.Where(x => x.Id == models[i].CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                models[i].ModelName = _context.CarModels.Where(x => x.Id == models[i].CarModelId).Select(y => y.ModelName).FirstOrDefault();
            }

            return View(models);
        }

        /// <summary>
        /// Addition of a research bar on the Home page to search cars by VIN, MakeName, ModelName or Year
        /// </summary>
        /// <param name="CarSearch"></param>
        /// <returns>
        /// The Home/Index view with the result of the Car search or the sorted list of cars
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Index(string CarSearch, string sortingCars)
        {
            ViewData["FindCar"] = CarSearch;

            ViewData["SoldCar"] = "SoldCar";
            ViewData["AvailableCar"] = "AvailableCar";
            ViewData["CarMakeNameASC"] = "MakeASC";
            ViewData["CarMakeNameDESC"] = "MakeDESC";
            ViewData["CarModelNameASC"] = "ModelASC";
            ViewData["CarModelNameDESC"] = "ModelDESC";
            ViewData["CarYearASC"] = "YearASC";
            ViewData["CarYearDESC"] = "YearDESC";
            ViewData["CarMilleageASC"] = "MilleageASC";
            ViewData["CarMilleageDESC"] = "MilleageDESC";
            ViewData["PurchasePriceASC"] = "PurchasePriceASC";
            ViewData["PurchasrPiceDESC"] = "PurchasPriceDESC";
            ViewData["RepairCostASC"] = "RepairCostASC";
            ViewData["RepairCostDESC"] = "RepairCostDESC";
            ViewData["SellingPriceASC"] = "SellingPriceASC";
            ViewData["SellingPriceDESC"] = "SellingPriceDESC";
            ViewData["PurchaseDateASC"] = "PurchaseDateASC";
            ViewData["PurchaseDateDESC"] = "PurchaseDateDESC";
            ViewData["LotDateASC"] = "LotDateASC";
            ViewData["LotDateDESC"] = "LotDateDESC";
            ViewData["SaleDateASC"] = "SaleDateASC";
            ViewData["SaleDateDESC"] = "SaleDateDESC";

            var CarQuery = from x in _context.Cars select x;

            // sort cars by their characteristics (Make, Model, Year, Sale price...)
            switch(sortingCars)
            {
                case "SoldCar":
                    CarQuery = CarQuery.Where(c => c.SaleDate != null);
                    break;
                case "AvailableCar":
                    CarQuery = CarQuery.Where(c => c.SaleDate == null);
                    break;
                case "MakeASC":
                    CarQuery = CarQuery.OrderBy(x => x.MakeName);
                    break;
                case "MakeDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.MakeName);
                    break;
                case "ModelASC":
                    CarQuery = CarQuery.OrderBy(x => x.ModelName);
                    break;
                case "ModelDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.ModelName);
                    break;
                case "YearASC":
                    CarQuery = CarQuery.OrderBy(x => x.Year);
                    break;
                case "YearDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.Year);
                    break;
                case "MilleageASC":
                    CarQuery = CarQuery.OrderBy(x => x.Milleage);
                    break;
                case "MilleageDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.Milleage);
                    break;
                case "PurchasePriceASC":
                    CarQuery = CarQuery.OrderBy(x => x.PurchasePrice);
                    break;
                case "PurchasePriceDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.PurchasePrice);
                    break;
                case "RepairCostASC":
                    CarQuery = CarQuery.OrderBy(x => x.RepairCost);
                    break;
                case "RepairCostDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.RepairCost);
                    break;
                case "SellingPriceASC":
                    CarQuery = CarQuery.OrderBy(x => x.SellingPrice);
                    break;
                case "SellingPriceDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.SellingPrice);
                    break;
                case "PurchaseDateASC":
                    CarQuery = CarQuery.OrderBy(x => x.PurchaseDate);
                    break;
                case "PurchaseDateDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.PurchaseDate);
                    break;
                case "LotDateASC":
                    CarQuery = CarQuery.OrderBy(x => x.LotDate);
                    break;
                case "LotDateDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.LotDate);
                    break;
                case "SaleDateASC":
                    CarQuery = CarQuery.OrderBy(x => x.SaleDate);
                    break;
                case "SaleDateDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.SaleDate);
                    break;
                default:
                    CarQuery = CarQuery.OrderByDescending(x => x.MakeName);
                    break;
            }

            if (!string.IsNullOrEmpty(CarSearch))
            {
                CarQuery = CarQuery.Where(x => x.VIN.Contains(CarSearch) || x.MakeName.Contains(CarSearch) || x.ModelName.Contains(CarSearch) || x.Year.ToString().Equals(CarSearch));
            }
            return View(await CarQuery.AsNoTracking().ToListAsync());
        }

            /// <summary>
            /// GET: Cars/Details/5 => Display details for the car with the specified Id.
            /// </summary>
            /// <param name="id"></param>
            /// <returns>
            /// The Cars/Detail/id view with the details for the specified car.
            /// </returns>
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);

            // Get all the photoes associated with the car of the specified id into a list
            var photos = await _context.CarPhotos.Where(p => p.CarId == id).ToListAsync();
            // Add the list to the ViewBag
            ViewBag.Photoes = photos;

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        /// <summary>
        /// GET: Cars/Create  => Display the form to Create a new car, using dropdown lists to select existing CarMakes, CarModels and Years.
        /// </summary>
        /// <returns>
        /// The Cars/Create view with the form to complete.
        /// </returns>
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

        /// <summary>
        /// POST: Cars/Create => Create a car using data from the form and save it into the database.
        /// To protect from overposting attacks, enable the specific properties you want to bind to.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// The Cars/Details view for the newly created car if the form is completed and valid, else stay on the Create page.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarMakeId,CarModelId,VIN,Year,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SellingPrice,SaleDate,Description,Milleage,Color,MakeName,ModelName,Photos")] CarCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get MakeName and ModelName from CarMakes and CarModels tables
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

                // Save the (list of) photo file(s) inside the CarPhotos table in the database
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

                        // update the photo's properties for the carPhoto object and add the photo to the list
                        carPhoto.PhotoName = uniqueFileName;
                        carPhoto.CarId = newcar.Id;
                        carPhoto.PhotoTitle = model.MakeName + "_" + model.ModelName + "_" + model.VIN + "-" + counter;
                        photos.Add(carPhoto);
                        counter++;
                    }
                }

                // Add the list of photoes to the Carphotos table in the database
                _context.CarPhotos.AddRange(photos);
                await _context.SaveChangesAsync();

                return RedirectToAction("details", new { id = newcar.Id });
            }

            return View(model);
        }

        /// <summary>
        /// GET: Cars/Edit/5 => Display the form to Edit the specified car.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The Cars/Edit/id view with the form to Edit the selected car.
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get all properties from the selected car in the datadabase
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            List<CarMake> carMakeList = new List<CarMake>();
            List<CarModel> carModelList = new List<CarModel>();

            // Fill the lists with all items from the CarMakes and CarModels tables, in order to fill the Select fields
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
            TempData["Photoes"] = photoes;

            return View(car);
        }

        /// <summary>
        /// POST: Cars/Edit/5 => Edit a car using data from the form and save it into the database.
        /// To protect from overposting attacks, enable the specific properties you want to bind to.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="car"></param>
        /// <returns>
        /// To the Cars/Index view if the form is completed and valid, else stay on the Edit page for the selected car.
        /// </returns>
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
                    // Get the list of photoes already associated with the current car, if there is one
                    List<CarPhoto> existingPhotoes = _context.CarPhotos.Where(p => p.CarId == id).ToList();
                    var nextphotoNumber = 0;
                    // Get MakeName and ModelName from CarMakes and CarModels tables
                    car.MakeName = _context.CarMakes.Where(x => x.Id == car.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                    car.ModelName = _context.CarModels.Where(x => x.Id == car.CarModelId).Select(y => y.ModelName).FirstOrDefault();

                    // Link newly uploaded photoes to the car and add them to the Images folder and to the database
                    string uniqueFileName = null;
                    List<CarPhoto> photos = new List<CarPhoto>();

                    if (car.CarPhotoes != null && car.CarPhotoes.Count > 0)
                    {
                        // If there already are photoes associated to that car
                        if (existingPhotoes.Count > 0)
                        {
                            // Get the number of the last photo associated with this car if there is one
                            IEnumerable<CarPhoto> query = _context.CarPhotos.Where(x => x.CarId == car.Id).OrderBy(photo => photo.Id);

                            var lastPhotoTitle = query.Last().PhotoTitle;
                            var LastphotoNumber = lastPhotoTitle.Substring(lastPhotoTitle.IndexOf("-") + 1);

                            // Increment that number to name the newly uploaded photo
                            nextphotoNumber = Convert.ToInt32(LastphotoNumber) + 1; 
                        }
                        else 
                        {
                            // If there was no photo previously associated to that car
                            nextphotoNumber = 1;
                        }

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
                        }
                    }

                    // Save the list of newly uploaded photoes into the CarPhotos table in the database
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


        /// <summary>
        /// GET: Cars/Delete/5 => Display the selected car to delete from the Cars table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The Cars/Delete view with all car information from the specified car.
        /// </returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the car with specified Id from the database
            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);

            car.MakeName = _context.CarMakes.Where(x => x.Id == car.CarMakeId).Select(y => y.MakeName).FirstOrDefault();
            car.ModelName = _context.CarModels.Where(x => x.Id == car.CarModelId).Select(y => y.ModelName).FirstOrDefault();

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        /// <summary>
        /// POST: Cars/Delete/5 => Delete a selected car from the Cars table, as well as its associated photoes (from Images folder and Carphotos table).
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// To the Cars/Index view.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get the car with specified Id from the database
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
        /// In the Edit Page, Delete a single car photo from the CarPhotos table and the Images folder. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// To the Cars/Edit view for the selected car => stay on the page until the form is saved.
        /// </returns>
        [HttpPost, ActionName("DeletePhoto")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            // Get the photo object associated with the Id from the parameters
            var Photo = _context.CarPhotos.Find(id);

            int carID = Photo.CarId;

            // Create the path for the file
            string PhotoFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", Photo.PhotoName);
            FileInfo file = new FileInfo(PhotoFilePath);
            // Delete the selected photo from the Images folder
            if (file.Exists)
            {
                file.Delete();
            }

            // Delete the selected photo from the CarPhotos table in the database
            _context.CarPhotos.Remove(Photo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = carID });
        }
         

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }


        /// <summary>
        /// Get the list of Car Models associated with a selected Make.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// The list of Car Models for the selected Make in a Json format.
        /// </returns>
        [HttpGet]
        public IActionResult FetchCarModel(int Id)
        {
            var data = _context.CarModels.Where(c => c.CarMakeId == Id).ToList();
            return Json(new { ModelList = data });
        }


        /// <summary>
        /// Create a limited list of years (from 1990 to the current year), to choose from in the Create and Edit pages
        /// </summary>
        /// <returns>
        /// A list of years
        /// </returns>
        public List<SelectListItem> GetYearRange()
        {
            int CurrentYear = int.Parse(DateTime.Now.Year.ToString());
            int FirstYear = 1990;
            var YearList = new List<SelectListItem>();
            // Add a default value on top of the list, to display in the Select field
            YearList.Insert(0, new SelectListItem { Text = "-- Select a Year --" });

            for (var i = FirstYear; i <= CurrentYear; i++)
            {
                YearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            return YearList;
        }
    }
}