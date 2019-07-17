using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = _context.SalesRecord.Select(p => p);
            if (minDate.HasValue)
                result = result.Where(p => p.Date >= minDate.Value);

            if (maxDate.HasValue)
                result = result.Where(p => p.Date <= maxDate.Value);

            return await result.Include(p => p.Seller)
                               .Include(p => p.Seller.Department)
                               .OrderByDescending(p => p.Date)
                               .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = _context.SalesRecord.Select(p => p);
            if (minDate.HasValue)
                result = result.Where(p => p.Date >= minDate.Value);

            if (maxDate.HasValue)
                result = result.Where(p => p.Date <= maxDate.Value);

            return await result.Include(p => p.Seller)
                               .Include(p => p.Seller.Department)
                               .OrderByDescending(p => p.Date)
                               .GroupBy(p => p.Seller.Department)
                               .ToListAsync();
        }
    }
}
