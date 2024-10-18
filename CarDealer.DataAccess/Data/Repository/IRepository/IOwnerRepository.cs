using CarDealerApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DataAccess.Data.Repository.IRepository
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Owner SearchOwner(string cardID);
    }
}
