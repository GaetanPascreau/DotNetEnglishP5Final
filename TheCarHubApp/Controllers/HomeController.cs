using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheCarHubApp.Data;
using TheCarHubApp.Models;

namespace TheCarHubApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Display the list of available car on the Home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var models = await _context.Cars.Where(x => x.SaleDate == null).ToListAsync();
            // Get the Make Name and the Model Name for each car on the list by using the foreign keys carMakeId and carModelId
            for (int i = 0; i < models.Count; i++)
            {
                models[i].MakeName = _context.CarMakes.Where(x => x.Id == models[i].CarMakeId).Select(y => y.MakeName).FirstOrDefault();
                models[i].ModelName = _context.CarModels.Where(x => x.Id == models[i].CarModelId).Select(y => y.ModelName).FirstOrDefault();
            }
            return View(models);
        }

        /// <summary>
        /// Addition of a research bar on the Home page to search cars by MakeName, ModelName or Year + sorting options to display cars list
        /// </summary>
        /// <param name="CarSearch"></param>
        /// <returns>
        /// The Home/Index view with the result of the Car search or the sorted list of cars
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Index(string CarSearch, string sortingCars)
        {
            ViewData["FindCar"] = CarSearch;

            ViewData["CarMakeNameASC"] = "MakeASC";
            ViewData["CarMakeNameDESC"] = "MakeDESC";
            ViewData["CarModelNameASC"] = "ModelASC";
            ViewData["CarModelNameDESC"] = "ModelDESC";
            ViewData["CarYearASC"] = "YearASC";
            ViewData["CarYearDESC"] = "YearDESC";
            ViewData["CarMilleageASC"] = "MilleageASC";
            ViewData["CarMilleageDESC"] = "MilleageDESC";
            ViewData["CarPriceASC"] = "PriceASC";
            ViewData["CarPriceDESC"] = "PriceDESC";

            var CarQuery = _context.Cars.Where(c => c.SaleDate == null);

            if (!string.IsNullOrEmpty(sortingCars))
            {
                if (!string.IsNullOrEmpty(CarSearch))
                {
                    CarQuery = CarQuery.Where(x => x.MakeName.Contains(CarSearch) || x.ModelName.Contains(CarSearch) || x.Year.ToString().Equals(CarSearch));
                    CarQuery = SortList(CarQuery, sortingCars);
                }
                else
                {
                    CarQuery = SortList(CarQuery, sortingCars);
                }
                
            }
            else
            {
                if (!string.IsNullOrEmpty(CarSearch))
                {
                    CarQuery = CarQuery.Where(x => x.MakeName.Contains(CarSearch) || x.ModelName.Contains(CarSearch) || x.Year.ToString().Equals(CarSearch));
                }
            }
            return View(await CarQuery.AsNoTracking().ToListAsync());
        }

        /// <summary>
        ///  GET: Home/Details/id => Display details for the selected car, without options to edit or delete the car
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The Home/Details/id view for the selected car
        /// </returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);

            // Get all the photos associated with the car of the given id into a list
            var photos = await _context.CarPhotos.Where(p => p.CarId == id).ToListAsync();
            ViewBag.Photoes = photos;

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["API_KEY"] = "AIzaSyBfVYH1kT7ouXSc1BOeCwiDrZ2kagqyuyE";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IQueryable<Car> SortList(IQueryable<Car> CarQuery, string sortingCars)
        {
            switch (sortingCars)
            {
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
                case "PriceASC":
                    CarQuery = CarQuery.OrderBy(x => x.SellingPrice);
                    break;
                case "PriceDESC":
                    CarQuery = CarQuery.OrderByDescending(x => x.SellingPrice);
                    break;
                default:
                    CarQuery = CarQuery.OrderByDescending(x => x.MakeName);
                    break;
            }
            return CarQuery;
        }
    }
}
