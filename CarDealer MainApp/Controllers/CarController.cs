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
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CarController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;   
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Car> carList = _context.Cars.Include(r => r.Owner).ToList();

            return View(carList);
        }

        public IActionResult Edit(int id) 
        {
            Car car = _context.Cars.Where(r => r.CarId.Equals(id)).FirstOrDefault();
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

                    _context.Cars.Update(car);
                    _context.SaveChanges();
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
                    
                    _context.Cars.Add(car);
                    _context.SaveChanges();
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
            Car car = _context.Cars.Where(r => r.CarId == id).FirstOrDefault();
            if (car == null)
            {
                return NotFound();
            }
            _context.Remove(car);
            _context.SaveChanges();
            TempData["success"] = "You have deleted the car successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string plate, string Brand)
        {
            try
            {
                
                if (string.IsNullOrEmpty(plate) && string.IsNullOrEmpty(Brand))
                {
                    return View("NotFound");
                }

                    if (!(string.IsNullOrEmpty(plate)))
                    {
                        plate = plate.ToUpper();
                        Car car = _context.Cars.Where(r => r.LicencePlate == plate).Include(r => r.Owner).FirstOrDefault();
                            if(car == null)
                        {
                            return View("NotFound");
                        }           
                        return View(car);   
                    }

                    if (!(string.IsNullOrEmpty(Brand)))
                    {
                        Brand = Brand.ToUpper();
                        IEnumerable<Car> car = _context.Cars.Where(r => r.Brand == Brand).Include(r => r.Owner).ToList();
                        if (car == null )
                        {
                            return View("NotFound");
                        }
                        return View("SearchBrand", car);
                    }


                return View("NotFound");

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return NoContent();
            }
            
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
            string fileName = "";
            string productPath = "";
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);//creating name for file uploaded
                productPath = Path.Combine(wwwRootPath, @"images"); //getting the route of the right folder

                if (!string.IsNullOrEmpty(car.ImgUrl)) //check if there is an imageUrl in the folder for this particular product
                {
                    //delete old img
                    var oldImagePath = Path.Combine(wwwRootPath, car.ImgUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath)) //check if that img exists
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                car.ImgUrl = @"/images/" + fileName;
            }

            return car.ImgUrl;
        }



        [HttpGet]
        public JsonResult GetOwners(string term)
        {
            // Filtra los dueños que contengan el término de búsqueda
            Console.WriteLine("Término de búsqueda: " + term);  // Verifica el término que llega al backend

            var matchedOwners = _context.Owners
                .Where(o => EF.Functions.Like(o.FullName, $"%{term}%"))
                .Select(o => new { label = o.FullName, value = o.Id })
                .ToList();

            Console.WriteLine("Dueños encontrados: " + matchedOwners.Count);  // Verifica cuántos dueños se encontraron

            return Json(matchedOwners);
        }
    }
}
