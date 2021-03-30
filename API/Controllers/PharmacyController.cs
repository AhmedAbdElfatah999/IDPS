using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class PharmacyController : BaseApiController
    {
        private readonly IGenericRepository<Pharmacy> _PharmacyRepo;

        private readonly IMapper _mapper;
        public PharmacyController(IGenericRepository<Pharmacy> PharmacyRepo, IMapper mapper)
        {
            _PharmacyRepo = PharmacyRepo;
            _mapper = mapper;

        }

        [HttpGet("Pharmacies")]
        public async Task<ActionResult<IReadOnlyList<List<Pharmacy>>>> GetPharmacys()
        {
            
            var pharmacies = await _PharmacyRepo.ListAllAsync();
          
            return Ok(pharmacies);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
           return await _PharmacyRepo.GetByIdAsync(id);
        }
    }
}