using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarDealerApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;   
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                car.LicencePlate = car.LicencePlate.ToUpper();
                car.Brand = car.Brand.ToUpper();
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction("Index");
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
    }
}
