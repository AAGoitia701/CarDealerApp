using CarDealer.DataAccess.Data.Repository.IRepository;
using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Web.Helpers;

namespace CarDealerApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository _CarContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CarController(ICarRepository context, IWebHostEnvironment webHostEnvironment)
        {
            _CarContext = context;   
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Car> carList = _CarContext.GetAll(includeProperties:"Owner").ToList();

            return View(carList);
        }

        public IActionResult Edit(int id) 
        {
            Car car = _CarContext.GetOne(r => r.CarId == id, includeProperties:"Owner");
            Console.WriteLine($"ImgUrl before update: {car.ImgUrl}");
            if (car == null)
            {
                return NotFound();
            }

            return View(car); 
        }

        [HttpPost]
        public IActionResult Edit(Car car, IFormFile? file) 
        {

            try
            {
                if (ModelState.IsValid)
                {
                    car.LicencePlate = car.LicencePlate.ToUpper();
                    car.Brand = car.Brand.ToUpper();

                    // if there is an img, it will manage it
                    if (file != null)
                    {
                        car.ImgUrl = SaveImage(file, car);
                    }

                    _CarContext.Update(car);
                    _CarContext.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    ModelState.AddModelError("OwnerId", "The Owner ID is not valid");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Opps! An Error Ocurred.");
                }
            }
            return View();
        }



        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        public IActionResult Create(Car car, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    car.LicencePlate = car.LicencePlate.ToUpper();
                    car.Brand = car.Brand.ToUpper();

                    //save image
                    SaveImage(file, car);

                    _CarContext.Add(car);
                    _CarContext.Save();
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (DbUpdateException ex) 
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    ModelState.AddModelError("OwnerId", "The Owner ID is not valid");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Opps! An Error Ocurred.");
                }
            }

            return View();

        }



        /*
        public IActionResult Delete(int id)
        {
            Car car = _context.Cars.Where(r => r.CarId == id).FirstOrDefault();
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        */
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            Car car = _CarContext.GetOne(r => r.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            _CarContext.Delete(car);
            _CarContext.Save();
            TempData["success"] = "You have deleted the car successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string plate, string Brand)
        {
            if (string.IsNullOrEmpty(plate) && string.IsNullOrEmpty(Brand))
            {
                return View("NotFound");
            }

           var car = _CarContext.Search(plate, Brand);

            if (!car.Any())
            {
                return View("NotFound"); // O cualquier otra vista que desees mostrar
            }
            if (car.Count() == 1)
            {
                return View("Search", car.First()); // Si solo hay un coche, pasa el coche a la vista
            }

            return View("SearchBrand", car); // Si hay varios coches, pasa la lista a la vista;
        }
        /*
        public IActionResult SearchBrand(string brand)
        {
            if (brand == null)
            {
                return View("NotFound");
            }

            IEnumerable<Car> listCar = _context.Cars.Where(r => r.Brand == brand).ToList();
            return View(listCar);

        }*/

        [HttpPost]
        private string SaveImage(IFormFile? file, Car car)
        {
            var img = _CarContext.SaveImg(file, car);

            return img;
        }



        [HttpGet]
        public JsonResult GetOwners(string term)
        {
            // Filtra los dueños que contengan el término de búsqueda
            Console.WriteLine("Término de búsqueda: " + term);  // Verifica el término que llega al backend

            var matchedOwners = _CarContext.GetOwners(term); 
            Console.WriteLine(matchedOwners.Count());  // Verifica cuántos dueños se encontraron

            // Retorna como una lista de DTOs si es necesario
            var ownerDtos = matchedOwners.Select(o => new { label = o.FullName, value = o.Id }).ToList();

            return Json(ownerDtos);
        }
    }
}
