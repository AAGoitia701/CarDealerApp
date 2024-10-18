using CarDealer.DataAccess.Data.Repository.IRepository;
using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DataAccess.Data.Repository
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Owner> GetOwners(string term)
        {
            var matchedOwners = _context.Owners
            .Where(o => EF.Functions.Like(o.FullName, $"%{term}%"))
            .ToList();

            Console.WriteLine("Dueños encontrados: " + matchedOwners.Count);

            return matchedOwners;
        }

        public string SaveImg(IFormFile? file, Car car)
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

        public IEnumerable<Car> Search(string plate, string Brand)
        {
            try
            {
                if (!(string.IsNullOrEmpty(plate)))
                {
                    plate = plate.ToUpper();
                    Car car = _context.Cars.Where(r => r.LicencePlate == plate).Include(r => r.Owner).FirstOrDefault();

                    if (car == null)
                    {
                        return Enumerable.Empty<Car>();
                    }
                    return new List<Car> { car};
                }

                if (!(string.IsNullOrEmpty(Brand)))
                {
                    Brand = Brand.ToUpper();
                    IEnumerable<Car> car = _context.Cars.Where(r => r.Brand == Brand).Include(r => r.Owner).ToList();
                    if (car == null)
                    {
                        // Devuelve una lista vacía si no se encuentran coches
                        return Enumerable.Empty<Car>();
                    }
                    return car;
                }


                return Enumerable.Empty<Car>();

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw new Exception("An error occurred while searching for cars.", err);
            }
        }
    }
}
