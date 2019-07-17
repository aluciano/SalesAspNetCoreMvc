using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public void Insert(Seller seller)
        {
            _context.Seller.Add(seller);
            _context.SaveChanges();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Seller.Add(seller);
            await _context.SaveChangesAsync();
        }

        public Seller FindById(int id)
        {
            return _context.Seller
                           .FirstOrDefault(p => p.Id == id);
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Seller FindByIdWithDepartment(int id)
        {
            return _context.Seller
                           .Include(p => p.Department)
                           .FirstOrDefault(p => p.Id == id);
        }

        public async Task<Seller> FindByIdWithDepartmentAsync(int id)
        {
            return await _context.Seller
                                 .Include(p => p.Department)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(int id)
        {
            try
            {
                var seller = FindById(id);
                _context.Seller.Remove(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var seller = await FindByIdAsync(id);
                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete seller because he/she has sales.");
            }            
        }

        public void Update(Seller seller)
        {
            bool hasAny = _context.Seller.Any(p => p.Id == seller.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id not fount.");
            }

            try
            {
                _context.Update(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrenceException(e.Message);
            }
            
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool hasAny = await _context.Seller.AnyAsync(p => p.Id == seller.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id not fount.");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrenceException(e.Message);
            }

        }
    }
}
