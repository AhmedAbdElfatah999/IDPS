using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly IDPSContext _context;
        public DiseaseRepository(IDPSContext context)
        {
            _context=context;
        }

        public async Task<Disease> GetDiseaseByIdAsync(int id)
        {
            return await  _context.Diseases.FindAsync(id);
        }

        public async Task<IReadOnlyList<Disease>> GetDiseasesAsync()
        {
            return await _context.Diseases.ToListAsync();
        }
    }
}