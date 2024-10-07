using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<Car> carList = _context.Cars.ToList();

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
        public IActionResult Delete(int? id)
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
    }
}
