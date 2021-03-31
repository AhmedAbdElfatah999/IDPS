using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class DoctorController : BaseApiController
    {
         private readonly IGenericRepository<Doctor> _DoctorRepo;
         public readonly IGenericRepository<Specialization> _SpecializationRepo; 
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
        [HttpGet("specializations")]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDoctorSpecialization()
        {
            return Ok(await _SpecializationRepo.ListAllAsync());
        }
        
    }
}