using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
