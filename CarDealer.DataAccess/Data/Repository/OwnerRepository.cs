using CarDealer.DataAccess.Data.Repository.IRepository;
using CarDealerApp.Data;
using CarDealerApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DataAccess.Data.Repository
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        ApplicationDbContext _context;
        public OwnerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Owner SearchOwner(string cardID)
        {
            if (string.IsNullOrEmpty(cardID))
            {
                throw new ArgumentException("Card ID cannot be null or empty", nameof(cardID));
            }
            Owner owner = _context.Owners.Where(r => r.CardId == cardID).FirstOrDefault();

            return owner;
        }
    }
}
