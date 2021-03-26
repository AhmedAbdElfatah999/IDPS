using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class PatientController :ControllerBase
    {
        private readonly IGenericRepository<Patient> _PatientRepo;

        private readonly IMapper _mapper;
        public PatientController(IGenericRepository<Patient> PatientRepo, IMapper mapper)
        {
            _PatientRepo = PatientRepo;
            _mapper = mapper;

        }

        [HttpGet("Patients")]
        public async Task<ActionResult<IReadOnlyList<List<Patient>>>> GetPatients()
        {
            
            var patients = await _PatientRepo.ListAllAsync();
          
            return Ok(patients);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
           return await _PatientRepo.GetByIdAsync(id);
        }   
    }
}