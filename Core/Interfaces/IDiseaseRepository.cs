using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDiseaseRepository
    {
        Task<Disease> GetDiseaseByIdAsync(int id);
        Task<IReadOnlyList<Disease>> GetDiseasesAsync();

    }
}