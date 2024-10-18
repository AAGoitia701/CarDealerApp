using CarDealer.DataAccess.Data.Repository.IRepository;
using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerContext;

        public OwnerController(IOwnerRepository context)
        {
            _ownerContext = context;
        }
        public IActionResult Index()
        {
            List<Owner> ownerList = _ownerContext.GetAll(includeProperties:"ListCars").ToList();
            return View(ownerList);
        }

        public IActionResult Edit(int id)
        {
            Owner ownerObj = _ownerContext.GetOne(r => r.Id == id);
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
                _ownerContext.Update(owner);
                _ownerContext.Save();
                return RedirectToAction("Index", "Owner");

            }
            else
            {
                return View(); //has to be changed

            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Owner owner)
        {
            if (ModelState.IsValid) 
            {
                _ownerContext.Add(owner);
                _ownerContext.Save();
                return RedirectToAction("Index", "Owner");
            }
            else
            {
                return View();
            }
            
        }

        public IActionResult Delete(int id) 
        {
            Owner owner = _ownerContext.GetOne(r => r.Id==id);

            if (owner == null) 
            {
                return View("NotFound");
            }

           return View(owner);
        }
        [HttpPost]
        public IActionResult Delete(Owner owner)
        {
            if (owner == null)
            {
                return View("NotFound");
            }
            _ownerContext.Delete(owner);
            _ownerContext.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Search(string cardId)
        {
            Owner owner = _ownerContext.SearchOwner(cardId);
            if (owner == null) {
                return View("NotFound");
            }

            return View(owner);

        }
        
    }
}
