using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class SpecializationController :BaseApiController
    {
        private readonly IGenericRepository<Specialization> _SpecializationRepo;

        private readonly IMapper _mapper;
        public SpecializationController(IGenericRepository<Specialization> SpecializationRepo, IMapper mapper)
        {
            _SpecializationRepo = SpecializationRepo;
            _mapper = mapper;

        }

        [HttpGet("Specializations")]
        public async Task<ActionResult<IReadOnlyList<List<Specialization>>>> GetSpecializations()
        {
            
            var specializations = await _SpecializationRepo.ListAllAsync();
          
            return Ok(specializations);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
           return await _SpecializationRepo.GetByIdAsync(id);
        }
    }
}