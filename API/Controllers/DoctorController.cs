using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class DoctorController :ControllerBase
    {
         private readonly IGenericRepository<Doctor> _DoctorRepo;

        private readonly IMapper _mapper;
        public DoctorController(IGenericRepository<Doctor> DoctorRepo, IMapper mapper)
        {
            _DoctorRepo = DoctorRepo;
            _mapper = mapper;

        }

        [HttpGet("Doctors")]
        public async Task<ActionResult<IReadOnlyList<List<Doctor>>>> GetDoctors()
        {
            
            var doctors = await _DoctorRepo.ListAllAsync();
          
            return Ok(doctors);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
           return await _DoctorRepo.GetByIdAsync(id);
        }
        
    }
}