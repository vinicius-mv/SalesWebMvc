using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordsService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordsService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> result = _context.SalesRecords.Select(s => s);

            if(minDate.HasValue)
            {
                result = result.Where(sr => sr.Date >= minDate);
            }

            if(maxDate.HasValue)
            {
                result = result.Where(sr => sr.Date <= maxDate);
            }

            return await result.Include(sr => sr.Seller)
                                .Include(sr => sr.Seller.Department)
                                .OrderByDescending(sr => sr.Date)
                                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<SalesRecord> result = _context.SalesRecords.Select(s => s);

            if (minDate.HasValue)
            {
                result = result.Where(sr => sr.Date >= minDate);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(sr => sr.Date <= maxDate);
            }

            return await result.Include(sr => sr.Seller)
                                .Include(sr => sr.Seller.Department)
                                .OrderByDescending(sr => sr.Date)
                                .GroupBy(sr => sr.Seller.Department)
                                .ToListAsync();
        }
    }
}
