using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarDealerApp.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OwnerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Owner> ownerList = _context.Owners.ToList();
            return View(ownerList);
        }

        public IActionResult Edit(int id)
        {
            Owner ownerObj = _context.Owners.Where(r => r.Id == id).FirstOrDefault();
            if (ownerObj == null)
            {
                return View("NotFound");
            }
            return View(ownerObj);

        }

        [HttpPost]
        public IActionResult Edit(Owner owner)
        {
            if (ModelState.IsValid) 
            {
                _context.Owners.Update(owner);
                _context.SaveChanges();
                return RedirectToAction("Index", "Owner");

            }
            else
            {
                return View("NotFound"); //has to be changed

            }
        }
    }
}
