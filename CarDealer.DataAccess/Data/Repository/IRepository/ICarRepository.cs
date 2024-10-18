using CarDealerApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DataAccess.Data.Repository.IRepository
{
    public interface ICarRepository : IRepository<Car>
    {
        IEnumerable<Car> Search(string plate, string brand);

        String SaveImg(IFormFile? file, Car car);

        IEnumerable<Owner> GetOwners(string term);
    }
}
