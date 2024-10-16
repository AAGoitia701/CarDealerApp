using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
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
            if (car == null)
            {
                return NotFound();
            }

            return View(car); 
        }

        [HttpPost]
        public IActionResult Edit(Car car) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    car.LicencePlate = car.LicencePlate.ToUpper();
                    car.Brand = car.Brand.ToUpper();
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

                    // if there is an img, it will manage it
                    if (file != null)
                    {
                        car.ImgUrl = SaveImage(file, car.ImgUrl);
                    }

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
        private string SaveImage(IFormFile file, string? currentImgUrl)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName); // unique name
            string productPath = Path.Combine(wwwRootPath, "images");

            // delete the img if it already exists
            if (!string.IsNullOrEmpty(currentImgUrl))
            {
                string oldImagePath = Path.Combine(wwwRootPath, currentImgUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            // create directory if it doesn't exist
            if (!Directory.Exists(productPath))
                Directory.CreateDirectory(productPath);

            // save new img
            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return $"/images/{fileName}"; // return new url for img
        }
    }
}
