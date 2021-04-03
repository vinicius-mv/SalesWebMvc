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

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Sellers.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Sellers.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                Seller seller = await _context.Sellers.FindAsync(id);
                _context.Sellers.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new IntegrityException("Can not delete a seller with associated sales record");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            try
            {
                bool hasAny = await _context.Sellers.AnyAsync(s => s.Id == seller.Id);
                if (!hasAny)
                {
                    throw new NotFoundException("Seller Id not found");
                }
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // the exception from database(repository) layer is captured and converted to an exception from the service layer
                throw new ConcurrencyException(ex.Message);
            }
        }
    }
}
