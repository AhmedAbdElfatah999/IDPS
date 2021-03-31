using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class MedicineController :BaseApiController
    {
        private readonly IGenericRepository<Medicine> _MedicineRepo;

        private readonly IMapper _mapper;
        public MedicineController(IGenericRepository<Medicine> MedicineRepo, IMapper mapper)
        {
            _MedicineRepo = MedicineRepo;
            _mapper = mapper;

        }

        [HttpGet("Medicines")]
        public async Task<ActionResult<IReadOnlyList<List<Medicine>>>> GetMedicines()
        {
            
            var medicines = await _MedicineRepo.ListAllAsync();
          
            return Ok(medicines);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
           return await _MedicineRepo.GetByIdAsync(id);
        } 
    }
}