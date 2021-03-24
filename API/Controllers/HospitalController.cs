using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class HospitalController :ControllerBase
    {
        private readonly IGenericRepository<Hospital> _HospitalRepo;

        private readonly IMapper _mapper;
        public HospitalController(IGenericRepository<Hospital> HospitalRepo, IMapper mapper)
        {
            _HospitalRepo = HospitalRepo;
            _mapper = mapper;

        }

        [HttpGet("Hospitals")]
        public async Task<ActionResult<IReadOnlyList<List<Hospital>>>> GetHospitals()
        {
            
            var hospitals = await _HospitalRepo.ListAllAsync();
          
            return Ok(hospitals);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> GetAdmin(int id)
        {
           return await _HospitalRepo.GetByIdAsync(id);
        }
    }
}