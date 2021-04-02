using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellersService
    {
        private readonly SalesWebMvcContext _context;

        public SellersService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Sellers.ToList();
        }

        public void Insert(Seller seller)
        {
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Sellers.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
        }

        public void Remove(int id)
        {
            Seller seller = _context.Sellers.Find(id);
            _context.Sellers.Remove(seller);
            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            try
            {
                if (!_context.Sellers.Any(s => s.Id == seller.Id))
                {
                    throw new NotFoundException("Seller Id not found");
                }
                _context.Update(seller);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                // the exception from database(repository) layer is captured and converted to an exception from the service layer
                throw new ConcurrencyException(ex.Message, ex);
            }
        }
    }
}
